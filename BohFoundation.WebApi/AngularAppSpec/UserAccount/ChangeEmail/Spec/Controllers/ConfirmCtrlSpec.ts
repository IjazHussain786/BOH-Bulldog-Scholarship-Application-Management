module BohFoundation.UserAccount.ChangeEmail.Spec.Controllers {
    describe('ConfirmCtrl', () => {
        var $routeParams, scope: ChangeEmail.Controllers.IConfirmCtrlScope;
        var ctrl: ChangeEmail.Controllers.IConfirmCtrl;
        var changeEmailRepo;
        var confirmationCode = 'confimationCode-asdfafweva32';
        var q: ng.IQService;
        var deferred, resource, deferred2, resource2;
        var promise: ng.IPromise<any>, promise2: ng.IPromise<any>;
        var errorMessage = "errorMessage";
        var successMessage = "Great! You have successfully confirmed your email address. We will now log you in automatically. Please wait.";
        var returnedErrorData;
        var alertHelpers;
        var username = "username";
        var password = "password";
        var loginService;
        var loginRepo;
        var loginCtrl;
        var form;

        beforeEach(inject(($rootScope, $q) => {
            scope = $rootScope;

            $routeParams = sinon.stub({
                confirmationCode: confirmationCode
            });

            changeEmailRepo = sinon.stub({
                cancelConfirmation: () => { },
                confirmEmail: () => { }
            });

            alertHelpers = sinon.stub({
                addWarningAlert: () => { },
                addSuccessAlert: () => { },
                addDangerAlert: () => { }
            });

            loginService = sinon.stub({
                transformAndPersistLoginToken: () => { }
            });

            loginRepo = sinon.stub({
                getToken: () => { }
            });

            loginCtrl = sinon.stub({
                loginFromOutsideController: () => { }
            });

            form = sinon.stub({ $valid: true });

            q = $q;
            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };
            returnedErrorData = { data: { message: errorMessage } };

            deferred2 = q.defer();
            promise2 = deferred2.promise;
            resource2 = { $promise: promise2 };

            changeEmailRepo.confirmEmail.returns(resource);
            loginRepo.getToken.returns(resource2);

            scope.loginModel = new Login.Models.LoginModel(username, password, false);

            ctrl = new ChangeEmail.Controllers.ConfirmCtrl(scope, alertHelpers, changeEmailRepo, $routeParams, loginService, loginRepo);
        }));

        it('should set verificationKey as the confirmationCode in routeParams.', () => {
            expect(scope.verificationKey).toBe(confirmationCode);
        });

        describe('verifyEmail', () => {

            it('should create an error alert when there is an error.', () => {
                scope.verifyEmail(form);

                expect(changeEmailRepo.confirmEmail.calledWithMatch(new Models.VerifyEmailModel(password, confirmationCode))).toBeTruthy();

                deferred.reject(returnedErrorData);

                expect(alertHelpers.addDangerAlert.calledWith(errorMessage)).toBeFalsy();

                scope.$apply();

                expect(alertHelpers.addDangerAlert.calledWith(errorMessage)).toBeTruthy();
            });

            it('should flag the isWorking boolean as false when there is an error.', () => {
                scope.verifyEmail(form);

                expect(scope.processing).toBeTruthy();

                deferred.reject(returnedErrorData);

                scope.$apply();

                expect(scope.processing).toBeFalsy();
            });

            it('should create a success alert when all goes smoothly.', () => {
                scope.verifyEmail(form);

                expect(changeEmailRepo.confirmEmail.calledWithMatch(new Models.VerifyEmailModel(password, confirmationCode))).toBeTruthy();

                deferred.resolve();

                expect(alertHelpers.addSuccessAlert.calledWith(successMessage)).toBeFalsy();

                scope.$apply();

                expect(alertHelpers.addSuccessAlert.calledWith(successMessage)).toBeTruthy();
            });

            it('should flag the isWorking boolean as true when verifyEmail is submitted.', () => {
                expect(scope.processing).toBeFalsy();

                scope.verifyEmail(form);

                expect(scope.processing).toBeTruthy();
            });

            describe('calling LoginService', () => {
                var loginResponseModel;
                var loginResponse;

                beforeEach(() => {
                    loginResponse = "?af893n";
                    loginResponseModel = new Login.Models.LoginServerResponseModel(loginResponse, false);

                    scope.verifyEmail(form);
                    deferred.resolve();
                    scope.$apply();
                });

                it('should create a LoginServerResponseModel with the results when it is an error.', () => {
                    expect(loginService.transformAndPersistLoginToken.calledWithMatch(loginResponseModel, scope.loginModel.keepMeLoggedIn)).toBeFalsy();

                    deferred2.reject(loginResponse);
                    scope.$apply();

                    expect(loginService.transformAndPersistLoginToken.calledWithMatch(loginResponseModel, scope.loginModel.keepMeLoggedIn)).toBeTruthy();
                });

                it('should create a lognServiceResponseModel with a positive result.', () => {
                    loginResponseModel.success = true;
                    expect(loginService.transformAndPersistLoginToken.calledWithMatch(loginResponseModel, scope.loginModel.keepMeLoggedIn)).toBeFalsy();

                    deferred2.resolve(loginResponse);
                    scope.$apply();

                    expect(loginService.transformAndPersistLoginToken.calledWithMatch(loginResponseModel, scope.loginModel.keepMeLoggedIn)).toBeTruthy();
                });
            });

            describe('invalid form', () => {
                beforeEach(() => {
                    form.$valid = false;
                    scope.verifyEmail(form);
                });
                
                it('should not call changeEmailRepo confirmEmail.', () => {
                    sinon.assert.notCalled(changeEmailRepo.confirmEmail);
                });

                it('should keep processing to false.', () => {
                    expect(scope.processing).toBeFalsy();
                });
            });

            describe('processing is true', () => {
                beforeEach(() => {
                    scope.processing = true;
                    scope.verifyEmail(form);
                });

                it('should not call changeEmailRepo confirmEmail.', () => {
                    sinon.assert.notCalled(changeEmailRepo.confirmEmail);
                });
            });
        });

    });
} 