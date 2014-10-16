module BohFoundation.UserAccount.Manage.Controllers {
    'use strict';

    export interface IManageUserCtrlScope extends ng.IScope {

        deleteRememberMe(): void;
        deviceRemembers(): boolean;

        usersEmail(): string;

        changePassword(form: ng.IFormController, changePasswordModel: Models.ChangePasswordModel): void;
        changeEmail(form: ng.IFormController, changeEmailModel:Models.ChangeEmailModel): void;

        processing: boolean;

        changePasswordModel: Models.ChangePasswordModel;
        passwordScore: PasswordStrength.Models.PasswordScore;
        goodPasswordExplain: string;
        changeEmailModel: Models.ChangeEmailModel;

        checkScore(string): void;
    }

    export interface IManageUserCtrl {
        $scope: IManageUserCtrlScope;
    }

    export class ManageUserCtrl implements IManageUserCtrl {
        private genericError = 'There was an error processing your request.';

        static $inject = ['$scope', 'UserInformationService', 'AlertHelperService', 'AuthRequiredRepository', 'PasswordStrengthChecker'];
        constructor(public $scope: IManageUserCtrlScope, private userInformationService: Common.Services.UserInformation.IUserInformationService,
            private alertHelperService: Common.Services.UiServices.IAlertHelperServices, private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private passwordStrengthChecker: PasswordStrength.Services.IPasswordStrengthChecker) {
            
            $scope.processing = false;

            $scope.goodPasswordExplain = passwordStrengthChecker.goodPasswordExplaination;

            $scope.deleteRememberMe = () => {
                userInformationService.removeTokenFromLocalStorage();
                alertHelperService.addSuccessAlert('This device will not remember you anymore.');
            };

            $scope.deviceRemembers = () => {
                return userInformationService.getRemembered();
            };

            $scope.usersEmail = () => {
                return userInformationService.getUserEmail();
            };

            $scope.checkScore = (passwordToCheck) => {
                $scope.passwordScore = passwordStrengthChecker.check(passwordToCheck);
            };

            $scope.changeEmail = (form : ng.IFormController, model:Models.ChangeEmailModel) => {
                if (form.$valid && !$scope.processing) {
                    this.deleteRememberMeSilentlyIfUserIsRemembered();
                    this.flipProcessing();
                    var promise = authRequiredRepository.post(model, '/api/useraccount/changeemail/request');
                    this.resolvePromise(promise, 'You will receive an email at ' + model.newEmail + '. Please follow its instructions to confirm your new email address.', this.genericError);
                }
            };

            $scope.changePassword = (form: ng.IFormController, changePasswordModel: Models.ChangePasswordModel) => {
                if (form.$valid && !$scope.processing && $scope.passwordScore.score > 2) {
                    this.flipProcessing();
                    var promise = authRequiredRepository.post(changePasswordModel, '/api/useraccount/changepassword');
                    this.resolvePromise(promise, 'You have successfully changed your password.', this.genericError);
                }
            };
        }

        private deleteRememberMeSilentlyIfUserIsRemembered() {
            if (this.userInformationService.getRemembered()) {
                this.userInformationService.removeTokenFromLocalStorage();
            }
        }

        private flipProcessing() {
            this.$scope.processing = !this.$scope.processing;
        }

        private resolvePromise(promise: ng.resource.IResource<any>, successMessage: string, errorMessage: string) {
            promise.$promise.then(() => {
                 this.alertHelperService.addSuccessAlert(successMessage);
                 this.flipProcessing();
            },
                () => {
                    this.alertHelperService.addDangerAlert(errorMessage);
                    this.flipProcessing();
                });
        }
    }
}

module BohFoundation.Main {
    Register.UserManagement.controller("ManageUserCtrl", UserAccount.Manage.Controllers.ManageUserCtrl);
}