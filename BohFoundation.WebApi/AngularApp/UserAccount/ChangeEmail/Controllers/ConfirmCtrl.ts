module BohFoundation.UserAccount.ChangeEmail.Controllers {
    'use strict';
    export interface IConfirmCtrlScope extends ng.IScope {

        verificationKey: string;
        loginModel: Login.Models.LoginModel;
        processing: boolean;

        verificationForm: ng.IFormController;

        verifyEmail(form: ng.IFormController): void;

    }

    export interface IConfirmCtrl {
        $scope: IConfirmCtrlScope;
    }

    export class ConfirmCtrl implements IConfirmCtrl {

        static $inject = ['$scope', 'AlertHelperService', 'ChangeEmailRepository', '$routeParams', 'LoginService', 'LoginRepository'];
        constructor(public $scope: IConfirmCtrlScope, public alertHelperService: Common.Services.UiServices.IAlertHelperServices,
            public changeEmailRepo: Repositories.IChangeEmailRepository, public $routeParams: IConfirmEmailRoute,
            public loginService: Login.Services.ILoginService, public loginRepository: Login.Repositories.ILoginRepository) {

            this.setVerificationCode();

            $scope.verifyEmail = (form: ng.IFormController) => {
                if (form.$valid && !$scope.processing) {
                    this.verifyEmail();
                }
            };
        }

        private setVerificationCode() {
            this.$scope.verificationKey = this.$routeParams.confirmationCode;
        }

        private verifyEmail() {
            this.$scope.processing = true;
            var model = this.createVerificationModel();
            var promise = this.changeEmailRepo.confirmEmail(model);
            this.resultOfConfirmationCall(promise);
        }

        private createVerificationModel() {
            return new Models.VerifyEmailModel(this.$scope.loginModel.password, this.$scope.verificationKey);
        }

        private resultOfConfirmationCall(promise: ng.resource.IResource<any>) {
            promise.$promise.then(
                () => { this.verificationCallBackComesPositive(); },
                (response) => { this.endInError(response); });
        }

        private endInError(response) {
            this.$scope.processing = false;
            this.alertHelperService.addDangerAlert(response.data.message);
        }

        private verificationCallBackComesPositive() {
            this.alertHelperService.addSuccessAlert("Great! You have successfully confirmed your email address. We will now log you in automatically. Please wait.");
            var loginPromise = this.loginRepository.getToken(this.$scope.loginModel);
            this.resultofLoginCall(loginPromise);
        }

        private resultofLoginCall(loginPromise: ng.resource.IResource<any>) {
            loginPromise.$promise.then(
                (response) => { this.createLoginServerResponseModelandPassToLoginService(response, true); },
                (response) => { this.createLoginServerResponseModelandPassToLoginService(response, false); });
        }

        private createLoginServerResponseModelandPassToLoginService(data, success: boolean) {
            var model = new Login.Models.LoginServerResponseModel(data, success);
            this.loginService.transformAndPersistLoginToken(model, this.$scope.loginModel.keepMeLoggedIn);
        }
    }
}

module BohFoundation.Main {
    Register.UserManagement.controller("ConfirmCtrl", UserAccount.ChangeEmail.Controllers.ConfirmCtrl)
        .config(['$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
            $routeProvider.when('/UserAccount/ChangeEmail/Confirm/:confirmationCode', {
                templateUrl: '/AngularApp/UserAccount/ChangeEmail/Templates/ConfirmTemplate.html',
                controller: 'ConfirmCtrl',
                publicAccess: true,
                title: 'Confirm Email'
            });
        })]);
}