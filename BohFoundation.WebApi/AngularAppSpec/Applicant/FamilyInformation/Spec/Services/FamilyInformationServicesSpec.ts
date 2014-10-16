module BohFoundation.Applicant.FamilyInformation.Spec.Services {
    describe("", () => {
        var apiEndpoint = "/api/applicant/familyinformation";
        var typeOfData = "family information";
        var service, authRepo, errorResponse, successResponse, genericResolver;
        var result, resource;

        beforeEach(() => {
            successResponse = new Common.Models.ServerResponseModel(null, true);
            errorResponse = new Common.Models.ServerResponseModel(null, false);

            var commonStubs = new TestHelpers.CommonStubs();

            authRepo = commonStubs.createAuthRepo();
            genericResolver = commonStubs.createGenericResolver();

            resource = { a: { b: { c: 10 } } };

            service = new FamilyInformation.Services.FamilyInformationService(authRepo, genericResolver);
        });

        describe('constructor', () => {
            it('should have an undefined familyInformationModel.', () => {
                expect(service.familyInformationModel).toBeUndefined();
            });
        });

        describe('getFamilyInformationModel', () => {
            it('should return whatever is on the service.', () => {
                service.familyInformationModel = typeOfData;
                expect(service.getFamilyInformationModel()).toBe(typeOfData);
            });
        });

        describe('getFamilyInformationFromServer', () => {
            describe('get', () => {
                beforeEach(() => {
                    authRepo.get.returns(resource);
                    result = service.getFamilyInformationFromServer();
                });

                it('should call get with apiEndpoint.', () => {
                    sinon.assert.calledWith(authRepo.get, apiEndpoint);
                });

                it('should return whatever the get returns.', () => {
                    expect(result).toBe(resource);
                });
            });

            describe('resolve', () => {
                beforeEach(() => {
                    genericResolver.genericGetResolver.returns(apiEndpoint);
                    service.resolveGetFamilyInformationFromServer(errorResponse);
                });

                it('should keep familyInformationModel to be undefined.', () => {
                    expect(service.familyInformationModel).toBe(apiEndpoint);
                });

                it('should call generic resolver.', () => {
                    sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, errorResponse);
                });
            });
        });

        describe('postFamilyInformation', () => {
            describe('post', () => {
                beforeEach(() => {
                    authRepo.post.returns(resource);
                    result = service.postFamilyInformation(typeOfData);
                });

                it('should call post with the endpoint and whatever is passed in.', () => {
                    sinon.assert.calledWith(authRepo.post, typeOfData, apiEndpoint);
                });

                it('should return whatever the post returns.', () => {
                    expect(result).toBe(resource);
                });
            });

            describe('resolve', () => {
                beforeEach(() => {
                    service.resolvePostFamilyInformation(successResponse);
                });

                it('should pass successResponse on to the genericResolver.', () => {
                    sinon.assert.calledWith(genericResolver.genericPostResolver, typeOfData, successResponse);
                });
            });
        });
    });
} 