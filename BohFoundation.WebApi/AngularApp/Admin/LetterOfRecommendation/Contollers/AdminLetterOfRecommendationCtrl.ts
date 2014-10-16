module BohFoundation.Admin.LetterOfRecommendation.Controllers {
    export interface IAdminLetterOfRecommendationCtrl {
        $scope: IAdminLetterOfRecommendationCtrlScope
    }


    export interface IAdminLetterOfRecommendationCtrlScope extends ng.IScope {
        getGuidSentToReferenceModel(): Dtos.Admin.References.GuidSentToReferenceModel;

        getGuidSentToReferenceModelFromServer(): void;

        canGoToReference(): boolean;
        goToReferenceForm(): void;

        modalInstance: ng.ui.bootstrap.IModalServiceInstance;
        modelSentToServer: Dtos.Admin.References.GetLetterOfRecommendationGuidModel;
        processing: boolean;
    }

    export class AdminLetterOfRecommendationCtrl implements IAdminLetterOfRecommendationCtrl {
        
        static $inject = ['$scope', '$modal', "AdminLetterOfRecommendationService"];
        constructor(
            public $scope: IAdminLetterOfRecommendationCtrlScope,
            private $modal: ng.ui.bootstrap.IModalService,
            private adminLetterOfRecommendationService: Services.IAdminLetterOfRecommendationService) {

            $scope.getGuidSentToReferenceModel = () => {
                return this.adminLetterOfRecommendationService.getGuidSentToReference();
            };

            $scope.getGuidSentToReferenceModelFromServer = () => {
                if (!$scope.processing) {
                    this.openModalAndGetModelFromServer();
                }
            };

            $scope.canGoToReference = () => {
                return this.adminLetterOfRecommendationService.canGoToReference();
            };

            $scope.goToReferenceForm = () => {
                if (!$scope.processing) {
                    this.adminLetterOfRecommendationService.goToReferenceForm();
                }
            };
        }

        private openModalAndGetModelFromServer() {
            this.$scope.modelSentToServer = new Dtos.Admin.References.GetLetterOfRecommendationGuidModel();
            this.$scope.modalInstance = this.$modal.open({
                templateUrl: "/AngularApp/Admin/LetterOfRecommendation/Templates/CreateGetLetterOfRecommendationGuidModelModal.html",
                controller: "CreateGetLetterOfRecommendationGuidModelModalCtrl",
                resolve: {
                    getLetterOfRecommendationGuidModel:() => this.$scope.modelSentToServer    
                }
            });

            this.$scope.modalInstance.result.then(modelFromModal => {
                this.$scope.modelSentToServer = modelFromModal;
                this.getGuidSentToReferenceFromServer();
            }, () => {
                this.$scope.modelSentToServer = undefined;
            });
        }

        private getGuidSentToReferenceFromServer() {
            this.$scope.processing = true;
            var resource = this.adminLetterOfRecommendationService.getGuidSentToReferenceFromServer(this.$scope.modelSentToServer);
            resource.$promise.then(dataFromServer => {
                this.resolveGet(dataFromServer);
            }, () => {
                this.resolveGet(null);
            });
        }

        private resolveGet(dataFromServer) {
            this.$scope.processing = false;
            if (dataFromServer != null) {
                this.adminLetterOfRecommendationService.resolveGetGuidSentToReferenceFromServer(new Common.Models.ServerResponseModel(dataFromServer, true));
            } else {
                this.adminLetterOfRecommendationService.resolveGetGuidSentToReferenceFromServer(new Common.Models.ServerResponseModel(null, false));
            }
        }
    }
} 

module BohFoundation.Main {
    Register.Admin.controller("AdminLetterOfRecommendationCtrl", Admin.LetterOfRecommendation.Controllers.AdminLetterOfRecommendationCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/Admin/AddLetterOfRecommendation', {
                    templateUrl: '/AngularApp/Admin/LetterOfRecommendation/Templates/GetGuidForAdminLetterOfRecommendation.html',
                    controller: 'AdminLetterOfRecommendationCtrl',
                    publicAccess: false,
                    title: 'Add Letter Of Recommendation'
                });
            })]);
}