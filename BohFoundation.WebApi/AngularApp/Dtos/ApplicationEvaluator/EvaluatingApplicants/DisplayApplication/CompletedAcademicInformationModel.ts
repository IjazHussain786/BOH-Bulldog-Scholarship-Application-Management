module BohFoundation.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication {
    export class CompletedAcademicInformationModel {
        constructor(
            public basicAcademicInformation?: Applicant.Academic.AcademicInformationModel,
            public lowGrades?: Array<Applicant.Academic.LowGradeModel>,
            public transcriptBlobReference?: Applicant.Academic.TranscriptBlobReferenceModel,
            public transcriptMatchesDatabaseValues?: boolean,
            public transcriptDoesNotMatchDatabaseValues?: boolean
            ) {}
    }
} 