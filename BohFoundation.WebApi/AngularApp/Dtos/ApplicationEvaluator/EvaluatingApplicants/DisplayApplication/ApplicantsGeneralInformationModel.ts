module BohFoundation.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication {
    export class ApplicantsGeneralInformationModel {
        constructor(
            public name?: Person.NameModel,
            public contactInformation?: Person.ContactInformationModel,
            public familyInformation?: Applicant.Family.FamilyInformationModel,
            public personalInformation?: Applicant.PersonalInformation.ApplicantPersonalInformationModel,
            public yourFirstImpressionRating?: Common.GenericRatingModel,
            public yourFinalOverallRating?: Common.GenericRatingModel,
            public applicantGuid?: string) {
        }
    }
} 