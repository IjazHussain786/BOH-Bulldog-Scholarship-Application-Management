module BohFoundation.UserAccount.ResetPassword.Spec.Repos {
    describe('ResetPasswordRepository', () => {

        var $httpBackend: ng.IHttpBackendService, repo: ResetPassword.Repositories.IResetPasswordRepository;
        var success = true;
        var error = false;
        var email = "email@emailinstien.com";
        var key = "key";
        var password = "password";
        var resetPasswordThruEmailModel: Models.ResetPasswordThruEmailModel;
        var changePasswordFromResetKeyModel: Models.ChangePasswordFromResetKeyModel;


        beforeEach(module('BohFoundation.UserManagement'));

        beforeEach(inject((ResetPasswordRepository, _$httpBackend_) => {
            $httpBackend = _$httpBackend_;
            repo = ResetPasswordRepository;

            resetPasswordThruEmailModel = new UserAccount.ResetPassword.Models.ResetPasswordThruEmailModel(email);
            changePasswordFromResetKeyModel = new Models.ChangePasswordFromResetKeyModel(password, key);
        }));

        afterEach(() => {
            $httpBackend.verifyNoOutstandingExpectation();
            $httpBackend.verifyNoOutstandingRequest();
        });

        describe('requestPasswordResetKey', () => {
            it('should return a successful response.', () => {
                $httpBackend.expectPOST('/api/useraccount/changepassword/request', resetPasswordThruEmailModel).respond(200);

                requestPasswordResetKey(true);
            });

            it('should return a successful response.', () => {
                $httpBackend.expectPOST('/api/useraccount/changepassword/request', resetPasswordThruEmailModel).respond(500);

                requestPasswordResetKey(false);
            });

            function requestPasswordResetKey(works: boolean) {
                var returnedValue;
                repo.requestPasswordResetKey(resetPasswordThruEmailModel).$promise.then(() => { returnedValue = success; }, () => { returnedValue = error; });

                expect(returnedValue).toBeUndefined();

                $httpBackend.flush();

                if (works) {
                    expect(returnedValue).toBeTruthy();
                } else {
                    expect(returnedValue).toBeFalsy();
                }
            }
        });

        describe('changePasswordFromResetKey', () => {
            it('should return a successful response.', () => {
                $httpBackend.expectPOST('/api/useraccount/changepassword/withkey', changePasswordFromResetKeyModel).respond(200);

                requestPasswordResetKey(true);
            });

            it('should return a successful response.', () => {
                $httpBackend.expectPOST('/api/useraccount/changepassword/withkey', changePasswordFromResetKeyModel).respond(500);

                requestPasswordResetKey(false);
            });

            function requestPasswordResetKey(works: boolean) {
                var returnedValue;
                repo.changePasswordFromResetKey(changePasswordFromResetKeyModel).$promise.then(() => { returnedValue = success; }, () => { returnedValue = error; });

                expect(returnedValue).toBeUndefined();

                $httpBackend.flush();

                if (works) {
                    expect(returnedValue).toBeTruthy();
                } else {
                    expect(returnedValue).toBeFalsy();
                }
            }
        });

    });
}