namespace BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate
{
    public class EssayRatingDto : RatingWithApplicantsGuidDto
    {
        public int EssayTopicId { get; set; }
    }
}