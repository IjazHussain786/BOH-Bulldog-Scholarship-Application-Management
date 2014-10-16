module BohFoundation.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.LetterOfRecommendation {
    export class LetterOfRecommendationSummaryModel {
        constructor(
            public referencePersonalInformation?: Reference.Anonymous.ReferencePersonalInformationModel,
            public referenceRelationshipToApplicant?: string,
            public letterOfRecommendationKeyValues?: Common.AzureQueuryObjects.AzureTableStorageEntityKeyModel) {
        }
    }
} 