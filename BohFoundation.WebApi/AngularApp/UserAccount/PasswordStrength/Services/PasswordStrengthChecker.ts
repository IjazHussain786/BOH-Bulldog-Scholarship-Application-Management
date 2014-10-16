module BohFoundation.UserAccount.PasswordStrength.Services {
    'use strict';
    export interface IPasswordStrengthChecker {
        check(string): Models.PasswordScore;

        goodPasswordExplaination: string;
    }

    declare var zxcvbn;

    export class PasswordStrengthChecker implements IPasswordStrengthChecker {
        static $inject = ['ZxcvbnService'];

        private score: number;

        public goodPasswordExplaination = "Good passwords are either very long, or use non-obvious spelling, punctuation, words and phrases. You will be able to continue when your password is complex enough to mostly fill the meter and a checkmark appears.";

        constructor(private zxcvbnService: BohFoundation.Common.Services.Utilities.IZxcvbnService) {
            zxcvbnService.getZxcvbn();
        }

        check(password: string): Models.PasswordScore {
            if (password != null) {
                var score = this.checkScore(password);
                var style = this.whatStyle(score);
                var message = this.whatMessage(score);
                return new Models.PasswordScore(style, score, message);
            } else {
                return null;
            }
        }

        private whatStyle(score: number): string {
            switch (score) {
                case 0:
                    return 'danger';
                case 1:
                    return 'danger';
                case 2:
                    return 'warning';
                case 3:
                    return 'primary';
                case 4:
                    return 'success';
                default:
                    return 'danger';
            }
        }

        private whatMessage(score: number): string {
            switch (score) {
                case 0:
                    return 'Bad';
                case 1:
                    return 'Not Good';
                case 2:
                    return 'OK';
                case 3:
                    return 'Good';
                case 4:
                    return 'Great';
                default:
                    return 'Danger';
            }
        }

        private checkScore(password: string): number {
            if (this.zxcvbnService.zxcvbnLoaded()) {
                return this.zxcvbnService.zxcvbnScore(password);
            }
            return 0;
        }
    }
}

module BohFoundation.Main {
    Register.UserManagement.service("PasswordStrengthChecker", UserAccount.PasswordStrength.Services.PasswordStrengthChecker);
}