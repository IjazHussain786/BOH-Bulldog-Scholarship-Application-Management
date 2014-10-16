module BohFoundation.UserAccount.RegisterApplicationEvaluator.Spec.Controllers {
    describe('RegisterApplicationEvaluatorCtrl', () => {
        var registerApplicationEvaluatorApiEndPoint = '/api/useraccount/registerapplicationevaluator';
        var firstName = 'firstname';
        var lastName = 'lastName';
        var email = 'email';
        var password = 'password';
        var passwordStrengthChecker, alertHelpers, scope: RegisterApplicationEvaluator.Controllers.IRegisterApplicationEvaluatorCtrlScope, mockRepo, routeParams;
        var passwordScore;
        var q;
        var successMessage = "You have successful registered as an evaluator. You will get an email with instructions on what to do next. You will not have access to the site until an admin confirms your identity manually.";
        var errorPrefix = "There was an error: ";


        beforeEach(inject(($rootScope, $q) => {
            scope = $rootScope;

            q = $q;

            passwordStrengthChecker = sinon.stub({
                check: () => { }
            });

            routeParams = sinon.stub({
                firstName: firstName,
                lastName: lastName,
                emailAddress: email
            });

            mockRepo = sinon.stub({
                post:()=>{}
            });

            passwordStrengthChecker = sinon.stub({
                check: () => {},
                goodPasswordExplaination: errorPrefix
            });

            alertHelpers = sinon.stub({
                addSuccessAlert: () => { },
                addDangerAlert: () => { }
            });
            
            passwordScore = { style: "style", score: 4, message: "message" };

            new RegisterApplicationEvaluator.Controllers.RegisterApplicationEvaluatorCtrl(scope, routeParams, mockRepo, passwordStrengthChecker, alertHelpers);
        }));

        describe('contructor', () => {
            it('should set registerInputModels email address.', () => {
                expect(scope.registerInputModel.emailAddress).toBe(email);
            });

            it('should set registerInputModels firstName.', () => {
                expect(scope.registerInputModel.firstName).toBe(firstName);
            });

            it('should set registerInputModels email address.', () => {
                expect(scope.registerInputModel.lastName).toBe(lastName);
            });

            it('should set processing to false.', () => {
                expect(scope.processing).toBeFalsy();
            });

            it('should set goodPasswordExplain to errorPrefix.', () => {
                expect(scope.goodPasswordExplain).toBe(errorPrefix);
            });
        });


        describe('submitRegisteration', () => {

            var form;

            describe('validation', () => {
                describe('passwordScore.score < 2', () => {
                    beforeEach(() => {
                        passwordScore.score = 2;
                        scope.passwordScore = passwordScore;
                        form = { $valid: true };
                        scope.processing = false;
                        scope.registerInputModel.password = password;
                        scope.submitRegistration(form);
                    });

                    it('should not flip processing to true.', () => {
                        expect(scope.processing).toBeFalsy();
                    });

                    it('should not call post.', () => {
                        sinon.assert.notCalled(mockRepo.post);
                    });
                });

                describe('processing is true', () => {
                    beforeEach(() => {
                        scope.passwordScore = passwordScore;
                        form = { $valid: true };
                        scope.processing = true;
                        scope.registerInputModel.password = password;
                        scope.submitRegistration(form);
                    });

                    it('should not call post.', () => {
                        sinon.assert.notCalled(mockRepo.post);
                    });
                });

                describe('invalid form', () => {
                    beforeEach(() => {
                        scope.passwordScore = passwordScore;
                        form = { $valid: false };
                        scope.processing = false;
                        scope.registerInputModel.password = password;
                        scope.submitRegistration(form);
                    });

                    it('should not flip processing to true.', () => {
                        expect(scope.processing).toBeFalsy();
                    });

                    it('should not call post.', () => {
                        sinon.assert.notCalled(mockRepo.post);
                    });
                });
            });

            describe('all valid', () => {
                var deferred, resource;
                var promise: ng.IPromise<any>;
                var errorMessage = "errorMessage";
                var returnedErrorData;

                beforeEach(() => {
                    deferred = q.defer();
                    promise = deferred.promise;
                    resource = { $promise: promise };
                    returnedErrorData = { data: { message: errorMessage } };

                    mockRepo.post.returns(resource);

                    scope.passwordScore = passwordScore;
                    form = { $valid: true };
                    scope.processing = false;
                    scope.registerInputModel.password = password;
                    scope.submitRegistration(form);
                });

                it('should call post with the expected registration model.', () => {
                    var objectToSend = new RegisterApplicant.Models.RegisterInputModel(firstName, lastName, email, password);
                    sinon.assert.calledWith(mockRepo.post, objectToSend, registerApplicationEvaluatorApiEndPoint);
                });

                it('should set processing to true.', () => {
                    expect(scope.processing).toBeTruthy();
                });

                describe('error', () => {
                    beforeEach(() => {
                        deferred.reject(returnedErrorData);
                        scope.$apply();
                    });

                    it('should call alert helper with the error.', () => {
                        sinon.assert.calledWith(alertHelpers.addDangerAlert, errorPrefix + errorMessage);
                    });

                    it('should set processing to false.', () => {
                        expect(scope.processing).toBeFalsy();
                    });
                });

                describe('success', () => {
                    it('should notify the user that they will be recieving and email to confirm their email address.', () => {
                        deferred.resolve();
                        scope.$apply();

                        sinon.assert.calledWith(alertHelpers.addSuccessAlert, successMessage);
                    });
                });
            });
        });

        describe('checkScore', () => {
            beforeEach(() => {
                passwordStrengthChecker.check.returns(passwordScore);
                scope.checkScore(password);
            });

            it('should call checkScore with the password.', () => {
                sinon.assert.calledWith(passwordStrengthChecker.check, password);
            });

            it('should return a passwordScore object.', () => {
                expect(scope.passwordScore.style).toBe("style");
                expect(scope.passwordScore.message).toBe("message");
                expect(scope.passwordScore.score).toBe(4);
            });
        });
    });
}