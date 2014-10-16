using BohFoundation.Domain.Dtos.Common.AzureQueuryObjects;
using BohFoundation.Domain.Dtos.Reference.Anonymous;

namespace BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.LetterOfRecommendation
{
    public class LetterOfRecommendationSummaryDto
    {
        public ReferencePersonalInformationDto ReferencePersonalInformation { get; set; }
        
        public string ReferenceRelationshipToApplicant { get; set; }

        public AzureTableStorageEntityKeyDto LetterOfRecommendationKeyValues { get; set; }
    }
}
