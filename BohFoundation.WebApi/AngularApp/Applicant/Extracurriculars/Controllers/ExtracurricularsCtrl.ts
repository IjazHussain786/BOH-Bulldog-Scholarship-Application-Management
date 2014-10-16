module BohFoundation.Applicant.Extracurriculars.Controllers {
    export interface IExtracurricularsCtrl {
        $scope: IExtracurricularsCtrlScope
    }


    export interface IExtracurricularsCtrlScope extends ng.IScope {
        processing: boolean;
        successfullySaved: boolean;

        getChangedButNotSaved(): boolean;

        currentJobModel: Dtos.Applicant.Extracurricular.JobModel;
        currentActivityModel: Dtos.Applicant.Extracurricular.ActivityModel;
        modalInstance: ng.ui.bootstrap.IModalServiceInstance;

        getExtracurricularActivitiesModel(): Dtos.Applicant.Extracurricular.ExtracurricularActivitiesModel;

        ableToSave(): boolean;

        addJob(): void;
        editJob(index: number): void;
        deleteJob(index: number): void;

        addActivity(): void;
        editActivity(index: number): void;
        deleteActivity(index: number): void;

        saveExtracurriculars(): void;
    }

    export class ExtracurricularsCtrl implements IExtracurricularsCtrl {
        
        static $inject = ['$scope', '$modal', 'ExtracurricularsService'];
        
        constructor(
            public $scope: IExtracurricularsCtrlScope,
            private $modal: ng.ui.bootstrap.IModalService,
            private extracurricularsService: Services.IExtracurricularsService) {

            this.initialize();

            $scope.getChangedButNotSaved = () => {
                return this.extracurricularsService.getChangedButNotSaved();
            };

            $scope.getExtracurricularActivitiesModel = () => {
                return this.getExtracurricularActivitiesModel();
            };

            $scope.ableToSave = () => {
                return this.ableToSave();
            };

            $scope.saveExtracurriculars = () => {
                if (this.ableToSave()) {
                    this.saveExtracurriculars();
                }     
            };

            $scope.addActivity = () => {
                if (!$scope.processing) {
                    this.addActivity();
                }
            };

            $scope.editActivity = (index: number) => {
                if (!$scope.processing) {
                    this.editActivity(index);
                }
            };

            $scope.deleteActivity = (index: number) => {
                this.extracurricularsService.deleteActivity(index);
            };
            
            $scope.addJob = () => {
                if (!$scope.processing) {
                    this.addJob();
                }
            };

            $scope.editJob = (index: number) => {
                if (!$scope.processing) {
                    this.editJob(index);
                }
            };

            $scope.deleteJob = (index: number) => {
                this.extracurricularsService.deleteJob(index);
            };
        }

        private initialize() {
            this.$scope.processing = true;
            var resource = this.extracurricularsService.getExtracurricularsFromServer();
            resource.$promise.then( responseFromServer => {
                this.flipProcessing();
                this.extracurricularsService.resolveGetExtracurricularsFromServer(new Common.Models.ServerResponseModel(responseFromServer, true));
            }, () => {
                this.extracurricularsService.resolveGetExtracurricularsFromServer(new Common.Models.ServerResponseModel(null, false));
            });
        }

        private flipProcessing() {
            this.$scope.processing = !this.$scope.processing;
        }

        private getExtracurricularActivitiesModel() {
            return this.extracurricularsService.getExtracurricularActivitiesModel();
        }

        private ableToSave(): boolean {
            if (this.$scope.processing || this.jobInvalid() || this.activitiesInvalid()) {
                return false;
            } else {
                return true;
            }
        }

        private jobInvalid() {
            var model = this.getExtracurricularActivitiesModel();
            if (model.paidWork) {
                if (model.jobs == undefined || model.jobs.length < 1) {
                    return true;
                }
            }
            return false;
        }

        private activitiesInvalid(): boolean {
            var model = this.getExtracurricularActivitiesModel();
            if (model.hasNonPaidActivities) {
                if (model.activities == undefined || model.activities.length < 1) {
                    return true;
                }
            }
            return false;
        }

        private saveExtracurriculars() {
            this.$scope.processing = true;
            var resource = this.extracurricularsService.postExtracurriculars(this.getExtracurricularActivitiesModel());
            resource.$promise.then(() => {
                this.$scope.successfullySaved = true;
                this.resolvePost(true);
            }, () => {
                this.resolvePost(false);
            });
        }

        private resolvePost(success: boolean) {
            this.extracurricularsService.resolvePostExtracurriculars(new Common.Models.ServerResponseModel(null, success));
            this.flipProcessing();
        }

        private addJob() {
            this.$scope.currentJobModel = new Dtos.Applicant.Extracurricular.JobModel();
            this.openJobModal(false);
        }

        private editJob(index: number) {
            this.$scope.currentJobModel = this.extracurricularsService.getExtracurricularActivitiesModel().jobs[index];
            this.openJobModal(true, index);
        }

        private openJobModal(edit:boolean, editIndex?:number) {
            this.$scope.modalInstance = this.$modal.open({
                templateUrl: "/AngularApp/Applicant/Extracurriculars/Templates/JobModal.html",
                controller: "JobModalCtrl",
                resolve: {
                    jobModel: () => this.$scope.currentJobModel
                }
            });

            this.$scope.modalInstance.result.then(
                modelFromModal => {
                    if (edit) {
                        this.extracurricularsService.editJob(modelFromModal, editIndex);
                    } else {
                        this.extracurricularsService.addJob(modelFromModal);
                    }
                },
                () => {

                });
        }

        private addActivity() {
            this.$scope.currentActivityModel = new Dtos.Applicant.Extracurricular.ActivityModel();
            this.openActivityModal(false);
        }

        private editActivity(index: number) {
            this.$scope.currentActivityModel = this.extracurricularsService.getExtracurricularActivitiesModel().activities[index];
            this.openActivityModal(true, index);
        }

        private openActivityModal(edit: boolean, editIndex?: number) {
            this.$scope.modalInstance = this.$modal.open({
                templateUrl: "/AngularApp/Applicant/Extracurriculars/Templates/ActivityModal.html",
                controller: "ActivityModalCtrl",
                resolve: {
                    activityModel: () => this.$scope.currentActivityModel
                }
            });

            this.$scope.modalInstance.result.then(modelFromModal => {
                if (edit) {
                    this.extracurricularsService.editActivity(modelFromModal, editIndex);
                } else {
                    this.extracurricularsService.addActivity(modelFromModal);
                }
            }, () => {});
        }
    }
} 

module BohFoundation.Main {
    Register.Applicant.controller("ExtracurricularsCtrl", Applicant.Extracurriculars.Controllers.ExtracurricularsCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/Applicant/Extracurriculars', {
                    templateUrl: '/AngularApp/Applicant/Extracurriculars/Templates/ExtracurricularsTemplate.html',
                    controller: 'ExtracurricularsCtrl',
                    publicAccess: false,
                    title: 'Extracurriculars'
                });
            })]);
}