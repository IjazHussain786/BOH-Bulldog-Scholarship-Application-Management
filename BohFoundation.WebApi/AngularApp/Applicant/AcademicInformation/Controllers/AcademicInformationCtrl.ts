module BohFoundation.Applicant.AcademicInformation.Controllers {
    export interface IAcademicInformationCtrlScope extends ng.IScope {
        processing: boolean;
        modalInstance: ng.ui.bootstrap.IModalServiceInstance;
        successfullySaved: boolean;
        academicInformationModelForModal: Dtos.Applicant.Academic.AcademicInformationModel;
        
        getAcademicInformationModel(): Dtos.Applicant.Academic.AcademicInformationModel;
        editAcademicInformationModel(): void;
    }

    export interface IAcademicInformationCtrl {
        $scope:IAcademicInformationCtrlScope;
    }

    export class AcademicInformationCtrl implements IAcademicInformationCtrl {

        static $inject = ['$scope', 'AcademicInformationServices', '$modal'];
        constructor(
            public $scope: IAcademicInformationCtrlScope,
            private academicInformationServices: Services.IAcademicInformationServices,
            private $modal: ng.ui.bootstrap.IModalService) {

            this.getAcademicInformation();

            $scope.getAcademicInformationModel = () => {
                return this.getAcademicInformationModel();
            };

            $scope.editAcademicInformationModel = () => {
                this.editAcademicInformationModel();
            };
        }

        private getAcademicInformation() {
            this.setProcessingToTrue();
            var resource = this.academicInformationServices.getAcademicInformation();
            resource.$promise.then(
                (fromServer) => { this.resolveGetAcademicInformation(new Common.Models.ServerResponseModel(fromServer, true)); },
                () => { this.resolveGetAcademicInformation(new Common.Models.ServerResponseModel(null, false)); });
        }

        private resolveGetAcademicInformation(serverResponse: Common.Models.ServerResponseModel) {
            this.academicInformationServices.resolveGetAcademicInformation(serverResponse);
            if (serverResponse.success) {
                this.flipProcessing();
            }
        }

        private flipProcessing() {
            this.$scope.processing = !this.$scope.processing;
        }

        private postAcademicInformation(academicInfoModal: Dtos.Applicant.Academic.AcademicInformationModel) {
            this.setProcessingToTrue();
            var resource = this.academicInformationServices.postAcademicInformation(academicInfoModal);
            resource.$promise.then(
                () => { this.resolvePostAcademicInformation(true); },
                () => { this.resolvePostAcademicInformation(false);
            });
        }

        private resolvePostAcademicInformation(success: boolean) {
            this.academicInformationServices.resolvePostAcademicInformation(new Common.Models.ServerResponseModel(null, success));
            if (success) {
                this.$scope.successfullySaved = true;
                this.getAcademicInformation();
            } else {
                this.flipProcessing();    
            }
        }

        private setProcessingToTrue() {
            this.$scope.processing = true;
        }

        private editAcademicInformationModel() {
            if (!this.$scope.processing) {
                this.$scope.academicInformationModelForModal = this.getAcademicInformationModel();
                this.openModal();
            }
        }

        private getAcademicInformationModel() {
            return this.academicInformationServices.getApplicantAcademicInformationModel();
        }

        private openModal() {
            this.$scope.modalInstance = this.$modal.open({
                templateUrl: "/AngularApp/Applicant/AcademicInformation/Templates/EditAcademicInfoModal.html",
                controller: "EditAcademicInformationModalCtrl",
                resolve: {
                    academicInformationModel: () => this.$scope.academicInformationModelForModal
                }
            });

            this.$scope.modalInstance.result.then(
                modelFromModal => {
                    this.postAcademicInformation(modelFromModal);
                },
                () => {

                });
        }
    }
} 

module BohFoundation.Main {
    Register.Applicant.controller('AcademicInformationCtrl', Applicant.AcademicInformation.Controllers.AcademicInformationCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/Applicant/AcademicInformation', {
                    templateUrl: '/AngularApp/Applicant/AcademicInformation/Templates/AcademicInformation.html',
                    controller: 'AcademicInformationCtrl',
                    publicAccess: false,
                    title: 'Academic Information'
                });
            })]);
}