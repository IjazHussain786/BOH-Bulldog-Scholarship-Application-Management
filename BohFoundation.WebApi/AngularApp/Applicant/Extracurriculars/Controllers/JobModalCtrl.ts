module BohFoundation.Applicant.Extracurriculars.Controllers {
    export interface IJobModalCtrl {
        $scope: IJobModalCtrlScope
    }


    export interface IJobModalCtrlScope extends ng.IScope {
        ok(form: ng.IFormController): void;
        cancel(): void;

        jobModel: Dtos.Applicant.Extracurricular.JobModel;
    }

    export class JobModalCtrl implements IJobModalCtrl {
        
        static $inject = ['$scope', '$modalInstance', 'jobModel'];

        constructor(
            public $scope: IJobModalCtrlScope,
            private $modalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private jobModel: Dtos.Applicant.Extracurricular.JobModel) {

            $scope.jobModel = jobModel;

            $scope.cancel = () => {
                this.$modalInstance.dismiss();
            };

            $scope.ok = (form: ng.IFormController) => {
                if (form.$valid) {
                    this.$modalInstance.close(this.$scope.jobModel);
                }
            };
        }
    }
} 

module BohFoundation.Main {
    Register.Applicant.controller("JobModalCtrl", Applicant.Extracurriculars.Controllers.JobModalCtrl);
}