module BohFoundation.UserAccount.RegisterApplicant.Controllers {
    'use strict';
    export interface IRegisterApplicantCtrlScope extends ng.IScope {
        passwordScore: PasswordStrength.Models.PasswordScore;

        registrationInputModel: Models.RegisterInputModel;
        processing: boolean;

        goodPasswordExplain: string;

        checkScore(string): void;
        submitRegistration(form: ng.IFormController): void;

        graduatingYearArray: Array<number>;
        graduatingYear: number;

        registerForm: ng.IFormController;
    }

    export interface IRegisterApplicantCtrl {
        $scope: IRegisterApplicantCtrlScope;
    }

    export class RegisterApplicantCtrl implements IRegisterApplicantCtrl {

        private registerApplicantApiEndPoint = '/api/useraccount/registerapplicant';

        static $inject = ['$scope', 'PasswordStrengthChecker', 'AllowAnonymousRepository', 'AlertHelperService', 'CreateYearArrayService'];
        constructor(public $scope: IRegisterApplicantCtrlScope, private passwordStrengthChecker: PasswordStrength.Services.IPasswordStrengthChecker,
            private allowAnonymousRepository: Common.Repositories.IAllowAnonymousRepository, private alertHelpers: BohFoundation.Common.Services.UiServices.IAlertHelperServices,
            private createYearArrayService: Common.Services.Utilities.ICreateYearArrayService) {

            this.setGraduationYearOptions();

            $scope.processing = false;

            $scope.checkScore = (password) => {
                this.checkscore(password);
            };
            
            $scope.submitRegistration = (form: ng.IFormController) => {
                if (form.$valid && !$scope.processing && $scope.passwordScore.score > 2) {
                    this.$scope.processing = true;
                    this.submitRegistration($scope.registrationInputModel);
                }
            };

            $scope.goodPasswordExplain = passwordStrengthChecker.goodPasswordExplaination;
        }

        private setGraduationYearOptions() {
            this.$scope.graduatingYearArray = this.createYearArrayService.getThisYearPlusFourArray();
            this.$scope.graduatingYear = this.$scope.graduatingYearArray[0];
        }

        private checkscore(password) {
            this.$scope.passwordScore = this.passwordStrengthChecker.check(password);
        }

        private submitRegistration(registrationModel: Models.RegisterInputModel) {
            registrationModel.graduatingYear = this.$scope.graduatingYear;
            this.allowAnonymousRepository.post(registrationModel, this.registerApplicantApiEndPoint)
                .$promise.then(() => {
                    this.addSuccessMessage(registrationModel);
                },
                (response) => { this.addErrorMessage(response); });
        }

        private addErrorMessage(response) {
            this.alertHelpers.addDangerAlert(response.data.message);
            this.$scope.processing = false;
        }

        private addSuccessMessage(registrationModel: Models.RegisterInputModel) {
            this.alertHelpers.addSuccessAlert("You should be recieve an email soon at " + registrationModel.emailAddress + " to confirm your registration. Please follow the instructions.");
        }

    }
}

module BohFoundation.Main {
    Register.UserManagement.controller("RegisterApplicantCtrl", UserAccount.RegisterApplicant.Controllers.RegisterApplicantCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/UserAccount/RegisterApplicant', {
                    templateUrl: '/AngularApp/UserAccount/RegisterApplicant/Templates/RegisterApplicant.html',
                    controller: 'RegisterApplicantCtrl',
                    publicAccess: true,
                    title: 'Registration'
                });
            })
        ]);
} 