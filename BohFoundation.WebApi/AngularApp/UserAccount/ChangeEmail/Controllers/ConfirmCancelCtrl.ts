module BohFoundation.UserAccount.ChangeEmail.Controllers {
    'use strict';
    export interface IConfirmCancelCtrlScope extends ng.IScope {
        confirmationCode: string;
    }

    export interface IConfirmCancelCtrl {
        $scope: IConfirmCancelCtrlScope;
        $routeParams: ng.route.IRouteParamsService;
        changeEmailRepository: Repositories.IChangeEmailRepository;
    }

    export interface IConfirmEmailRoute extends ng.route.IRouteParamsService {
        confirmationCode: string;
    }

    export class ConfirmCancelCtrl implements IConfirmCancelCtrl {

        static $inject = ['$routeParams', '$scope', 'ChangeEmailRepository', 'AlertHelperService'];

        constructor(public $routeParams: IConfirmEmailRoute, public $scope: IConfirmCancelCtrlScope, public changeEmailRepository: Repositories.IChangeEmailRepository, private alertHelperService: Common.Services.UiServices.IAlertHelperServices) {

            this.cancelConfirmation();
        }

        private getConfirmationCode() {
            return this.$routeParams.confirmationCode;
        }

        private cancelConfirmation() {
            var promise = this.changeEmailRepository.cancelConfirmation(this.getConfirmationCode());
            this.resultOfCancelConfirmationCall(promise);
        }

        private resultOfCancelConfirmationCall(result: ng.resource.IResource<any>) {
            result.$promise.then(() =>
            { this.alertHelperService.addSuccessAlert("Great! You have successfully cancelled the email verification."); },
                () =>
                { this.alertHelperService.addDangerAlert("Drat! There was an error canceling email verification."); });
        }
    }

}

module BohFoundation.Main {
    Register.UserManagement.controller("ConfirmCancelCtrl", UserAccount.ChangeEmail.Controllers.ConfirmCancelCtrl)
        .config(['$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
            $routeProvider.when('/UserAccount/ChangeEmail/Cancel/:confirmationCode', {
                templateUrl: '/AngularApp/UserAccount/ChangeEmail/Templates/CancelTemplate.html',
                controller: 'ConfirmCancelCtrl',
                publicAccess: true,
                title: 'Cancel E-Mail Confirmation'
            });
        })]);
}