module BohFoundation.Applicant.FamilyInformation.Controllers {
    export interface IFamilyInformationCtrl {
        $scope: IFamilyInformationCtrlScope
    }


    export interface IFamilyInformationCtrlScope extends ng.IScope {
        getFamilyInformationModel(): Dtos.Applicant.Family.FamilyInformationModel;
        editFamilyInformation(): void;

        processing: boolean;
        modalInstance: ng.ui.bootstrap.IModalServiceInstance;
        newFamilyInformationInputModel: Dtos.Applicant.Family.FamilyInformationModel;
        successfullySaved: boolean;
    }

    export class FamilyInformationCtrl implements IFamilyInformationCtrl {
        
        static $inject = ['$scope', '$modal', 'FamilyInformationService'];

        constructor(
            public $scope: IFamilyInformationCtrlScope,
            private $modal: ng.ui.bootstrap.IModalService,
            private familyInformationService: Services.IFamilyInformationService) {

            this.getFamilyInforamtion();

            $scope.getFamilyInformationModel = () => {
                return this.getFamilyInformationModel();
            };

            $scope.editFamilyInformation = () => {
                this.editFamilyInformation();
            };

        }

        private getFamilyInformationModel() {
            return this.familyInformationService.getFamilyInformationModel();
        }

        private getFamilyInforamtion() {
            this.$scope.processing = true;
            var resource = this.familyInformationService.getFamilyInformationFromServer();
            resource.$promise.then(
                dataFromServer => {
                    this.familyInformationService.resolveGetFamilyInformationFromServer(new Common.Models.ServerResponseModel(dataFromServer, true));
                    this.flipProcessing();
                },
            () => {
                this.familyInformationService.resolveGetFamilyInformationFromServer(new Common.Models.ServerResponseModel(null, false));
            });
        }

        private flipProcessing() {
            this.$scope.processing = !this.$scope.processing;
        }

        private editFamilyInformation() {
            if (!this.$scope.processing) {
                this.$scope.newFamilyInformationInputModel = this.getFamilyInformationModel();
                this.openModal();
            }
        }

        private openModal() {
            this.$scope.modalInstance = this.$modal.open({
                templateUrl: "/AngularApp/Applicant/FamilyInformation/Templates/FamilyInformationModal.html",
                controller: "FamilyInformationModalCtrl",
                resolve: {
                    familyInformationModel: () => this.$scope.newFamilyInformationInputModel
                }
            });

            this.$scope.modalInstance.result.then(
                modelFromModal => {
                    this.postFamilyInformation(modelFromModal);
                },
                () => {

                });
        }

        private postFamilyInformation(modelFromModal: Dtos.Applicant.Family.FamilyInformationModel) {
            this.$scope.processing = true;
            var resource = this.familyInformationService.postFamilyInformation(modelFromModal);
            resource.$promise.then(() => {
                this.$scope.successfullySaved = true;
                this.resolvePost(true);
                this.getFamilyInforamtion();
            }, () => {
                this.flipProcessing();
                this.resolvePost(false);
            });
        }

        private resolvePost(success: boolean) {
            this.familyInformationService.resolvePostFamilyInformation(new Common.Models.ServerResponseModel(null, success));
        }
    }
    
} 

module BohFoundation.Main {
    Register.Applicant.controller("FamilyInformationCtrl", Applicant.FamilyInformation.Controllers.FamilyInformationCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/Applicant/FamilyInformation', {
                    templateUrl: '/AngularApp/Applicant/FamilyInformation/Templates/FamilyInformationTemplate.html',
                    controller: 'FamilyInformationCtrl',
                    publicAccess: false,
                    title: 'Family Information'
                });
            })]);
}