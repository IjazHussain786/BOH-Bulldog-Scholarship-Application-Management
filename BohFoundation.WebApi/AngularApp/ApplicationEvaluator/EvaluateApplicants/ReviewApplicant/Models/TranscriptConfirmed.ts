module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Models {
    export class TranscriptConfirmed {
        constructor(
            public confirmed?: boolean,
            public notChecked?: boolean,
            public notCorrect?: boolean) { }
    }
}  