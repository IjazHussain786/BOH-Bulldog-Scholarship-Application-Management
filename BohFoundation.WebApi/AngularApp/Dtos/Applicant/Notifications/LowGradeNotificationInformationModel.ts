module BohFoundation.Dtos.Applicant.Notifications{
    export interface ILowGradeNotificationInformationModel {
        numberOfLowGradeExplainationsNeeded: INumberOfLowGradeExplainationsNeededModel;
        lowGradesNeededOutstanding: number;
        gpa: number;
        lastUpdatedLowGrade: Date;
        numberOfLowGradesInformationSaved: number;
    }

    export class LowGradeNotificationInformationModel implements ILowGradeNotificationInformationModel {

        constructor(public numberOfLowGradesInformationSaved: number, public gpa?: number, public lastUpdatedLowGrade?: Date) {

        }

        numberOfLowGradeExplainationsNeeded: INumberOfLowGradeExplainationsNeededModel = new NumberOfLowGradeExplainationsNeededModel(this.gpa);
        lowGradesNeededOutstanding: number = this.numberOfLowGradeExplainationsNeeded.totalExplanationsNeeded - this.numberOfLowGradesInformationSaved;
    }
} 