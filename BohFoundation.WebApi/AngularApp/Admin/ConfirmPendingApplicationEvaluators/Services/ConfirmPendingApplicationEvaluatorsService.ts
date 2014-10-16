module BohFoundation.Admin.ConfirmPendingApplicationEvaluators.Services {
    'use strict';
    export interface IConfirmPendingApplicationEvaluatorsService {
        getPendingApplicationEvaluators(): ng.resource.IResource<any>;
        resolveGetPendingApplicationEvaluators(serverResponseModel: Common.Models.ServerResponseModel): void;
        
        confirmOrRejectEvaluator(index: number, confirm: boolean, createAdmin: boolean): ng.resource.IResource<any>;
        resolveConfirmOrRejectEvaluator(success: boolean): void;
        
        listOfPendingApplicationEvaluators: Array<Models.PendingApplicationEvaluatorModel>;
    }

    export class ConfirmPendingApplicationEvaluatorsService implements IConfirmPendingApplicationEvaluatorsService {

        listOfPendingApplicationEvaluators : Array<Models.PendingApplicationEvaluatorModel> = []; 

        private getPendingApplicationEvaluatorsApiEndpoint = "/api/admin/inviteconfirmapplicationevaluator/pendingevaluators";
        private confirmApplicationEvaluatorApiEndpoint = "/api/admin/inviteconfirmapplicationevaluator/confirm";

        static $inject = ['AuthRequiredRepository', 'AlertHelperService'];
        constructor(private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private alertHelperService: Common.Services.UiServices.IAlertHelperServices) { }

        getPendingApplicationEvaluators(): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.getPendingApplicationEvaluatorsApiEndpoint);
        }

        resolveGetPendingApplicationEvaluators(serverResponseModel: Common.Models.ServerResponseModel): void {
            if (!serverResponseModel.success) {
                this.errorMessage("There was an error getting the pending application evaluators. Try again later.");
            } else {
                this.setListOfPendingApplicationEvaluators(serverResponseModel.dataFromServer);
            }
        }

        confirmOrRejectEvaluator(indexOfItemToEvaluate: number, confirm: boolean, createAdmin: boolean) {
            this.changeTheEvaluatedStateOfItem(indexOfItemToEvaluate, confirm);
            var personConfirming = this.listOfPendingApplicationEvaluators[indexOfItemToEvaluate];
            var objectToSend = new Dtos.Admin.ConfirmApplicationEvaluatorModel(personConfirming.emailAddress, confirm, createAdmin);
            return this.authRequiredRepository.post(objectToSend, this.confirmApplicationEvaluatorApiEndpoint);
        }

        resolveConfirmOrRejectEvaluator(success: boolean): void {
            if (success) {
                this.alertHelperService.addSuccessAlert("You successfully confirmed or rejected a application evaluator.");
            } else {
                this.errorMessage("There was an error confirming or rejecting an application evaluator. Please try again later.");
            }
        }

        private errorMessage(message: string) {
            this.alertHelperService.addDangerAlert(message);
        }

        private setListOfPendingApplicationEvaluators(listOfPendingApplicationEvaluators) {
            var list = [];
            angular.forEach(listOfPendingApplicationEvaluators, (item: Common.Models.PersonModel) => {
                list.push(new Models.PendingApplicationEvaluatorModel(item.firstName, item.lastName, item.emailAddress, Models.ConfirmRejectedOrNotEvaluated.NotEvaluated));
            });
            this.listOfPendingApplicationEvaluators = list;
        }

        private changeTheEvaluatedStateOfItem(indexOfItemToEvaluate: number, confirm: boolean) {
            if (confirm) {
                this.listOfPendingApplicationEvaluators[indexOfItemToEvaluate].confirmed = Models.ConfirmRejectedOrNotEvaluated.Confirmed;
            } else {
                this.listOfPendingApplicationEvaluators[indexOfItemToEvaluate].confirmed = Models.ConfirmRejectedOrNotEvaluated.Rejected;
            }
        }
    }
}

module BohFoundation.Main {
    Register.Admin.service("ConfirmPendingApplicationEvaluatorsService", Admin.ConfirmPendingApplicationEvaluators.Services.ConfirmPendingApplicationEvaluatorsService);
}