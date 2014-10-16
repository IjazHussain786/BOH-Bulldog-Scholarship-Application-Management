using System;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;

namespace BohFoundation.Domain.EntityFrameworkModels.References
{
    public class LetterOfRecommendation
    {
        public int Id { get; set; }
        public string LetterOfRecommendationRowKey { get; set; }
        public string LetterOfRecommendationPartitionKey { get; set; }
        public string ReferenceRelationshipToApplicant { get; set; }
        public DateTime? LastUpdated { get; set; }
        public Guid GuidSentToReference { get; set; }

        public virtual Reference Reference { get; set; }
        public virtual Applicant Applicant { get; set; }
    }
}