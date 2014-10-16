module BohFoundation.UserAccount.ResetPassword.Spec.Controllers {
    describe('ResetPasswordCtrl', () => {
        var scope;
        var ctrl;
        var $routeParams;
        var q: ng.IQService, deferred, promise: ng.IPromise<any>, resource, returnedErrorData;
        var q2: ng.IQService, deferred2, promise2: ng.IPromise<any>, resource2;
        var errorMessage = "errorMessage";
        var alertHelpers;
        var password = "password";
        var key = 'key';
        var username = 'username';
        var loginModel;
        var rememberMe = true;
        var changePasswordFromResetKeyModel: Models.ChangePasswordFromResetKeyModel;
        var resetPasswordRepository, loginRepo, loginService;
        var passwordStrengthChecker;
        var loginModelPassedIn;
        var form;

        beforeEach(inject(($rootScope, $q) => {
            scope = $rootScope;
            form = sinon.stub();
            
            q = $q;
            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };

            q2 = $q;
            deferred2 = q.defer();
            promise2 = deferred2.promise;
            resource2 = { $promise: promise2 };
            returnedErrorData = { data: { message: errorMessage } };

            $routeParams = sinon.stub({
                key: key
            });

            resetPasswordRepository = sinon.stub({
                changePasswordFromResetKey: () => { }
            });

            alertHelpers = sinon.stub({
                addDangerAlert: () => { },
                addSuccessAlert: () => { }
            });

            loginRepo = sinon.stub({
                getToken: () => { }
            });

            loginService = sinon.stub({
                transformAndPersistLoginToken: () => { }
            });

            passwordStrengthChecker = sinon.stub({
                check: () => { },
                goodPasswordExplaination: username
            });

            changePasswordFromResetKeyModel = new Models.ChangePasswordFromResetKeyModel(key, password);
            resetPasswordRepository.changePasswordFromResetKey.returns(resource);
            loginRepo.getToken.returns(resource2);

            ctrl = new ResetPassword.Controllers.ResetPasswordCtrl(scope, $routeParams, resetPasswordRepository, alertHelpers, loginRepo, loginService, passwordStrengthChecker);;

            loginModelPassedIn = new Login.Models.LoginModel(username, null, rememberMe);
            loginModel = new Login.Models.LoginModel(username, password, rememberMe);
        }));

        it('should always set the routeParams key to scope.key', () => {
            expect(scope.changePasswordModel.key).toBe(key);
        });

        it('scope.goodPasswordExplain should equal to services', () => {
            expect(scope.goodPasswordExplain).toBe(username);
        });

        describe('checkScore', () => {

            var passwordScore = new PasswordStrength.Models.PasswordScore("s", 2, "message");

            beforeEach(() => {
                passwordStrengthChecker.check.returns(passwordScore);
            });

            it('should be called with password.', () => {
                expect(passwordStrengthChecker.check.calledWith(password)).toBeFalsy();
                scope.checkScore(password);
                expect(passwordStrengthChecker.check.calledWith(password)).toBeTruthy();
            });

            it('should set scope.passwordScore to the result of the check call.', () => {
                expect(scope.passwordScore).toBeUndefined();
                scope.checkScore(password);
                expect(scope.passwordScore).toBe(passwordScore);
            });
        });

        describe('changePasswordFromResetKey', () => {
            beforeEach(() => {
                form.$valid = true;
                scope.changePasswordModel = new Models.ChangePasswordFromResetKeyModel(key, password);
                scope.loginModel = { email: username };
                scope.passwordScore = new PasswordStrength.Models.PasswordScore("", 4, "");
                changePasswordFromKey();
            });

            it('should call changePasswordFromResetKey when a model is fed into the thing.', () => {
                expect(resetPasswordRepository.changePasswordFromResetKey.calledWith(changePasswordFromResetKeyModel)).toBeTruthy();
            });

            it('should change processing to true.', () => {
                expect(scope.processing).toBeTruthy();
            });

            it('should let the user know when there are errors.', () => {
                deferred.reject();
                scope.$apply();
                expect(alertHelpers.addDangerAlert.calledWith('There was an error processing your request.')).toBeTruthy();
            });

            it('should let the user know when there is a success.', () => {
                deferred.resolve();
                scope.$apply(); 
                expect(alertHelpers.addSuccessAlert.calledWith("Great! You have successfully confirmed your email address. We will now log you in automatically. Please wait.")).toBeTruthy();
            });

            it('should flip the processing bit after a failure.', () => {
                expect(scope.processing).toBeTruthy();
                deferred.reject();
                scope.$apply();
                expect(scope.processing).toBeFalsy();
            });

            it('should add the password to loginModel.', () => {
                expect(scope.loginModel.password).toBe(password);
            });

            describe('login after success', () => {

                var serverResponseModel;
                var token = 'token';

                beforeEach(() => {
                    scope.changePasswordModel = new Models.ChangePasswordFromResetKeyModel(key, password);
                    scope.loginModel = { email: username, keepMeLoggedIn: rememberMe };
                    scope.processing = false;
                    serverResponseModel = new Login.Models.LoginServerResponseModel(token, false);
                    callAndResolve();
                });

                it('should call loginRepo with a transformed loginModel.', () => {
                    sinon.assert.calledWithExactly(loginRepo.getToken, loginModel);
                });

                it('should call loginService with a failure message on failure.', () => {
                    deferred2.reject(token);
                    scope.$apply();
                    sinon.assert.calledWith(loginService.transformAndPersistLoginToken, serverResponseModel, rememberMe);
                });

                it('should call loginService with a success message on success.', () => {
                    deferred2.resolve(token);
                    scope.$apply();
                    serverResponseModel.success = true;
                    sinon.assert.calledWith(loginService.transformAndPersistLoginToken, serverResponseModel, rememberMe);
                });

                function callAndResolve() {
                    changePasswordFromKey();
                    resolveAndApply();
                }

                function resolveAndApply() {
                    deferred.resolve(); 
                    scope.$apply();
                }
            });
        });

        describe('JS validation', () => {
            beforeEach(() => {
                scope.changePasswordModel = new Models.ChangePasswordFromResetKeyModel(key, password);
                scope.loginModel = { email: username };
            });

            describe('form is invalid', () => {
                beforeEach(() => {
                    form.$valid = false;
                    scope.processing = false;
                    changePasswordFromKey();
                });

                it('should not set processing to true.', () => {
                    expect(scope.processing).toBeFalsy();
                });

                it('should not call resetPasswordRepo.', () => {
                    sinon.assert.notCalled(resetPasswordRepository.changePasswordFromResetKey);
                });
            });

            describe('processing is true', () => {
                beforeEach(() => {
                    form.$valid = true;
                    scope.processing = true;
                    
                    changePasswordFromKey();
                });

                it('should not call resetPasswordRepo.', () => {
                    sinon.assert.notCalled(resetPasswordRepository.changePasswordFromResetKey);
                });
            });

            describe('passwordScore.score < 2', () => {
                beforeEach(() => {
                    form.$valid = true;
                    scope.passwordScore = { score: 1 };
                    changePasswordFromKey();
                });

                it('should not set processing to true.', () => {
                    expect(scope.processing).toBeFalsy();
                });

                it('should not call resetPasswordRepo.', () => {
                    sinon.assert.notCalled(resetPasswordRepository.changePasswordFromResetKey);
                });
            });
        });

        function changePasswordFromKey() {
            scope.changePasswordFromResetKey(form);
        }

    });
} 