module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Spec.Controllers {
    describe("CurrentApplicationCtrl", () => {

        var scope, q, modal, currentApplicantOrchestrationService, $routeParams;

        var deferred, promise, resource;
        var deferred2, promise2, resource2;
        var deferred3, promise3, resource3;
        var deferred4, promise4, resource4;
        var deferred5, promise5, resource5;
        var deferred6, promise6, resource6;
        var deferred7, promise7, resource7;
        var deferred8, promise8, resource8;
        var deferred9, promise9, resource9;
        var deferred10, promise10, resource10;

        var applicantGuid = "guid";
        var data = "data";
        var ctrl;

        beforeEach(inject(($rootScope, $q) => { 
            var commonStubs = new TestHelpers.CommonStubs();

            q = $q;
            scope = $rootScope;

            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };

            deferred2 = q.defer();
            promise2 = deferred2.promise;
            resource2 = { $promise: promise2 };

            deferred3 = q.defer();
            promise3 = deferred3.promise;
            resource3 = { $promise: promise3 };

            deferred4 = q.defer();
            promise4 = deferred4.promise;
            resource4 = { $promise: promise4 };

            deferred5 = q.defer();
            promise5 = deferred5.promise;
            resource5 = { $promise: promise5 };

            deferred6 = q.defer();
            promise6 = deferred6.promise;
            resource6 = { $promise: promise6 };

            deferred7 = q.defer();
            promise7 = deferred7.promise;
            resource7 = { $promise: promise7 };

            deferred8 = q.defer();
            promise8 = deferred8.promise;
            resource8 = { $promise: promise8 };

            deferred9 = q.defer();
            promise9 = deferred9.promise;
            resource9 = { $promise: promise9 };

            deferred10 = q.defer();
            promise10 = deferred10.promise;
            resource10 = { $promise: promise10 };

            modal = commonStubs.getModalStub();

            $routeParams = sinon.stub({
                applicantsGuid: applicantGuid
            });

            currentApplicantOrchestrationService = sinon.stub({
                setCurrentApplicantsGuid: () => {},

                getGeneralInformation: () => { },
                getGeneralInformationFromServer: () => { },
                resolveGetGeneralInformationFromServer: () => { },

                getExtracurricularActivitiesInformation: () => { },
                getExtracurricularActivitiesFromServer: () => { },
                resolveGetExtracurricularActivitiesFromServer: () => { },

                getEssayAndLetterOfRecommendationSummaries: () => { },
                getEssayAndLetterOfRecommendationSummariesFromServer: () => { },
                resolveGetEssayAndLetterOfRecommendationSummariesFromServer: () => { },

                getAcademicInformation: () => { },
                getAcademicInformationFromServer: () => { },
                resolveGetAcademicInformationFromServer: () => { },

                postEssayRating:()=>{},
                resolvePostEssayRating: () => { },

                postFirstImpressionRating: () => { },
                postFinalOverallRating: () => { },
                resolvePostNonEssayRating: () => { },

                getTranscriptFromServer: () => { },
                resolveGetTranscriptFromServer: () => { },
                postTranscriptConfirmation: () => { },
                resolvePostTranscriptConfirmation: () => { },
                getTranscriptConfirmed: () => { },

                canGotoNextRandomApplicant: () => { },
                gotoNextRandomApplicant: () => { },
                getFinalizedApplicantsFromServer: () => { },
                resolveGetFinalizedApplicants: () => { },

                canGotoSummaryOfAllRatings: () => { },
                gotoSummaryOfAllRatings: () => { }
            });

            
            currentApplicantOrchestrationService.getGeneralInformationFromServer.returns(resource);
            currentApplicantOrchestrationService.getExtracurricularActivitiesFromServer.returns(resource2);
            currentApplicantOrchestrationService.getEssayAndLetterOfRecommendationSummariesFromServer.returns(resource3);
            currentApplicantOrchestrationService.getAcademicInformationFromServer.returns(resource4);
            currentApplicantOrchestrationService.postFirstImpressionRating.returns(resource5);
            currentApplicantOrchestrationService.postFinalOverallRating.returns(resource6);
            currentApplicantOrchestrationService.postEssayRating.returns(resource7);
            currentApplicantOrchestrationService.getTranscriptFromServer.returns(resource8);
            currentApplicantOrchestrationService.postTranscriptConfirmation.returns(resource9);
            currentApplicantOrchestrationService.getFinalizedApplicantsFromServer.returns(resource10);

            ctrl = new ReviewApplicant.Controllers.CurrentApplicationCtrl(scope, modal, $routeParams, currentApplicantOrchestrationService);
        }));

        describe('construction', () => {
            it('should call the setCurrentApplicantsGuid with the guid on route.', () => {
                sinon.assert.calledWith(currentApplicantOrchestrationService.setCurrentApplicantsGuid, applicantGuid);
            });

            it('should have a false default canGetAllFinalizedApplications.', () => {
                expect(ctrl.canGetAllFinalizedApplications).toBeFalsy();
            });

            describe('getAcademicInformationFromServer', () => {
                describe('get', () => {
                    it('should call the get method.', () => {
                        sinon.assert.called(currentApplicantOrchestrationService.getAcademicInformationFromServer);
                    });

                    it('should call the get method after set CurrentApplicantsGuid.', () => {
                        sinon.assert.callOrder(currentApplicantOrchestrationService.setCurrentApplicantsGuid, currentApplicantOrchestrationService.getAcademicInformationFromServer);
                    });
                });

                describe('resolve', () => {
                    describe('success', () => {

                        beforeEach(() => {
                            deferred4.resolve(data);
                            scope.$apply();
                        });

                        it('should call resolve.', () => {
                            sinon.assert.calledWith(currentApplicantOrchestrationService.resolveGetAcademicInformationFromServer, new Common.Models.ServerResponseModel(data, true));
                        });
                    });

                    describe('failure', () => {
                        beforeEach(() => {
                            deferred4.reject();
                            scope.$apply();
                        });

                        it('should call resolve.', () => {
                            sinon.assert.calledWith(currentApplicantOrchestrationService.resolveGetAcademicInformationFromServer, new Common.Models.ServerResponseModel(null, false));
                        });
                    });
                });
            });

            describe('getEssayAndLetterOfRecommendationSummariesFromServer', () => {
                describe('get', () => {
                    it('should call the get method.', () => {
                        sinon.assert.called(currentApplicantOrchestrationService.getEssayAndLetterOfRecommendationSummariesFromServer);
                    });

                    it('should call the get method after set CurrentApplicantsGuid.', () => {
                        sinon.assert.callOrder(currentApplicantOrchestrationService.setCurrentApplicantsGuid, currentApplicantOrchestrationService.getEssayAndLetterOfRecommendationSummariesFromServer);
                    });
                });

                describe('resolve', () => {
                    describe('success', () => {

                        beforeEach(() => {
                            deferred3.resolve(data);
                            scope.$apply();
                        });

                        it('should call resolve.', () => {
                            sinon.assert.calledWith(currentApplicantOrchestrationService.resolveGetEssayAndLetterOfRecommendationSummariesFromServer, new Common.Models.ServerResponseModel(data, true));
                        });
                    });

                    describe('failure', () => {
                        beforeEach(() => {
                            deferred3.reject();
                            scope.$apply();
                        });

                        it('should call resolve.', () => {
                            sinon.assert.calledWith(currentApplicantOrchestrationService.resolveGetEssayAndLetterOfRecommendationSummariesFromServer, new Common.Models.ServerResponseModel(null, false));
                        });
                    });
                });
            });

            describe('getExtracurricularActivitiesFromServer', () => {
                describe('get', () => {
                    it('should call the get method.', () => {
                        sinon.assert.called(currentApplicantOrchestrationService.getExtracurricularActivitiesFromServer);
                    });

                    it('should call the get method after set CurrentApplicantsGuid.', () => {
                        sinon.assert.callOrder(currentApplicantOrchestrationService.setCurrentApplicantsGuid, currentApplicantOrchestrationService.getExtracurricularActivitiesFromServer);
                    });
                });

                describe('resolve', () => {
                    describe('success', () => {

                        beforeEach(() => {
                            deferred2.resolve(data);
                            scope.$apply();
                        });

                        it('should call resolve.', () => {
                            sinon.assert.calledWith(currentApplicantOrchestrationService.resolveGetExtracurricularActivitiesFromServer, new Common.Models.ServerResponseModel(data, true));
                        });
                    });

                    describe('failure', () => {
                        beforeEach(() => {
                            deferred2.reject();
                            scope.$apply();
                        });

                        it('should call resolve.', () => {
                            sinon.assert.calledWith(currentApplicantOrchestrationService.resolveGetExtracurricularActivitiesFromServer, new Common.Models.ServerResponseModel(null, false));
                        });
                    });
                });
            });


            describe('getGeneralInformationFromServer', () => {
                describe('get', () => {
                    it('should call the get method.', () => {
                        sinon.assert.called(currentApplicantOrchestrationService.getGeneralInformationFromServer);
                    });

                    it('should call the get method after set CurrentApplicantsGuid.', () => {
                        sinon.assert.callOrder(currentApplicantOrchestrationService.setCurrentApplicantsGuid, currentApplicantOrchestrationService.getGeneralInformationFromServer);
                    });
                });

                describe('resolve', () => {
                    describe("success and don't getAllApplicants", () => {
                        beforeEach(() => {
                            ctrl.canGetAllFinalizedApplications = false;
                            deferred.resolve(data);
                            scope.$apply();
                        });

                        it('should call resolve.', () => {
                            sinon.assert.calledWith(currentApplicantOrchestrationService.resolveGetGeneralInformationFromServer, new Common.Models.ServerResponseModel(data, true));
                        });

                        it('should not call getFinalizedApplicantsFromService.', () => {
                            sinon.assert.notCalled(currentApplicantOrchestrationService.getFinalizedApplicantsFromServer);
                        });
                    });

                    describe('success and getAllApplicants', () => {

                        beforeEach(() => {
                            ctrl.canGetAllFinalizedApplications = true;
                            deferred.resolve(data);
                            scope.$apply();
                        });

                        it('should call resolve.', () => {
                            sinon.assert.calledWith(currentApplicantOrchestrationService.resolveGetGeneralInformationFromServer, new Common.Models.ServerResponseModel(data, true));
                        });

                        describe('getAllApplicants for next applicant', () => {
                            it('should call getFinalizedApplicantsFromService.', () => {
                                sinon.assert.calledOnce(currentApplicantOrchestrationService.getFinalizedApplicantsFromServer);
                            });

                            describe('success', () => {
                                beforeEach(() => {
                                    deferred10.resolve(data);
                                    scope.$apply();
                                });

                                it('should call the resolver with data.', () => {
                                    sinon.assert.calledWith(currentApplicantOrchestrationService.resolveGetFinalizedApplicants, new Common.Models.ServerResponseModel(data, true));
                                });

                                it('should set canGetAllFinalizedApplications to false.', () => {
                                    expect(ctrl.canGetAllFinalizedApplications).toBeFalsy();
                                });
                            });

                            describe('failure', () => {
                                beforeEach(() => {
                                    deferred10.reject();
                                    scope.$apply();
                                });

                                it('should call the resolver with null.', () => {
                                    sinon.assert.calledWith(currentApplicantOrchestrationService.resolveGetFinalizedApplicants, new Common.Models.ServerResponseModel(null, false));
                                });

                                it('should set canGetAllFinalizedApplications to false.', () => {
                                    expect(ctrl.canGetAllFinalizedApplications).toBeFalsy();
                                });
                            });
                        });
                    });

                    describe('failure', () => {
                        beforeEach(() => {
                            deferred.reject();
                            scope.$apply();
                        });

                        it('should call resolve.', () => {
                            sinon.assert.calledWith(currentApplicantOrchestrationService.resolveGetGeneralInformationFromServer, new Common.Models.ServerResponseModel(null, false));
                        });
                    });
                });
            });
        });

        describe('successful construction', () => {
            beforeEach(() => {
                deferred.resolve(data);
                scope.$apply();
            });

            describe("modals",() => {
                var fakeModal;
                var finishedRating;

                beforeEach(() => {
                    finishedRating = new Dtos.Common.GenericRatingModel(Common.Enums.RatingEnum.F, "He is bad");

                    fakeModal = new TestHelpers.FakeModal().createFakeModel();

                    modal.open.returns(fakeModal);
                });

                describe('confirmTranscript', () => {

                    beforeEach(() => {
                        scope.confirmTranscript();
                    });

                    it('should create a new confirm transcript model on scope.', () => {
                        expect(scope.confirmTranscriptModel).toBeDefined();
                    });

                    it('should open the modal.', () => {
                        sinon.assert.calledOnce(modal.open);
                    });

                    describe('cancel modal', () => {
                        beforeEach(() => {
                            scope.modalInstance.dismiss();
                        });

                        it('should not call post.', () => {
                            sinon.assert.notCalled(currentApplicantOrchestrationService.postTranscriptConfirmation);
                        });
                    });

                    describe('success modal', () => {
                        beforeEach(() => {
                            scope.modalInstance.close(applicantGuid);
                        });

                        describe('post', () => {
                            it('should post the model.', () => {
                                sinon.assert.calledWith(currentApplicantOrchestrationService.postTranscriptConfirmation, applicantGuid);
                            });

                            describe('on success', () => {
                                beforeEach(() => {
                                    deferred9.resolve();
                                    scope.$apply();
                                });

                                it('should resolve.', () => {
                                    sinon.assert.calledWith(currentApplicantOrchestrationService.resolvePostTranscriptConfirmation, new Common.Models.ServerResponseModel(null, true));
                                });

                                it('should call academic info twice.', () => {
                                    sinon.assert.calledTwice(currentApplicantOrchestrationService.getAcademicInformationFromServer);
                                });
                            });

                            describe('on failure', () => {
                                beforeEach(() => {
                                    deferred9.reject();
                                    scope.$apply();
                                });

                                it('should resolve.', () => {
                                    sinon.assert.calledWith(currentApplicantOrchestrationService.resolvePostTranscriptConfirmation, new Common.Models.ServerResponseModel(null, false));
                                });

                                it('should not call academic info twice.', () => {
                                    sinon.assert.calledOnce(currentApplicantOrchestrationService.getAcademicInformationFromServer);
                                });
                            });
                        });
                    });
                });

                describe('viewLetterOfRecommendation', () => {
                    var summariesOfLettersOfRecommendation;
                    var targetLetterOfRecommendation;

                    beforeEach(() => {
                        targetLetterOfRecommendation = new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.LetterOfRecommendation.LetterOfRecommendationSummaryModel(null, "target", null);

                        summariesOfLettersOfRecommendation = [
                            new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.LetterOfRecommendation.LetterOfRecommendationSummaryModel(null, null, null),
                            targetLetterOfRecommendation,
                            new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.LetterOfRecommendation.LetterOfRecommendationSummaryModel(null, null, null),
                            new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.LetterOfRecommendation.LetterOfRecommendationSummaryModel(null, null, null)];

                        currentApplicantOrchestrationService.getEssayAndLetterOfRecommendationSummaries.returns(new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.CollectionsOfEssaysAndLettersOfRecommendationModel(null, summariesOfLettersOfRecommendation));
                        scope.viewLetterOfRecommendation(1);
                    });

                    it('should make the target letterOfRecommendation the currentLetterOfRecommendation summary.', () => {
                        expect(scope.currentLetterOfRecommendationSummary).toBe(targetLetterOfRecommendation);
                    });

                    it('should open a modal', () => {
                        sinon.assert.calledOnce(modal.open);
                    });
                });

                describe('viewAndRateEssay', () => {
                    var summariesOfEssays;
                    var targetEssaySummary;

                    beforeEach(() => {
                        targetEssaySummary = new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.Essay.EssaySummaryModel(1,"asdf","Problem",1234,null,null);

                        summariesOfEssays = [
                            new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.Essay.EssaySummaryModel(null, null, null),
                            targetEssaySummary,
                            new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.Essay.EssaySummaryModel(null, null, null),
                            new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.Essay.EssaySummaryModel(null, null, null)];

                        currentApplicantOrchestrationService.getEssayAndLetterOfRecommendationSummaries.returns(new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.CollectionsOfEssaysAndLettersOfRecommendationModel(summariesOfEssays, null));
                        scope.viewAndRateEssay(1);
                    });

                    it('should make the target letterOfRecommendation the currentLetterOfRecommendation summary.', () => {
                        expect(scope.currentEssaySummary).toBe(targetEssaySummary);
                    });

                    it('should open a modal', () => {
                        sinon.assert.calledOnce(modal.open);
                    });

                    describe('cancel modal', () => {
                        beforeEach(() => {
                            scope.modalInstance.dismiss();
                        });

                        it('should not call postEssayRating.', () => {
                            sinon.assert.notCalled(currentApplicantOrchestrationService.postEssayRating);
                        });

                        it('should not call getEssayAndLetterOfRecommendations again.', () => {
                            sinon.assert.calledOnce(currentApplicantOrchestrationService.getEssayAndLetterOfRecommendationSummariesFromServer);
                        });
                    });

                    describe('ok modal', () => {
                        beforeEach(() => {
                            scope.modalInstance.close(finishedRating);
                        });

                        it('should call postEssayRating.', () => {
                            sinon.assert.calledWith(currentApplicantOrchestrationService.postEssayRating, finishedRating);
                        });

                        it('should not call postFirstImpressionRating.', () => {
                            sinon.assert.notCalled(currentApplicantOrchestrationService.postFirstImpressionRating);
                        });

                        it('should not call postFinalOverallRating.', () => {
                            sinon.assert.notCalled(currentApplicantOrchestrationService.postFinalOverallRating);
                        });

                        describe('success', () => {
                            beforeEach(() => {
                                deferred7.resolve();
                                scope.$apply();
                            });

                            it('should call resolvePost.', () => {
                                sinon.assert.calledWith(currentApplicantOrchestrationService.resolvePostEssayRating, new Common.Models.ServerResponseModel(null, true));
                            });

                            it('should call getEssayAndLetterOfRecommendationSummariesFromServer again.', () => {
                                sinon.assert.calledTwice(currentApplicantOrchestrationService.getEssayAndLetterOfRecommendationSummariesFromServer);
                            });
                        });

                        describe('failure', () => {
                            beforeEach(() => {
                                deferred7.reject();
                                scope.$apply();
                            });

                            it('should call resolvePost.', () => {
                                sinon.assert.calledWith(currentApplicantOrchestrationService.resolvePostEssayRating, new Common.Models.ServerResponseModel(null, false));
                            });

                            it('should call not getGeneralInformationFromServer again.', () => {
                                sinon.assert.calledOnce(currentApplicantOrchestrationService.getEssayAndLetterOfRecommendationSummariesFromServer);
                            });
                        });
                    });
                });

                describe('finalOverallRating', () => {
                    describe('ratingForModal with null rating', () => {
                        beforeEach(() => {
                            currentApplicantOrchestrationService.getGeneralInformation.returns(new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.ApplicantsGeneralInformationModel(null, null, null, null, null, null, null));
                            scope.finalOverallRating();
                        });

                        it('should have a new rating object.', () => {
                            expect(scope.ratingForModal).toMatch(new Dtos.Common.GenericRatingModel());
                        });
                    });

                    describe('firstImpressionRating with instanciated rating', () => {
                        var currentRating;

                        beforeEach(() => {
                            currentRating = new Dtos.Common.GenericRatingModel(Common.Enums.RatingEnum.CPlus, "rating");
                            var generalInforamtion = new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.ApplicantsGeneralInformationModel(null, null, null, null, null, currentRating, null);
                            currentApplicantOrchestrationService.getGeneralInformation.returns(generalInforamtion);
                            scope.finalOverallRating();
                        });

                        it('should have current rating object.', () => {
                            expect(scope.ratingForModal).toBe(currentRating);
                        });
                    });

                    describe('finalOverallRating', () => {
                        beforeEach(() => {
                            currentApplicantOrchestrationService.getGeneralInformation.returns(new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.ApplicantsGeneralInformationModel(null, null, null, null, null, null, null));
                            scope.finalOverallRating();
                        });

                        it('should open a modal', () => {
                            sinon.assert.calledOnce(modal.open);
                        });

                        describe('cancel modal', () => {
                            beforeEach(() => {
                                scope.modalInstance.dismiss();
                            });

                            it('should not call postFirstImpressionRating.', () => {
                                sinon.assert.notCalled(currentApplicantOrchestrationService.postFirstImpressionRating);
                            });

                            it('should not call postFinalOverallRating.', () => {
                                sinon.assert.notCalled(currentApplicantOrchestrationService.postFinalOverallRating);
                            });

                            it('should not call getGeneralInformation again.', () => {
                                sinon.assert.calledOnce(currentApplicantOrchestrationService.getGeneralInformation);
                            });

                            it('should have a false canGetAllFinalizedApplications.', () => {
                                expect(ctrl.canGetAllFinalizedApplications).toBeFalsy();
                            });
                        });

                        describe('ok modal', () => {
                            beforeEach(() => {
                                scope.modalInstance.close(finishedRating);
                            });

                            it('should call postFinalOverallRating.', () => {
                                sinon.assert.calledWith(currentApplicantOrchestrationService.postFinalOverallRating, finishedRating);
                            });

                            it('should not call postFirstImpressionRating.', () => {
                                sinon.assert.notCalled(currentApplicantOrchestrationService.postFirstImpressionRating);
                            });

                            describe('success', () => {
                                beforeEach(() => {
                                    deferred6.resolve();
                                    scope.$apply();
                                });

                                it('should call resolvePost.', () => {
                                    sinon.assert.calledWith(currentApplicantOrchestrationService.resolvePostNonEssayRating, new Common.Models.ServerResponseModel(null, true));
                                });

                                it('should call getGeneralInformationFromServer again.', () => {
                                    sinon.assert.calledTwice(currentApplicantOrchestrationService.getGeneralInformationFromServer);
                                });

                                it('should have a true canGetAllFinalizedApplications.', () => {
                                    expect(ctrl.canGetAllFinalizedApplications).toBeTruthy();
                                });
                            });

                            describe('failure', () => {
                                beforeEach(() => {
                                    deferred6.reject();
                                    scope.$apply();
                                });

                                it('should call resolvePost.', () => {
                                    sinon.assert.calledWith(currentApplicantOrchestrationService.resolvePostNonEssayRating, new Common.Models.ServerResponseModel(null, false));
                                });

                                it('should call not getGeneralInformationFromServer again.', () => {
                                    sinon.assert.calledOnce(currentApplicantOrchestrationService.getGeneralInformationFromServer);
                                });

                                it('should not call postFinalOverallRating.', () => {
                                    sinon.assert.notCalled(currentApplicantOrchestrationService.postFirstImpressionRating);
                                });

                                it('should have a false canGetAllFinalizedApplications.', () => {
                                    expect(ctrl.canGetAllFinalizedApplications).toBeFalsy();
                                });
                            });
                        });
                    });
                });

                describe('firstImpressionModal', () => {
                    describe('ratingForModal with null rating', () => {
                        beforeEach(() => {
                            currentApplicantOrchestrationService.getGeneralInformation.returns(new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.ApplicantsGeneralInformationModel(null, null, null, null, null, null, null));
                            scope.firstImpressionRating();
                        });

                        it('should have a new rating object.', () => {
                            expect(scope.ratingForModal).toMatch(new Dtos.Common.GenericRatingModel());
                        });
                    });

                    describe('firstImpressionRating with instanciated rating', () => {
                        var currentRating;

                        beforeEach(() => {
                            currentRating = new Dtos.Common.GenericRatingModel(Common.Enums.RatingEnum.CPlus, "rating");
                            var generalInforamtion = new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.ApplicantsGeneralInformationModel(null, null, null, null, currentRating, null, null);
                            currentApplicantOrchestrationService.getGeneralInformation.returns(generalInforamtion);
                            scope.firstImpressionRating();
                        });

                        it('should have current rating object.', () => {
                            expect(scope.ratingForModal).toBe(currentRating);
                        });
                    });

                    describe('firstImpressionRating', () => {
                        beforeEach(() => {
                            currentApplicantOrchestrationService.getGeneralInformation.returns(new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.ApplicantsGeneralInformationModel(null, null, null, null, null, null, null));
                            scope.firstImpressionRating();
                        });

                        it('should open a modal', () => {
                            sinon.assert.calledOnce(modal.open);
                        });

                        describe('cancel modal', () => {
                            beforeEach(() => {
                                scope.modalInstance.dismiss();
                            });

                            it('should not call postFirstImpressionRating.', () => {
                                sinon.assert.notCalled(currentApplicantOrchestrationService.postFirstImpressionRating);
                            });

                            it('should not call postFinalOverallRating.', () => {
                                sinon.assert.notCalled(currentApplicantOrchestrationService.postFinalOverallRating);
                            });

                            it('should not call getGeneralInformation again.', () => {
                                sinon.assert.calledOnce(currentApplicantOrchestrationService.getGeneralInformation);
                            });
                        });

                        describe('ok modal', () => {
                            beforeEach(() => {
                                scope.modalInstance.close(finishedRating);
                            });

                            it('should call postFirstImpressionRating.', () => {
                                sinon.assert.calledWith(currentApplicantOrchestrationService.postFirstImpressionRating, finishedRating);
                            });

                            it('should not call postFinalOverallRating.', () => {
                                sinon.assert.notCalled(currentApplicantOrchestrationService.postFinalOverallRating);
                            });

                            describe('success', () => {
                                beforeEach(() => {
                                    deferred5.resolve();
                                    scope.$apply();
                                });

                                it('should call resolvePost.', () => {
                                    sinon.assert.calledWith(currentApplicantOrchestrationService.resolvePostNonEssayRating, new Common.Models.ServerResponseModel(null, true));
                                });

                                it('should call getGeneralInformationFromServer again.', () => {
                                    sinon.assert.calledTwice(currentApplicantOrchestrationService.getGeneralInformationFromServer);
                                });

                                it('should have a false canGetAllFinalizedApplications.', () => {
                                    expect(ctrl.canGetAllFinalizedApplications).toBeFalsy();
                                });
                            });

                            describe('failure', () => {
                                beforeEach(() => {
                                    deferred5.reject();
                                    scope.$apply();
                                });

                                it('should call resolvePost.', () => {
                                    sinon.assert.calledWith(currentApplicantOrchestrationService.resolvePostNonEssayRating, new Common.Models.ServerResponseModel(null, false));
                                });

                                it('should call not getGeneralInformationFromServer again.', () => {
                                    sinon.assert.calledOnce(currentApplicantOrchestrationService.getGeneralInformationFromServer);
                                });

                                it('should not call postFinalOverallRating.', () => {
                                    sinon.assert.notCalled(currentApplicantOrchestrationService.postFinalOverallRating);
                                });
                            });
                        });
                    });
                });
            });

            describe('getTranscriptFromServer', () => {
                describe('get', () => {
                    beforeEach(() => {
                        scope.getTranscript();
                    });

                    it('should call once.', () => {
                        sinon.assert.calledOnce(currentApplicantOrchestrationService.getTranscriptFromServer);
                    });

                    describe('failure', () => {
                        beforeEach(() => {
                            deferred8.reject();
                            scope.$apply();
                        });

                        it('should call resolveGet.', () => {
                            sinon.assert.calledWith(currentApplicantOrchestrationService.resolveGetTranscriptFromServer, new Common.Models.ServerResponseModel(null, false));
                        });
                    });

                    describe('success', () => {
                        beforeEach(() => {
                            deferred8.resolve(data);
                            scope.$apply();
                        });

                        it('should call resolveGet.', () => {
                            sinon.assert.calledWith(currentApplicantOrchestrationService.resolveGetTranscriptFromServer, new Common.Models.ServerResponseModel(data, true));
                        });
                    });
                });
            });

            describe('get methods', () => {
                describe('getTranscriptConfirmed', () => {
                    it('should return whatever the service returns.', () => {
                        currentApplicantOrchestrationService.getTranscriptConfirmed.returns(data);
                        expect(scope.getTranscriptConfirmedModel()).toBe(data);
                    });
                });
                
                describe('getGeneralInformation', () => {
                    it('should return whatever the service returns.', () => {
                        currentApplicantOrchestrationService.getGeneralInformation.returns(data);
                        expect(scope.getGeneralInformation()).toBe(data);
                    });

                    describe('getExtracurricularActivitiesInformation', () => {
                        it('should return whatever the service returns.', () => {
                            currentApplicantOrchestrationService.getExtracurricularActivitiesInformation.returns(data);
                            expect(scope.getExtracurricularActivitiesInformation()).toBe(data);
                        });
                    });

                    describe('getEssayAndLetterOfRecommendationSummaries', () => {
                        it('should return whatever the service returns.', () => {
                            currentApplicantOrchestrationService.getEssayAndLetterOfRecommendationSummaries.returns(data);
                            expect(scope.getEssayAndLetterOfRecommendationSummaries()).toBe(data);
                        });
                    });

                    describe('getAcademicInformation', () => {
                        it('should return whatever the service returns.', () => {
                            currentApplicantOrchestrationService.getAcademicInformation.returns(data);
                            expect(scope.getAcademicInformation()).toBe(data);
                        });
                    });

                    describe('canGotoNextRandomApplicant', () => {
                        it('should return whatever the service returns.', () => {
                            currentApplicantOrchestrationService.canGotoNextRandomApplicant.returns(data);
                            expect(scope.canGotoNextRandomApplicant()).toBe(data);
                        });
                    });

                    describe('canGotoSummaryOfAllRatings', () => {
                        it('should call the service.', () => {
                            currentApplicantOrchestrationService.canGotoSummaryOfAllRatings.returns(data);
                            expect(scope.canGotoSummaryOfAllRatings()).toBe(data);
                        });
                    });

                    describe('gotoSummaryOfAllRatings', () => {
                        it('should call the service.', () => {
                            scope.gotoSummaryOfAllRatings();
                            sinon.assert.calledOnce(currentApplicantOrchestrationService.gotoSummaryOfAllRatings);
                        });
                    });

                    describe('gotoNextRandomApplicant', () => {
                        describe('if canGotoNextRandomApplicant is true', () => {
                            it('should call the service.', () => {
                                currentApplicantOrchestrationService.canGotoNextRandomApplicant.returns(true);
                                scope.gotoNextRandomApplicant();
                                sinon.assert.calledOnce(currentApplicantOrchestrationService.gotoNextRandomApplicant);
                            });
                        });

                        describe('if canGotoNextRandomApplicant is false', () => {
                            it('should not call the service.', () => {
                                currentApplicantOrchestrationService.canGotoNextRandomApplicant.returns(false);
                                scope.gotoNextRandomApplicant();
                                sinon.assert.notCalled(currentApplicantOrchestrationService.gotoNextRandomApplicant);
                            });
                        });
                    });
                });
            });
        });
    });
} 