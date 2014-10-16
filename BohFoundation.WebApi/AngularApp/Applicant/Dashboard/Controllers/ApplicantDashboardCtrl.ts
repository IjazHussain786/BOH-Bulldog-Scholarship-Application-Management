module BohFoundation.Applicant.Dashboard.Controllers {
    export interface IApplicantDashboardCtrl {
        $scope: IApplicantDashboardCtrlScope;
    }

    export interface IApplicantDashboardCtrlScope extends ng.IScope {
        getPersonalInfoDashboardItem(): Models.ApplicantDashboardListItemInputModel;
        getContactInfoDashboardItem(): Models.ApplicantDashboardListItemInputModel;
        getTransciptUploadedDashboardItem(): Models.ApplicantDashboardListItemInputModel;
        getAcademicInformationDashboardItem(): Models.ApplicantDashboardListItemInputModel;
        getLowGradeDashboardItem(): Models.ApplicantDashboardListItemInputModel;
        getHideLowGrade(): boolean;
        getEssayDashboardInputModels(): Array<Models.ApplicantDashboardListItemInputModel>;
        getReferenceItem(): Models.ApplicantDashboardListItemInputModel;
        getFamilyInformationDashboardItem(): Models.ApplicantDashboardListItemInputModel;
        getExtracurricularActivitiesItem(): Models.ApplicantDashboardListItemInputModel;

        canFinalizeApplication(): boolean;
        finalizeApplication(): void;
        modalInstance: ng.ui.bootstrap.IModalServiceInstance;

        getDeadlineDate(): Date;
    }

    export class ApplicantDashboardCtrl implements IApplicantDashboardCtrl {

        static $inject = ['$scope', 'ApplicantNotificationService', "$modal", "FinalizeApplicationService"];
        constructor(
            public $scope: IApplicantDashboardCtrlScope,
            private applicantNotificationService: Services.IApplicantNotificationService,
            private $modal: ng.ui.bootstrap.IModalService,
            private finalizeApplicationService: Services.IFinalizeApplicationService) {

            this.getNotifications();

            $scope.getPersonalInfoDashboardItem = () => {
                return applicantNotificationService.getPersonalInformationDashboardInputModel();
            };

            $scope.getContactInfoDashboardItem = () => {
                return applicantNotificationService.getContactInformationDashboardInputModel();
            };

            $scope.getTransciptUploadedDashboardItem = () => {
                return applicantNotificationService.getTranscriptUploadedDashboardInputModel();
            };

            $scope.getAcademicInformationDashboardItem = () => {
                return this.applicantNotificationService.getAcademicInformationDashboardInputModel();
            };

            $scope.getLowGradeDashboardItem = () => {
                return this.applicantNotificationService.getLowGradeInformationDashboardInputModel();
            }

            $scope.getHideLowGrade = () => {
                return this.applicantNotificationService.getHideLowGradeInformation();
            }

            $scope.getEssayDashboardInputModels = () => {
                return this.applicantNotificationService.getEssayDashboardInputModels();
            };

            $scope.getReferenceItem = () => {
                return this.applicantNotificationService.getReferenceDashboardInputModels();
            };

            $scope.getFamilyInformationDashboardItem = () => {
                return this.applicantNotificationService.getFamilyInformationDashboardInputModel();
            };

            $scope.getExtracurricularActivitiesItem = () => {
                return this.applicantNotificationService.getExtracurricularActivitiesDashboardInputModel();
            };

            $scope.canFinalizeApplication = () => {
                return this.finalizeApplicationService.canFinalizeEssay();
            };

            $scope.finalizeApplication = () => {
                this.finalizeApplication();
            };

            $scope.getDeadlineDate = () => {
                return this.applicantNotificationService.getDeadlineDate();
            };
        }

        private getNotifications() {
            var resource = this.applicantNotificationService.getNotifications();
            resource.$promise.then(
                (data) => { this.resolveGetNotifications(new Common.Models.ServerResponseModel(data, true)); },
                () => { this.resolveGetNotifications(new Common.Models.ServerResponseModel(null, false)); });
        }

        private resolveGetNotifications(model: Common.Models.ServerResponseModel) {
            this.applicantNotificationService.resolveGetNotifications(model);
        }

        private finalizeApplication() {
            this.$scope.modalInstance = this.$modal.open({
                templateUrl: "/AngularApp/Applicant/Dashboard/Templates/AreYouSureFinalizeModal.html",
                controller: "AreYouSureFinalizeModalCtrl"
            });

            this.$scope.modalInstance.result.then(() => {
                var resource = this.finalizeApplicationService.postFinalizeApplication();
                resource.$promise.then(() => {
                    this.resolveFinalizeApplication(true);
                }, () => {
                    this.resolveFinalizeApplication(false);
                });
            }, () => {});
        }

        private resolveFinalizeApplication(success: boolean) {
            this.finalizeApplicationService.resolveFinalizeApplication(new Common.Models.ServerResponseModel(null, success));
        }
    }
}

module BohFoundation.Main {
    Register.Applicant.controller('ApplicantDashboardCtrl', Applicant.Dashboard.Controllers.ApplicantDashboardCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/Applicant', {
                    templateUrl: '/AngularApp/Applicant/Dashboard/Templates/ApplicantDashboard.html',
                    controller: 'ApplicantDashboardCtrl',
                    publicAccess: false,
                    title: 'Applicant Dashboard'
                });
            })]);
}