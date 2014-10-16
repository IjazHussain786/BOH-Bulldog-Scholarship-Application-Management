module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services {
    export interface ICurrentApplicantExtracurricularActivitiesService {

        getExtracurricularActivitiesInformation(): Dtos.Applicant.Extracurricular.ExtracurricularActivitiesModel;
        getExtracurricularActivitiesFromServer(applicantsGuid: string): ng.resource.IResource<any>;
        resolveGetExtracurricularActivitiesFromServer(serverMessage: Common.Models.ServerResponseModel): void;
    }

    export class CurrentApplicantExtracurricularActivitiesService implements ICurrentApplicantExtracurricularActivitiesService {
        static $inject = ['AuthRequiredRepository', 'GenericResolver'];
        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }


        private extracurricularActivitiesModel: Dtos.Applicant.Extracurricular.ExtracurricularActivitiesModel;
        private extracurricularActivitiesInformationApi = "/api/applicationevaluator/evaluatingapplicants/displayapplication/extracurricularactivities/";

        getExtracurricularActivitiesInformation(): Dtos.Applicant.Extracurricular.ExtracurricularActivitiesModel {
            return this.extracurricularActivitiesModel;
        }

        getExtracurricularActivitiesFromServer(applicantsGuid: string): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.extracurricularActivitiesInformationApi + applicantsGuid);
        }

        resolveGetExtracurricularActivitiesFromServer(serverMessage: Common.Models.ServerResponseModel): void {
            this.extracurricularActivitiesModel = this.genericResolver.genericGetResolver("applicant's extracurricular activities information", serverMessage);
        }
    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.service("CurrentApplicantExtracurricularActivitiesService", ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.CurrentApplicantExtracurricularActivitiesService);
}