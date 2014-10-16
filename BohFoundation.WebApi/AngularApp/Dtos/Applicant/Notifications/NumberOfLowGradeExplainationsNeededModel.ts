module BohFoundation.Dtos.Applicant.Notifications {

    export interface INumberOfLowGradeExplainationsNeededModel {
        totalExplanationsNeeded:number;
    }

     export class NumberOfLowGradeExplainationsNeededModel implements INumberOfLowGradeExplainationsNeededModel {
         private gpa: number;
         totalExplanationsNeeded: number;

         constructor(gpa: number) {
             this.gpa = gpa;

             this.setTotalExplanationsNeeded();
         }

         private setTotalExplanationsNeeded() {
             var absoluteValueOfGpaMinusPerfect = Math.abs(this.gpa - 4.0);
             absoluteValueOfGpaMinusPerfect = absoluteValueOfGpaMinusPerfect * 3.33;
             absoluteValueOfGpaMinusPerfect = Math.round(absoluteValueOfGpaMinusPerfect);
             if (absoluteValueOfGpaMinusPerfect > 5) {
                 absoluteValueOfGpaMinusPerfect = 5;
             }
             if (this.gpa > 3.7) {
                 absoluteValueOfGpaMinusPerfect = 0;
             }
             this.totalExplanationsNeeded = absoluteValueOfGpaMinusPerfect;
         }
     }
 }