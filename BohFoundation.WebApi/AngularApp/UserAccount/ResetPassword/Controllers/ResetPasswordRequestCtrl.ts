module BohFoundation.UserAccount.ResetPassword.Controllers {
    'use strict';
    export interface IResetPasswordRequestCtrlScope extends ng.IScope {
        requestResetKey(form: ng.IFormController): void;

        resetPasswordThruEmailModel: Models.ResetPasswordThruEmailModel;

        processing: boolean;
    }

    export interface IResetPasswordRequestCtrl {
        $scope: IResetPasswordRequestCtrlScope;
    }

    export class ResetPasswordRequestCtrl implements IResetPasswordRequestCtrl {
         static $inject = ['$scope', 'ResetPasswordRepository', 'AlertHelperService']
         constructor(public $scope: IResetPasswordRequestCtrlScope,
            private resetPasswordRepository: Repositories.IResetPasswordRepository,
            private alertHelperService: Common.Services.UiServices.IAlertHelperServices) {

            $scope.requestResetKey = (form: ng.IFormController) => {
                if (form.$valid && !$scope.processing) {
                    this.flipProcessing();
                    var promise = resetPasswordRepository.requestPasswordResetKey($scope.resetPasswordThruEmailModel);
                    this.resolvePromise(promise);
                }
            };
        }

        private resolvePromise(promise: ng.resource.IResource<any>) {
            promise.$promise.then(
                () => { this.alertHelperService.addSuccessAlert('You will recieve a message in your inbox soon with your temporary key.'); },
                (callback) => {
                    this.alertHelperService.addDangerAlert(callback.data.message);
                    this.flipProcessing();
                });
        }

        private flipProcessing() {
            this.$scope.processing = !this.$scope.processing;
        }
    }
}

module BohFoundation.Main {
    Register.UserManagement.controller("ResetPasswordRequestCtrl", UserAccount.ResetPassword.Controllers.ResetPasswordRequestCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/UserAccount/ResetPassword/Request', {
                    templateUrl: '/AngularApp/UserAccount/ResetPassword/Templates/ResetPasswordRequest.html',
                    controller: 'ResetPasswordRequestCtrl',
                    publicAccess: true,
                    title: 'Request Password Reset'
                });
            })
        ]);
}