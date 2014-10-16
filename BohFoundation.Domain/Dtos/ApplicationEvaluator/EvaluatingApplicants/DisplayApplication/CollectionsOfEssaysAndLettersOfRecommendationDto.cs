using System.Collections.Generic;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.Essay;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.LetterOfRecommendation;

namespace BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication
{
    public class CollectionsOfEssaysAndLettersOfRecommendationDto
    {
        public ICollection<EssaySummaryDto> EssaySummaries { get; set; }
        public ICollection<LetterOfRecommendationSummaryDto> LetterOfRecommendationSummaries { get; set; } 
    }
}