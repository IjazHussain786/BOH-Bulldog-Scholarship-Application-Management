module BohFoundation.Reference.LetterOfRecommendation.Anon.Controllers {
    export interface IAnonReferencePersonalInfoModalCtrl {
        $scope: IAnonReferencePersonalInfoModalCtrlScope;    
    }

    export interface IAnonReferencePersonalInfoModalCtrlScope extends ng.IScope {
        referencePersonalInformationModel: Dtos.Reference.Anonymous.ReferencePersonalInformationModel;
        bestTimeToContactByPhone: Array<Common.Enums.TimeOfDayEnum>;

        ok(form: ng.IFormController): void;
        cancel(): void;
    }

    export class AnonReferencePersonalInfoModalCtrl implements IAnonReferencePersonalInfoModalCtrl{
        
        static $inject = ['$scope', '$modalInstance', 'referencePersonalInformationModel'];

        constructor(
            public $scope: IAnonReferencePersonalInfoModalCtrlScope,
            private $modalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private referencePersonalInformationModel: Dtos.Reference.Anonymous.ReferencePersonalInformationModel) {

            this.$scope.bestTimeToContactByPhone =
            [
                Common.Enums.TimeOfDayEnum.Morning,
                Common.Enums.TimeOfDayEnum.Noon,
                Common.Enums.TimeOfDayEnum.Afternoon,
                Common.Enums.TimeOfDayEnum.Evening,
                Common.Enums.TimeOfDayEnum.Anytime
            ];

            $scope.referencePersonalInformationModel = referencePersonalInformationModel;

            $scope.cancel = () => {
                $modalInstance.dismiss();
            };

            $scope.ok = (form: ng.IFormController) => {
                if (form.$valid) {
                    $modalInstance.close(this.$scope.referencePersonalInformationModel);
                }
            };
        }
    }
}

module BohFoundation.Main {
    Register.Reference.controller('AnonReferencePersonalInfoModalCtrl', Reference.LetterOfRecommendation.Anon.Controllers.AnonReferencePersonalInfoModalCtrl);
}