module BohFoundation.Dtos.ApplicationEvaluator.EvaluatingApplicants {
    export class ConfirmTranscriptModel {
        constructor(
            public applicantsGuid?: string,
            public informationMatchesTranscriptPdf?: boolean
            ) {
        }
    }
} 