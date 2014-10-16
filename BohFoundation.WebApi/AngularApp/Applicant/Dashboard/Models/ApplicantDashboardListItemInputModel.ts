module BohFoundation.Applicant.Dashboard.Models {
    export class ApplicantDashboardListItemInputModel {
        constructor(public title: string, public link: string, public buttonStyle?: Common.Enums.StyleEnum, public lastUpdated?: Date){}
    }
}