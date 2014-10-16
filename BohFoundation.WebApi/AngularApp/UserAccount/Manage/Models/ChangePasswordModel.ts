module BohFoundation.UserAccount.Manage.Models {
    'use strict';
    export class ChangePasswordModel {
        constructor(public oldPassword: string, public newPassword: string) { }
    }
}