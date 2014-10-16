module BohFoundation.Applicant.Extracurriculars.Controllers {
    export interface IActivityModalCtrl {
        $scope: IActivityModalCtrlScope
    }


    export interface IActivityModalCtrlScope extends ng.IScope {
        ok(form: ng.IFormController): void;
        cancel(): void;

        activityModel: Dtos.Applicant.Extracurricular.ActivityModel;
    }

    export class ActivityModalCtrl implements IActivityModalCtrl {
        static $inject = ['$scope', '$modalInstance', 'activityModel'];

        constructor(
            public $scope: IActivityModalCtrlScope,
            private $modalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private activityModel: Dtos.Applicant.Extracurricular.ActivityModel) {

            $scope.activityModel = activityModel;

            $scope.cancel = () => {
                this.$modalInstance.dismiss();
            };

            $scope.ok = (form: ng.IFormController) => {
                if (form.$valid) {
                    this.$modalInstance.close(this.$scope.activityModel);
                }
            };
        }
    }
}

module BohFoundation.Main {
    Register.Applicant.controller("ActivityModalCtrl", Applicant.Extracurriculars.Controllers.ActivityModalCtrl);
}