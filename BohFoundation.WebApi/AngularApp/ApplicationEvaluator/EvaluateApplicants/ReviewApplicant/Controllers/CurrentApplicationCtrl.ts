module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Controllers {
    export interface ICurrentApplicationCtrl {
        $scope: ICurrentApplicationCtrlScope
    }


    export interface ICurrentApplicationCtrlScope extends ng.IScope {
        getGeneralInformation(): Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.ApplicantsGeneralInformationModel;
        getExtracurricularActivitiesInformation(): Dtos.Applicant.Extracurricular.ExtracurricularActivitiesModel;
        getEssayAndLetterOfRecommendationSummaries(): Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.CollectionsOfEssaysAndLettersOfRecommendationModel;
        getAcademicInformation(): Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.CompletedAcademicInformationModel;


        firstImpressionRating(): void;
        finalOverallRating(): void;

        viewLetterOfRecommendation(index:number): void;

        viewAndRateEssay(index: number): void;

        modalInstance: ng.ui.bootstrap.IModalServiceInstance;

        ratingForModal: Dtos.Common.GenericRatingModel;
        currentLetterOfRecommendationSummary: Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.LetterOfRecommendation.LetterOfRecommendationSummaryModel;
        currentEssaySummary: Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.Essay.EssaySummaryModel;

        getTranscript(): void;
        
        confirmTranscript(): void;
        confirmTranscriptModel: Dtos.ApplicationEvaluator.EvaluatingApplicants.ConfirmTranscriptModel;
        
        getTranscriptConfirmedModel(): Models.TranscriptConfirmed;

        canGotoNextRandomApplicant(): boolean;
        gotoNextRandomApplicant(): void;

        canGotoSummaryOfAllRatings(): boolean;
        gotoSummaryOfAllRatings(): void;
    }

    export interface ICurrentApplicationCtrlRouteProvider extends ng.route.IRouteParamsService {
        applicantsGuid: string;
    }

    export class CurrentApplicationCtrl implements ICurrentApplicationCtrl {
        
        private canGetAllFinalizedApplications;

        static $inject = ['$scope', '$modal', "$routeParams", "CurrentApplicantOrchestrationService"];
        constructor(
            public $scope: ICurrentApplicationCtrlScope,
            private $modal: ng.ui.bootstrap.IModalService,
            private $routeParams: ICurrentApplicationCtrlRouteProvider,
            private currentApplicantOrchestrationService: Services.ICurrentApplicantOrchestrationService) {

            this.initialize();

            $scope.getGeneralInformation = () => {
                return this.currentApplicantOrchestrationService.getGeneralInformation();
            };

            $scope.getExtracurricularActivitiesInformation = () => {
                return this.currentApplicantOrchestrationService.getExtracurricularActivitiesInformation();
            };

            $scope.getEssayAndLetterOfRecommendationSummaries = () => {
                return this.currentApplicantOrchestrationService.getEssayAndLetterOfRecommendationSummaries();
            };

            $scope.getAcademicInformation = () => {
                return this.currentApplicantOrchestrationService.getAcademicInformation();
            };

            $scope.firstImpressionRating = () => {
                this.firstImpressionRating();
            };

            $scope.finalOverallRating = () => {
                this.finalOverallRating();
            };

            $scope.viewLetterOfRecommendation = (index: number) => {
                this.viewLetterOfRecommendation(index);
            };

            $scope.viewAndRateEssay = (index: number) => {
                this.viewAndRateEssay(index);
            };

            $scope.getTranscriptConfirmedModel = () => {
                return this.currentApplicantOrchestrationService.getTranscriptConfirmed();
            };

            $scope.getTranscript = () => {
                this.getTranscript();
            };

            $scope.confirmTranscript = () => {
                this.confirmTranscript();
            };

            $scope.canGotoNextRandomApplicant = () => {
                return this.currentApplicantOrchestrationService.canGotoNextRandomApplicant();
            };

            $scope.gotoNextRandomApplicant = () => {
                if ($scope.canGotoNextRandomApplicant()) {
                    this.currentApplicantOrchestrationService.gotoNextRandomApplicant();
                }
            };

            $scope.gotoSummaryOfAllRatings = () => {
                this.currentApplicantOrchestrationService.gotoSummaryOfAllRatings();
            };

            $scope.canGotoSummaryOfAllRatings = () => {
                return this.currentApplicantOrchestrationService.canGotoSummaryOfAllRatings();
            };
        }

        private initialize() {
            this.canGetAllFinalizedApplications = false;
            this.currentApplicantOrchestrationService.setCurrentApplicantsGuid(this.$routeParams.applicantsGuid);
            this.getGeneralInformationFromServer();
            this.getExtracurricularInformationFromServer();
            this.getEssaysAndLettersOfRecommendationFromServer();
            this.getAcademicInformationFromServer();
        }
        
        private getGeneralInformationFromServer() {
            var resource = this.currentApplicantOrchestrationService.getGeneralInformationFromServer();
            resource.$promise.then(data => {
                this.currentApplicantOrchestrationService.resolveGetGeneralInformationFromServer(this.createServerResponse(data));
                if (this.canGetAllFinalizedApplications == true) {
                    this.getFinalizedApplicants();
                }
            }, () => {
                this.currentApplicantOrchestrationService.resolveGetGeneralInformationFromServer(this.createServerResponse(null));
            });
        }

        private getExtracurricularInformationFromServer() {
            var resource = this.currentApplicantOrchestrationService.getExtracurricularActivitiesFromServer();
            resource.$promise.then(data => {
                this.currentApplicantOrchestrationService.resolveGetExtracurricularActivitiesFromServer(this.createServerResponse(data));
            }, () => {
                this.currentApplicantOrchestrationService.resolveGetExtracurricularActivitiesFromServer(this.createServerResponse(null));
            });
        }

        private getEssaysAndLettersOfRecommendationFromServer() {
            var resource = this.currentApplicantOrchestrationService.getEssayAndLetterOfRecommendationSummariesFromServer();
            resource.$promise.then(data => {
                this.currentApplicantOrchestrationService.resolveGetEssayAndLetterOfRecommendationSummariesFromServer(this.createServerResponse(data));
            }, () => {
                this.currentApplicantOrchestrationService.resolveGetEssayAndLetterOfRecommendationSummariesFromServer(this.createServerResponse(null));
            });; 
        }

        private getAcademicInformationFromServer() {
            var resource = this.currentApplicantOrchestrationService.getAcademicInformationFromServer();
            resource.$promise.then(data => {
                this.currentApplicantOrchestrationService.resolveGetAcademicInformationFromServer(this.createServerResponse(data));
            }, () => {
                    this.currentApplicantOrchestrationService.resolveGetAcademicInformationFromServer(this.createServerResponse(null));
                });
        }

        private createServerResponse(data: any) {
            if (data == null) {
                return new Common.Models.ServerResponseModel(null, false);
            } else {
                return new Common.Models.ServerResponseModel(data, true);
            }
        }

        private firstImpressionRating() {
            this.setRatingForModal(this.currentApplicantOrchestrationService.getGeneralInformation().yourFirstImpressionRating);
            this.$scope.modalInstance = this.$modal.open({
                templateUrl: "/AngularApp/ApplicationEvaluator/EvaluateApplicants/ReviewApplicant/Templates/Modals/AddRatingModal.html",
                controller: "CreateRatingModalCtrl",
                resolve: {
                    genericRatingModel: () => this.$scope.ratingForModal
                }
            });

            this.$scope.modalInstance.result.then(
                ratingFromModal => {
                    this.postFirstImpression(ratingFromModal);
                }, () => { });
        }

        private postFirstImpression(model: Dtos.Common.GenericRatingModel) {
            var resource = this.currentApplicantOrchestrationService.postFirstImpressionRating(model);
            this.resolveNonEssayPostBeginning(resource);
        }

        private finalOverallRating() {
            this.setRatingForModal(this.currentApplicantOrchestrationService.getGeneralInformation().yourFinalOverallRating);
            this.$scope.modalInstance = this.$modal.open({
                templateUrl: "/AngularApp/ApplicationEvaluator/EvaluateApplicants/ReviewApplicant/Templates/Modals/AddRatingModal.html",
                controller: "CreateRatingModalCtrl",
                resolve: {
                    genericRatingModel: () => this.$scope.ratingForModal
                }
            });

            this.$scope.modalInstance.result.then(modalResult => {
                this.postFinalRating(modalResult);
            }, () => { });
        }

        private postFinalRating(modalResult: Dtos.Common.GenericRatingModel) {
            var resource = this.currentApplicantOrchestrationService.postFinalOverallRating(modalResult);
            this.resolvePostFinalRating(resource);
        }

        private resolvePostFinalRating(resource: ng.resource.IResource<any>) {
            resource.$promise.then(() => {
                this.canGetAllFinalizedApplications = true;
                this.resolveNonEssayPostFinal(true);
            }, () => {
                    this.resolveNonEssayPostFinal(false);
                });
        }

        private resolveNonEssayPostBeginning(resource: ng.resource.IResource<any>) {
            resource.$promise.then(() => {
                this.resolveNonEssayPostFinal(true);
            }, () => {
                this.resolveNonEssayPostFinal(false);
            });
        }

        private resolveNonEssayPostFinal(success: boolean) {
            this.currentApplicantOrchestrationService.resolvePostNonEssayRating(new Common.Models.ServerResponseModel(null, success));
            if (success) {
                this.getGeneralInformationFromServer();
            }
        }

        private setRatingForModal(genericRatingModel) {
            if (genericRatingModel == null) {
                genericRatingModel = new Dtos.Common.GenericRatingModel();
            }
            this.$scope.ratingForModal = genericRatingModel;
        }

        private viewLetterOfRecommendation(index: number) {
            this.$scope.currentLetterOfRecommendationSummary = this.currentApplicantOrchestrationService.getEssayAndLetterOfRecommendationSummaries().letterOfRecommendationSummaries[index];

            this.$scope.modalInstance = this.$modal.open({
                templateUrl: "/AngularApp/ApplicationEvaluator/EvaluateApplicants/ReviewApplicant/Templates/Modals/ViewLetterOfRecommendationModal.html",
                controller: "ViewLetterOfRecommendationModalCtrl",
                resolve: {
                    letterOfRecommendationSummary: () => this.$scope.currentLetterOfRecommendationSummary
                },
                size: 'lg'
            });

            this.$scope.modalInstance.result.then(() => {}, () => {});
        }

        private viewAndRateEssay(index: number) {
            this.$scope.currentEssaySummary = this.currentApplicantOrchestrationService.getEssayAndLetterOfRecommendationSummaries().essaySummaries[index];
            
            this.$scope.modalInstance = this.$modal.open({
                templateUrl: "/AngularApp/ApplicationEvaluator/EvaluateApplicants/ReviewApplicant/Templates/Modals/ViewAndRateEssayModal.html",
                controller: "ViewAndRateEssayModalCtrl",
                resolve: {
                    essaySummary: () => this.$scope.currentEssaySummary
                },
                size: 'lg'
            });

            this.$scope.modalInstance.result.then(essayRatingDto => {
                this.addEssayRating(essayRatingDto);
            }, () => {});
        }

        private addEssayRating(essayRatingDto: Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate.EssayRatingModel) {
            var resource = this.currentApplicantOrchestrationService.postEssayRating(essayRatingDto);
            resource.$promise.then(() => {
                this.resolveRateEssay(true);
            }, () => {
                this.resolveRateEssay(false);
            });
        }

        private resolveRateEssay(success: boolean) {
            this.currentApplicantOrchestrationService.resolvePostEssayRating(new Common.Models.ServerResponseModel(null, success));
            if (success) {
                this.getEssaysAndLettersOfRecommendationFromServer();
            }
        }

        private getTranscript() {
            var resource = this.currentApplicantOrchestrationService.getTranscriptFromServer();
            resource.$promise.then(dataFromServer => {
                this.resolveGetTranscript(dataFromServer);
            }, () => { this.resolveGetTranscript(null); });
        }

        private resolveGetTranscript(data) {
            var success = true;
            if (data == null) {
                success = false;
            }
            this.currentApplicantOrchestrationService.resolveGetTranscriptFromServer(new Common.Models.ServerResponseModel(data, success));
        }

        private confirmTranscript() {
            this.$scope.confirmTranscriptModel = new Dtos.ApplicationEvaluator.EvaluatingApplicants.ConfirmTranscriptModel();

            this.$scope.modalInstance = this.$modal.open({
                templateUrl: "/AngularApp/ApplicationEvaluator/EvaluateApplicants/ReviewApplicant/Templates/Modals/ConfirmTranscriptModal.html",
                controller: "ConfirmTranscriptModalCtrl",
                resolve: {
                    confirmTranscriptModel: () => this.$scope.confirmTranscriptModel
                },
            });

            this.$scope.modalInstance.result.then(result => {
                this.postTranscriptConfirmation(result);
            },()=>{});
        }

        private postTranscriptConfirmation(result: Dtos.ApplicationEvaluator.EvaluatingApplicants.ConfirmTranscriptModel) {
            var resouce = this.currentApplicantOrchestrationService.postTranscriptConfirmation(result);
            resouce.$promise.then(() => {
                this.resolvePostTranscriptConfirmation(true);
            }, () => { this.resolvePostTranscriptConfirmation(false); });
        }

        private resolvePostTranscriptConfirmation(success: boolean) {
            this.currentApplicantOrchestrationService.resolvePostTranscriptConfirmation(new Common.Models.ServerResponseModel(null, success));
            if (success) {
                this.getAcademicInformationFromServer();
            }
        }

        private getFinalizedApplicants() {
            var resource = this.currentApplicantOrchestrationService.getFinalizedApplicantsFromServer();
            resource.$promise.then(dataFromServer => {
                this.resolveGetFinalizedApplicants(dataFromServer);
            },() => {
                this.resolveGetFinalizedApplicants(null);
            });
        }

        private resolveGetFinalizedApplicants(dataFromServer) {
            this.canGetAllFinalizedApplications = false;
            var success = true;
            if (dataFromServer == null) {
                success = false;
            }
            this.currentApplicantOrchestrationService.resolveGetFinalizedApplicants(new Common.Models.ServerResponseModel(dataFromServer, success));
        }
    }
} 

module BohFoundation.Main {
    Register.ApplicationEvaluator.controller("CurrentApplicationCtrl", ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Controllers.CurrentApplicationCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/ApplicationEvaluator/ReviewApplicant/:applicantsGuid', {
                    templateUrl: '/AngularApp/ApplicationEvaluator/EvaluateApplicants/ReviewApplicant/Templates/ReviewApplicant.html',
                    controller: 'CurrentApplicationCtrl',
                    publicAccess: false,
                    title: 'Current Application'
                });
            })]);
}