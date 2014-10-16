module BohFoundation.Dtos.Applicant.PersonalInformation {
    export class ApplicantPersonalInformationModel {
        constructor(
            public graduatingYear: number,
            public birthdate?: Date,
            public lastUpdated?: Date)
        { }
    }
}