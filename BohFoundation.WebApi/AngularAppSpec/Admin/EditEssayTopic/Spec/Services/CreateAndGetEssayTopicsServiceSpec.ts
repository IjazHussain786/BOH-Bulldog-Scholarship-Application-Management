module BohFoundation.Admin.EditEssayTopic.Spec.Services {
    describe('CreateAndGetEssayTopics', () => {
        var apiEndpoint = "/api/admin/essaytopic";
        var typeOfData = "essay topics";
        var service;
        var authRepo,  result, errorResponse, modelFromServer;
        var fullArray, essayTopic1, essayTopic2;
        var titleOfEssay = "title", prompt = "prompt", revision1 = new Date(), revision2 = new Date(2013, 2, 10), lastAuthor1, lastAuthor2, years1, years2;
        var firstName = "firstName", lastName = "lastName";
        var successResponse, genericResolver;

        beforeEach(() => {

            successResponse = new Common.Models.ServerResponseModel(null, true);

            genericResolver = new TestHelpers.CommonStubs().createGenericResolver();

            lastAuthor1 = new Dtos.Person.NameModel(firstName + 1, lastName + 1);
            lastAuthor2 = new Dtos.Person.NameModel(firstName + 2, lastName + 2);
            years1 = [1, 2, 3];
            years2 = [4];

            essayTopic1 = new Dtos.Admin.EssayTopics.EssayTopicModel(1, titleOfEssay + 1, prompt + 1, revision1, years1, lastAuthor1);
            essayTopic2 = new Dtos.Admin.EssayTopics.EssayTopicModel(2, titleOfEssay + 2, prompt + 2, revision2, years2, lastAuthor2);

            fullArray = [essayTopic1, essayTopic2];

            modelFromServer = { data: null };

            errorResponse = new Common.Models.ServerResponseModel(null, false);

            authRepo = new TestHelpers.CommonStubs().createAuthRepo();

            service = new EditEssayTopic.Services.CreateAndGetEssayTopicsService(authRepo, genericResolver);
        });

        describe('constructor', () => {
            it('should have an empty essayTopic array.', () => {
                expect(service.essayTopicArray.length).toBe(0);
            });
        });

        describe('get Methods', () => {
            beforeEach(() => {
                service.essayTopicArray = modelFromServer;
            });

            it('should return essayTopicArray.', () => {
                expect(service.getEssayTopicArray()).toBe(modelFromServer);
            });
        });

        describe('postCreateOrModifyEssayTopic', () => {
            var model;

            beforeEach(() => {
                model = new Dtos.Admin.EssayTopics.CreateAndModifyEssayTopicModel(titleOfEssay, prompt);
            });

            describe('post', () => {
                beforeEach(() => {
                    authRepo.post.returns(typeOfData);
                    result = service.postCreateOrModifyEssayTopic(model);
                });

                it('should call auth repo.', () => {
                    sinon.assert.calledWith(authRepo.post, model, apiEndpoint);
                });

                it('should return whatever authrepo returns.', () => {
                    expect(result).toBe(typeOfData);
                });
            });

            describe('resolve', () => {
                it('should pass whatever is passed in to the resolver with the correct information type to the generic resolver.', () => {
                    service.resolvePostCreateOrModifyEssayTopic(successResponse);
                    sinon.assert.calledWith(genericResolver.genericPostResolver, typeOfData, successResponse);
                });
            });
        });

        describe('getEssayTopics', () => {
            describe('get', () => {
                beforeEach(() => {
                    authRepo.get.returns(typeOfData);
                    result = service.getEssayTopicsFromServer();
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
                    genericResolver.genericGetResolver.returns(fullArray);
                    service.resolveGetEssayTopics(successResponse);
                });

                it('should call genericResolver with the success.', () => {
                    sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, successResponse);
                });

                it('should put whatever is returned as the array.', () => {
                    expect(service.essayTopicArray).toBe(fullArray);
                });
            });
        });
    });
} 