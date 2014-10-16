module BohFoundation.Admin.InviteApplicationEvaluator.Controllers {
    'use strict';
    export interface IInviteApplicationEvaluatorCtrlScope extends ng.IScope {
        processing: boolean;
        newSendEmailContact: Dtos.Email.SendEmailContactModel;
        invitePerson(form: ng.IFormController): void;
        inviteApplicationEvaluatorForm: ng.IFormController;
    }

    export interface IInviteApplicationEvaluatorCtrl {
        $scope: IInviteApplicationEvaluatorCtrlScope;
    }

    export class InviteApplicationEvaluatorCtrl implements IInviteApplicationEvaluatorCtrl {
        private inviteApplicationEvaluatorApiEndpoint = '/api/admin/inviteconfirmapplicationevaluator/invite'
        private successMessagePostfix = " has been invited to start a evaluator's account!";

        static $inject = ['$scope', 'AuthRequiredRepository', 'AlertHelperService'];
        constructor(public $scope: IInviteApplicationEvaluatorCtrlScope,
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private alertHelperService: Common.Services.UiServices.IAlertHelperServices) {

            $scope.invitePerson = (form: ng.IFormController) => {
                this.invitePerson(form);
            };
        }

        private invitePerson(form: ng.IFormController) {
             if (form.$valid && !this.$scope.processing) {
                 this.flipProcessing();
                 var resource = this.authRequiredRepository.post(this.$scope.newSendEmailContact, this.inviteApplicationEvaluatorApiEndpoint);
                 this.resolvePromise(resource);
             }
        }

        private flipProcessing() {
            this.$scope.processing = !this.$scope.processing;
        }

        private resolvePromise(resource: ng.resource.IResource<any>) {
            resource.$promise.then(() => {
                this.success();
            },() => {
                this.failure();
            });
        }

        private failure() {
            this.flipProcessing();
            this.alertHelperService.addDangerAlert("There are an error sending the invite. Try again later.");
        }

        private success() {
            this.flipProcessing();
            this.alertHelperService.addSuccessAlert(this.$scope.newSendEmailContact.recipientFirstName + " " + this.$scope.newSendEmailContact.recipientLastName + this.successMessagePostfix);
            this.$scope.newSendEmailContact = undefined;
            this.$scope.inviteApplicationEvaluatorForm.$setPristine();
        }
    }
}

module BohFoundation.Main {
    Register.Admin.controller("InviteApplicationEvaluatorCtrl", Admin.InviteApplicationEvaluator.Controllers.InviteApplicationEvaluatorCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/Admin/InviteApplicationEvaluator', {
                    templateUrl: '/AngularApp/Admin/InviteApplicationEvaluator/Templates/InviteApplicationEvaluator.html',
                    controller: 'InviteApplicationEvaluatorCtrl',
                    publicAccess: false,
                    title: 'Invite Application Evaluators'
                });
            })]);
} 