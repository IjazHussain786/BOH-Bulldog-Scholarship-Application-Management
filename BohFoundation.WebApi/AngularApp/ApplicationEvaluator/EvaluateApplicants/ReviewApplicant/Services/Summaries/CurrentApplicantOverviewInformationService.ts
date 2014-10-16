module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services {
    export interface ICurrentApplicantOverviewInformationService {

        getGeneralInformation(): Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.ApplicantsGeneralInformationModel;
        getGeneralInformationFromServer(applicantsGuid: string): ng.resource.IResource<any>; 
        resolveGetGeneralInformationFromServer(serverMessage: Common.Models.ServerResponseModel): void;
    }

    export class CurrentApplicantOverviewInformationService implements ICurrentApplicantOverviewInformationService {
        static $inject = ['AuthRequiredRepository', 'GenericResolver'];
        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }


        private generalInformationModel: Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.ApplicantsGeneralInformationModel;
        private generalInformationApi = "/api/applicationevaluator/evaluatingapplicants/displayapplication/generalinformation/";

        getGeneralInformation() {
            return this.generalInformationModel;
        }

        getGeneralInformationFromServer(applicantsGuid: string): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.generalInformationApi + applicantsGuid);
        }

        resolveGetGeneralInformationFromServer(serverMessage: Common.Models.ServerResponseModel) {
            this.generalInformationModel = this.genericResolver.genericGetResolver("applicant's general information", serverMessage);
        }
    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.service("CurrentApplicantOverviewInformationService", ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.CurrentApplicantOverviewInformationService);
}