module BohFoundation.UserAccount.Manage.Spec.Controllers {
    describe('ManageUserCtrl', () => {
        var scope;
        var ctrl: Manage.Controllers.IManageUserCtrl;
        var alertHelpers;
        var userInformationService;
        var authRequiredRepo;
        var password = "password";
        var newPassword = "newPassword";
        var passwordStrengthChecker;
        var genericErrorMessage = 'There was an error processing your request.';

        beforeEach(inject(($rootScope) => {
            scope = $rootScope;
            alertHelpers = sinon.stub({
                addSuccessAlert: () => { },
                addDangerAlert: () => { }
            });

            userInformationService = sinon.stub({
                removeTokenFromLocalStorage: () => { },
                getRemembered: () => { },
                logOut: () => { },
                getUserEmail: () => { }
            });

            authRequiredRepo = sinon.stub({
                post: () => { }
            });

            passwordStrengthChecker = sinon.stub({
                check: () => { },
                goodPasswordExplaination: password
            });


            ctrl = new Manage.Controllers.ManageUserCtrl(scope, userInformationService, alertHelpers, authRequiredRepo, passwordStrengthChecker);
        }));

        it('processing should default to false.', () => {
            expect(scope.processing).toBeFalsy();
        });

        describe('deleteRememberMe', () => {

            var message = 'This device will not remember you anymore.';
            it('should call removeTokenFromLocalStorage.', () => {
                expect(userInformationService.removeTokenFromLocalStorage.calledOnce).toBeFalsy();
                scope.deleteRememberMe();
                expect(userInformationService.removeTokenFromLocalStorage.calledOnce).toBeTruthy();
            });

            it('should call addSuccessAlert with message.', () => {
                expect(alertHelpers.addSuccessAlert.calledWith(message)).toBeFalsy();
                scope.deleteRememberMe();
                expect(alertHelpers.addSuccessAlert.calledWith(message)).toBeTruthy();
            });
        });

        describe('deviceRemembers', () => {
            it('should call getRemember.', () => {
                expect(userInformationService.getRemembered.calledOnce).toBeFalsy();
                scope.deviceRemembers();
                expect(userInformationService.getRemembered.calledOnce).toBeTruthy();
            });

            it('should return what getRemembered returns.', () => {
                userInformationService.getRemembered.returns(true);
                var result = scope.deviceRemembers();
                expect(result).toBeTruthy();
            });
        });

        describe('usersEmail', () => {
            it('should call getEmail.', () => {
                sinon.assert.notCalled(userInformationService.getUserEmail);
                scope.usersEmail();
                sinon.assert.calledOnce(userInformationService.getUserEmail);
            });

            it('should return what getUserEmail returns.', () => {
                var email = 'email';
                userInformationService.getUserEmail.returns(email);
                var result = scope.usersEmail();
                expect(result).toBe(email);
            });
        });

        describe('checkScore', () => {

            it('should call check on passwordStrengthChecker.', () => {
                expect(passwordStrengthChecker.check.calledWith(password)).toBeFalsy();
                scope.checkScore(password);
                expect(passwordStrengthChecker.check.calledWith(password)).toBeTruthy();
            });

            it('should set the result of the thing to passwordScore.', () => {
                var passwordStrength = new PasswordStrength.Models.PasswordScore("bl", 2, "he");
                passwordStrengthChecker.check.returns(passwordStrength);
                scope.checkScore(password);
                expect(scope.passwordScore).toBe(passwordStrength);
            });

            it('should have the same goodPasswordExplaination.', () => {
                expect(scope.goodPasswordExplain).toBe(password);
            });
        });

        describe('methods that require q', () => {

            var errorMessage = "errorMessage";
            var q: ng.IQService, deferred, promise: ng.IPromise<any>, resource, returnedErrorData;
            var ctrl2;

            beforeEach(inject(($q) => {
                q = $q;
                deferred = q.defer();
                promise = deferred.promise;
                resource = { $promise: promise };
                returnedErrorData = { data: { message: errorMessage } };

                authRequiredRepo.post.returns(resource);

                ctrl2 = new Manage.Controllers.ManageUserCtrl(scope, userInformationService, alertHelpers, authRequiredRepo, passwordStrengthChecker);
            }));

            describe('changeEmail', () => {
                var changeEmailModel;
                var email = "email";

                beforeEach(() => {
                    changeEmailModel = new Models.ChangeEmailModel(email);
                });

                it('should call authRequiredRepo once.', () => {
                    expectCallToAuthRequiredRepo(false);
                    changeEmail(true);
                    expectCallToAuthRequiredRepo(true);
                });

                it('should set processing as soon as the authRepo thing is called.', () => {
                    expect(scope.processing).toBeFalsy();
                    changeEmail(true);
                    expect(scope.processing).toBeTruthy();
                });

                it('should let the user know that there was a success when things go smoothly.', () => {
                    changeEmail(true);

                    deferred.resolve();
                    expect(alertHelpers.addSuccessAlert.calledWith('You will receive an email at ' + changeEmailModel.newEmail + '. Please follow its instructions to confirm your new email address.')).toBeFalsy();
                    scope.$apply();
                    expect(alertHelpers.addSuccessAlert.calledWith('You will receive an email at ' + changeEmailModel.newEmail + '. Please follow its instructions to confirm your new email address.')).toBeTruthy();
                });

                it('should let the user know that there was an error.', () => {
                    changeEmail(true);

                    deferred.reject();
                    expect(alertHelpers.addDangerAlert.calledWith(genericErrorMessage)).toBeFalsy();
                    scope.$apply();
                    expect(alertHelpers.addDangerAlert.calledWith(genericErrorMessage)).toBeTruthy();
                });

                it('should set processing as false after everything is done.', () => {
                    changeEmail(true);

                    deferred.resolve();
                    scope.$apply();

                    expect(scope.processing).toBeFalsy();
                });

                it('should set processing as false after everything is done with error.', () => {
                    changeEmail(true);

                    deferred.reject();
                    scope.$apply();

                    expect(scope.processing).toBeFalsy();
                });

                it('should trigger dont remember me if the user is remembered.', () => {
                    userInformationService.getRemembered.returns(true);
                    expect(userInformationService.removeTokenFromLocalStorage.calledOnce).toBeFalsy();
                    changeEmail(true);
                    expect(userInformationService.removeTokenFromLocalStorage.calledOnce).toBeTruthy();
                });

                it('should trigger dont remember me if the user is not remembered.', () => {
                    userInformationService.getRemembered.returns(false);
                    expect(userInformationService.removeTokenFromLocalStorage.calledOnce).toBeFalsy();
                    changeEmail(true);
                    expect(userInformationService.removeTokenFromLocalStorage.calledOnce).toBeFalsy();
                });

                describe('JS validation', () => {
                    describe('invalid form', () => {
                        beforeEach(() => {
                            changeEmail(false);
                        });

                        it('should not switch processing to true.', () => {
                            expect(scope.processing).toBeFalsy();
                        });

                        it('should not call removeTokenFromLocalStorage.', () => {
                            sinon.assert.notCalled(authRequiredRepo.post);
                        });
                    });

                    describe('is already processing', () => {
                        beforeEach(() => {
                            changeEmail(true, true);
                        });

                        it('should not call removeTokenFromLocalStorage.', () => {
                            sinon.assert.notCalled(authRequiredRepo.post);
                        });
                    });

                });

                function changeEmail(valid: boolean, processing?: boolean) {
                    var form = { $valid: valid };
                    if (processing == true) {
                        scope.processing = processing;
                    }
                    scope.changeEmail(form, changeEmailModel);

                }

                function expectCallToAuthRequiredRepo(called: boolean) {
                    if (called) {
                        expect(authRequiredRepo.post.calledWith(changeEmailModel, "/api/useraccount/changeemail/request")).toBeTruthy();
                    } else {
                        expect(authRequiredRepo.post.calledWith(changeEmailModel, "/api/useraccount/changeemail/request")).toBeFalsy();
                    }
                }
            });


            describe('changePassword', () => {
                var changePasswordModel;

                beforeEach(() => {
                    changePasswordModel = new Models.ChangePasswordModel(password, newPassword);
                });

                it('should call authRequiredRepo once.', () => {
                    changePassword(true);
                    sinon.assert.calledWith(authRequiredRepo.post, changePasswordModel, "/api/useraccount/changepassword");
                });

                it('should set processing as soon as the authRepo thing is called.', () => {
                    expect(scope.processing).toBeFalsy();
                    changePassword(true);
                    expect(scope.processing).toBeTruthy();
                });

                it('should let the user know that there was a success when things go smoothly.', () => {
                    changePassword(true);

                    deferred.resolve();
                    expect(alertHelpers.addSuccessAlert.calledWith('You have successfully changed your password.')).toBeFalsy();
                    scope.$apply();
                    expect(alertHelpers.addSuccessAlert.calledWith('You have successfully changed your password.')).toBeTruthy();
                });

                it('should let the user know that there was an error.', () => {
                    changePassword(true);

                    deferred.reject();
                    expect(alertHelpers.addDangerAlert.calledWith(genericErrorMessage)).toBeFalsy();
                    scope.$apply();
                    expect(alertHelpers.addDangerAlert.calledWith(genericErrorMessage)).toBeTruthy();
                });

                it('should set processing as false after everything is done.', () => {
                    changePassword(true);

                    deferred.resolve();
                    scope.$apply();

                    expect(scope.processing).toBeFalsy();
                });

                it('should set processing as false after everything is done with error.', () => {
                    changePassword(true);

                    deferred.reject();
                    scope.$apply();

                    expect(scope.processing).toBeFalsy();
                });

                describe('JS validation', () => {
                    describe('invalid form', () => {
                        beforeEach(() => {
                            changePassword(false);
                        });

                        it('should not switch processing to true.', () => {
                            expect(scope.processing).toBeFalsy();
                        });

                        it('should not call removeTokenFromLocalStorage.', () => {
                            sinon.assert.notCalled(authRequiredRepo.post);
                        });
                    });

                    describe('is already processing', () => {
                        beforeEach(() => {
                            changePassword(true, true);
                        });

                        it('should not call removeTokenFromLocalStorage.', () => {
                            sinon.assert.notCalled(authRequiredRepo.post);
                        });
                    });

                    describe('is below the 3 password score needed', () => {
                        beforeEach(() => {
                            changePassword(true, false, 2);
                        });

                        it('should not switch processing to true.', () => {
                            expect(scope.processing).toBeFalsy();
                        });

                        it('should not call removeTokenFromLocalStorage.', () => {
                            sinon.assert.notCalled(authRequiredRepo.post);
                        });
                    });

                });

                function expectCallToAuthRequiredRepo(called: boolean) {
                    if (called) {
                        expect(authRequiredRepo.post.calledWith(changePasswordModel, "/api/useraccount/changepassword")).toBeTruthy();
                    } else {
                        expect(authRequiredRepo.post.calledWith(changePasswordModel, "/api/useraccount/changepassword")).toBeFalsy();
                    }
                }

                function changePassword(valid: boolean, processing?: boolean, score?:number) {
                    var form = { $valid: valid };
                    if (processing) {
                        scope.processing = true;
                    }
                    if (score != undefined) {
                        scope.passwordScore = { score: score };
                    } else {
                        scope.passwordScore = { score: 4 };
                    }
                    scope.changePassword(form, changePasswordModel);

                }
            });
        });
    });
} 