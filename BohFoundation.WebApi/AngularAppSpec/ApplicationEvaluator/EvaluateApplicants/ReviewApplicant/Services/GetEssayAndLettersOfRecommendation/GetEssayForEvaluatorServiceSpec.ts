module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.Spec.GetEssayAndLettersOfRecommendation {
    describe("GetEssayForEvaluatorService", () => {
        var service, authRepo, errorResponse, successResponse, genericResolver, result;

        var essayKey;
        var partitionKey = "part";
        var rowKey = "row";

        var privateField = "privateField";

        beforeEach(() => {
            successResponse = new Common.Models.ServerResponseModel(null, true);
            errorResponse = new Common.Models.ServerResponseModel(null, false);

            var commonStubs = new TestHelpers.CommonStubs();

            essayKey = new Dtos.Common.AzureQueuryObjects.AzureTableStorageEntityKeyModel(partitionKey, rowKey);

            authRepo = commonStubs.createAuthRepo();
            genericResolver = commonStubs.createGenericResolver();

            service = new Services.GetEssayAndLettersOfRecommendation.GetEssayForEvaluatorService(authRepo, genericResolver);
        });

        describe('getEssay', () => {
            it('should return whatever is on the service.', () => {
                var essay = "essay";
                var essayDto = new Dtos.Applicant.Essay.EssayModel(null, null, essay);
                service.essayDto = essayDto;
                expect(service.getEssay()).toBe(essay);
            });

            it('should return null if letter of recommendationDto is undefined.', () => {
                service.essayDto = undefined;
                expect(service.getEssay()).toBeNull();
            });
        });

        describe('getEssayFromServer', () => {
            beforeEach(() => {
                authRepo.get.returns(privateField);
                result = service.getEssayFromServer(essayKey);
            });

            it('should get item with proper uri.', () => {
                sinon.assert.calledWith(authRepo.get, "/api/applicationevaluator/evaluatingapplicants/essay/partitionkey/" + partitionKey + "/rowkey/" + rowKey);
            });

            it('should return what the authRepo returns..', () => {
                expect(result).toBe(privateField);
            });
        });

        describe('resolveEssayFromServer', () => {
            beforeEach(() => {
                genericResolver.genericGetResolver.returns(privateField);
                service.resolveEssayFromServer(rowKey);
            });

            it('should call genericResolver with message and "essay"', () => {
                sinon.assert.calledWith(genericResolver.genericGetResolver, "essay", rowKey);
            });

            it('should set whatever is returned on the private field.', () => {
                expect(service.essayDto).toBe(privateField);
            });
        });
    });
}   