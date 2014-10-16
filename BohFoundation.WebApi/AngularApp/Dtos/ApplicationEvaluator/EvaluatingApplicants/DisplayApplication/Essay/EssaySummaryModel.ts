module BohFoundation.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.Essay {
    export class EssaySummaryModel {
        constructor(
            public essayTopicId?: number,
            public titleOfEssay?: string,
            public essayPrompt?: string,
            public essayCharacterLength?: number,
            public yourRating?: Common.GenericRatingModel,
            public essayKeyValues?: Common.AzureQueuryObjects.AzureTableStorageEntityKeyModel) {
        }
    }
} 