 module BohFoundation.Dtos.Applicant.Notifications {
     export class EssayNotificationsModel {
         constructor(
             public essayPrompt: string,
             public titleOfEssay: string,
             public essayTopicId: number,
             public revisionDateTime?: Date) { }
     }
 }