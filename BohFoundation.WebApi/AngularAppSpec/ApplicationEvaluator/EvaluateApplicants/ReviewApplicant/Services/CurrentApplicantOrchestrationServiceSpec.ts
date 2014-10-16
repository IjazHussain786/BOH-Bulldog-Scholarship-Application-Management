module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Spec.Services {
    describe("CurrentApplicantOrchestrationService", () => {
        var service, authRepo, errorResponse, successResponse, genericResolver, result;

        var currentApplicantOverviewInformationService, currentExtracurricularService, currentApplicantEssayAndLettersOfRecommendationSummariesService, currentApplicantAcademicInformationService, confirmTranscriptService;
        var generalRatingService, essayRatingService, getAllApplicantsService;
        var applicantsGuid = "guid";
        var privateField = "private";
        var genericRating;

        beforeEach(() => {
            successResponse = new Common.Models.ServerResponseModel(null, true);
            errorResponse = new Common.Models.ServerResponseModel(null, false);

            genericRating = new Dtos.Common.GenericRatingModel(Common.Enums.RatingEnum.APlus, "a");

            var commonStubs = new TestHelpers.CommonStubs();

            authRepo = commonStubs.createAuthRepo();
            genericResolver = commonStubs.createGenericResolver();

            currentApplicantOverviewInformationService = sinon.stub({
                getGeneralInformation : () => {},
                getGeneralInformationFromServer: () => { },
                resolveGetGeneralInformationFromServer: () => { }
            });

            currentExtracurricularService = sinon.stub({
                getExtracurricularActivitiesInformation:() => {},
                getExtracurricularActivitiesFromServer: () => { },
                resolveGetExtracurricularActivitiesFromServer: () => { }
            });

            currentApplicantEssayAndLettersOfRecommendationSummariesService = sinon.stub({
                getEssayAndLetterOfRecommendationSummaries:()=>{},
                getEssayAndLetterOfRecommendationSummariesFromServer: () => { },
                resolveGetEssayAndLetterOfRecommendationSummariesFromServer: () => { }
            });

            currentApplicantAcademicInformationService = sinon.stub({
                getAcademicInformation:() => {},
                getAcademicInformationFromServer: () => { },
                resolveGetAcademicInformationFromServer: () => { }
            });

            essayRatingService = sinon.stub({
                postEssayRating: () => { },
                resolvePostEssayRating: () => { }
            });

            generalRatingService = sinon.stub({
                postFirstImpressionRating:() => {},
                postFinalOverallRating: () => { },
                resolvePostNonEssayRating: () => { }
            });

            confirmTranscriptService = sinon.stub({
                getTranscriptFromServer:() => {},
                resolveGetTranscriptFromServer: () => { },
                postTranscriptConfirmation: () => { },
                resolvePostTranscriptConfirmation: () => { }
            });

            getAllApplicantsService = sinon.stub({
                gotoRandomApplicant: () => { },
                canGotoNextRandomApplicant: () => { },
                getFinalizedApplicantsFromServer:()=>{},
                resolveGetFinalizedApplicants: () => { },
                canGotoSummaryOfRatings: () => { },
                gotoSummaryOfRatings: () => { }
            });

            service = new ReviewApplicant.Services.CurrentApplicantOrchestrationService(currentApplicantOverviewInformationService, currentExtracurricularService, currentApplicantEssayAndLettersOfRecommendationSummariesService, currentApplicantAcademicInformationService, essayRatingService, generalRatingService, confirmTranscriptService, getAllApplicantsService);
            service.applicantsGuid = applicantsGuid;
        });

        describe('getFinalizedApplicantsFromServer', () => {
            describe('get', () => {
                beforeEach(() => {
                    getAllApplicantsService.getFinalizedApplicantsFromServer.returns(essayRatingService);
                    result = service.getFinalizedApplicantsFromServer();
                });

                it('should just call the service.', () => {
                    sinon.assert.calledOnce(getAllApplicantsService.getFinalizedApplicantsFromServer);
                });

                it('should return whatever is returned from the service.', () => {
                    expect(result).toBe(essayRatingService);
                });
            });

            describe('resolve', () => {
                it('should call the service with whatever is passed in.', () => {
                    service.resolveGetFinalizedApplicants(currentApplicantAcademicInformationService);
                    sinon.assert.calledWith(getAllApplicantsService.resolveGetFinalizedApplicants, currentApplicantAcademicInformationService);
                });
            });
        });

        describe('canGotoNextRandomApplicant', () => {
            beforeEach(() => {
                getAllApplicantsService.canGotoNextRandomApplicant.returns(privateField);
                result = service.canGotoNextRandomApplicant();
            });
            it('should return whatever the service returns.', () => {
                expect(result).toBe(privateField);
            });

            it('should call the service with the applicants guid.', () => {
                sinon.assert.calledWith(getAllApplicantsService.canGotoNextRandomApplicant, applicantsGuid);
            });
        });

        describe('gotoNextRandomApplicant', () => {
            it('should call the service.', () => {
                service.gotoNextRandomApplicant();
                sinon.assert.calledOnce(getAllApplicantsService.gotoRandomApplicant);
            });
        });

        describe('gotoSummaryOfAllRatings',()=> {
            it('should call the service.', () => {
                service.gotoSummaryOfAllRatings();
                sinon.assert.calledOnce(getAllApplicantsService.gotoSummaryOfRatings);
            });
        });

        describe('canGotoSummaryOfAllRatings', () => {
            it('should return what the service returns.', () => {
                getAllApplicantsService.canGotoSummaryOfRatings.returns(applicantsGuid);
                expect(service.canGotoSummaryOfAllRatings()).toBe(applicantsGuid);
            });
        });

        describe('setCurrentApplicantsGuid', () => {
            it('should put whatever is passed on into the privateField.', () => {
                service.setCurrentApplicantsGuid(privateField);
                expect(service.applicantsGuid).toBe(privateField);
            });
        });

        describe('transcriptConfirmed', () => {
            var completedAcademicInformation;

            beforeEach(() => {
                completedAcademicInformation = new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.CompletedAcademicInformationModel();
            });

            describe('no academic info', () => {
                it('should return null when academic info is null.', () => {
                    currentApplicantAcademicInformationService.getAcademicInformation.returns(null);
                    expect(service.getTranscriptConfirmed()).toBeNull();
                });
            });

            describe('not checked', () => {
                beforeEach(() => {
                    completedAcademicInformation.transcriptDoesNotMatchDatabaseValues = false;
                    completedAcademicInformation.transcriptMatchesDatabaseValues = false;
                    currentApplicantAcademicInformationService.getAcademicInformation.returns(completedAcademicInformation);
                });

                it('should return true notChecked when two falses come back from server.', () => {
                    expect(service.getTranscriptConfirmed().notChecked).toBeTruthy();
                });

                it('should return false confirmed when it comes back from server.', () => {
                    expect(service.getTranscriptConfirmed().confirmed).toBeFalsy();
                });

                it('should return false notCorrect when it comes back from server.', () => {
                    expect(service.getTranscriptConfirmed().notCorrect).toBeFalsy();
                });
            });

            describe('not correct', () => {
                beforeEach(() => {
                    completedAcademicInformation.transcriptDoesNotMatchDatabaseValues = true;
                    completedAcademicInformation.transcriptMatchesDatabaseValues = false;
                    currentApplicantAcademicInformationService.getAcademicInformation.returns(completedAcademicInformation);
                });

                it('should return false notChecked when two falses come back from server.', () => {
                    expect(service.getTranscriptConfirmed().notChecked).toBeFalsy();
                });

                it('should return false confirmed when it comes back from server.', () => {
                    expect(service.getTranscriptConfirmed().confirmed).toBeFalsy();
                });

                it('should return true notCorrect when it comes back from server.', () => {
                    expect(service.getTranscriptConfirmed().notCorrect).toBeTruthy();
                });
            });

            describe('confirmed', () => {
                beforeEach(() => {
                    completedAcademicInformation.transcriptDoesNotMatchDatabaseValues = false;
                    completedAcademicInformation.transcriptMatchesDatabaseValues = true;
                    currentApplicantAcademicInformationService.getAcademicInformation.returns(completedAcademicInformation);
                });

                it('should return false notChecked when two falses come back from server.', () => {
                    expect(service.getTranscriptConfirmed().notChecked).toBeFalsy();
                });

                it('should return true confirmed when it comes back from server.', () => {
                    expect(service.getTranscriptConfirmed().confirmed).toBeTruthy();
                });

                it('should return false notCorrect when it comes back from server.', () => {
                    expect(service.getTranscriptConfirmed().notCorrect).toBeFalsy();
                });
            });
        });

        describe('generalInformation', () => {
            describe('getGeneralInformation', () => {
                it('should return whatever the server returns.', () => {
                    currentApplicantOverviewInformationService.getGeneralInformation.returns(privateField);
                    expect(service.getGeneralInformation()).toBe(privateField);
                });
            });

            describe('getGeneralInformationFromServer', () => {
                beforeEach(() => {
                    currentApplicantOverviewInformationService.getGeneralInformationFromServer.returns(privateField);
                    result = service.getGeneralInformationFromServer();
                });

                it('should call the generalinformationservice from server method.', () => {
                    sinon.assert.calledWith(currentApplicantOverviewInformationService.getGeneralInformationFromServer, applicantsGuid);
                });

                it('should return whatever the service returns.', () => {
                    expect(result).toBe(privateField);
                });
            });

            describe('resolveGetGeneralInformationFromServer', () => {
                it('should just pass the object on to the service.', () => {
                    service.resolveGetGeneralInformationFromServer(privateField);
                    sinon.assert.calledWith(currentApplicantOverviewInformationService.resolveGetGeneralInformationFromServer, privateField);
                });
            });
        });

        describe('ExtracurricularActivities', () => {
            describe('getExtracurricularActivitiesInformation', () => {
                it('should return whatever the server returns.', () => {
                    currentExtracurricularService.getExtracurricularActivitiesInformation.returns(privateField);
                    expect(service.getExtracurricularActivitiesInformation()).toBe(privateField);
                });
            });

            describe('getExtracurricularActivitiesInformationFromServer', () => {
                beforeEach(() => {
                    currentExtracurricularService.getExtracurricularActivitiesFromServer.returns(privateField);
                    result = service.getExtracurricularActivitiesFromServer();
                });

                it('should call the generalinformationservice from server method.', () => {
                    sinon.assert.calledWith(currentExtracurricularService.getExtracurricularActivitiesFromServer, applicantsGuid);
                });

                it('should return whatever the service returns.', () => {
                    expect(result).toBe(privateField);
                });
            });

            describe('resolveGetGeneralInformationFromServer', () => {
                it('should just pass the object on to the service.', () => {
                    service.resolveGetExtracurricularActivitiesFromServer(privateField);
                    sinon.assert.calledWith(currentExtracurricularService.resolveGetExtracurricularActivitiesFromServer, privateField);
                });
            });
        });

        describe('currentApplicantEssayAndLettersOfRecommendationSummariesService', () => {
            describe('getEssayAndLetterOfRecommendationSummaries', () => {
                it('should return whatever the server returns.', () => {
                    currentApplicantEssayAndLettersOfRecommendationSummariesService.getEssayAndLetterOfRecommendationSummaries.returns(privateField);
                    expect(service.getEssayAndLetterOfRecommendationSummaries()).toBe(privateField);
                });
            });

            describe('getEssayAndLetterOfRecommendationSummariesFromServer', () => {
                beforeEach(() => {
                    currentApplicantEssayAndLettersOfRecommendationSummariesService.getEssayAndLetterOfRecommendationSummariesFromServer.returns(privateField);
                    result = service.getEssayAndLetterOfRecommendationSummariesFromServer();
                });

                it('should call the generalinformationservice from server method.', () => {
                    sinon.assert.calledWith(currentApplicantEssayAndLettersOfRecommendationSummariesService.getEssayAndLetterOfRecommendationSummariesFromServer, applicantsGuid);
                });

                it('should return whatever the service returns.', () => {
                    expect(result).toBe(privateField);
                });
            });

            describe('resolveGetEssayAndLetterOfRecommendationSummariesFromServer', () => {
                it('should just pass the object on to the service.', () => {
                    service.resolveGetEssayAndLetterOfRecommendationSummariesFromServer(privateField);
                    sinon.assert.calledWith(currentApplicantEssayAndLettersOfRecommendationSummariesService.resolveGetEssayAndLetterOfRecommendationSummariesFromServer, privateField);
                });
            });
        });

        describe('CurrentApplicantAcademicInformationService', () => {
            describe('getAcademicInformation', () => {
                it('should return whatever the server returns.', () => {
                    currentApplicantAcademicInformationService.getAcademicInformation.returns(privateField);
                    expect(service.getAcademicInformation()).toBe(privateField);
                });
            });

            describe('getAcademicInformationFromServer', () => {
                beforeEach(() => {
                    currentApplicantAcademicInformationService.getAcademicInformationFromServer.returns(privateField);
                    result = service.getAcademicInformationFromServer();
                });

                it('should call the generalinformationservice from server method.', () => {
                    sinon.assert.calledWith(currentApplicantAcademicInformationService.getAcademicInformationFromServer, applicantsGuid);
                });

                it('should return whatever the service returns.', () => {
                    expect(result).toBe(privateField);
                });
            });

            describe('resolveGetAcademicInformationFromServer', () => {
                it('should just pass the object on to the service.', () => {
                    service.resolveGetAcademicInformationFromServer(privateField);
                    sinon.assert.calledWith(currentApplicantAcademicInformationService.resolveGetAcademicInformationFromServer, privateField);
                });
            });
        });

        describe('EssayRatingService', () => {
            beforeEach(() => {
                service.applicantsGuid = applicantsGuid;
            });

            describe('postEssayRating', () => {
                

                beforeEach(() => {
                    var essayRatingModel = new Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate.EssayRatingModel(1, genericRating);
                    essayRatingService.postEssayRating.returns(privateField);
                    result = service.postEssayRating(essayRatingModel);
                });

                it('should pass on whatever is passed into it.', () => {
                    sinon.assert.calledWith(essayRatingService.postEssayRating, new Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate.EssayRatingModel(1, genericRating, applicantsGuid));
                });

                it('should return what the service returns.', () => {
                    expect(result).toBe(privateField);
                });
            });

            describe('resolvePostEssayRating', () => {
                it('should pass the object to the service.', () => {
                    service.resolvePostEssayRating(applicantsGuid);
                    sinon.assert.calledWith(essayRatingService.resolvePostEssayRating, applicantsGuid);
                });
            });
        });

        describe('ConfirmTranscriptService', () => {
            describe('getTranscriptFromServer', () => {
                var transcriptBlobReference;

                beforeEach(() => {
                    transcriptBlobReference = new Dtos.Applicant.Academic.TranscriptBlobReferenceModel("1", "2");

                    currentApplicantAcademicInformationService.getAcademicInformation.returns(new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.CompletedAcademicInformationModel(null, null, transcriptBlobReference));
                    confirmTranscriptService.getTranscriptFromServer.returns(privateField);
                    result = service.getTranscriptFromServer();
                });

                it('should call getTranscript with the object passed in.', () => {
                    sinon.assert.calledWith(confirmTranscriptService.getTranscriptFromServer, transcriptBlobReference);
                });

                it('should return what the service returns.', () => {
                    expect(result).toBe(privateField);
                });
            });

            describe('resolveGetTranscriptFromServer', () => {
                beforeEach(() => {
                    service.resolveGetTranscriptFromServer(applicantsGuid);
                });

                it('should call service with the object passed in.', () => {
                    sinon.assert.calledWith(confirmTranscriptService.resolveGetTranscriptFromServer, applicantsGuid);
                });
            });

            describe('postTranscriptConfirmation', () => {
                var transcriptConfirmation;

                beforeEach(() => {
                    transcriptConfirmation = new Dtos.ApplicationEvaluator.EvaluatingApplicants.ConfirmTranscriptModel(null, false);
                    service.applicantsGuid = applicantsGuid;

                    confirmTranscriptService.postTranscriptConfirmation.returns(privateField);
                    result = service.postTranscriptConfirmation(transcriptConfirmation);
                });

                it('should call postTranscriptConfirmation with the object passed in.', () => {
                    sinon.assert.calledWith(confirmTranscriptService.postTranscriptConfirmation, new Dtos.ApplicationEvaluator.EvaluatingApplicants.ConfirmTranscriptModel(applicantsGuid, false));
                });

                it('should return what the service returns.', () => {
                    expect(result).toBe(privateField);
                });
            });

            describe('resolvePostTranscriptConfirmation', () => {
                beforeEach(() => {
                    service.resolvePostTranscriptConfirmation(applicantsGuid);
                });

                it('should call service with the object passed in.', () => {
                    sinon.assert.calledWith(confirmTranscriptService.resolvePostTranscriptConfirmation, applicantsGuid);
                });
            });
        });

        describe('GeneralRatingService', () => {
            beforeEach(() => {
                service.applicantsGuid = applicantsGuid;
            });

            describe("postFirstImpressionRating", () => {
                beforeEach(() => {
                    generalRatingService.postFirstImpressionRating.returns(privateField);
                    result = service.postFirstImpressionRating(genericRating);
                });

                it('should pass on whatever is passed into it.', () => {
                    sinon.assert.calledWith(generalRatingService.postFirstImpressionRating, new Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate.RatingWithApplicantsGuidModel(genericRating, applicantsGuid));
                });

                it('should return what the service returns.', () => {
                    expect(result).toBe(privateField);
                });
            });

            describe("postFinalOverallRating", () => {
                beforeEach(() => {
                    generalRatingService.postFinalOverallRating.returns(privateField);
                    result = service.postFinalOverallRating(genericRating);
                });

                it('should pass on whatever is passed into it.', () => {
                    sinon.assert.calledWith(generalRatingService.postFinalOverallRating, new Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate.RatingWithApplicantsGuidModel(genericRating, applicantsGuid));
                });

                it('should return what the service returns.', () => {
                    expect(result).toBe(privateField);
                });
            });

            describe("resolvePostNonEssayRating", () => {
                it('should pass the object to the service.', () => {
                    service.resolvePostNonEssayRating(applicantsGuid);
                    sinon.assert.calledWith(generalRatingService.resolvePostNonEssayRating, applicantsGuid);
                });
            });
        });
    });
} 