module BohFoundation.Applicant.References.Controllers {
    export interface IApplicantReferenceCtrl {
        $scope: IApplicantReferenceCtrlScope;
        
    }

    export interface IApplicantReferenceCtrlScope extends ng.IScope {
        processing: boolean;
        getArrayOfReferenceModels(): Array<Dtos.Applicant.References.ReferenceModel>;
        addReference(): void;
        ableToAddNewReference(): boolean;

        modalInstance: ng.ui.bootstrap.IModalServiceInstance;
        newReferenceInputModel: Dtos.Applicant.References.ApplicantReferenceInputModel;
    }

    export class ApplicantReferenceCtrl implements IApplicantReferenceCtrl {
        
        static $inject = ['$scope', 'ApplicantReferenceService', '$modal'];

        constructor(
            public $scope: IApplicantReferenceCtrlScope,
            private applicantReferenceService: Services.IApplicantReferenceService,
            private $modal: ng.ui.bootstrap.IModalService) {

            this.initialize();

            $scope.getArrayOfReferenceModels = () => {
                return this.getArrayOfReferenceModels();
            };

            $scope.ableToAddNewReference = () => {
                return this.ableToAddNewReference();
            };

            $scope.addReference = () => {
                this.addReference();
            };
        }

        private initialize() {
            this.getSubmittedReferenceFromServer();
        }

        private getSubmittedReferenceFromServer() {
            this.$scope.processing = true;
            var resource = this.applicantReferenceService.getSubmittedReferencesFromServer();
            resource.$promise.then(
                (data) => {
                    this.applicantReferenceService.resolveGetSubmittedReference(new Common.Models.ServerResponseModel(data, true));
                    this.flipProcessing();
                },
                () => { this.applicantReferenceService.resolveGetSubmittedReference(new Common.Models.ServerResponseModel(null, false)); }
                );
        }

        private flipProcessing() {
            this.$scope.processing = !this.$scope.processing;
        }

        private ableToAddNewReference(): boolean {
            if (!this.$scope.processing && this.getArrayOfReferenceModels().length < 3) {
                return true;
            }
            return false;
        }

        private getArrayOfReferenceModels(): Array<Dtos.Applicant.References.ReferenceModel> {
            return this.applicantReferenceService.getArrayOfReferenceModels();
        }

        private addReference() {
            if (this.ableToAddNewReference()) {
                this.$scope.newReferenceInputModel = new Dtos.Applicant.References.ApplicantReferenceInputModel();
                this.openModal();
            }
        }

        private openModal() {
            this.$scope.modalInstance = this.$modal.open({
                templateUrl: "/AngularApp/Applicant/References/Templates/AddReferenceModal.html",
                controller: 'ApplicantReferenceModalCtrl',
                resolve: {
                    refernceInputModelForModal: () => this.$scope.newReferenceInputModel
                }
            });

            this.$scope.modalInstance.result.then(
                modelFromModal => {
                    this.$scope.newReferenceInputModel = modelFromModal;
                    this.postReferenceRequest();
                },
                () => {
                    this.$scope.newReferenceInputModel = null;
                });
        }

        private postReferenceRequest() {
            this.flipProcessing();
            var resource = this.applicantReferenceService.postReferenceRequest(this.$scope.newReferenceInputModel);
            resource.$promise.then(() => {
                this.applicantReferenceService.resolvePostReferenceRequest(new Common.Models.ServerResponseModel(null, true));
                this.$scope.newReferenceInputModel = null;
                this.getSubmittedReferenceFromServer();
            }, () => {
                this.applicantReferenceService.resolvePostReferenceRequest(new Common.Models.ServerResponseModel(null, false));
            });
        }
    }
}

module BohFoundation.Main {
    Register.Applicant.controller('ApplicantReferenceCtrl', Applicant.References.Controllers.ApplicantReferenceCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/Applicant/References', {
                    templateUrl: '/AngularApp/Applicant/References/Templates/ApplicantReferencesTemplate.html',
                    controller: 'ApplicantReferenceCtrl',
                    publicAccess: false,
                    title: 'References'
                });
            })]);
}