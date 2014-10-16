module BohFoundation.UserAccount.ChangeEmail.Spec.Repositories {
    describe('ChangeEmailRepository', () => {

        var $httpBackend: ng.IHttpBackendService, repo: ChangeEmail.Repositories.IChangeEmailRepository;
        var success = true;
        var error = false;
        var verificationKey = "asdkfjlnklas093an0";

        beforeEach(module('BohFoundation.UserManagement'));

        beforeEach(inject((ChangeEmailRepository, _$httpBackend_) => {
            $httpBackend = _$httpBackend_;
            repo = ChangeEmailRepository;
        }));

        afterEach(() => {
            $httpBackend.verifyNoOutstandingExpectation();
            $httpBackend.verifyNoOutstandingRequest();
        });

        describe('cancelConfirmation', () => {
            it('should return a successful response and call with the right param.', () => {
                $httpBackend.expectPOST('/api/useraccount/changeemail/cancel/' + verificationKey).respond(200);

                expectASuccessForCancelConfirm(true);
            });

            it('should return a error response', () => {
                $httpBackend.expectPOST('/api/useraccount/changeemail/cancel/' + verificationKey).respond(500);

                expectASuccessForCancelConfirm(false);
            });

            function expectASuccessForCancelConfirm(isSuccess: boolean) {
                var returnedValue;
                repo.cancelConfirmation(verificationKey).$promise.then(() => { returnedValue = success; }, () => { returnedValue = error; });

                expect(returnedValue).toBeUndefined();

                $httpBackend.flush();

                if (isSuccess) {
                    expect(returnedValue).toBeTruthy();
                } else {
                    expect(returnedValue).toBeFalsy();
                }
            }
        });

        describe('confirmEmail', () => {
            var verifyEmailModel = new Models.VerifyEmailModel("password", verificationKey);

            it('should return a successful response and call with the right param.', () => {
                $httpBackend.expectPOST('/api/useraccount/changeemail/confirm', verifyEmailModel).respond(200);

                expectASuccessForConfirmEmail(true);
            });

            it('should return a error response', () => {
                $httpBackend.expectPOST('/api/useraccount/changeemail/confirm', verifyEmailModel).respond(500);

                expectASuccessForConfirmEmail(false);
            });

            function expectASuccessForConfirmEmail(isSuccess: boolean) {
                var returnedValue;
                repo.confirmEmail(verifyEmailModel).$promise.then(() => { returnedValue = success; }, () => { returnedValue = error; });

                expect(returnedValue).toBeUndefined();

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