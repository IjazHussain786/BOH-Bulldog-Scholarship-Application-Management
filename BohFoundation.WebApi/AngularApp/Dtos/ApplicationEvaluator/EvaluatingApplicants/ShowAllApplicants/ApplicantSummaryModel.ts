module BohFoundation.Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants {
    export class ApplicantSummaryModel {
        constructor(
            public applicantName: Dtos.Person.NameModel,
            public applicantGuid: string,
            public gpa: number,
            public incomeRange: BohFoundation.Common.Enums.IncomeRangeEnum,
            public yourRating: BohFoundation.Common.Enums.RatingEnum) {
        }
    }
} 