 module BohFoundation.Dtos.Applicant.Essay {
     export class EssayModel {
         constructor(
             public essayPrompt?: string,
             public essayTopicId?: number,
             public essay?: string,
             public revisionDateTime?: Date) { }
     }
 }