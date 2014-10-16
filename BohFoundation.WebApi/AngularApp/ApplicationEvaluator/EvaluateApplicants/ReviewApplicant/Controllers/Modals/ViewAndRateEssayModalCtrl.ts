module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Controllers.Modals {
    export interface IViewAndRateEssayModalCtrl {
        $scope: IViewAndRateEssayModalCtrlScope
    }


    export interface IViewAndRateEssayModalCtrlScope extends ng.IScope {

        getRatingsArray(): Array<Common.Enums.RatingEnum>;

        ok(form: ng.IFormController): void;
        cancel(): void;

        getEssay(): string;
        essaySummary: Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.Essay.EssaySummaryModel;
    }

    export class ViewAndRateEssayModalCtrl implements IViewAndRateEssayModalCtrl {
        static $inject = ['$scope', '$modalInstance', 'essaySummary', 'GetEssayForEvaluatorService'];
        constructor(
            public $scope: IViewAndRateEssayModalCtrlScope,
            private $modalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private essaySummary: Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.Essay.EssaySummaryModel,
            private getEssayForEvaluatorService: Services.GetEssayAndLettersOfRecommendation.IGetEssayForEvaluatorService) {

            this.$scope.essaySummary = essaySummary;

            this.initialize();

            $scope.getEssay = () => {
                return this.getEssayForEvaluatorService.getEssay();
            };

            $scope.cancel = () => {
                $modalInstance.dismiss();
            };

            $scope.ok = (form: ng.IFormController) => {
                if (form.$valid == true) {
                    $modalInstance.close(new Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate.EssayRatingModel(this.$scope.essaySummary.essayTopicId, this.$scope.essaySummary.yourRating));
                }
            };

            $scope.getRatingsArray = () => {
                return new Models.RatingsArray().array;
            };
        }

        private initialize() {
            var resource = this.getEssayForEvaluatorService.getEssayFromServer(this.essaySummary.essayKeyValues);
            resource.$promise.then(dataFromServer => { this.resolve(dataFromServer); }, () => { this.resolve(null); });
        }

        private resolve(dataFromServer) {
            var success = true;
            if (dataFromServer == null) {
                success = false;
            }
            this.getEssayForEvaluatorService.resolveEssayFromServer(new Common.Models.ServerResponseModel(dataFromServer, success));
        }
    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.controller("ViewAndRateEssayModalCtrl", ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Controllers.Modals.ViewAndRateEssayModalCtrl);
}  