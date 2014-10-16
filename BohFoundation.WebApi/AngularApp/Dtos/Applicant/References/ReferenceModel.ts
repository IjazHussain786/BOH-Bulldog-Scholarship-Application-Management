 module BohFoundation.Dtos.Applicant.References {
     export class ReferenceModel {
         constructor(
             public name: Person.NameModel,
             public referenceEmailAddress: string,
             public referenceLetterReceived: boolean)
         { }
     }
 }