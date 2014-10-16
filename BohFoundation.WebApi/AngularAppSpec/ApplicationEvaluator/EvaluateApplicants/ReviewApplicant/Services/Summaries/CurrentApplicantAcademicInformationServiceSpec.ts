module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Spec.Services {
    describe("CurrentApplicantAcademicInformationService", () => {
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

            service = new ReviewApplicant.Services.CurrentApplicantAcademicInformationService(authRepo, genericResolver);
        });

        describe('academicInformation', () => {
            describe('getAcademicInformation', () => {
                it('should return whatever is on the private field.', () => {
                    service.completedAcademicInformationModal = privateField;
                    expect(service.getAcademicInformation()).toBe(privateField);
                });
            });

            describe('getAcademicInformationFromServer', () => {
                beforeEach(() => {
                    authRepo.get.returns(privateField);
                    result = service.getAcademicInformationFromServer(applicantsGuid);
                });

                it('should call authRepo with propery uri.', () => {
                    sinon.assert.calledWith(authRepo.get, "/api/applicationevaluator/evaluatingapplicants/displayapplication/academicinformation/" + applicantsGuid);
                });

                it('should return what the authRepo returns..', () => {
                    expect(result).toBe(privateField);
                });
            });

            describe('resolveGetAcademicInformationFromServer', () => {
                var privateField2 = "generalInformation";

                beforeEach(() => {
                    
                    genericResolver.genericGetResolver.returns(privateField2);
                    service.resolveGetAcademicInformationFromServer(serverMessage);
                });

                it('should call genericResolver with message and "applicants academic information"', () => {
                    sinon.assert.calledWith(genericResolver.genericGetResolver, "applicant's academic information", serverMessage);
                });

                it('should populate the field with whatever is returned from the resolver.', () => {
                    expect(service.completedAcademicInformationModal).toBe(privateField2);
                });
            });
        });
    });
} 