module BohFoundation.Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate {
    export class RatingWithApplicantsGuidModel {
        constructor(
            public rating?: Common.GenericRatingModel,
            public applicantsGuid?: string) {
        }
    }
} 