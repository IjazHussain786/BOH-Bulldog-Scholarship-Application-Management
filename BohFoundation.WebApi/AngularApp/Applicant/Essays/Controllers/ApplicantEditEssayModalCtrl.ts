module BohFoundation.Applicant.Essays.Controllers {
    export interface IApplicantEditEssayModalCtrlScope extends ng.IScope {
        essayModelInModal: Dtos.Applicant.Essay.EssayModel;

        ok(): void;
        cancel(): void;
    }

    export interface IApplicantEditEssayModalCtrl {
        $scope : IApplicantEditEssayModalCtrlScope;
    }

    export class ApplicantEditEssayModalCtrl implements IApplicantEditEssayModalCtrl {
        static $inject = ['$scope','$modalInstance', 'essayModelForModal'];

        constructor(public $scope: IApplicantEditEssayModalCtrlScope,
            private $modalInstance,
            private essayModelForModal: Dtos.Applicant.Essay.EssayModel) {

            $scope.essayModelInModal = essayModelForModal;

            $scope.cancel = () => {
                $modalInstance.dismiss();
            };

            $scope.ok = () => {
                if (essayModelForModal.essay.length < 4000) {
                    $modalInstance.close($scope.essayModelInModal);
                }
            }
        }
    }
}

module BohFoundation.Main {
    Register.Applicant.controller("ApplicantEditEssayModalCtrl", Applicant.Essays.Controllers.ApplicantEditEssayModalCtrl);
}