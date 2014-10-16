module BohFoundation.Reference.LetterOfRecommendation.Anon.Controllers {
    export interface IRecommendAnonCtrl {
        $scope: IRecommendAnonCtrlScope;
    }

    export interface IRecommendAnonCtrlScope extends ng.IScope {
        processing: boolean;
        modalInstance: ng.ui.bootstrap.IModalServiceInstance;
        referencePersonalInformationModel: Dtos.Reference.Anonymous.ReferencePersonalInformationModel;
        letterOfRecommendation: string;

        mustCompletePersonalInformation(): boolean;
        editPersonalInformation(): void;
        postLetterOfRecommendation(): void;
        getInformationForReferenceFormModel(): Dtos.Reference.Anonymous.InformationForReferenceFormModel;
    }

    export interface IRecommendAnonCtrlRouteProvider extends ng.route.IRouteParamsService {
        letterOfRecommendationGuid: string;
    }

    export class RecommendAnonCtrl implements IRecommendAnonCtrl {
        
        private failureServerResponseMessage = new Common.Models.ServerResponseModel(null, false);

        static $inject = ['$scope', '$routeParams', "RecommendAnonService", "$modal", 'cssInjector'];
        constructor(
            public $scope: IRecommendAnonCtrlScope,
            private $routeParams: IRecommendAnonCtrlRouteProvider,
            private recommendAnonService: Services.IRecommendAnonService,
            private $modal: ng.ui.bootstrap.IModalService,
            private cssInjector: any
            ) {

            this.initialize();

            $scope.editPersonalInformation = () => {
                if (!$scope.processing) {
                    this.editPersonalInformation();
                }
            };

            $scope.mustCompletePersonalInformation = () => {
                return this.mustCompletePersonalInformation();
            };

            $scope.postLetterOfRecommendation = () => {
                if (!$scope.processing) {
                    this.postLetterOfRecommendation();
                }
            };

            $scope.getInformationForReferenceFormModel = () => {
                return this.recommendAnonService.getInformationForReferenceFormModel();
            };
        }

        private initialize() {
            this.cssInjector.add("https://maxcdn.bootstrapcdn.com/font-awesome/4.1.0/css/font-awesome.min.css");
            this.setGuidFromRoute();
            this.getLetterOfRecommendationInformationFromServer();
        }

        private setGuidFromRoute() {
            this.recommendAnonService.setGuidOfLetterOfRecommendation(this.$routeParams.letterOfRecommendationGuid);
        }

        private getLetterOfRecommendationInformationFromServer() {
            this.$scope.processing = true;
            var resource = this.recommendAnonService.getLetterOfRecommendationInformation();
            resource.$promise.then(
                data => { this.successfulGetLetterOfRecommendationInformation(data); },
                () => { this.recommendAnonService.resolveGetLetterOfRecommendationInformation(this.failureServerResponseMessage); });
        }

        private successfulGetLetterOfRecommendationInformation(data) {
            this.recommendAnonService.resolveGetLetterOfRecommendationInformation(new Common.Models.ServerResponseModel(data, true));
            this.flipProcessing();
        }

        private flipProcessing() {
            this.$scope.processing = !this.$scope.processing;
        }

        private editPersonalInformation() {
            this.instantiateReferencePersonalInfoModel();
            this.openModal();
        }

        private openModal() {
            this.$scope.modalInstance = this.$modal.open({
                templateUrl: "/AngularApp/Reference/LetterOfRecommendation/Anon/Templates/ReferencePersonalInformationModal.html",
                controller: "AnonReferencePersonalInfoModalCtrl",
                resolve: {
                    referencePersonalInformationModel: () => this.$scope.referencePersonalInformationModel
                }
            });

            this.$scope.modalInstance.result.then(
                modelFromModal => {
                    this.postPersonalInformation(modelFromModal);
                },
                () => {
                    this.$scope.referencePersonalInformationModel = null;
                });
        }

        private instantiateReferencePersonalInfoModel() {
            if (this.mustCompletePersonalInformation()) {
                this.$scope.referencePersonalInformationModel = new Dtos.Reference.Anonymous.ReferencePersonalInformationModel();
            } else {
                this.$scope.referencePersonalInformationModel = new Dtos.Reference.Anonymous.ReferencePersonalInformationModel(this.recommendAnonService.getInformationForReferenceFormModel().referencesName);
            }
        }

        private mustCompletePersonalInformation() {
            if (this.recommendAnonService.getInformationForReferenceFormModel() != undefined) {
                if (this.recommendAnonService.getInformationForReferenceFormModel().phoneNumberLastUpdated != null) {
                    return false;
                }
            }
            return true;
        }

        private postPersonalInformation(modelFromModal) {
            this.$scope.processing = true;
            this.recommendAnonService.postReferencePersonalInformation(modelFromModal).$promise.then(
                () => {
                    this.resolvePostPersonalInformation(true);
                    this.getLetterOfRecommendationInformationFromServer();
                },
                () => { this.resolvePostPersonalInformation(false); });
        }

        private resolvePostPersonalInformation(bool: boolean) {
            this.recommendAnonService.resolvePostReferencePersonalInformation(new Common.Models.ServerResponseModel(null, bool));
        }

        private postLetterOfRecommendation() {
            this.$scope.processing = true;
            this.recommendAnonService.postLetterOfRecommendation(this.$scope.letterOfRecommendation).$promise.then(
                () => { this.resolvePostLetterOfRecommendation(true); },
                () => {
                    this.resolvePostLetterOfRecommendation(false);
                    this.$scope.processing = false;
                });
        }

        private resolvePostLetterOfRecommendation(success: boolean) {
            this.recommendAnonService.resolvePostLetterOfRecommendation(new Common.Models.ServerResponseModel(null, success));
        }
    }
}

module BohFoundation.Main {
    Register.Reference.controller('RecommendAnonCtrl', Reference.LetterOfRecommendation.Anon.Controllers.RecommendAnonCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when("/Reference/LetterOfRecommendation/Anon/:letterOfRecommendationGuid", {
                    templateUrl: '/AngularApp/Reference/LetterOfRecommendation/Anon/Templates/LettersOfRecommendation.html',
                    controller: 'RecommendAnonCtrl',
                    publicAccess: true,
                    title: 'Letter of Recommendation'
                });
            })]);
}