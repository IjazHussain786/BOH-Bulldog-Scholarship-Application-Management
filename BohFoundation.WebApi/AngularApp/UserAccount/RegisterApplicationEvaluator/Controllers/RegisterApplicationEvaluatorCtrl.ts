module BohFoundation.UserAccount.RegisterApplicationEvaluator.Controllers {
    export interface IRegisterApplicationEvaluatorCtrlScope extends ng.IScope {
        registerInputModel: RegisterApplicant.Models.RegisterInputModel;

        processing: boolean;

        passwordScore: PasswordStrength.Models.PasswordScore;

        checkScore(password: string): void;
        goodPasswordExplain: string;

        submitRegistration(form: ng.IFormController): void;
    } 

    export interface IRegisterApplicationEvaluatorCtrl {
        $scope: IRegisterApplicationEvaluatorCtrlScope;
    }

    export interface IRegisterApplicationEvaluatorRoute extends ng.route.IRouteParamsService {
        firstName: string;
        lastName: string;
        emailAddress: string;
    }

    export class RegisterApplicationEvaluatorCtrl implements IRegisterApplicationEvaluatorCtrl {
        private registerApplicationEvaluatorApiEndPoint = '/api/useraccount/registerapplicationevaluator';
        private successMessage = "You have successful registered as an evaluator. You will get an email with instructions on what to do next. You will not have access to the site until an admin confirms your identity manually.";
        private errorMessage = "There was an error: "

        static $inject = ['$scope', '$routeParams', 'AllowAnonymousRepository', 'PasswordStrengthChecker', 'AlertHelperService'];
        constructor(public $scope: IRegisterApplicationEvaluatorCtrlScope,
            private $routeParams: IRegisterApplicationEvaluatorRoute,
            private allowAnonymousRepositor: Common.Repositories.IAllowAnonymousRepository,
            private passwordStrengthChecker: PasswordStrength.Services.IPasswordStrengthChecker,
            private alertHelperService: Common.Services.UiServices.IAlertHelperServices) {

            this.createModelFromRoute();

            $scope.goodPasswordExplain = passwordStrengthChecker.goodPasswordExplaination;

            $scope.checkScore = (password: string) => {
                $scope.passwordScore = passwordStrengthChecker.check(password);
            };

            $scope.submitRegistration = (form: ng.IFormController) => {
                if (this.valid(form)) {
                    this.flipProcessing();
                    this.registerUser();
                }
            };
        }

        private createModelFromRoute() {
            this.$scope.registerInputModel = new RegisterApplicant.Models.RegisterInputModel(this.$routeParams.firstName, this.$routeParams.lastName, this.$routeParams.emailAddress);
        }

        private registerUser(): void {
            var resource = this.allowAnonymousRepositor.post(this.$scope.registerInputModel, this.registerApplicationEvaluatorApiEndPoint);
            resource.$promise.then(() => {
                this.alertHelperService.addSuccessAlert(this.successMessage);
            }, (object) => {
                this.alertHelperService.addDangerAlert(this.errorMessage + object.data.message);
                this.flipProcessing();
            });
        }

        private valid(form: ng.IFormController): boolean {
            if (form.$valid && this.$scope.passwordScore.score > 2 && !this.$scope.processing) {
                return true;
            }
            return false;
        }

        private flipProcessing() {
            this.$scope.processing = !this.$scope.processing;
        }
    }
} 

module BohFoundation.Main {
    Register.UserManagement.controller("RegisterApplicationEvaluatorCtrl", UserAccount.RegisterApplicationEvaluator.Controllers.RegisterApplicationEvaluatorCtrl)
        .config(['$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
            $routeProvider.when('/D49E4466995642C1887C591BB87FDE74/:emailAddress/:firstName/:lastName', {
                templateUrl: '/AngularApp/UserAccount/RegisterApplicationEvaluator/Templates/RegisterApplicationEvaluator.html',
                controller: 'RegisterApplicationEvaluatorCtrl',
                publicAccess: true,
                title: 'Register Application Evaluator'
            });
        })]);
}