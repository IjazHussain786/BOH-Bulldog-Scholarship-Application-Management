module BohFoundation.Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants {
    export class AllFinalizedApplicantsForAGraduatingYearModel {
        constructor(
            public graduatingYear?: number,
            public applicantSummaries?: Array<ApplicantSummaryModel>,
            public percentRated?: number,
            public numberOfApplicantsNotYetRated?: number,
            public nextRandomApplicantForReview?: string) {
        }
    }
}