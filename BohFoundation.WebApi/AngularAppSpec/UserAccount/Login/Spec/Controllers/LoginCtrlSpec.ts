module BohFoundation.UserAccount.Login.Spec.Controllers {
    describe('LoginCtrl', () => {
        var loginCtrl: UserAccount.Login.Controllers.ILoginCtrl;
        var scope;
        var service, repo;
        var loginModel: Models.LoginModel;
        var email = "email@email.com";
        var password = "password";
        var errorMessage = "errorMessage";
        var q: ng.IQService, deferred, promise: ng.IPromise<any>, resource, returnedErrorData;
        var loginResponse;
        var loginResponseModel;
        var alertHelper;

        beforeEach(inject(($rootScope, $q) => {
            scope = $rootScope;

            service = sinon.stub({
                transformAndPersistLoginToken: () => { }
            });

            repo = sinon.stub({
                getToken: () => { }
            });

            alertHelper = sinon.stub({
                alerts: [],
                removeAlert: () => { }
            });

            q = $q;
            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };
            returnedErrorData = { data: { message: errorMessage } };

            repo.getToken.returns(resource);

            loginCtrl = new Login.Controllers.LoginCtrl(scope, service, repo, alertHelper);

            loginModel = new Models.LoginModel(email, password, false);

            loginResponse = "?af893n";
            loginResponseModel = new Login.Models.LoginServerResponseModel(loginResponse, loginModel.keepMeLoggedIn);
        }));

        it('should default to false setProcessing.', () => {
            expect(scope.processing).toBeFalsy();
        });

        describe('scope.logIn', () => {

            beforeEach(() => {
                scope.loginModel = loginModel;
                scope.loginForm = sinon.stub({});
                scope.loginForm.$valid = true;
            });

            it('should have loginModel on scope.', () => {
                expect(scope.loginModel).toBe(loginModel);
            });

            it('should call loginRepo with the loginModel.', () => {
                expect(repo.getToken.calledWithExactly(loginModel)).toBeFalsy();
                logIn();
                expect(repo.getToken.calledWithExactly(loginModel)).toBeTruthy();
            });

            it('should set processing as true.', () => {
                expect(scope.processing).toBeFalsy();
                logIn();
                expect(scope.processing).toBeTruthy();
            });

            it('should create a LoginServerResponseModel with the results when it is an error.', () => {
                expect(service.transformAndPersistLoginToken.calledWithMatch(loginResponseModel, scope.loginModel.keepMeLoggedIn)).toBeFalsy();

                logIn();

                deferred.reject(loginResponse);
                scope.$apply();

                expect(service.transformAndPersistLoginToken.calledWithMatch(loginResponseModel, scope.loginModel.keepMeLoggedIn)).toBeTruthy();
            });

            it('should create a lognServiceResponseModel with a positive result.', () => {
                loginResponseModel.success = true;
                expect(service.transformAndPersistLoginToken.calledWithMatch(loginResponseModel, scope.loginModel.keepMeLoggedIn)).toBeFalsy();

                logIn();

                deferred.resolve(loginResponse);
                scope.$apply();

                expect(service.transformAndPersistLoginToken.calledWithMatch(loginResponseModel, scope.loginModel.keepMeLoggedIn)).toBeTruthy();
            });

            it('should flag processing as done only when I bad result comes up.', () => {
                logIn();
                expect(scope.processing).toBeTruthy();
                deferred.reject(loginResponse);
                scope.$apply();

                expect(scope.processing).toBeFalsy();
            });

            it('should keep processing as true if it is a success.', () => {
                logIn();
                expect(scope.processing).toBeTruthy();
                deferred.resolve(loginResponse);
                scope.$apply();

                expect(scope.processing).toBeTruthy();
            });

            describe('invalid form', () => {
                beforeEach(() => {
                    scope.loginForm.$valid = false;
                    logIn();
                });
                    
                it('should not call loginRepo with the loginModel.', () => {
                    sinon.assert.notCalled(repo.getToken);
                });

                it('should not set processing as true.', () => {
                    expect(scope.processing).toBeFalsy();
                });
            });


            describe('invalid form', () => {
                beforeEach(() => {
                    scope.processing = true;
                    logIn();
                });

                it('should not call loginRepo with the loginModel.', () => {
                    sinon.assert.notCalled(repo.getToken);
                });
            });


            function logIn() {
                scope.logIn(scope.loginForm);
            }
        });
    });
}