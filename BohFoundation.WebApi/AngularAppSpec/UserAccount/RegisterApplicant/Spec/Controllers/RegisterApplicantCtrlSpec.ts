module BohFoundation.UserAccount.RegisterApplicant.Spec.Controllers {
    describe('RegistrationCtrl', () => {

        var scope, ctrl: RegisterApplicant.Controllers.RegisterApplicantCtrl;
        var mockRepo, createYearArrayService;
        var passwordStrengthChecker;
        var alertHelpers;
        var firstName = 'firstname';
        var lastName = 'lastName';
        var email = 'email';
        var password = 'password';
        var graduatingYear = 2031;
        var graduatingYearArray = [0, 1, 2, 3, 4];
        var passwordScore;
        var registerApplicantApiEndPoint = '/api/useraccount/registerapplicant';

        beforeEach(inject(($rootScope) => {

            scope = $rootScope;

            passwordStrengthChecker = sinon.stub({
                check: () => {},
                goodPasswordExplaination: registerApplicantApiEndPoint
            });

            mockRepo = sinon.stub({
                post: () => { }
            });

            alertHelpers = sinon.stub({
                alerts: [],
                addWarningAlert: () => { },
                addSuccessAlert: () => { },
                addDangerAlert: () => { },
                removeAlert: () => { }
            });

            createYearArrayService = sinon.stub({
                getThisYearPlusFourArray:()=>{}
            });

            scope.registerForm = sinon.stub({});
            passwordScore = { style: "style", score: 4, message: "message" };
            scope.passwordScore = passwordScore;

            createYearArrayService.getThisYearPlusFourArray.returns(graduatingYearArray);
            ctrl = new RegisterApplicant.Controllers.RegisterApplicantCtrl(scope, passwordStrengthChecker, mockRepo, alertHelpers, createYearArrayService);
        }));

        describe('constructor', () => {
            it('should have scope.graduatingYearArray = [0,1,2,3,4].', () => {
                expect(scope.graduatingYearArray).toBe(graduatingYearArray);
            });

            it('should set graduatingYear to the first item in the array.', () => {
                expect(scope.graduatingYear).toBe(0);
            });

            it('should set scope.processing to false as default.', () => {
                expect(scope.processing).toBeFalsy();
            });
        });

        describe('checkScore()', () => {
            beforeEach(() => {
                passwordStrengthChecker.check.returns(passwordScore);
                scope.checkScore(password);
            });

            it("should call to checkscore with the password.", () => {
                sinon.assert.calledWith(passwordStrengthChecker.check, password);
            });

            it('should return a passwordScore object.', () => {
                expect(scope.passwordScore.style).toBe("style");
                expect(scope.passwordScore.message).toBe("message");
                expect(scope.passwordScore.score).toBe(4);
            });
        });

        describe('submitRegistration()', () => {

            var q: ng.IQService;
            var deferred, resource;
            var promise: ng.IPromise<any>;
            var userModel: RegisterApplicant.Models.RegisterInputModel;
            var errorMessage = "errorMessage";
            var returnedErrorData;


            beforeEach(inject(($q) => {
                q = $q;
                deferred = q.defer();
                promise = deferred.promise;
                resource = { $promise: promise };
                returnedErrorData = { data: { message: errorMessage } };

                scope.registerForm.$valid = true;

                userModel = new Models.RegisterInputModel(firstName,lastName,email,password,graduatingYear);
                scope.registrationInputModel = userModel;

                mockRepo.post.returns(resource);
            }));

            it('should trigger an error and post an error message.', () => {
                scope.submitRegistration(scope.registerForm);

                deferred.reject(returnedErrorData);

                expect(alertHelpers.addDangerAlert.calledWith(errorMessage)).toBeFalsy();

                scope.$apply();

                expect(alertHelpers.addDangerAlert.calledWith(errorMessage)).toBeTruthy();
            });

            it('should transform scope.graduatingYear to scope.registrationModel.graduatingYear before submission.', () => {
                scope.graduatingYear = 2;

                scope.submitRegistration(scope.registerForm);

                var shouldMatch = new Models.RegisterInputModel(firstName, lastName, email, password, 2);

                sinon.assert.calledWith(mockRepo.post, shouldMatch, registerApplicantApiEndPoint);
            });

            it('should succeed and post a success message.', () => {
                expect(mockRepo.post.called).toBeFalsy();

                scope.submitRegistration(scope.registerForm);

                expect(mockRepo.post.called).toBeTruthy();

                deferred.resolve();

                expect(alertHelpers.addSuccessAlert.calledWith('You should be recieve an email soon at ' + userModel.emailAddress + ' to confirm your registration. Please follow the instructions.')).toBeFalsy();

                scope.$apply();

                expect(alertHelpers.addSuccessAlert.calledWith('You should be recieve an email soon at ' + userModel.emailAddress + ' to confirm your registration. Please follow the instructions.')).toBeTruthy();
            });

            it('should switch the formSubmitted property when started, but not after a success. No reason to resubmit.', () => {
                expect(scope.processing).toBeFalsy();

                scope.submitRegistration(scope.registerForm);

                expect(scope.processing).toBeTruthy();

                deferred.resolve();

                expect(scope.processing).toBeTruthy();

                scope.$apply();

                expect(scope.processing).toBeTruthy();
            });

            it('should switch the formSubmitted property when there is a error.', () => {
                expect(scope.processing).toBeFalsy();

                scope.submitRegistration(scope.registerForm);

                expect(scope.processing).toBeTruthy();

                deferred.reject(returnedErrorData);

                expect(scope.processing).toBeTruthy();

                scope.$apply();

                expect(scope.processing).toBeFalsy();
            });

            describe('invalid form', () => {
                beforeEach(() => {
                    scope.registerForm.$valid = false;
                    scope.submitRegistration(scope.registerForm);
                });

                it('should not set processing to true.', () => {
                    expect(scope.processing).toBeFalsy();
                });

                it('should not call mockRepo.registerNewMember.', () => {
                    sinon.assert.notCalled(mockRepo.post);
                });
            });

            describe('processing is true', () => {
                beforeEach(() => {
                    scope.processing = true;
                    scope.submitRegistration(scope.registerForm);
                });

                it('should not call mockRepo.registerNewMember.', () => {
                    sinon.assert.notCalled(mockRepo.post);
                });
            });

            describe('passwordScore.score < 2', () => {
                beforeEach(() => {
                    scope.passwordScore.score = 2;
                    scope.submitRegistration(scope.registerForm);
                });

                it('should not call mockRepo.registerNewMember.', () => {
                    sinon.assert.notCalled(mockRepo.post);
                });

                it('should not set processing to true.', () => {
                    expect(scope.processing).toBeFalsy();
                });
            });

            it('should set goodPasswordExplain to registerApplicantApiEndPoint.', () => {
                expect(scope.goodPasswordExplain).toBe(registerApplicantApiEndPoint);
            });
        });
    });
}