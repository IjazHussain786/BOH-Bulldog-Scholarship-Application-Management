module BohFoundation.Applicant.References.Spec.Services {
    describe('ApplicantReferenceService', () => {
        var apiEndpoint = "/api/applicant/references";
        var typeOfData = "reference";
        
        var service;
        var authRepo, result, errorResponse, modelFromServer, genericResolver;

        beforeEach(() => {
            var commonStubs: TestHelpers.ICommonStubs = new TestHelpers.CommonStubs();

            modelFromServer = { data: null };

            errorResponse = new Common.Models.ServerResponseModel(null, false);

            authRepo = commonStubs.createAuthRepo();

            genericResolver = commonStubs.createGenericResolver();

            service = new References.Services.ApplicantReferenceService(authRepo, genericResolver);
        });

        describe('constructor', () => {
            it('should have an empty arrayOfReferenceModels.', () => {
                expect(service.arrayOfReferenceModels.length).toBe(0);
            });
        });

        describe('getArrayOfReferenceModels', () => {
            it('should returns whatever is on the local field.', () => {
                service.arrayOfReferenceModels = typeOfData;
                expect(service.getArrayOfReferenceModels()).toBe(typeOfData);
            });
        });


        describe('postReferenceRequest', () => {
            describe('post', () => {
                beforeEach(() => {
                    authRepo.post.returns(typeOfData);
                    result = service.postReferenceRequest(errorResponse);
                });

                it('should call authRepo with endpoint string and object to send.', () => {
                    sinon.assert.calledWith(authRepo.post, errorResponse, apiEndpoint);
                });

                it('should return what authRepo get returns.', () => {
                    expect(result).toBe(typeOfData);
                }); 
            });

            describe('resolve', () => {
                it('should pass whatever is passed in to the resolver with the correct information type to the generic resolver.', () => {
                    service.resolvePostReferenceRequest(errorResponse);
                    sinon.assert.calledWith(genericResolver.genericPostResolver, typeOfData, errorResponse);
                });
            });
        });

        describe('getSubmittedReferencesFromServer', () => {
            describe('get', () => {
                beforeEach(() => {
                    authRepo.get.returns(typeOfData);
                    result = service.getSubmittedReferencesFromServer();
                });

                it('should call authRepo with endpoint string.', () => {
                    sinon.assert.calledWith(authRepo.get, apiEndpoint);
                });

                it('should return what authRepo get returns.', () => {
                    expect(result).toBe(typeOfData);
                }); 
            });

            describe('resolve', () => {
                beforeEach(() => {
                    genericResolver.genericGetResolver.returns(modelFromServer);
                    service.resolveGetSubmittedReference(errorResponse);
                });

                it('should put returned object on the arrayOfReferenceModels.', () => {
                    expect(service.arrayOfReferenceModels).toBe(modelFromServer);
                });

                it('should call genericResolve.', () => {
                    sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, errorResponse);
                });
            });
        });
    });
} 