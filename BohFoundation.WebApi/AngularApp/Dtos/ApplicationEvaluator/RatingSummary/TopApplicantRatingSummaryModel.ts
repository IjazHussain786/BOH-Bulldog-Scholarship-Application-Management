module BohFoundation.Dtos.ApplicationEvaluator.RatingSummary {
    export class TopApplicantRatingSummaryModel {
        constructor(
            public averageRating?: BohFoundation.Common.Enums.RatingEnum,
            public applicantsName?: Person.NameModel,
            public applicantsGuid?: string,
            public applicantEvaluatorsRatings?: Array<IndividualRatingModel>
            ) { }
    }
} 