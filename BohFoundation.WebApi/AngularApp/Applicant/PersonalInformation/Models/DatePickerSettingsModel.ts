 module BohFoundation.Applicant.PersonalInformation.Models {
     export class DatePickerSettingsModel {
         constructor() {
             this.maxDate = this.substractYear(15);
             this.initialDate = this.substractYear(18);
         }

         public yearRange = 5;
         public showWeeks = false;
         public currentText = false;

         public format = "shortDate";

         public maxDate;
         public initialDate;

         private substractYear(yearsToSubtract: number) {
             var now = new Date();
             return new Date(now.getFullYear() - yearsToSubtract, now.getMonth(), now.getDate());
         }

     }
 }