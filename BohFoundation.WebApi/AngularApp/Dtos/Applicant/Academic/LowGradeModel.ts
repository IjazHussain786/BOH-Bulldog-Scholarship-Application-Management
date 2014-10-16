 module BohFoundation.Dtos.Applicant.Academic {
     export class LowGradeModel {
         constructor(public grade?: string, public classTitle?: string, public wasAp?: boolean, public yearOfHighSchool?: BohFoundation.Common.Enums.YearOfHighSchool, public explanation?: string, public lastUpdated?: Date) {

         }
     }
 }