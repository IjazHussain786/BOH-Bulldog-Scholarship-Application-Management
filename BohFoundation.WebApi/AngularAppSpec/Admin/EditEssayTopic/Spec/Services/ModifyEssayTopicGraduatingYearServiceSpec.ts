module BohFoundation.Admin.EditEssayTopic.Spec.Services {
    describe('ModifyEssayTopicGraduatingYearService', () => {
        var apiEndpoint = "/api/admin/essaytopic/graduatingyear";
        var typeOfData = "update graduating class essay topics";
        var service;
        var authRepo, result, errorResponse, modelFromServer;
        var classYear = 2013, topicId = 1293;
        var editClassYearTopicModel, genericResolver;

        beforeEach(() => {
            editClassYearTopicModel = new Dtos.Admin.EssayTopics.EditEssayTopicByGraduatingClassModel(topicId, classYear);

            modelFromServer = { data: null };

            genericResolver = new TestHelpers.CommonStubs().createGenericResolver();

            errorResponse = new Common.Models.ServerResponseModel(null, false);

            authRepo = new TestHelpers.CommonStubs().createAuthRepo();

            service = new EditEssayTopic.Services.ModifyEssayTopicGraduatingYearService(authRepo, genericResolver);
        });

        describe('postAddYearToTopic', () => {
            describe('post', () => {
                beforeEach(() => {
                    authRepo.post.returns(typeOfData);
                    result = service.postAddYearToTopic(editClassYearTopicModel);
                });

                it('should call auth repo.', () => {
                    sinon.assert.calledWith(authRepo.post, editClassYearTopicModel, apiEndpoint);
                });

                it('should return whatever authrepo returns.', () => {
                    expect(result).toBe(typeOfData);
                });
            });

            describe('resolve', () => {
                it('should pass whatever is passed in to the resolver with the correct information type to the generic resolver.', () => {
                    service.resolvePostAddYearToTopic(errorResponse);
                    sinon.assert.calledWith(genericResolver.genericPostResolver, typeOfData, errorResponse);
                });
            });
        });

        describe('deleteYearToTopic', () => {
            describe('delete', () => {
                beforeEach(() => {
                    authRepo.delete.returns(typeOfData);
                    result = service.deleteYearToTopic(editClassYearTopicModel);
                });

                it('should call auth repo.', () => {
                    sinon.assert.calledWith(authRepo.delete, apiEndpoint + "/" + topicId + "/" + classYear);
                });

                it('should return whatever authrepo returns.', () => {
                    expect(result).toBe(typeOfData);
                });
            });

            describe('resolve', () => {
                it('should pass whatever is passed in to the resolver with the correct information type to the generic resolver.', () => {
                    service.resolveDeleteYearToTopic(errorResponse);
                    sinon.assert.calledWith(genericResolver.genericPostResolver, typeOfData, errorResponse);
                });
            });
        });
    });
}