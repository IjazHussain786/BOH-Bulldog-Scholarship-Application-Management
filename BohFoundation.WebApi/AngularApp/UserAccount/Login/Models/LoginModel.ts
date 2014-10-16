module BohFoundation.UserAccount.Login.Models {
    'use strict';
    export class LoginModel {
        constructor(public email: string, public password: string, public keepMeLoggedIn: boolean) { }
    }
}