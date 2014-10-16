using System;
using System.Collections.Generic;
using BohFoundation.Domain.Dtos.Person;
using BohFoundation.Domain.Enums;

namespace BohFoundation.Domain.Dtos.ApplicationEvaluator.RatingSummary
{
    public class TopApplicantRatingSummaryDto
    {
        public RatingEnum? AverageRating { get; set; }
        public NameDto ApplicantsName { get; set; }
        public Guid ApplicantsGuid { get; set; }
        public ICollection<IndividualRatingDto> ApplicantEvaluatorsRatings { get; set; } 
    }
}