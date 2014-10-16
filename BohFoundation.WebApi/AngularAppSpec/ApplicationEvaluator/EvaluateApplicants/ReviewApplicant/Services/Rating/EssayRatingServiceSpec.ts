module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Spec.Services.Rating {
    describe("EssayRatingService", () => {
        var service, authRepo, errorResponse, successResponse, genericResolver, result;

        var privateField = "private";
        var applicantsGuid = "guid";
        var serverMessage = "serverMessage";

        beforeEach(() => {
            successResponse = new Common.Models.ServerResponseModel(null, true);
            errorResponse = new Common.Models.ServerResponseModel(null, false);

            var commonStubs = new TestHelpers.CommonStubs();

            authRepo = commonStubs.createAuthRepo();
            genericResolver = commonStubs.createGenericResolver();

            service = new ReviewApplicant.Services.Rating.EssayRatingService(authRepo, genericResolver);
        });

        describe('postEssayRating', () => {
            beforeEach(() => {
                authRepo.post.returns(privateField);
                result = service.postEssayRating(applicantsGuid);
            });

            it('should post item with proper uri.', () => {
                sinon.assert.calledWith(authRepo.post, applicantsGuid, "/api/applicationevaluator/evaluatingapplicants/essay/rating");
            });

            it('should return what the authRepo returns..', () => {
                expect(result).toBe(privateField);
            });
        });

        describe('resolvePostEssayRating', () => {
            beforeEach(() => {
                service.resolvePostEssayRating(serverMessage);
            });

            it('should call genericResolver with message and "essay rating update"', () => {
                sinon.assert.calledWith(genericResolver.genericPostResolver, "essay rating update", serverMessage);
            });
        });
    });
}  