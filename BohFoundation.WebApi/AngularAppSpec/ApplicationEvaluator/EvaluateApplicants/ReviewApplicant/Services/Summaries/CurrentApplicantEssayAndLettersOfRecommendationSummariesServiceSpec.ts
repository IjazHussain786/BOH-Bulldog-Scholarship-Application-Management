module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Spec.Services {
    describe("CurrentApplicantEssayAndLettersOfRecommendationSummariesService", () => {
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

            service = new ReviewApplicant.Services.CurrentApplicantEssayAndLettersOfRecommendationSummariesService(authRepo, genericResolver);
        });

        describe('essayAndLetterOfRecommendationSummaries', () => {
            describe('getEssayAndLetterOfRecommendationSummaries', () => {
                it('should return whatever is on the private field.', () => {
                    service.essayAndLetterOfRecommendationSummariesModel = privateField;
                    expect(service.getEssayAndLetterOfRecommendationSummaries()).toBe(privateField);
                });
            });

            describe('getEssayAndLetterOfRecommendationSummariesFromServer', () => {
                beforeEach(() => {
                    authRepo.get.returns(privateField);
                    result = service.getEssayAndLetterOfRecommendationSummariesFromServer(applicantsGuid);
                });

                it('should call authRepo with propery uri.', () => {
                    sinon.assert.calledWith(authRepo.get, "/api/applicationevaluator/evaluatingapplicants/displayapplication/collectionessaysandlettersofrecommendation/" + applicantsGuid);
                });

                it('should return what the authRepo returns..', () => {
                    expect(result).toBe(privateField);
                });
            });

            describe('resolveGetExtracurricularActivitiesFromServer', () => {
                var privateField2 = "essay";

                beforeEach(() => {
                    genericResolver.genericGetResolver.returns(privateField2);
                    service.resolveGetEssayAndLetterOfRecommendationSummariesFromServer(serverMessage);
                });

                it('should call genericResolver with message and "applicants essays and letter of recommedation summaries"', () => {
                    sinon.assert.calledWith(genericResolver.genericGetResolver, "applicant's essays and letter of recommedation summaries", serverMessage);
                });

                it('should populate the field with whatever is returned from the resolver.', () => {
                    expect(service.essayAndLetterOfRecommendationSummariesModel).toBe(privateField2);
                });
            });
        });
    });
} 