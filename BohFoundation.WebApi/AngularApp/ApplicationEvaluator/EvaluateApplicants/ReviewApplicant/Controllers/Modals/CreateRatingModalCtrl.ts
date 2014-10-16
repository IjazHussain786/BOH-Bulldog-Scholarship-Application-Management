module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Controllers.Modals {
    export interface ICreateRatingModalCtrl {
        $scope: ICreateRatingModalCtrlScope
    }


    export interface ICreateRatingModalCtrlScope extends ng.IScope {
        ok(form: ng.IFormController): void;
        cancel(): void;

        getRatingsArray(): Array<Common.Enums.RatingEnum>;

        genericRatingModel: Dtos.Common.GenericRatingModel;
    }

    export class CreateRatingModalCtrl implements ICreateRatingModalCtrl {

        private ratingsArray = new Models.RatingsArray();

        static $inject = ['$scope', '$modalInstance', 'genericRatingModel'];
        constructor(
            public $scope: ICreateRatingModalCtrlScope,
            private $modalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private genericRatingModel: Dtos.Common.GenericRatingModel) {

            $scope.genericRatingModel = genericRatingModel;

            $scope.getRatingsArray = () => {
                return this.ratingsArray.array;
            };

            $scope.cancel = () => {
                $modalInstance.dismiss();
            };

            $scope.ok = (form: ng.IFormController) => {
                this.ok(form);
            };
        }

        private ok(formController: ng.IFormController) {
            if (formController.$valid) {
                this.$modalInstance.close(this.$scope.genericRatingModel);
            }
        }
    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.controller("CreateRatingModalCtrl", ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Controllers.Modals.CreateRatingModalCtrl);
}