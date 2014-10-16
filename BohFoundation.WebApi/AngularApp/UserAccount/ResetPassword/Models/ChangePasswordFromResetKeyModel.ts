module BohFoundation.UserAccount.ResetPassword.Models {
    'use strict';
    export class ChangePasswordFromResetKeyModel {
        constructor(public key: string, public newPassword?: string) { }
    }
}