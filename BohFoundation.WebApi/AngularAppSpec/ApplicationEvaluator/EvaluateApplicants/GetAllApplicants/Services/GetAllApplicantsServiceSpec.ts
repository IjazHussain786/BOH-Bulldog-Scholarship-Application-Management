module BohFoundation.ApplicationEvaluator.EvaluateApplicants.GetAllApplicants.Spec.Services {
    describe("GetAllApplicantsService", () => {
        var apiEndpoint = "/api/applicationevaluator/evaluatingapplicants/getlistofapplicants";
        var typeOfData = "finalized applicants";
        var service, authRepo, errorResponse, successResponse, genericResolver, location;
        var result;

        beforeEach(() => {
            successResponse = new Common.Models.ServerResponseModel(null, true);
            errorResponse = new Common.Models.ServerResponseModel(null, false);

            var commonStubs = new TestHelpers.CommonStubs();

            authRepo = commonStubs.createAuthRepo();
            genericResolver = commonStubs.createGenericResolver();
            location = commonStubs.create$Location();

            service = new GetAllApplicants.Services.GetAllApplicantsService(authRepo, genericResolver, location);
        });


        describe('canGotoNextRandomApplicant', () => {
            beforeEach(() => {
                service.allFinalizedApplicantForAGraduatingYearModel = new Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.AllFinalizedApplicantsForAGraduatingYearModel(null, null, null, null, typeOfData);
            });

            it('should return true if something else is passed in.', () => {
                expect(service.canGotoNextRandomApplicant("asdf")).toBeTruthy();
            });

            it('should return false if the same thing is passed in.', () => {
                expect(service.canGotoNextRandomApplicant(typeOfData)).toBeFalsy();
            });

            it('should be false is the guid = 00000000-0000-0000-0000-000000000000', () => {
                service.allFinalizedApplicantForAGraduatingYearModel = new Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.AllFinalizedApplicantsForAGraduatingYearModel(null, null, null, null, "00000000-0000-0000-0000-000000000000");
                expect(service.canGotoNextRandomApplicant()).toBeFalsy();
            });
        });

        describe('canGotoSummaryOfRatings', () => {
            it('should default to false.', () => {
                expect(service.canGotoSummaryOfRatings()).toBeFalsy();
            });

            it('should default to false.', () => {
                service.allFinalizedApplicantForAGraduatingYearModel = new Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.AllFinalizedApplicantsForAGraduatingYearModel(null, null, null, null, "00000000-0000-0030-0000-000000000000");
                expect(service.canGotoSummaryOfRatings()).toBeFalsy();
            });

            it('should be true is the guid = 00000000-0000-0000-0000-000000000000', () => {
                service.allFinalizedApplicantForAGraduatingYearModel = new Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.AllFinalizedApplicantsForAGraduatingYearModel(null, null, null, null, "00000000-0000-0000-0000-000000000000");
                expect(service.canGotoSummaryOfRatings()).toBeTruthy();
            });
        });

        describe('gotoSummaryOfRatings',()=> {
            it('should redirect to the summary uri.', () => {
                service.gotoSummaryOfRatings();
                sinon.assert.calledWith(location.path, "/ApplicationEvaluator/RatingsSummaries");
            });
        });

        describe('gotoRandomApplicant', () => {
            beforeEach(() => {
                service.allFinalizedApplicantForAGraduatingYearModel = new Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.AllFinalizedApplicantsForAGraduatingYearModel(null, null, null, null, typeOfData);
                service.gotoRandomApplicant();
            });

            it('should redirect with the random guid.', () => {
                sinon.assert.calledWith(location.path, "/ApplicationEvaluator/ReviewApplicant/" + typeOfData);
            });
        });

        describe('gotoApplicant', () => {
            var applicantSummary1, applicantSummary2, applicantSummary3;
            var guid1 = "guid1", guid2 = "guid2", guid3 = 'guid3';

            beforeEach(() => {
                applicantSummary1 = new Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.ApplicantSummaryModel(new Dtos.Person.NameModel("a", "b"), guid1, 1, Common.Enums.IncomeRangeEnum.Mt20000Lt30000, Common.Enums.RatingEnum.APlus);
                applicantSummary2 = new Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.ApplicantSummaryModel(new Dtos.Person.NameModel("a", "b"), guid2, 1, Common.Enums.IncomeRangeEnum.Mt20000Lt30000, Common.Enums.RatingEnum.APlus);
                applicantSummary3 = new Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.ApplicantSummaryModel(new Dtos.Person.NameModel("a", "b"), guid3, 1, Common.Enums.IncomeRangeEnum.Mt20000Lt30000, Common.Enums.RatingEnum.APlus);

                var applicantSummaries = [applicantSummary1, applicantSummary2, applicantSummary3];

                service.allFinalizedApplicantForAGraduatingYearModel = new Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.AllFinalizedApplicantsForAGraduatingYearModel(1, applicantSummaries, .1);
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
                var modelOnService;
                var percentRated = 0.12;
                var classYear = 1201;
                var numberLeft = 123;
                var appSummary;

                beforeEach(() => {
                    appSummary = new Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.ApplicantSummaryModel(new Dtos.Person.NameModel("a","b"), "1", 1, Common.Enums.IncomeRangeEnum.Mt20000Lt30000, Common.Enums.RatingEnum.APlus);
                    modelOnService = new Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.AllFinalizedApplicantsForAGraduatingYearModel(classYear, [appSummary], percentRated, numberLeft);
                    service.allFinalizedApplicantForAGraduatingYearModel = modelOnService;
                });

                describe('getListOfApplicantSummaries', () => {
                    it('should return whatever is on the model.', () => {
                        expect(service.getListOfApplicantSummaries().length).toBe(1);
                    });

                    it('should return whatever is on the model.', () => {
                        expect(service.getListOfApplicantSummaries()[0]).toBe(appSummary);
                    });
                });

                describe('getAllFinalizedApplicantsForAGraduatingYearModel', () => {
                    it('should return whatever is on the server.', () => {
                        expect(service.getAllFinalizedApplicantsForAGraduatingYearModel()).toBe(modelOnService);
                    });
                });

                describe('getPercentRated', () => {
                    it('should return whatever is on the model.', () => {
                        expect(service.getPercentRated()).toBe(percentRated);
                    });
                });

                describe('getClassYear', () => {
                    it('should return whatever is on the model.', () => {
                        expect(service.getClassYear()).toBe(classYear);
                    });
                });

                describe('getNumberOfApplicantsNotYetRated', () => {
                    it('should return whatever is on the model.', () => {
                        expect(service.getNumberOfApplicantsNotYetRated()).toBe(numberLeft);
                    });
                });
            });

            describe('modelOnService is not defined', () => {
                describe('getListOfApplicantSummaries', () => {
                    it('should return [].', () => {
                        expect(service.getListOfApplicantSummaries().length).toBe(0);
                    });
                });

                describe('getAllFinalizedApplicantsForAGraduatingYearModel', () => {
                    it('should return undefined.', () => {
                        expect(service.getAllFinalizedApplicantsForAGraduatingYearModel()).toBeUndefined();
                    });
                });

                describe('getPercentRated', () => {
                    it('should return 0.', () => {
                        expect(service.getPercentRated()).toBe(0);
                    });
                });

                describe('getClassYear', () => {
                    it('should return 0.', () => {
                        expect(service.getClassYear()).toBe(0);
                    });
                });

                describe('getNumberOfApplicantsNotYetRated', () => {
                    it('should return 0.', () => {
                        expect(service.getNumberOfApplicantsNotYetRated()).toBe(0);
                    });
                });

                describe('canGotoNextRandomApplicant', () => {
                    it('should return false', () => {
                        expect(service.canGotoNextRandomApplicant()).toBeFalsy();
                    });
                });
            });
        });

        describe('getFinalizedApplicantsFromServer', () => {
            describe('get', () => {
                beforeEach(() => {
                    authRepo.get.returns(typeOfData);
                    result = service.getFinalizedApplicantsFromServer();
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
                        service.resolveGetFinalizedApplicants(errorResponse);
                    });

                    it('should call genericGetResolver.', () => {
                        sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, errorResponse);
                    });
                });

                describe('success', () => {
                    beforeEach(() => {
                        genericResolver.genericGetResolver.returns(errorResponse);
                        service.resolveGetFinalizedApplicants(successResponse);
                    });

                    it('should call genericGetResolver.', () => {
                        sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, successResponse);
                    });

                    it('should put whatever is returns as a property.', () => {
                        expect(service.allFinalizedApplicantForAGraduatingYearModel).toBe(errorResponse);
                    });
                });
            });
        });
    });
} 