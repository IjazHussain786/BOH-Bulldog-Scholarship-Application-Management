module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services {
    export interface ICurrentApplicantEssayAndLettersOfRecommendationSummariesService {

        getEssayAndLetterOfRecommendationSummaries(): Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.CollectionsOfEssaysAndLettersOfRecommendationModel;
        getEssayAndLetterOfRecommendationSummariesFromServer(applicantsGuid: string): ng.resource.IResource<any>;
        resolveGetEssayAndLetterOfRecommendationSummariesFromServer(serverMessage: Common.Models.ServerResponseModel): void;
    }

    export class CurrentApplicantEssayAndLettersOfRecommendationSummariesService implements ICurrentApplicantEssayAndLettersOfRecommendationSummariesService {
        static $inject = ['AuthRequiredRepository', 'GenericResolver'];
        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }


        private essayAndLetterOfRecommendationSummariesModel: Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.CollectionsOfEssaysAndLettersOfRecommendationModel;
        private essayAndLetterOfRecommendationSummariesApi = "/api/applicationevaluator/evaluatingapplicants/displayapplication/collectionessaysandlettersofrecommendation/";

        getEssayAndLetterOfRecommendationSummaries(): Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.CollectionsOfEssaysAndLettersOfRecommendationModel {
            return this.essayAndLetterOfRecommendationSummariesModel;
        }

        getEssayAndLetterOfRecommendationSummariesFromServer(applicantsGuid: string): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.essayAndLetterOfRecommendationSummariesApi + applicantsGuid);
        }

        resolveGetEssayAndLetterOfRecommendationSummariesFromServer(serverMessage: Common.Models.ServerResponseModel): void {
            this.essayAndLetterOfRecommendationSummariesModel = this.genericResolver.genericGetResolver("applicant's essays and letter of recommedation summaries", serverMessage);
        }
    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.service("CurrentApplicantEssayAndLettersOfRecommendationSummariesService", ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.CurrentApplicantEssayAndLettersOfRecommendationSummariesService);
} 