module BohFoundation.Dtos.Admin {
    export class ConfirmApplicationEvaluatorModel {
        constructor(public emailAddress: string, public confirm: boolean, public createAdmin: boolean){}
    }
} 