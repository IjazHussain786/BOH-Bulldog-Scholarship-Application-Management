module BohFoundation.UserAccount.Login.Spec.Repos {
    describe('LoginRepository', () => {

        var $httpBackend: ng.IHttpBackendService, repo: Login.Repositories.ILoginRepository;
        var success = true;
        var error = false;
        var loginModel: Models.LoginModel;
        var urlEncode;

        beforeEach(module('BohFoundation.UserManagement'));

        beforeEach(inject((LoginRepository, _$httpBackend_) => {
            $httpBackend = _$httpBackend_;
            repo = LoginRepository;
            loginModel = new Models.LoginModel("email@domain.com", "password", false);
            urlEncode = 'grant_type=password&username=' + loginModel.email + '&password=' + loginModel.password;
        }));

        afterEach(() => {
            $httpBackend.verifyNoOutstandingExpectation();
            $httpBackend.verifyNoOutstandingRequest();
        });

        describe('getToken', () => {
            it('should return a success and call with the right params.', () => {
                $httpBackend.expectPOST('/token', urlEncode, { 'Content-Type': 'application/x-www-form-urlencoded', 'Accept': 'application/json, text/plain, */*' }).respond(200);

                expectASuccessForToken(true);
            });

            it('should return a error message.', () => {
                $httpBackend.expectPOST('/token', urlEncode, { 'Content-Type': 'application/x-www-form-urlencoded', 'Accept': 'application/json, text/plain, */*' }).respond(500);

                expectASuccessForToken(false);
            });

            function expectASuccessForToken(isSuccess: boolean) {
                var returnedValue;
                repo.getToken(loginModel).$promise.then(() => { returnedValue = success; }, () => { returnedValue = error; });

                expect(returnedValue).toBe(undefined);

                $httpBackend.flush();

                if (isSuccess) {
                    expect(returnedValue).toBeTruthy();
                } else {
                    expect(returnedValue).toBeFalsy();
                }
            }
        });
    });
} 