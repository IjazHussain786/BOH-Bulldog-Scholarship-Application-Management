module BohFoundation.UserAccount.ChangeEmail.Models {
    'use strict';
    export class VerifyEmailModel {
        constructor(public password: string, public verificationKey: string) { }
    }
} 