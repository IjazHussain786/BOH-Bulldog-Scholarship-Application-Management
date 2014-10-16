module BohFoundation.Dtos.Applicant.Extracurricular {
    export class ExtracurricularActivitiesModel {
        constructor(
            public paidWork?: boolean,
            public hasNonPaidActivities?: boolean,
            public lastUpdated?: Date,
            public jobs?: Array<JobModel>,
            public activities?: Array<ActivityModel>) { }
    }
} 