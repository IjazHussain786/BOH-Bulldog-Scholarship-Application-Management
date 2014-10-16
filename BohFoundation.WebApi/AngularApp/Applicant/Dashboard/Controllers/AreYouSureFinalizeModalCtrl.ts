module BohFoundation.Applicant.Dashboard.Controllers {
    export interface IAreYouSureFinalizeModalCtrl {
        $scope: IAreYouSureFinalizeModalCtrlScope;
    }

    export interface IAreYouSureFinalizeModalCtrlScope extends ng.IScope {
        ok(): void;
        cancel(): void;
    }

    export class AreYouSureFinalizeModalCtrl implements IAreYouSureFinalizeModalCtrl {
        
        static $inject = ['$scope', '$modalInstance'];

        constructor(
            public $scope: IAreYouSureFinalizeModalCtrlScope,
            private $modalInstance: ng.ui.bootstrap.IModalServiceInstance) {
            $scope.ok = () => {
                $modalInstance.close();
            };

            $scope.cancel = () => {
                $modalInstance.dismiss();
            };
        }
    }
} 

module BohFoundation.Main {
    Register.Applicant.controller("AreYouSureFinalizeModalCtrl", Applicant.Dashboard.Controllers.AreYouSureFinalizeModalCtrl);
}