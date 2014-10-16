module BohFoundation.Admin.ConfirmPendingApplicationEvaluators.Models {
    export class PendingApplicationEvaluatorModel{
        constructor(public firstName: string, public lastName: string, public emailAddress: string, public confirmed: ConfirmRejectedOrNotEvaluated) {
        }
    } 

    export enum ConfirmRejectedOrNotEvaluated {
        Confirmed,
        Rejected,
        NotEvaluated
    } 
} 