using System.Collections.Generic;

namespace BohFoundation.Domain.Dtos.ApplicationEvaluator.RatingSummary
{
    public class Top5ApplicantsDto
    {
        public ICollection<TopApplicantRatingSummaryDto> TopApplicants { get; set; } 
    }
}
