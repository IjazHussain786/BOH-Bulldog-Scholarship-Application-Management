 module BohFoundation.Dtos.Applicant {
     export class ApplicantNotificationsModel {
         constructor(public lastUpdatedPersonalInformation: Date,
             public lastUpdatedContactInformation: Date,
             public lastUpdatedTranscriptUpload: Date,
             public lastUpdatedAcademicInformation: Date,
             public lowGradeNotificationInformation: Notifications.ILowGradeNotificationInformationModel,
             public essayNotifications: Array<Notifications.EssayNotificationsModel>,
             public applicantReferenceCounts: Notifications.ApplicantReferenceCountsModel,
             public lastUpdatedFamilyInformation?: Date,
             public lastUpdatedExtracurriculars?: Date,
             public deadlineDate?: Date) { }  
     }
 }