module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.Rating {
    export interface IGeneralRatingService {

        postFirstImpressionRating(firstImpressionRating: Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate.RatingWithApplicantsGuidModel): ng.resource.IResource<any>;
        postFinalOverallRating(finalOverallRating: Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate.RatingWithApplicantsGuidModel): ng.resource.IResource<any>;

        resolvePostNonEssayRating(serverMessage: Common.Models.ServerResponseModel): void;
    }

    export class GeneralRatingService implements IGeneralRatingService {
        static $inject = ['AuthRequiredRepository', 'GenericResolver'];
        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }

        private firstImpressionApiEndpoint = "/api/applicationevaluator/evaluatingapplicants/rateapplicants/firstimpression";
        private finalOverallRatingApiEndpoint = "/api/applicationevaluator/evaluatingapplicants/rateapplicants/final";

        postFirstImpressionRating(firstImpressionRating: Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate.RatingWithApplicantsGuidModel): ng.resource.IResource<any> {
            return this.authRequiredRepository.post(firstImpressionRating, this.firstImpressionApiEndpoint);
        }

        postFinalOverallRating(finalOverallRating: Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate.RatingWithApplicantsGuidModel): ng.resource.IResource<any> {
            return this.authRequiredRepository.post(finalOverallRating, this.finalOverallRatingApiEndpoint);
        }

        resolvePostNonEssayRating(serverMessage: Common.Models.ServerResponseModel): void {
            this.genericResolver.genericPostResolver("rating update", serverMessage);
        }
    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.service("GeneralRatingService", ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.Rating.GeneralRatingService);
} 