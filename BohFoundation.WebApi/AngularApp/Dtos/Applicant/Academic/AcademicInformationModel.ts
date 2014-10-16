module BohFoundation.Dtos.Applicant.Academic {
    export class AcademicInformationModel {
        constructor(
            public classRank: ClassRankModel,
            public gpa?: number,
            public careerChoice?: string,
            public probableNextSchool?: string,
            public lastUpdated?: Date) { }
    }
} 