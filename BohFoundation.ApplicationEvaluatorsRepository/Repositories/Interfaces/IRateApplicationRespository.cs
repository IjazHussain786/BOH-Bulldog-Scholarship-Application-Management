using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate;

namespace BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces
{
    public interface IRateApplicationRespository
    {
        void UpsertEssayRating(EssayRatingDto essayRatingDto);
        void UpsertFirstImpression(RatingWithApplicantsGuidDto firstImpressionRatingDto);
        void UpsertFinalOverallRating(RatingWithApplicantsGuidDto finalOverallRatingDto);
    }
}
