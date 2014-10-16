module BohFoundation.Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate {
    export class EssayRatingModel {
        constructor(
            public essayTopicId?: number,
            public rating?: Common.GenericRatingModel,
            public applicantsGuid?: string)
        {}
    }
} 