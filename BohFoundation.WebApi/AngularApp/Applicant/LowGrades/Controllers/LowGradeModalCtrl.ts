module BohFoundation.Applicant.LowGrades.Controllers {
    export interface ILowGradeModalCtrlScope extends ng.IScope {
        lowGradeModel: Dtos.Applicant.Academic.LowGradeModel;

        grades: Array<string>;
        yearsOfHighSchool: Array<Common.Enums.YearOfHighSchool>;

        ok(form: ng.IFormController): void;
        cancel(): void;
    }

    export interface ILowGradeModalCtrl {
        $scope: ILowGradeModalCtrlScope;
    }

    export class LowGradeModalCtrl implements ILowGradeModalCtrl {

        static $inject = ['$scope', '$modalInstance', 'lowGradeModel'];
        constructor(public $scope: ILowGradeModalCtrlScope,
            private $modalInstance,
            private lowGradeModel: Dtos.Applicant.Academic.LowGradeModel) {

            $scope.lowGradeModel = lowGradeModel;

            $scope.grades = ["A", "A-", "B+", "B", "B-", "C+", "C", "C-", "D+", "D", "D-", "F"];

            $scope.yearsOfHighSchool = [
                Common.Enums.YearOfHighSchool.Freshman,
                Common.Enums.YearOfHighSchool.Sophomore,
                Common.Enums.YearOfHighSchool.Junior,
                Common.Enums.YearOfHighSchool.Senior
            ];

            this.setDefaults();

            $scope.ok = (form: ng.IFormController) => {
                if (form.$valid) {
                    $modalInstance.close($scope.lowGradeModel);
                }
            };

            $scope.cancel = () => {
                $modalInstance.dismiss();
            };
        }

        private setDefaults() {
            if (this.$scope.lowGradeModel.grade == undefined) {
                this.$scope.lowGradeModel.grade = this.$scope.grades[0]; 
            }
            if (this.$scope.lowGradeModel.yearOfHighSchool == undefined) {
                this.$scope.lowGradeModel.yearOfHighSchool = this.$scope.yearsOfHighSchool[0]; 
            }
        }
    }
}

module BohFoundation.Main {
    Register.Applicant.controller('LowGradeModalCtrl', Applicant.LowGrades.Controllers.LowGradeModalCtrl);
}