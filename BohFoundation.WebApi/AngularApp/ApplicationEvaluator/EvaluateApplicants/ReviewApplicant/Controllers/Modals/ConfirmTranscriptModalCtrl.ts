module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Controllers.Modals {
    export interface IConfirmTranscriptModalCtrl {
        $scope: IConfirmTranscriptModalCtrlScope
    }


    export interface IConfirmTranscriptModalCtrlScope extends ng.IScope {
        ok(confirmed: boolean): void;
        cancel(): void;

        getRatingsArray(): Array<Common.Enums.RatingEnum>;

        confirmTranscriptModel: Dtos.ApplicationEvaluator.EvaluatingApplicants.ConfirmTranscriptModel;
    }

    export class ConfirmTranscriptModalCtrl implements IConfirmTranscriptModalCtrl {

        private ratingsArray = new Models.RatingsArray();

        static $inject = ['$scope', '$modalInstance', 'confirmTranscriptModel'];
        constructor(
            public $scope: IConfirmTranscriptModalCtrlScope,
            private $modalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private confirmTranscriptModel: Dtos.ApplicationEvaluator.EvaluatingApplicants.ConfirmTranscriptModel) {

            $scope.confirmTranscriptModel = confirmTranscriptModel;

            $scope.getRatingsArray = () => {
                return this.ratingsArray.array;
            };

            $scope.cancel = () => {
                $modalInstance.dismiss();
            };

            $scope.ok = (confirmed: boolean) => {
                this.ok(confirmed);
            };
        }

        private ok(confirmed: boolean) {
            this.$scope.confirmTranscriptModel.informationMatchesTranscriptPdf = confirmed;
            this.$modalInstance.close(this.$scope.confirmTranscriptModel);
        }
    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.controller("ConfirmTranscriptModalCtrl", ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Controllers.Modals.ConfirmTranscriptModalCtrl);
}