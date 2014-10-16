module BohFoundation.Common.Services.UserInformation {
    'use strict';
    export interface IUserInformationService {
        getFirstName(): string;
        getLastName(): string;
        getLastnameFirst(): string;
        getFullName(): string;
        getGraduatingYear(): number;
        
        isApplicant():boolean;
        isAdmin(): boolean;
        isReference(): boolean;
        isApplicationEvaluator():boolean;

        getUserEmail(): string;
        getUserAuthHeader(): string;
		getUserGuid(): string;
        getRemembered():boolean;
        getLoggedIn(): boolean;

        persistLogin(userInformationToken: Models.UserInformationModel): void;
        logOut(urlToForwardTo: string): void;
        removeTokenFromLocalStorage(): void;
        
        lastUrlVisitedBesidesLogin:string;
        loggedIn: boolean;
        userInfo: Models.UserInformationModel;
    }

    export class UserInformationService implements IUserInformationService{
        
        userInfo: Models.UserInformationModel;
        loggedIn = false;
        lastUrlVisitedBesidesLogin = '/';

        static $inject = ['SessionCookieService','UserInformationLocalStore', '$location'];
        constructor(private sessionCookieService: Services.UserInformation.ISessionCookieService, private userInformationLocalStore: Common.Services.UserInformation.IUserInformationLocalStore, private $location:ng.ILocationService) {
            this.checkCookieThenLocal();
        }

        getUserGuid() {
            if (!this.loggedIn) {
                return '00000000-0000-0000-0000-000000000000';
            }
            return this.userInfo.guid;
        }

        getLastnameFirst() {
            if (!this.loggedIn) {
                return "noLastnameFirst";
            }
            return this.getLastName() + ", " + this.getFirstName();
        }

        getFirstName() {
            if (!this.loggedIn) {
                return "noFirstName";
            }
            return this.userInfo.firstName;
        }

        getLastName() {
            if (!this.loggedIn) {
                return "noLastName";
            }
            return this.userInfo.lastName;
        }

        getFullName() {
            if (!this.loggedIn) {
                return "noFullName";
            }
            return this.getFirstName() + " " + this.getLastName();
        }

        getGraduatingYear() {
            if (!this.loggedIn) {
                return 0;
            }
            return this.userInfo.graduatingYear;
        }
        
        getUserEmail(): string {
            if (!this.loggedIn) {
                return 'noEmail';
            }
            return this.userInfo.emailAddress;
        }

        getUserAuthHeader(): string {
            if (!this.loggedIn) {
                return 'noAuthHeader';
            }
            return "Bearer " + this.userInfo.accessToken;
        }

        getLoggedIn(): boolean {
            this.checkIfTokenIsExpired();
            return this.loggedIn;
        }

        getRemembered(): boolean {
            if (!this.loggedIn) {
                return false;
            }
            return this.userInfo.rememberedOnDevice;
        }

        isApplicant(): boolean {
            if (!this.loggedIn) {
                return false;
            }
            return this.userInfo.applicant;
        }

        isAdmin(): boolean {
            if (!this.loggedIn) {
                return false;
            }
            return this.userInfo.admin;
        }

        isReference(): boolean {
            if (!this.loggedIn) {
                return false;
            }
            return this.userInfo.reference;
        }

        isApplicationEvaluator(): boolean {
            if (!this.loggedIn) {
                return false;
            }
            return this.userInfo.applicationEvaluator;
        }

        persistLogin(userInformationToken: Models.UserInformationModel) {
            this.sessionCookieService.setCookie(userInformationToken);
            this.setUserInfo(userInformationToken);
            if (userInformationToken.rememberedOnDevice) {
                this.userInformationLocalStore.add(userInformationToken);
            }
        }

        removeTokenFromLocalStorage() {
            this.userInformationLocalStore.remove();
            this.userInfo.rememberedOnDevice = false;
            this.persistLogin(this.userInfo);
        }

        logOut(urlToForwardTo: string) {
            this.sessionCookieService.deleteSessionCookie();
            this.loggedIn = false;
            this.userInfo = null;
            this.$location.path(urlToForwardTo);
        }

        private checkLocalStorageForToken() {
            var sessionCookie = this.userInformationLocalStore.get();
            if (sessionCookie != null) {
                this.persistLogin(sessionCookie);
            }
        }

        private checkCookieThenLocal() {
            var sessionCookie = this.sessionCookieService.getUserInformationFromCookie();
            if (sessionCookie == null) {
                this.checkLocalStorageForToken();
            } else {
                this.setUserInfo(sessionCookie);
            }
        }

        private setUserInfo(userInformationToken) {
            this.loggedIn = true;
            this.userInfo = userInformationToken;
        }

        private checkIfTokenIsExpired() {
            if(this.loggedIn){
            var now = new Date();
                if (now > this.userInfo.expireDate) {
                    this.removeTokenFromLocalStorage();
                    this.logOut("");
                }
            }
        }
    }
} 

module BohFoundation.Main {
    Register.Common.service("UserInformationService", Common.Services.UserInformation.UserInformationService);
}