module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Spec.Services {
    describe("CurrentApplicantExtracurricularActivitiesService", () => {
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

            service = new ReviewApplicant.Services.CurrentApplicantExtracurricularActivitiesService(authRepo, genericResolver);
        });

        describe('extracurricularActivities', () => {
            describe('getExtracurricularActivitiesInformation', () => {
                it('should return whatever is on the private field.', () => {
                    service.extracurricularActivitiesModel = privateField;
                    expect(service.getExtracurricularActivitiesInformation()).toBe(privateField);
                });
            });

            describe('getExtracurricularActivitiesFromServer', () => {
                beforeEach(() => {
                    authRepo.get.returns(privateField);
                    result = service.getExtracurricularActivitiesFromServer(applicantsGuid);
                });

                it('should call authRepo with propery uri.', () => {
                    sinon.assert.calledWith(authRepo.get, "/api/applicationevaluator/evaluatingapplicants/displayapplication/extracurricularactivities/" + applicantsGuid);
                });

                it('should return what the authRepo returns..', () => {
                    expect(result).toBe(privateField);
                });
            });

            describe('resolveGetExtracurricularActivitiesFromServer', () => {
                var privateField2 = "academic";

                beforeEach(() => {
                    genericResolver.genericGetResolver.returns(privateField2);
                    service.resolveGetExtracurricularActivitiesFromServer(serverMessage);
                });

                it('should call genericResolver with message and "applicants extracurricular activities information"', () => {
                    sinon.assert.calledWith(genericResolver.genericGetResolver, "applicant's extracurricular activities information", serverMessage);
                });

                it('should populate the field with whatever is returned from the resolver.', () => {
                    expect(service.extracurricularActivitiesModel).toBe(privateField2);
                });
            });
        });
    });
} 