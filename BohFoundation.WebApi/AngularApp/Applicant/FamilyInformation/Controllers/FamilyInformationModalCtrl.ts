module BohFoundation.Applicant.FamilyInformation.Controllers {
    export interface IFamilyInformationModalCtrl {
        $scope: IFamilyInformationModalCtrlScope;
    }

    export interface IFamilyInformationModalCtrlScope extends ng.IScope {
        ok(form: ng.IFormController): void;
        cancel(): void;

        yearlyHouseholdIncomeRange: Array<Common.Enums.IncomeRangeEnum>;
        highestAttainedDegreeInHome: Array<Common.Enums.EducationalDegreesEnum>;

        familyInformationModel: Dtos.Applicant.Family.FamilyInformationModel;
    }

    export class FamilyInformationModalCtrl implements IFamilyInformationModalCtrl {
        
        static $inject = ['$scope', '$modalInstance', 'familyInformationModel'];

        constructor(
            public $scope: IFamilyInformationModalCtrlScope,
            private $modalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private familyInformationModel: Dtos.Applicant.Family.FamilyInformationModel) {

            this.initialize(familyInformationModel);

            $scope.cancel = () => {
                $modalInstance.dismiss();
            };

            $scope.ok = (form: ng.IFormController) => {
                if (form.$valid) {
                    $modalInstance.close($scope.familyInformationModel);
                }
            };

            $scope.highestAttainedDegreeInHome = [
                Common.Enums.EducationalDegreesEnum.NoDegrees,
                Common.Enums.EducationalDegreesEnum.HighSchool,
                Common.Enums.EducationalDegreesEnum.Associates,
                Common.Enums.EducationalDegreesEnum.Bachelors,
                Common.Enums.EducationalDegreesEnum.Masters,
                Common.Enums.EducationalDegreesEnum.Doctorate
            ];

            $scope.yearlyHouseholdIncomeRange = [
                Common.Enums.IncomeRangeEnum.Lt20000,
                Common.Enums.IncomeRangeEnum.Mt20000Lt30000,
                Common.Enums.IncomeRangeEnum.Mt30000Lt40000,
                Common.Enums.IncomeRangeEnum.Mt40000Lt50000,
                Common.Enums.IncomeRangeEnum.Mt50000Lt60000,
                Common.Enums.IncomeRangeEnum.Mt60000Lt70000,
                Common.Enums.IncomeRangeEnum.Mt70000Lt80000,
                Common.Enums.IncomeRangeEnum.Mt80000Lt90000, 
                Common.Enums.IncomeRangeEnum.Mt90000Lt100000,
                Common.Enums.IncomeRangeEnum.Mt100000
            ];
        }

        private initialize(familyInformationModel: Dtos.Applicant.Family.FamilyInformationModel) {
            if (familyInformationModel == null) {
                this.$scope.familyInformationModel = new Dtos.Applicant.Family.FamilyInformationModel();
            } else {
                this.$scope.familyInformationModel = familyInformationModel;
            }
        }
    }
    
} 

module BohFoundation.Main {
    Register.Applicant.controller("FamilyInformationModalCtrl", Applicant.FamilyInformation.Controllers.FamilyInformationModalCtrl);
}