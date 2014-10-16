module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.Spec.Transcript {
    describe("ConfirmTranscriptService", () => {
        var service, authRepo, errorResponse, successResponse, genericResolver, result, window;

        var privateField = "private";
        var applicantsGuid = "guid";
        var serverMessage = "serverMessage";
        var typeOfData = "transcript";

        beforeEach(() => {
            successResponse = new Common.Models.ServerResponseModel(null, true);
            errorResponse = new Common.Models.ServerResponseModel(null, false);

            var commonStubs = new TestHelpers.CommonStubs();


            authRepo = commonStubs.createAuthRepo();
            genericResolver = commonStubs.createGenericResolver();
            window = commonStubs.get$Window();

            service = new Services.Transcript.ConfirmTranscriptService(authRepo, genericResolver, window);
        });

        describe('post', () => {
            describe('postTranscriptConfirmation', () => {
                beforeEach(() => {
                    authRepo.post.returns(privateField);
                    result = service.postTranscriptConfirmation(applicantsGuid);
                });

                it('should post item with proper uri.', () => {
                    sinon.assert.calledWith(authRepo.post, applicantsGuid, "/api/applicationevaluator/evaluatingapplicants/transcript/confirm");
                });

                it('should return what the authRepo returns.', () => {
                    expect(result).toBe(privateField);
                });
            });

            describe('resolvePostTranscriptConfirmation', () => {
                beforeEach(() => {
                    service.resolvePostTranscriptConfirmation(serverMessage);
                });

                it('should call genericResolver with message and "essay rating update"', () => {
                    sinon.assert.calledWith(genericResolver.genericPostResolver, typeOfData, serverMessage);
                });
            });
        });

        describe('get', () => {
            describe('getTranscriptFromServer', () => {
                var blobReference;
                var reference = "reference";
                var extension = "pdf";
                var referencePdf = reference + "." + extension;
                var blobContainerName = "blobContanerName";
                

                beforeEach(() => {
                    blobReference = new Dtos.Applicant.Academic.TranscriptBlobReferenceModel(referencePdf, blobContainerName);

                    authRepo.get.returns(privateField);
                    result = service.getTranscriptFromServer(blobReference);
                });

                it('should get item with proper uri.', () => {
                    sinon.assert.calledWith(authRepo.get, "/api/applicationevaluator/evaluatingapplicants/transcript/geturi/blobcontainername/" + blobContainerName + "/referencetotranscriptwithoutfileextension/" + reference + "/extension/" + extension);
                });

                it('should return what the authRepo returns.', () => {
                    expect(result).toBe(privateField);
                });
            });

            describe('resolveGetTranscriptFromServer', () => {
                describe('success', () => {
                    beforeEach(() => {
                        genericResolver.genericGetResolver.returns(applicantsGuid);
                        service.resolveGetTranscriptFromServer(successResponse);
                    });

                    it('should call location with the new uri that is returned from the resolver.', () => {
                        sinon.assert.calledWith(window.open, applicantsGuid);
                    });

                    it('should call the resolver.', () => {
                        sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, successResponse);
                    });
                });

                describe('failure', () => {
                    beforeEach(() => {
                        service.resolveGetTranscriptFromServer(errorResponse);
                    });

                    it('should not call location.', () => {
                        sinon.assert.notCalled(window.open);
                    });

                    it('should call resolver.', () => {
                        sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, errorResponse);
                    });
                });
            });
        });
    });
}  