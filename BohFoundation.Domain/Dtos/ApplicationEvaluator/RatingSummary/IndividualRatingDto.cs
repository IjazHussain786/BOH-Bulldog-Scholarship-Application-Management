using BohFoundation.Domain.Dtos.Common;
using BohFoundation.Domain.Dtos.Person;

namespace BohFoundation.Domain.Dtos.ApplicationEvaluator.RatingSummary
{
    public class IndividualRatingDto 
    {
        public NameDto ApplicantEvaluatorsName { get; set; }
        public GenericRatingDto OverallRating { get; set; }
    }
}