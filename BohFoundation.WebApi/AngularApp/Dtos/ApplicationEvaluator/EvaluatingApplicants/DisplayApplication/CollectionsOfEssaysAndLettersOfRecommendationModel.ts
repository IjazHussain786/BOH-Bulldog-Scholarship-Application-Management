module BohFoundation.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication {
    export class CollectionsOfEssaysAndLettersOfRecommendationModel {
        constructor(
            public essaySummaries?: Array<Essay.EssaySummaryModel>,
            public letterOfRecommendationSummaries?: Array<LetterOfRecommendation.LetterOfRecommendationSummaryModel>) {
        }
    }
} 