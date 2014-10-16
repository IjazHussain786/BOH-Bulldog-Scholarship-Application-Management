module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Spec.Services {
    describe("CurrentApplicantOverviewInformationService", () => {
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

            service = new ReviewApplicant.Services.CurrentApplicantOverviewInformationService(authRepo, genericResolver);
        });

        describe('generalInformation', () => {
            describe('getGeneralInformation', () => {
                it('should return whatever is on the private field.', () => {
                    service.generalInformationModel = privateField;
                    expect(service.getGeneralInformation()).toBe(privateField);
                });
            });

            describe('getGeneralInformationFromServer', () => {
                beforeEach(() => {
                    authRepo.get.returns(privateField);
                    result = service.getGeneralInformationFromServer(applicantsGuid);
                });

                it('should call authRepo with propery uri.', () => {
                    sinon.assert.calledWith(authRepo.get, "/api/applicationevaluator/evaluatingapplicants/displayapplication/generalinformation/" + applicantsGuid);
                });

                it('should return what the authRepo returns..', () => {
                    expect(result).toBe(privateField);
                });
            });

            describe('resolveGetGeneralInformationFromServer', () => {
                var privateField2 = "generalInformation";

                beforeEach(() => {
                    
                    genericResolver.genericGetResolver.returns(privateField2);
                    service.resolveGetGeneralInformationFromServer(serverMessage);
                });

                it('should call genericResolver with message and "applicants general information"', () => {
                    sinon.assert.calledWith(genericResolver.genericGetResolver, "applicant's general information", serverMessage);
                });

                it('should populate the field with whatever is returned from the resolver.', () => {
                    expect(service.generalInformationModel).toBe(privateField2);
                });
            });
        });
    });
} 