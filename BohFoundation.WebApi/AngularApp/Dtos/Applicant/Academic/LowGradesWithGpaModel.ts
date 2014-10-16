module BohFoundation.Dtos.Applicant.Academic {
    export class LowGradesWithGpaModel {
        constructor(public gpa?: number,
            public lowGrades?:Array<LowGradeModel>) {
        }
    }
}