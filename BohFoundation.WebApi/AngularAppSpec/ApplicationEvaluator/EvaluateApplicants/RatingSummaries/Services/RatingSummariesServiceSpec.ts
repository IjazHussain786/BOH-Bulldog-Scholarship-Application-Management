module BohFoundation.ApplicationEvaluator.EvaluateApplicants.RatingSummaries.Spec.Services {
     describe("RatingSummariesService", () => {
         var apiEndpoint = "/api/applicationevaluator/evaluatingapplicants/ratingSummaries";
         var typeOfData = "applicant's average ratings";
         var service, authRepo, errorResponse, successResponse, genericResolver, location;
         var result;

        beforeEach(() => {
            successResponse = new Common.Models.ServerResponseModel(null, true);
            errorResponse = new Common.Models.ServerResponseModel(null, false);

            var commonStubs = new TestHelpers.CommonStubs();

            authRepo = commonStubs.createAuthRepo();
            genericResolver = commonStubs.createGenericResolver();
            location = commonStubs.create$Location();

            service = new RatingSummaries.Services.RatingSummariesService(authRepo, genericResolver, location);
        });
        
        describe('gotoApplicant', () => {
            var applicantSummary1, applicantSummary2, applicantSummary3;
            var guid1 = "guid1", guid2 = "guid2", guid3 = 'guid3';

            beforeEach(() => {
                applicantSummary1 = new Dtos.ApplicationEvaluator.RatingSummary.TopApplicantRatingSummaryModel(null, null, guid1);
                applicantSummary2 = new Dtos.ApplicationEvaluator.RatingSummary.TopApplicantRatingSummaryModel(null, null, guid2);
                applicantSummary3 = new Dtos.ApplicationEvaluator.RatingSummary.TopApplicantRatingSummaryModel(null, null, guid3);

                var applicantSummaries = [applicantSummary1, applicantSummary2, applicantSummary3];

                service.top5ApplicantsModel = new Dtos.ApplicationEvaluator.RatingSummary.Top5ApplicantsModel(applicantSummaries);
            });

            it('should redirect to right url.', () => {
                service.gotoApplicant(0);
                sinon.assert.calledWith(location.path, "/ApplicationEvaluator/ReviewApplicant/" + guid1);
            });

            it('should redirect to right url.', () => {
                service.gotoApplicant(1);
                sinon.assert.calledWith(location.path, "/ApplicationEvaluator/ReviewApplicant/" + guid2);
            });

            it('should redirect to right url.', () => {
                service.gotoApplicant(2);
                sinon.assert.calledWith(location.path, "/ApplicationEvaluator/ReviewApplicant/" + guid3);
            });
        });

        describe('getMethods', () => {
            describe('modelOnService is defined', () => {
                it('should return whatever is on the service.', () => {
                    var array = [new Dtos.ApplicationEvaluator.RatingSummary.TopApplicantRatingSummaryModel()];
                    var model = new Dtos.ApplicationEvaluator.RatingSummary.Top5ApplicantsModel(array);
                    service.top5ApplicantsModel = model;
                    expect(service.getTop5Applicants()).toBe(array);
                });
            });

            describe('modelOnService is not defined', () => {
                describe('getTop5Applicants', () => {
                    it('should return [].', () => {
                        expect(service.getTop5Applicants().length).toBe(0);
                    });
                });
            });
        });

         describe('getRatingSummariesFromServer', () => {
            describe('get', () => {
                beforeEach(() => {
                    authRepo.get.returns(typeOfData);
                    result = service.getRatingSummariesFromServer();
                });

                it('should call authRepo with endpoint string.', () => {
                    sinon.assert.calledWith(authRepo.get, apiEndpoint);
                });

                it('should return what authRepo get returns.', () => {
                    expect(result).toBe(typeOfData);
                });
            });

            describe('resolve', () => {
                describe('error', () => {
                    beforeEach(() => {
                        service.resolveGetRatingSummariesFromServer(errorResponse);
                    });

                    it('should call genericGetResolver.', () => {
                        sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, errorResponse);
                    });
                });

                describe('success', () => {
                    beforeEach(() => {
                        genericResolver.genericGetResolver.returns(errorResponse);
                        service.resolveGetRatingSummariesFromServer(successResponse);
                    });

                    it('should call genericGetResolver.', () => {
                        sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, successResponse);
                    });

                    it('should put whatever is returns as a property.', () => {
                        expect(service.top5ApplicantsModel).toBe(errorResponse);
                    });
                });
            });
        });
    });
} 