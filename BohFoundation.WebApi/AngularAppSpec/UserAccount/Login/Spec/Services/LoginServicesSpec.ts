module BohFoundation.UserAccount.Login.Spec.Services {

    describe('LoginServices', () => {

        var alertHelperService;
        var loginService: Login.Services.ILoginService;
        var emailAddress = "email@email.com";
        var firstName = "firstName";
        var lastName = "lastName";
        var token = "token";
        var date = new Date("Tue, 22 Apr 2014 00:22:07 GMT");
        var errorMessage = 'there was an error';
        var returnedErrorData;
        var userInformationService;
        var userInformationModel;
        var rememberedOnDevice = true;
        var guid = 'guid';
        var graduatingYear = 1234;
        var $location;

        var applicantUrl = '/Applicant';
        var evaluateApplicantsUrl = '/ApplicationEvaluator';

        beforeEach(() => {

            alertHelperService = sinon.stub({
                addDangerAlert: () => { },
                addSuccessAlert: () => { }

            });

            userInformationService = sinon.stub({
                persistLogin: () => { },
                getLastUrlVisitedBesidesLogin: () => { },
                isApplicant: () => { },
                isApplicationEvaluator: () => { },
                getLoggedIn: () =>{ }
            });

            $location = sinon.stub({
                path: () => { }
            });

            returnedErrorData = { data: { error: errorMessage } };

            loginService = new Login.Services.LoginService(alertHelperService, userInformationService, $location);
            userInformationService.getLastUrlVisitedBesidesLogin.returns('url');

            userInformationModel = new Common.Models.UserInformationModel(token, emailAddress, firstName, lastName, rememberedOnDevice, guid, date, true, false, false, false, graduatingYear);
        });

        describe("constructor", () => {

            it('should check to see if the person is logged in.', () => {
                sinon.assert.calledOnce(userInformationService.getLoggedIn);
            });

            describe('currently logged in.', () => {
                beforeEach(() => {
                    userInformationService.getLoggedIn.returns(true);
                });

                it('should forward an applicant to /application.', () => {
                    userInformationService.isApplicant.returns(true);
                    new Login.Services.LoginService(alertHelperService, userInformationService, $location);
                    sinon.assert.calledWith($location.path, applicantUrl);
                });
                 
                it('should forward an applicationEvaluator to /evaluateApplicants.', () => {
                    userInformationService.isApplicationEvaluator.returns(true);
                    new Login.Services.LoginService(alertHelperService, userInformationService, $location);
                    sinon.assert.calledWith($location.path, evaluateApplicantsUrl);
                });
            });

            describe('not logged in.', () => {
                beforeEach(() => {
                    userInformationService.getLoggedIn.returns(false);
                });

                it('should not call $location.path', () => {
                    sinon.assert.notCalled($location.path);
                });
            });
            
        });

        describe('transformAndPersistLoginToken', () => {

            it('should call alertHelperService with the correct params when there was a failure logging in.', () => {
                expect(alertHelperService.addDangerAlert.calledWithMatch(errorMessage)).toBeFalsy();

                loginService.transformAndPersistLoginToken(createLoginServerResponseModel(false), false);

                expect(alertHelperService.addDangerAlert.calledWithMatch(errorMessage)).toBeTruthy();
            });

            describe('rememberMe', () => {
                beforeEach(() => {
                    loginService.transformAndPersistLoginToken(createLoginServerResponseModel(true), true);
                });

                it('should call createSessionCookie once.', () => {
                    expect(userInformationService.persistLogin.calledWithExactly(userInformationModel)).toBeTruthy();
                });

                loggedInNotice();
            });

            describe('dont rememberMe,', () => {
                beforeEach(() => {

                    loginService.transformAndPersistLoginToken(createLoginServerResponseModel(true), false);
                    userInformationModel.rememberedOnDevice = false;
                });

                it('should call createSessionCookie once.', () => {
                    expect(userInformationService.persistLogin.calledWithExactly(userInformationModel)).toBeTruthy();
                });

                loggedInNotice();
            });

            describe('forward after successful sign in', () => {
                describe('applicant', () => {
                    beforeEach(() => {
                        userInformationService.isApplicant.returns(true);
                        loginService.transformAndPersistLoginToken(createLoginServerResponseModel(true), false);
                    });

                    it('should forward an applicant to /application.', () => {
                        sinon.assert.calledWith($location.path, applicantUrl);
                    });
                });

                describe('applicationEvaluator', () => {
                    beforeEach(() => {
                        userInformationService.isApplicationEvaluator.returns(true);
                        loginService.transformAndPersistLoginToken(createLoginServerResponseModel(true), false);
                    });

                    it('should forward an applicationEvaluator to /evaluateApplicants.', () => {
                        sinon.assert.calledWith($location.path, evaluateApplicantsUrl);
                    });
                });
            });

            describe('undefinedToken', () => {
                var errorMessageUndefined = 'There as an error logging into your account. Sorry. Please try closing your browser and retrying. This happens when you try to log in multiple times quickly.';

                beforeEach(() => {
                    loginService.transformAndPersistLoginToken(createLoginServerResponseModel(true, true), false);
                });

                it('should call addDanger alert', () => {
                    expect(alertHelperService.addDangerAlert.calledWithMatch(errorMessageUndefined)).toBeTruthy();
                });

                it('shouldnt call persistLogin', () => {
                    expect(userInformationService.persistLogin.called).toBeFalsy();
                });

                it('shouldnt forward the user to the location they came from.', () => {
                    expect($location.path.called).toBeFalsy();
                });
            });


            function loggedInNotice() {
                it('should call the addSuccessAlert method once with the message.', () => {
                    sinon.assert.calledWith(alertHelperService.addSuccessAlert, "You are now logged in as " + firstName + " " + lastName + "!");
                });
            }

            describe('user is pending application evaluator', () => {
                beforeEach(() => {
                    var serverToken = {
                        applicationEvaluatorPendingConfirmation: true, emailAddress : emailAddress
                    };
                    var model = new Models.LoginServerResponseModel(serverToken, true);
                    loginService.transformAndPersistLoginToken(model, false);
                });

                it('should not call alertHelpers.', () => {
                    sinon.assert.notCalled(alertHelperService.addSuccessAlert);
                });

                it('should not call persistLogin.', () => {
                    sinon.assert.notCalled(userInformationService.persistLogin);
                });

                it('should forward user to /evaluateapplicants/pendingconfirmation.', () => {
                    sinon.assert.calledWith($location.path, '/EvaluateApplicants/PendingConfirmation');
                });
            });
        });

        function createLoginServerResponseModel(success: boolean, undefinedServerToken: boolean = false) {
            if (!success) {
                return new Models.LoginServerResponseModel(returnedErrorData, success);
            } else {
                return new Models.LoginServerResponseModel(createServerToken(undefinedServerToken), success);
            }
        }

        function createServerToken(undefined: boolean = false) {
            var serverToken;
            if (undefined) {
                serverToken = undefined;
            } else {
                serverToken = {
                    emailAddress: emailAddress,
                    firstName: firstName,
                    lastName: lastName,
                    access_token: token,
                    guid: guid,
                    expires: "Tue, 22 Apr 2014 00:22:07 GMT",
                    applicant: true,
                    admin: false,
                    reference: false,
                    applicationEvaluator: false,
                    applicationEvaluatorPendingConfirmation: false,
                    graduatingYear: graduatingYear
                };
            }
            return serverToken;
        }
    });
}