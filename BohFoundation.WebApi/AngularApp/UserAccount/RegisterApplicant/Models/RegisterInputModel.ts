module BohFoundation.UserAccount.RegisterApplicant.Models {
    'use strict';
    export class RegisterInputModel {
        constructor(public firstName: string, public lastName: string, public emailAddress: string, public password?: string, public graduatingYear?: number) { }
    }
} 