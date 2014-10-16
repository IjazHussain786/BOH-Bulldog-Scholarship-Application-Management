module BohFoundation.Common.Spec.Repositories {
    describe('AllowAnonymousRepository', () => {

        var $httpBackend: ng.IHttpBackendService, repo: Common.Repositories.IAllowAnonymousRepository;
        var objectToSend = { data: 123, stuff: 10, bool: true };
        var apiEndPoint = 'asdfjknjkn302';
        var success = true;
        var error = false;

        beforeEach(module('BohFoundation.Common'));

        beforeEach(inject((AllowAnonymousRepository, _$httpBackend_) => {
            repo = AllowAnonymousRepository;
            $httpBackend = _$httpBackend_;
        }));

        afterEach(() => {
            $httpBackend.verifyNoOutstandingExpectation();
            $httpBackend.verifyNoOutstandingRequest();
        });


        describe('get', () => {
            it('should return a success and call with the right path.', () => {
                $httpBackend.expectGET(apiEndPoint).respond(200);

                expectASuccess(true);
            });

            it('should return a failure and call with the right path.', () => {
                $httpBackend.expectGET(apiEndPoint).respond(500);

                expectASuccess(false);
            });

            function expectASuccess(success:boolean) {
                var returnedValue;
                repo.get(apiEndPoint).$promise.then(() => { returnedValue = success; }, () => { returnedValue = error; });

                expect(returnedValue).toBeUndefined();

                $httpBackend.flush();

                if (success) {
                    expect(returnedValue).toBeTruthy();
                } else {
                    expect(returnedValue).toBeFalsy();
                }
            }
        });

        describe('post', () => {
            it('should return a success response.', () => {
                $httpBackend.expectPOST(apiEndPoint, objectToSend).respond(200);

                var returnedValue;
                repo.post(objectToSend, apiEndPoint).$promise.then(() => { returnedValue = success; }, () => { returnedValue = error; });

                expect(returnedValue).toBe(undefined);

                $httpBackend.flush();

                expect(returnedValue).toBeTruthy();
            });

            it('should return a failure response.', () => {
                $httpBackend.expectPOST(apiEndPoint, objectToSend).respond(500);

                var returnedValue;
                repo.post(objectToSend, apiEndPoint).$promise.then(() => { returnedValue = success; }, () => { returnedValue = error; });

                expect(returnedValue).toBe(undefined);

                $httpBackend.flush();

                expect(returnedValue).toBeFalsy();
            });
        });
    });
} 