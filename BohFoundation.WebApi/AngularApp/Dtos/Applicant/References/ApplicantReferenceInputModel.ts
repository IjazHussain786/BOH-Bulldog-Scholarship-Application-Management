  module BohFoundation.Dtos.Applicant.References {
      export class ApplicantReferenceInputModel {
          constructor(
              public referenceEmail?: string,
              public salutation?: string,
              public signature?: string, 
              public messageParagraph?: string,
              public relationshipToReference?: string)
          { }
      }
  }