using System;
using BohFoundation.Domain.Dtos.Common;

namespace BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate
{
    public class RatingWithApplicantsGuidDto
    {
        public GenericRatingDto Rating { get; set; }
        public Guid ApplicantsGuid { get; set; }
    }
}
