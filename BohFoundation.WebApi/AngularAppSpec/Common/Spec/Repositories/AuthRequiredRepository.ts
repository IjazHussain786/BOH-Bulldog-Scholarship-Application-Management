module BohFoundation.Common.Spec.Repositories {
    describe('AuthRequiredRepository', () => {
        var $httpBackend: ng.IHttpBackendService, repo: Common.Repositories.IAuthRequiredRepository;
        var success = true;
        var error = false;
        var data = { user: "phillip", sect: "crazy" };
        var path = '/path/of/api/endpoint';
        var authHeader = "iamaauthheader.pleasesendmealong";
        var userInformationService;

        beforeEach(() => {
            userInformationService = sinon.stub({
                getUserAuthHeader: () => { }
            });

            userInformationService.getUserAuthHeader.returns(authHeader);
        });

        beforeEach(module('BohFoundation.Common', (($provide) => {
            $provide.value('UserInformationService', userInformationService);
        })));

        beforeEach(inject((_$httpBackend_, AuthRequiredRepository) => {
            $httpBackend = _$httpBackend_;
            repo = AuthRequiredRepository;
        }));

        afterEach(() => {
            $httpBackend.verifyNoOutstandingExpectation();
            $httpBackend.verifyNoOutstandingRequest();
        });

        describe('delete', () => {
            it('should return a success and call with the right params.', () => {
                $httpBackend.expectDELETE(path, { 'Authorization': authHeader, 'Accept': 'application/json, text/plain, */*'}).respond(200);

                expectASuccessForToken(true);
            });

            it('should return a failure and call with the right params.', () => {
                $httpBackend.expectDELETE(path, { 'Authorization': authHeader, 'Accept': 'application/json, text/plain, */*'}).respond(500);

                expectASuccessForToken(false);
            });

            function expectASuccessForToken(isSuccess: boolean) {
                var returnedValue;
                repo.delete(path).$promise.then(() => { returnedValue = success; }, () => { returnedValue = error; });

                expect(returnedValue).toBe(undefined);

                $httpBackend.flush();

                if (isSuccess) {
                    expect(returnedValue).toBeTruthy();
                } else {
                    expect(returnedValue).toBeFalsy();
                }
            }
        });

        describe('post', () => {
            it('should return a success and call with the right params.', () => {
                $httpBackend.expectPOST(path, data, { 'Authorization': authHeader, 'Accept': 'application/json, text/plain, */*', 'Content-Type': 'application/json;charset=utf-8' }).respond(200);

                expectASuccessForToken(true);
            });

            it('should not return a success and call with the right params.', () => {
                $httpBackend.expectPOST(path, data, { 'Authorization': authHeader, 'Accept': 'application/json, text/plain, */*', 'Content-Type': 'application/json;charset=utf-8' }).respond(500);

                expectASuccessForToken(false);
            });

            function expectASuccessForToken(isSuccess: boolean) {
                var returnedValue;
                repo.post(data, path).$promise.then(() => { returnedValue = success; }, () => { returnedValue = error; });

                expect(returnedValue).toBe(undefined);

                $httpBackend.flush();

                if (isSuccess) {
                    expect(returnedValue).toBeTruthy();
                } else {
                    expect(returnedValue).toBeFalsy();
                }
            }
        });

        describe('get', () => {
            it('should return a success and call with the right path.', () => {
                $httpBackend.expectGET(path, { 'Authorization': authHeader, 'Accept': 'application/json, text/plain, */*' }).respond(200);

                expectASuccessForToken(true);
            });

            it('should not return a success and call with the right path.', () => {
                $httpBackend.expectGET(path, { 'Authorization': authHeader, 'Accept': 'application/json, text/plain, */*' }).respond(500);

                expectASuccessForToken(false);
            });

            function expectASuccessForToken(isSuccess: boolean) {
                var returnedValue;
                repo.get(path).$promise.then(() => { returnedValue = success; }, () => { returnedValue = error; });

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