using BohFoundation.Domain.Dtos.Common;
using BohFoundation.Domain.Dtos.Common.AzureQueuryObjects;

namespace BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.Essay
{
    public class EssaySummaryDto
    {
        public int EssayTopicId { get; set; }
        public string TitleOfEssay { get; set; }
        public string EssayPrompt { get; set; }
        public int EssayCharacterLength { get; set; }
        
        public GenericRatingDto YourRating { get; set; }

        public AzureTableStorageEntityKeyDto EssayKeyValues { get; set; }
    }
}
