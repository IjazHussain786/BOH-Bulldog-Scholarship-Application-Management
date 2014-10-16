module BohFoundation.Applicant.Extracurriculars.Services {
    export interface IExtracurricularsService {
        getExtracurricularActivitiesModel(): Dtos.Applicant.Extracurricular.ExtracurricularActivitiesModel;

        getExtracurricularsFromServer(): ng.resource.IResource<any>;
        resolveGetExtracurricularsFromServer(serverResponse: Common.Models.ServerResponseModel): void;

        getChangedButNotSaved():boolean;

        postExtracurriculars(extracurriculars: Dtos.Applicant.Extracurricular.ExtracurricularActivitiesModel): ng.resource.IResource<any>;
        resolvePostExtracurriculars(serverResponse: Common.Models.ServerResponseModel): void;

        addActivity(model: Dtos.Applicant.Extracurricular.ActivityModel): void;
        editActivity(model: Dtos.Applicant.Extracurricular.ActivityModel, index: number): void;
        deleteActivity(index: number): void;

        addJob(model: Dtos.Applicant.Extracurricular.JobModel): void;
        editJob(model: Dtos.Applicant.Extracurricular.JobModel, index: number): void;
        deleteJob(index: number): void;
    }

    export class ExtracurricularsService implements IExtracurricularsService {
        private apiEndpoint = "/api/applicant/extracurriculars";
        private typeOfData = "extracurricular activities";
        private extracurricularActivitiesModel: Dtos.Applicant.Extracurricular.ExtracurricularActivitiesModel;
        private changedButNotSaved: boolean = false;

        static $inject = ['AuthRequiredRepository', 'GenericResolver'];

        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }

        getChangedButNotSaved(): boolean {
            return this.changedButNotSaved;
        }

        getExtracurricularActivitiesModel() {
            return this.extracurricularActivitiesModel;
        }

        getExtracurricularsFromServer() {
            return this.authRequiredRepository.get(this.apiEndpoint);
        }

        resolveGetExtracurricularsFromServer(serverResponse: Common.Models.ServerResponseModel): void {
            var modelFromServer = this.genericResolver.genericGetResolver(this.typeOfData, serverResponse);
            if (modelFromServer == null) {
                this.extracurricularActivitiesModel = new Dtos.Applicant.Extracurricular.ExtracurricularActivitiesModel(false,false);
            } else {
                this.extracurricularActivitiesModel = modelFromServer;
            }
        }

        postExtracurriculars(extracurriculars: Dtos.Applicant.Extracurricular.ExtracurricularActivitiesModel) {
            return this.authRequiredRepository.post(extracurriculars, this.apiEndpoint);
        }

        resolvePostExtracurriculars(serverResponse: Common.Models.ServerResponseModel): void {
            this.genericResolver.genericPostResolver(this.typeOfData, serverResponse);
            if (serverResponse.success) {
                this.changedButNotSaved = false;
            }
        }

        addActivity(model: Dtos.Applicant.Extracurricular.ActivityModel) {
            this.setChangedButNotSavedToTrue();
            if (this.extracurricularActivitiesModel.activities == undefined) {
                this.extracurricularActivitiesModel.activities = [];
            }
            this.extracurricularActivitiesModel.activities.push(model);
        }

        editActivity(model: Dtos.Applicant.Extracurricular.ActivityModel, index: number): void {
            this.setChangedButNotSavedToTrue();
            this.extracurricularActivitiesModel.activities[index] = model;
        }

        deleteActivity(index: number): void {
            this.setChangedButNotSavedToTrue();
            this.extracurricularActivitiesModel.activities.splice(index,1);
        }

        addJob(model: Dtos.Applicant.Extracurricular.JobModel): void {
            this.setChangedButNotSavedToTrue();
            if (this.extracurricularActivitiesModel.jobs == undefined) {
                this.extracurricularActivitiesModel.jobs = [];
            }
            this.extracurricularActivitiesModel.jobs.push(model);
        }

        editJob(model: Dtos.Applicant.Extracurricular.JobModel, index: number): void {
            this.setChangedButNotSavedToTrue();
            this.extracurricularActivitiesModel.jobs[index] = model; 
        }

        deleteJob(index: number): void {
            this.setChangedButNotSavedToTrue();
            this.extracurricularActivitiesModel.jobs.splice(index, 1);
        }

        private setChangedButNotSavedToTrue() {
            this.changedButNotSaved = true;
        }
    }
} 

module BohFoundation.Main {
    Register.Applicant.service("ExtracurricularsService", Applicant.Extracurriculars.Services.ExtracurricularsService);
}