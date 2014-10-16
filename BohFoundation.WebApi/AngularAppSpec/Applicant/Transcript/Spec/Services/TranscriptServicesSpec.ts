module BohFoundation.Applicant.Transcript.Spec.Services {
    describe('TranscriptService', () => {
        var apiEndpoint = '/api/applicant/transcript';
        var typeOfDate = "transcript";

        var authRepo, genericResolver, result, errorResponse, modelFromServer, successResponse, date;
        var service;
        var model;

        beforeEach(() => {
            date = new Date();

            model = new Dtos.Common.LastUpdatedModel(date);
            modelFromServer = { data: model };

            var commonStubs : TestHelpers.ICommonStubs = new TestHelpers.CommonStubs();

            errorResponse = new Common.Models.ServerResponseModel(null, false);
            successResponse = new Common.Models.ServerResponseModel(modelFromServer, true);

            authRepo = commonStubs.createAuthRepo();

            genericResolver = commonStubs.createGenericResolver();

            service = new Transcript.Services.TranscriptService(authRepo, genericResolver);
        });

        describe('getLastUpdated', () => {
            it('should return the date that is on the model.', () => {
                service.lastUpdatedModel = model;
                expect(service.getLastUpdated()).toBe(date);
            });

            it('should return undefined if the model is undefined.', () => {
                expect(service.getLastUpdated()).toBeUndefined();
            });
        });

        describe('getLastUpdatedTranscript', () => {
            describe('get', () => {
                beforeEach(() => {
                    authRepo.get.returns(typeOfDate);
                    result = service.getWhenTranscriptWasLastUpdatedFromServer();
                });

                it('should return whatever the repo returns.', () => {
                    expect(result).toBe(typeOfDate);
                });

                it('should return call repo once.', () => {
                    sinon.assert.calledWith(authRepo.get, apiEndpoint);
                });
            });

            describe('resolve', () => {
                beforeEach(() => {
                    genericResolver.genericGetResolver.returns(model);
                    service.resolveGetLastUpdatedTranscript(errorResponse);
                });

                it('should call the generic resolver.', () => {
                    sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfDate, errorResponse);
                });
                
                it('should set the return item in the service.', () => {
                    expect(service.lastUpdatedModel.lastUpdated).toBe(date);
                });
            });
        });
    });
}