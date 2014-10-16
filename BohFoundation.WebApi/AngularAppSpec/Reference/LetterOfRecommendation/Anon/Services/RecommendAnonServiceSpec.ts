module BohFoundation.Reference.LetterOfRecommendation.Anon.Spec.Services {
    describe('RecommendAnonServiceSpec', () => {
        var letterOfRecommendationInformationType = "letter of recommendation";
        var personalInfoType = "personal information";
        var letterOfRecommendationApiEndpoint = "/api/references/anon/letterofrecommendation";
        var letterOfRecommendationInformationApiEndpoint = "/api/references/anon/letterofrecommendationinformation";
        var allowAnonymousRepository, result, errorResponse, successResponse, genericResolver;
        var service;
        var guid = 'guid';

        beforeEach(() => {
            errorResponse = new Common.Models.ServerResponseModel(null, false);
            successResponse = new Common.Models.ServerResponseModel(null, true);

            var commonStubs = new TestHelpers.CommonStubs();

            allowAnonymousRepository = commonStubs.getAllowAnonymousRepositoryStub();

            genericResolver = commonStubs.createGenericResolver();
            
            service = new Anon.Services.RecommendAnonService(allowAnonymousRepository, genericResolver);
        });

        describe('setGuidOfLetterOfRecommendation', () => {
            beforeEach(() => {
                service.setGuidOfLetterOfRecommendation(guid);
            });

            it('should set whatever is passed in on the guidOfLetterOrRecommendation field.', () => {
                expect(service.guidOfLetterOfRecommendation).toBe(guid);
            });
        });

        describe('getInformationForReferenceFormModel', () => {
            beforeEach(() => {
                service.informationForReferenceFormModel = letterOfRecommendationInformationType;
            });

            it('should return whatever is on the informationForReferenceFormModel field.', () => {
                expect(service.getInformationForReferenceFormModel()).toBe(letterOfRecommendationInformationType);
            });
        });

        describe('postLetterOfRecommendation', () => {
            describe('post', () => {
                var letter = "letter";

                beforeEach(() => {
                    allowAnonymousRepository.post.returns(letterOfRecommendationApiEndpoint);
                    service.guidOfLetterOfRecommendation = guid;
                    result = service.postLetterOfRecommendation(letter);
                });

                it('should pass a model to the post with the guid on board.', () => {
                    sinon.assert.calledWith(allowAnonymousRepository.post, new Dtos.Reference.Anonymous.LetterOfRecommendationModel(letter, guid), letterOfRecommendationApiEndpoint);
                });

                it('should return whatever the repo returns.', () => {
                    expect(result).toBe(letterOfRecommendationApiEndpoint);
                });
            });

            describe('resolve', () => {
                it('should pass whatever is passed in to the resolver with the correct information type to the generic resolver.', () => {
                    service.resolvePostLetterOfRecommendation(successResponse);
                    sinon.assert.calledWith(genericResolver.genericPostResolver, letterOfRecommendationInformationType, successResponse);
                });
            });
        });

        describe('postReferencePersonalInformation', () => {
            describe('post', () => {
                var model;
                var occupation = "plumber";

                beforeEach(() => {
                    allowAnonymousRepository.post.returns(letterOfRecommendationInformationType);
                    service.guidOfLetterOfRecommendation = guid;
                    model = new Dtos.Reference.Anonymous.ReferencePersonalInformationModel(undefined, occupation);
                    result = service.postReferencePersonalInformation(model);
                });

                it('should pass a model to the post with guid on board.', () => {
                    sinon.assert.calledWith(allowAnonymousRepository.post, new Dtos.Reference.Anonymous.ReferencePersonalInformationModel(undefined, occupation, undefined, guid), letterOfRecommendationInformationApiEndpoint);
                });

                it('should return whatever the repo returns.', () => {
                    expect(result).toBe(letterOfRecommendationInformationType);
                });
            });

            describe('resolve', () => {
                it('should pass whatever is passed in to the resolver with the correct information type to the generic resolver.', () => {
                    service.resolvePostReferencePersonalInformation(false);
                    sinon.assert.calledWith(genericResolver.genericPostResolver, personalInfoType, false);
                });
            });
        });

        describe('getLetterOfRecommendationInformation', () => {
            describe('get', () => {
                beforeEach(() => {
                    service.guidOfLetterOfRecommendation = guid;
                    allowAnonymousRepository.get.returns(letterOfRecommendationInformationType);
                    result = service.getLetterOfRecommendationInformation();
                });

                it('should call get with the guid.', () => {
                    sinon.assert.calledWith(allowAnonymousRepository.get, letterOfRecommendationInformationApiEndpoint + "/" + guid);
                });

                it('should return whatever the repo returns.', () => {
                    expect(result).toBe(letterOfRecommendationInformationType);
                });
            });

            describe('resolve', () => {
                describe('error', () => {
                    beforeEach(() => {
                        genericResolver.genericGetResolver.returns(null);
                        service.resolveGetLetterOfRecommendationInformation(errorResponse);
                    });

                    it('should call genericResolver.', () => {
                        sinon.assert.calledWith(genericResolver.genericGetResolver, letterOfRecommendationInformationType, errorResponse);
                    });

                    it('should have an undefined informationForReferenceFormModel.', () => {
                        expect(service.informationForReferenceFormModel).toBeUndefined();
                    });
                });

                describe('success', () => {
                    beforeEach(() => {
                        genericResolver.genericGetResolver.returns(allowAnonymousRepository);
                        service.resolveGetLetterOfRecommendationInformation(successResponse);
                    });

                    it('should set the item in the success response to informationForReferenceFormModel.', () => {
                        expect(service.informationForReferenceFormModel).toBe(allowAnonymousRepository);
                    });

                    it('should call genericResolver.', () => {
                        sinon.assert.calledWith(genericResolver.genericGetResolver, letterOfRecommendationInformationType, successResponse);
                    });
                });
            });
        });
    });
} 