module BohFoundation.UserAccount.PasswordStrength.Models {
    'use strict';
    export class PasswordScore {
        constructor(public style: string, public score: number, public message) { }
    }
} 