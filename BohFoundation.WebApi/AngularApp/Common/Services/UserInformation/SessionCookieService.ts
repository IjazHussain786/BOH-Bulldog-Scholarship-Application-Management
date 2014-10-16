module BohFoundation.Common.Services.UserInformation {
    'use strict';
    export interface ISessionCookieService {
        setCookie(userInformationToken: Models.UserInformationModel): void;
        getUserInformationFromCookie(): Models.UserInformationModel;
        deleteSessionCookie():void;
    }

    export class SessionCookieService implements ISessionCookieService{

        private sessionCookieName = "93511211-9DB9-4109-B555-320DC375D1B6";

        setCookie(userInformationToken: Models.UserInformationModel) {
            var tokenString = this.stringafyJson(userInformationToken);
            this.createCookie(tokenString);
        }

        getUserInformationFromCookie() {
            var stringFromCookie = this.retrieveCookie();
            if (stringFromCookie == null || stringFromCookie.length < 2) {
                return null;
            } else {
                return this.deserializeJsonString(stringFromCookie);
            }
        }

        deleteSessionCookie() {
            document.cookie = this.sessionCookieName + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;'; 
        }

        private createCookie(cookieInput:string) {
            document.cookie = this.sessionCookieName + "=" + cookieInput;
        }

        private retrieveCookie() {
            var nameEQ = this.sessionCookieName + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') c = c.substring(1, c.length);
                if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
            }
            return null;
        }

        private stringafyJson(userInformationToken: Models.UserInformationModel) {
            return angular.toJson(userInformationToken);
        }

        private deserializeJsonString(token: string) : Models.UserInformationModel {
            return angular.fromJson(token);
        }
    }
}

module BohFoundation.Main {
    Register.Common.service('SessionCookieService', Common.Services.UserInformation.SessionCookieService);
}