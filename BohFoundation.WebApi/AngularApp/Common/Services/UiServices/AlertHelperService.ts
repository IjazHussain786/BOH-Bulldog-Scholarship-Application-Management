module BohFoundation.Common.Services.UiServices {
    'use strict';
    export interface IAlertHelperServices {
        addWarningAlert(string): void;
        addSuccessAlert(string): void;
        addDangerAlert(string): void;

        
        addGenericInformationPostSuccess(): void;
        addGenericInformationPostError(whatInformation: string): void;
        addGenericInformationGetError(whatInformation: string): void;
    }

    export class AlertHelperService implements IAlertHelperServices {

        static $inject = ['toaster'];
        constructor(private toaster) { }

        addWarningAlert(message: string) {
            this.toaster.pop('warning', 'Warning...', message);
        }

        addSuccessAlert(message: string) {
            this.toaster.pop('success', 'Success!', message);
        }

        addDangerAlert(message: string) {
            this.toaster.pop('error', 'Drat!', message);
        }

        addGenericInformationPostSuccess(): void {
            this.addSuccessAlert("You have successfully saved your information.");
        }

        addGenericInformationPostError(whatInformation: string): void {
            var message = "There was a problem saving your " + whatInformation + " information. Please try again later."
            this.addDangerAlert(message);
        }

        addGenericInformationGetError(whatInformation: string): void {
            var message = "There was a problem getting your " + whatInformation + " information.";
            this.addDangerAlert(message);
        }
    }
}

module BohFoundation.Main {
    Register.Common.service("AlertHelperService", Common.Services.UiServices.AlertHelperService);
}