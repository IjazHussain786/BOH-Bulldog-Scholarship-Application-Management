module BohFoundation.Dtos.ApplicationEvaluator.RatingSummary {
    export class IndividualRatingModel {
        constructor(
            public applicantEvaluatorsName?: Person.NameModel,
            public overallRating?: Common.GenericRatingModel
            ) {
        }
    }
} 