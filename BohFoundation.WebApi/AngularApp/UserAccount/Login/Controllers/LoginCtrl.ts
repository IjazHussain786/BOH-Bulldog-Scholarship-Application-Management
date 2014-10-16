module BohFoundation.UserAccount.Login.Controllers {
    'use strict';
    export interface ILoginCtrlScope extends ng.IScope {
        logIn(form: ng.IFormController): void;

        loginModel: Models.LoginModel;

        processing: boolean;

        loginForm : ng.IFormController;
    }

    export interface ILoginCtrl {
        $scope: ILoginCtrlScope;
    }

    export class LoginCtrl implements ILoginCtrl {
        static $inject = ['$scope', 'LoginService', 'LoginRepository', 'AlertHelperService'];
        constructor(public $scope: ILoginCtrlScope,
            private loginService: Services.ILoginService, 
            private loginRepo: Repositories.ILoginRepository,
            private alertHelper: Common.Services.UiServices.IAlertHelperServices) {

            $scope.processing = false;

            $scope.logIn = (loginForm: ng.IFormController) => {
                if (loginForm.$valid && !$scope.processing) {
                    this.logIn();
                }
            };
        }

        private logIn() {
            this.switchProcessing();
            var promise = this.loginRepo.getToken(this.$scope.loginModel);
            this.resultofLoginCall(promise);
        }

        private switchProcessing() {
            this.$scope.processing = !this.$scope.processing;
        }

        private resultofLoginCall(loginPromise: ng.resource.IResource<any>) {
            loginPromise.$promise.then(
                (response) => { this.createLoginServerResponseModelandPassToLoginService(response, true); },
                (response) => { this.createLoginServerResponseModelandPassToLoginService(response, false); });
        }

        private createLoginServerResponseModelandPassToLoginService(data, success: boolean) {
            var model = new Login.Models.LoginServerResponseModel(data, success);
            this.loginService.transformAndPersistLoginToken(model, this.$scope.loginModel.keepMeLoggedIn);
            if (!success) {
                this.switchProcessing();
            }
        }
    }
}

module BohFoundation.Main {
    Register.UserManagement.controller("LoginCtrl", UserAccount.Login.Controllers.LoginCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/UserAccount/LogIn', {
                    templateUrl: '/AngularApp/UserAccount/Login/Templates/LoginTemplate.html',
                    controller: 'LoginCtrl',
                    publicAccess: true,
                    title: 'Login'
                });
            })]);
}
