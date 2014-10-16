module BohFoundation.Admin.LetterOfRecommendation.Controllers {
    export interface ICreateGetLetterOfRecommendationGuidModelModalCtrl {
        $scope: ICreateGetLetterOfRecommendationGuidModelModalCtrlScope
    }


    export interface ICreateGetLetterOfRecommendationGuidModelModalCtrlScope extends ng.IScope {
        getLetterOfRecommendationGuidModel: Dtos.Admin.References.GetLetterOfRecommendationGuidModel;

        cancel(): void;
        ok(form:ng.IFormController): void;
    }

    export class CreateGetLetterOfRecommendationGuidModelModalCtrl implements ICreateGetLetterOfRecommendationGuidModelModalCtrl {
        
        static $inject = ['$scope', '$modalInstance','getLetterOfRecommendationGuidModel'];

        constructor(
            public $scope: ICreateGetLetterOfRecommendationGuidModelModalCtrlScope,
            private $modalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private getLetterOfRecommendationGuidModel: Dtos.Admin.References.GetLetterOfRecommendationGuidModel) {

            $scope.getLetterOfRecommendationGuidModel = getLetterOfRecommendationGuidModel;

            $scope.cancel = () => {
                $modalInstance.dismiss();
            };

            $scope.ok = (form: ng.IFormController) => {
                if (form.$valid) {
                    $modalInstance.close($scope.getLetterOfRecommendationGuidModel);
                };
            };
        }

    }
    
} 

module BohFoundation.Main {
    Register.Admin.controller("CreateGetLetterOfRecommendationGuidModelModalCtrl", Admin.LetterOfRecommendation.Controllers.CreateGetLetterOfRecommendationGuidModelModalCtrl);
}