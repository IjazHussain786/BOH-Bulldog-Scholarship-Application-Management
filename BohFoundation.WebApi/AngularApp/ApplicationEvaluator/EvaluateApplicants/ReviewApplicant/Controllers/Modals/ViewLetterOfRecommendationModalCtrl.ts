module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Controllers.Modals {
    export interface IViewLetterOfRecommendationModalCtrl {
        $scope: IViewLetterOfRecommendationModalCtrlScope
    }


    export interface IViewLetterOfRecommendationModalCtrlScope extends ng.IScope {

        cancel(): void;

        getLetterOfRecommendation(): string;
        letterOfRecommendationSummary: Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.LetterOfRecommendation.LetterOfRecommendationSummaryModel;
    }

    export class ViewLetterOfRecommendationModalCtrl implements IViewLetterOfRecommendationModalCtrl {
        static $inject = ['$scope', '$modalInstance', 'letterOfRecommendationSummary', 'GetLetterOfRecommendationForEvaluatorService'];
        constructor(
            public $scope: IViewLetterOfRecommendationModalCtrlScope,
            private $modalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private letterOfRecommendationSummary: Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.LetterOfRecommendation.LetterOfRecommendationSummaryModel,
            private getLetterOfRecommendationForEvaluatorService: Services.GetEssayAndLettersOfRecommendation.IGetLetterOfRecommendationForEvaluatorService) {

            $scope.letterOfRecommendationSummary = letterOfRecommendationSummary;

            this.initialize();

            $scope.cancel = () => {
                $modalInstance.dismiss();
            };

            $scope.getLetterOfRecommendation = () => {
                return this.getLetterOfRecommendationForEvaluatorService.getLetterOfRecommedation();
            };
        }

        private initialize() {
            var resource = this.getLetterOfRecommendationForEvaluatorService.getLetterOfRecommendationFromServer(this.letterOfRecommendationSummary.letterOfRecommendationKeyValues);
            resource.$promise.then(response => { this.resolve(response); }, () => { this.resolve(null); });
        }

        private resolve(dataFromServer) {
            var success = true;
            if (dataFromServer == null) {
                success = false;
            }
            this.getLetterOfRecommendationForEvaluatorService.resolveGetLetterOfRecommendation(new Common.Models.ServerResponseModel(dataFromServer, success));
        }
    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.controller("ViewLetterOfRecommendationModalCtrl", ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Controllers.Modals.ViewLetterOfRecommendationModalCtrl);
} 