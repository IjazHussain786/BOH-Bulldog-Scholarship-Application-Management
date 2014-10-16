module BohFoundation.UserAccount.Login.Services {
    'use strict';
    export interface ILoginService {
        transformAndPersistLoginToken(tokenReturnedFromServer: Models.LoginServerResponseModel, rememberMe: boolean): any;
    }

    export class LoginService implements ILoginService {

        static $inject = ['AlertHelperService', 'UserInformationService', '$location']
        constructor(private alertHelperService: Common.Services.UiServices.IAlertHelperServices,
            private userInformationService: Common.Services.UserInformation.IUserInformationService,
            private $location: ng.ILocationService) {

            this.forwardIfLoggedIn();
        }

        transformAndPersistLoginToken(tokenReturnedFromServer: Models.LoginServerResponseModel, rememberMe: boolean) {
            if (!tokenReturnedFromServer.success) {
                this.alertHelperService.addDangerAlert(tokenReturnedFromServer.userInformationToken.data.error);
            } else {
                if (tokenReturnedFromServer.userInformationToken.applicationEvaluatorPendingConfirmation) {
                    this.$location.path('/EvaluateApplicants/PendingConfirmation'); 
                } else {
                    var transformedData = this.transformDataToUserInformationModel(tokenReturnedFromServer, rememberMe);
                    if (transformedData.emailAddress == undefined) {
                        this.returnedDataUndefinedError(rememberMe);
                    } else {
                        this.everythingGoesWell(transformedData);
                    }
                }
            }
        }

        private everythingGoesWell(transformedData : Common.Models.UserInformationModel) {
            this.userInformationService.persistLogin(transformedData);
            this.alertHelperService.addSuccessAlert("You are now logged in as " + transformedData.firstName + " " + transformedData.lastName + "!");
            this.forwardToRightUrl();
        }

        private forwardToRightUrl() {
            if (this.userInformationService.isApplicant()) {
                this.$location.path('/Applicant');
            }
            if (this.userInformationService.isApplicationEvaluator()) {
                this.$location.path("/ApplicationEvaluator");
            }
        }

        private returnedDataUndefinedError(rememberMe: boolean) {
            var data = { data: { error: 'There as an error logging into your account. Sorry. Please try closing your browser and retrying. This happens when you try to log in multiple times quickly.' } };
            var token = new Models.LoginServerResponseModel(data, false);
            this.transformAndPersistLoginToken(token, rememberMe);
        }

        private transformDataToUserInformationModel(response: Models.LoginServerResponseModel, rememberMe: boolean): Common.Models.UserInformationModel {
            return new Common.Models.UserInformationModel(response.userInformationToken.access_token, response.userInformationToken.emailAddress, response.userInformationToken.firstName, response.userInformationToken.lastName, rememberMe, response.userInformationToken.guid, new Date(response.userInformationToken.expires), response.userInformationToken.applicant, response.userInformationToken.admin, response.userInformationToken.reference, response.userInformationToken.applicationEvaluator, response.userInformationToken.graduatingYear);
        }

        private forwardIfLoggedIn() {
            if (this.userInformationService.getLoggedIn()) {
                this.forwardToRightUrl();
            }
        }
    }

}

module BohFoundation.Main {
    Register.UserManagement.service("LoginService", UserAccount.Login.Services.LoginService);
}