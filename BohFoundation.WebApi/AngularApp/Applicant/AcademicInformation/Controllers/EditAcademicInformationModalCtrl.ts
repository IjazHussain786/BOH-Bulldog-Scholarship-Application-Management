module BohFoundation.Applicant.AcademicInformation.Controllers {
    export interface IEditAcademicInformationModalCtrl {
        $scope: IEditAcademicInformationModalCtrlScope;
    }

    export interface IEditAcademicInformationModalCtrlScope extends ng.IScope{
        ok(form: ng.IFormController): void;
        cancel(): void;

        academicInformationModel: Dtos.Applicant.Academic.AcademicInformationModel;
    }

    export class EditAcademicInformationModalCtrl implements IEditAcademicInformationModalCtrl {
        
        static $inject = ['$scope', '$modalInstance', 'academicInformationModel'];

        constructor(
            public $scope: IEditAcademicInformationModalCtrlScope,
            private $modalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private academicInformationModel: Dtos.Applicant.Academic.AcademicInformationModel) {

            $scope.academicInformationModel = academicInformationModel;

            $scope.cancel = () => {
                $modalInstance.dismiss();
            };

            $scope.ok = (form: ng.IFormController) => {
                this.ok(form);
            };
        }

        private ok(formController: ng.IFormController) {
            if (formController.$valid) {
                this.$modalInstance.close(this.$scope.academicInformationModel);
            }
        }
    }
}
module BohFoundation.Main {
    Register.Applicant.controller("EditAcademicInformationModalCtrl", Applicant.AcademicInformation.Controllers.EditAcademicInformationModalCtrl);
}