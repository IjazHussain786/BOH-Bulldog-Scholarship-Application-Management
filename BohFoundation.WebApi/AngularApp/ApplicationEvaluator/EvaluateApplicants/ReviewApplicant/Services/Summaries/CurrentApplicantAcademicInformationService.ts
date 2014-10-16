module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services {
    export interface ICurrentApplicantAcademicInformationService {

        getAcademicInformation(): Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.CompletedAcademicInformationModel;
        getAcademicInformationFromServer(applicantsGuid: string): ng.resource.IResource<any>;
        resolveGetAcademicInformationFromServer(serverMessage: Common.Models.ServerResponseModel): void;
    }

    export class CurrentApplicantAcademicInformationService implements ICurrentApplicantAcademicInformationService {
        static $inject = ['AuthRequiredRepository', 'GenericResolver'];
        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }


        private completedAcademicInformationModal: Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.CompletedAcademicInformationModel;
        private academicInformationApi = "/api/applicationevaluator/evaluatingapplicants/displayapplication/academicinformation/";

        getAcademicInformation(): Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.CompletedAcademicInformationModel {
            return this.completedAcademicInformationModal;
        }

        getAcademicInformationFromServer(applicantsGuid: string): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.academicInformationApi + applicantsGuid);;
        }

        resolveGetAcademicInformationFromServer(serverMessage: Common.Models.ServerResponseModel): void {
            this.completedAcademicInformationModal = this.genericResolver.genericGetResolver("applicant's academic information", serverMessage);
        }
    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.service("CurrentApplicantAcademicInformationService", ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.CurrentApplicantAcademicInformationService);
}