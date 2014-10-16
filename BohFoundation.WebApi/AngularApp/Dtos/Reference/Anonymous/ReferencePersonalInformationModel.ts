 module BohFoundation.Dtos.Reference.Anonymous {
     export class ReferencePersonalInformationModel {
         constructor(
             public name?: Person.NameModel,
             public occupation?: string,
             public phoneInformationDto?: Person.PhoneInformationModel,
             public guidSentToReference?: string,
             public emailAddress?: string
             ){}
     }
 }