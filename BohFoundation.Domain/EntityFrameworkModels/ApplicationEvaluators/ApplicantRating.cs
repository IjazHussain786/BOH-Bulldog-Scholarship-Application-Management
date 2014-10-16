using System.Collections.Generic;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Common;

namespace BohFoundation.Domain.EntityFrameworkModels.ApplicationEvaluators
{
    public class ApplicantRating
    {
        public int Id { get; set; }
        
        public virtual GenericRating OverallRating { get; set; }
        public virtual GenericRating FirstImpressionRating { get; set; }
        public virtual ICollection<EssayRating> EssayRatings { get; set; }
        
        public virtual ApplicationEvaluator ApplicationEvaluator { get; set; }
        public virtual Applicant Applicant { get; set; }
    }
}