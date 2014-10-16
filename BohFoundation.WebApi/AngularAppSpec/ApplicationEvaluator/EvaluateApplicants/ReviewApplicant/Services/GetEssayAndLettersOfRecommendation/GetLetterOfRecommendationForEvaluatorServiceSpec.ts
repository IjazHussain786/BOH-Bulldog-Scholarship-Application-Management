module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.Spec.GetEssayAndLettersOfRecommendation {
    describe("EssayRatingService", () => {
        var service, authRepo, errorResponse, successResponse, genericResolver, result;

        var letterOrRecommendationKey;
        var partitionKey = "part";
        var rowKey = "row";

        var privateField = "privateField";

        beforeEach(() => {
            successResponse = new Common.Models.ServerResponseModel(null, true);
            errorResponse = new Common.Models.ServerResponseModel(null, false);

            var commonStubs = new TestHelpers.CommonStubs();

            letterOrRecommendationKey = new Dtos.Common.AzureQueuryObjects.AzureTableStorageEntityKeyModel(partitionKey, rowKey);

            authRepo = commonStubs.createAuthRepo();
            genericResolver = commonStubs.createGenericResolver();

            service = new Services.GetEssayAndLettersOfRecommendation.GetLetterOfRecommendationForEvaluatorService(authRepo, genericResolver);
        });

        describe('getLetterOfRecommendation', () => {
            it('should return whatever is on the service.', () => {
                var letter = "letter";
                var letterOfRecommendation = new Dtos.Reference.Anonymous.LetterOfRecommendationModel(letter, null);
                service.letterOfRecommendation = letterOfRecommendation;
                expect(service.getLetterOfRecommedation()).toBe(letter);
            });

            it('should return null if letter of recommendationDto is undefined.', () => {
                service.letterOfRecommendation = undefined;
                expect(service.getLetterOfRecommedation()).toBeNull();
            });
        });

        describe('getLetterOfRecommendationFromServer', () => {
            beforeEach(() => {
                authRepo.get.returns(privateField);
                result = service.getLetterOfRecommendationFromServer(letterOrRecommendationKey);
            });

            it('should get item with proper uri.', () => {
                sinon.assert.calledWith(authRepo.get, "/api/applicationevaluator/evaluatingapplicants/displayapplication/letterofrecommendation/partitionkey/" + partitionKey + "/rowkey/" + rowKey);
            });

            it('should return what the authRepo returns..', () => {
                expect(result).toBe(privateField);
            });
        });

        describe('resolveGetLetterOfRecommendation', () => {
            beforeEach(() => {
                genericResolver.genericGetResolver.returns(privateField);
                service.resolveGetLetterOfRecommendation(rowKey);
            });

            it('should call genericResolver with message and "letter of recommendation"', () => {
                sinon.assert.calledWith(genericResolver.genericGetResolver, "letter of recommendation", rowKey);
            });

            it('should set whatever is returned on the private field.', () => {
                expect(service.letterOfRecommendation).toBe(privateField);
            });
        });
    });
}  