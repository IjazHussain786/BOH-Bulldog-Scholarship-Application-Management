module BohFoundation.Admin.Services {
    'use strict';
    export interface IAdminNotificationService {
        getNotifications(): ng.resource.IResource<any>;
        resolveNotifications(serverResponseModel: Common.Models.ServerResponseModel): void;

        getNumberOfTotalNotifications(): number;
        getNumberOfPendingApplicationEvaluators(): number;
    }

    export class AdminNotificationService implements IAdminNotificationService {

        private adminNotificationApiEndpoint = '/api/admin/notifications';
        private errorMessage = "There was an error getting your admin notifications."; 
        private adminNotificationCounts: Dtos.Admin.AdminNotificationCounts;

        static $inject = ['AuthRequiredRepository','AlertHelperService'];
        constructor(private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private alertHelperService: Common.Services.UiServices.IAlertHelperServices) { }

        getNotifications() : ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.adminNotificationApiEndpoint);
        }

        resolveNotifications(serverResponseModel: Common.Models.ServerResponseModel) {
            if (!serverResponseModel.success) {
                this.alertHelperService.addDangerAlert(this.errorMessage);
            } else {
                this.adminNotificationCounts = new Dtos.Admin.AdminNotificationCounts(serverResponseModel.dataFromServer.totalCount, serverResponseModel.dataFromServer.pendingApplicationEvaluators);
            }
        }

        getNumberOfTotalNotifications(): number {
            if (this.adminNotificationCounts == undefined) {
                return 0;
            }
            return this.adminNotificationCounts.totalCount;
        }

        getNumberOfPendingApplicationEvaluators(): number {
            if (this.adminNotificationCounts == undefined) {
                return 0;
            }
            return this.adminNotificationCounts.pendingApplicationEvaluators;
        }
    }
}

module BohFoundation.Main {
    Register.Admin.service("AdminNotificationService", BohFoundation.Admin.Services.AdminNotificationService);
}