module BohFoundation.Dtos.Email {
    export class SendEmailContactModel {
        constructor(public recipientFirstName:string, public recipientLastName:string, public recipientEmailAddress:string){}
    }
}