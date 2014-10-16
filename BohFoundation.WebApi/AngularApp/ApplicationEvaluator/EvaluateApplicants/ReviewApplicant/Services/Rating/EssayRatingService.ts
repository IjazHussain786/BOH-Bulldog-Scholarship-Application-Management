module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.Rating {
    export interface IEssayRatingService {

        postEssayRating(essayRating: Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate.EssayRatingModel): ng.resource.IResource<any>;
        resolvePostEssayRating(serverMessage: Common.Models.ServerResponseModel): void;
    }

    export class EssayRatingService implements IEssayRatingService {
        static $inject = ['AuthRequiredRepository', 'GenericResolver'];
        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }

        private apiEndpoint = "/api/applicationevaluator/evaluatingapplicants/essay/rating";

        postEssayRating(essayRating: Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate.EssayRatingModel): ng.resource.IResource<any> {
            return this.authRequiredRepository.post(essayRating,this.apiEndpoint);
        }

        resolvePostEssayRating(serverMessage: Common.Models.ServerResponseModel): void {
            this.genericResolver.genericPostResolver("essay rating update", serverMessage);
        }
    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.service("EssayRatingService", ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.Rating.EssayRatingService);
}