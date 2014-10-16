module BohFoundation.Dtos.Applicant.Notifications {
    export class ApplicantReferenceCountsModel {
        constructor(
            public numberOfReferenceInvitationsSent: number,
            public numberOfReferencesRecieved: number,
            public lastUpdated?: Date) { }
    }
} 