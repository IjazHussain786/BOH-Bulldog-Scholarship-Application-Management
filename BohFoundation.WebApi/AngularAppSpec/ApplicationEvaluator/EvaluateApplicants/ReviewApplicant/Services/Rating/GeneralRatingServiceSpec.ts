module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Spec.Services.Rating {
    describe("GeneralRatingService", () => {
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

            service = new ReviewApplicant.Services.Rating.GeneralRatingService(authRepo, genericResolver);
        });

        describe('getAcademicInformationFromServer', () => {
            beforeEach(() => {
                authRepo.post.returns(privateField);
                result = service.postFirstImpressionRating(applicantsGuid);
            });

            it('should post item with proper uri.', () => {
                sinon.assert.calledWith(authRepo.post, applicantsGuid, "/api/applicationevaluator/evaluatingapplicants/rateapplicants/firstimpression");
            });

            it('should return what the authRepo returns..', () => {
                expect(result).toBe(privateField);
            });
        });

        describe('postFinalOverallRating', () => {
            beforeEach(() => {
                authRepo.post.returns(privateField);
                result = service.postFinalOverallRating(applicantsGuid);
            });

            it('should post item with proper uri.', () => {
                sinon.assert.calledWith(authRepo.post, applicantsGuid, "/api/applicationevaluator/evaluatingapplicants/rateapplicants/final");
            });

            it('should return what the authRepo returns..', () => {
                expect(result).toBe(privateField);
            });
        });

        describe('resolvePostNonEssayRating', () => {
            beforeEach(() => {
                service.resolvePostNonEssayRating(serverMessage);
            });

            it('should call genericResolver with message and "rating update"', () => {
                sinon.assert.calledWith(genericResolver.genericPostResolver, "rating update", serverMessage);
            });
        });
    });
}  