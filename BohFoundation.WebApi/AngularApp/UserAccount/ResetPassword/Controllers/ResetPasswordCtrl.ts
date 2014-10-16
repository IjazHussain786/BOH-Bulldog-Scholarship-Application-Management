module BohFoundation.UserAccount.ResetPassword.Controllers {
    'use strict';
    export interface IResetPasswordCtrlScope extends ng.IScope {
        changePasswordFromResetKey(form: ng.IFormController): void;
        checkScore(string): void;

        passwordScore: PasswordStrength.Models.PasswordScore;
        goodPasswordExplain: string;
        processing: boolean;
        changePasswordModel: Models.ChangePasswordFromResetKeyModel;
        loginModel: Login.Models.LoginModel;
    }

    export interface IResetPasswordCtrl {
        $scope: IResetPasswordCtrlScope;
    }

    export interface IResetPasswordCtrlRouteProvider extends ng.route.IRouteParamsService {
        key: string;
    }

    export class ResetPasswordCtrl implements IResetPasswordCtrl {
        private genericError = 'There was an error processing your request.'

        static $inject = ['$scope', '$routeParams', 'ResetPasswordRepository', 'AlertHelperService', 'LoginRepository', 'LoginService', 'PasswordStrengthChecker'];
        constructor(public $scope: IResetPasswordCtrlScope, private $routeParams: IResetPasswordCtrlRouteProvider,
            private resetPasswordRepository: Repositories.IResetPasswordRepository, private alertHelperService: Common.Services.UiServices.IAlertHelperServices,
            private loginRepository: Login.Repositories.ILoginRepository, private loginService: Login.Services.ILoginService,
            private passwordStrengthChecker: PasswordStrength.Services.IPasswordStrengthChecker) {

            this.setKeyOnModel();

            $scope.goodPasswordExplain = this.passwordStrengthChecker.goodPasswordExplaination;

            $scope.changePasswordFromResetKey = (form: ng.IFormController) => {
                if (form.$valid && !$scope.processing && $scope.passwordScore.score > 2) {
                    this.setModels(); 
                    this.changePasswordWithKey();
                }
            };

            $scope.checkScore = (password: string) => {
                this.checkPasswordScore(password);
            };
        }

        private setModels() {
            this.$scope.loginModel.password = this.$scope.changePasswordModel.newPassword;
        }

        private checkPasswordScore(password: string) {
            this.$scope.passwordScore = this.passwordStrengthChecker.check(password);
        }

        private setKeyOnModel() {
            this.$scope.changePasswordModel = new Models.ChangePasswordFromResetKeyModel(this.$routeParams.key);
        }

        private changePasswordWithKey() {
            this.switchProcessing();
            var promise = this.resetPasswordRepository.changePasswordFromResetKey(this.$scope.changePasswordModel);
            this.resolveChangePasswordPromise(promise);
        }

        private resolveChangePasswordPromise(promise: ng.resource.IResource<any>) {
            promise.$promise.then(
                () => { this.successChangingPassword(); },
                () => { this.failureChangingPassword(); });
        }

        private switchProcessing() {
            this.$scope.processing = !this.$scope.processing;
        }

        private successChangingPassword() {
            this.alertHelperService.addSuccessAlert('Great! You have successfully confirmed your email address. We will now log you in automatically. Please wait.');
            this.login();
        }

        private login() {
            var promise = this.loginRepository.getToken(this.$scope.loginModel);
            this.resolveLoginPromise(promise);
        }

        private resolveLoginPromise(promise: ng.resource.IResource<any>) {
            promise.$promise.then(
                (response) => { this.createLoginServerResponseModelandPassToLoginService(response, true); },
                (response) => { this.createLoginServerResponseModelandPassToLoginService(response, false); });
        }

        private createLoginServerResponseModelandPassToLoginService(data, success: boolean) {
            var model = new Login.Models.LoginServerResponseModel(data, success);
            this.loginService.transformAndPersistLoginToken(model, this.$scope.loginModel.keepMeLoggedIn);
        }

        private failureChangingPassword() {
            this.alertHelperService.addDangerAlert(this.genericError);
            this.switchProcessing();
        }
    }
}

module BohFoundation.Main {
    Register.UserManagement.controller('ResetPasswordCtrl', UserAccount.ResetPassword.Controllers.ResetPasswordCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/UserAccount/ResetPassword/Confirm/:key', {
                    templateUrl: '/AngularApp/UserAccount/ResetPassword/Templates/ResetPassword.html',
                    controller: 'ResetPasswordCtrl',
                    publicAccess: true,
                    title: 'Reset Password'
                });
            })
        ]);
}