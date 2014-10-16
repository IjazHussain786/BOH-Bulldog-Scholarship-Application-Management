module BohFoundation.Dtos.ApplicationEvaluator.RatingSummary {
    export class Top5ApplicantsModel {
        constructor(
            public topApplicants?: Array<TopApplicantRatingSummaryModel>
            ) { }
    }
} 