module BohFoundation.Applicant.References.Controllers {
    export interface IApplicantReferenceModalCtrlScope {
        refernceInputModelInModal: Dtos.Applicant.References.ApplicantReferenceInputModel;

        ok(form: ng.IFormController): void;
        cancel(): void;
    }

    export interface IApplicantReferenceModalCtrl {
        $scope: IApplicantReferenceModalCtrlScope;
    }

    export class ApplicantReferenceModalCtrl implements IApplicantReferenceModalCtrl{
        
        static $inject = ['$scope', '$modalInstance', 'refernceInputModelForModal'];

        constructor(
            public $scope: IApplicantReferenceModalCtrlScope,
            private $modalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private refernceInputModelForModal: Dtos.Applicant.References.ApplicantReferenceInputModel) {

            $scope.refernceInputModelInModal = refernceInputModelForModal;

            $scope.cancel = () => {
                $modalInstance.dismiss();
            };

            $scope.ok = (form: ng.IFormController) => {
                if (form.$valid) {
                    $modalInstance.close($scope.refernceInputModelInModal);
                }
            };
        }

    }
}

module BohFoundation.Main {
    Register.Applicant.controller("ApplicantReferenceModalCtrl", Applicant.References.Controllers.ApplicantReferenceModalCtrl);
}