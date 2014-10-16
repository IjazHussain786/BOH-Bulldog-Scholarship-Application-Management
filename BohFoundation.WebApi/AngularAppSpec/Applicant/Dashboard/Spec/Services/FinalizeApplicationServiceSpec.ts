module BohFoundation.Applicant.Dashboard.Spec.Services {
    describe("FinalizeApplicationService", () => {
        var apiEndpoint = "/api/applicant/finalize";
        var typeOfData = "finalize application";
        var service, authRepo, errorResponse, successResponse, alertHelperService, applicantNotificationService, result, userInformationService;

        var finishedPanel = new Dashboard.Models.ApplicantDashboardListItemInputModel("", "", Common.Enums.StyleEnum.Success);
        var notFinishedPanel = new Dashboard.Models.ApplicantDashboardListItemInputModel("", "", Common.Enums.StyleEnum.Warning);

        beforeEach(() => {
            successResponse = new Common.Models.ServerResponseModel(null, true);
            errorResponse = new Common.Models.ServerResponseModel(null, false);

            var commonStubs = new TestHelpers.CommonStubs();

            authRepo = commonStubs.createAuthRepo();
            alertHelperService = commonStubs.getAlertHelperServiceStub();
            userInformationService = commonStubs.getUserInformationServiceStub();

            applicantNotificationService = sinon.stub({
                getPersonalInformationDashboardInputModel:() => {},
                getContactInformationDashboardInputModel: () => { },
                getTranscriptUploadedDashboardInputModel: () => { },
                getAcademicInformationDashboardInputModel: () => { },
                getLowGradeInformationDashboardInputModel: () => { },
                getEssayDashboardInputModels: () => { },
                getReferenceDashboardInputModels: () => { },
                getFamilyInformationDashboardInputModel: () => { },
                getExtracurricularActivitiesDashboardInputModel: () => { },

                getHideLowGradeInformation: () => { }
            });

            service = new Dashboard.Services.FinalizeApplicationService(authRepo, alertHelperService, applicantNotificationService, userInformationService);
        });

        beforeEach(() => {
            applicantNotificationService.getPersonalInformationDashboardInputModel.returns(finishedPanel);
            applicantNotificationService.getContactInformationDashboardInputModel.returns(finishedPanel);
            applicantNotificationService.getTranscriptUploadedDashboardInputModel.returns(finishedPanel);
            applicantNotificationService.getAcademicInformationDashboardInputModel.returns(finishedPanel);
            applicantNotificationService.getReferenceDashboardInputModels.returns(finishedPanel);
            applicantNotificationService.getFamilyInformationDashboardInputModel.returns(finishedPanel);
            applicantNotificationService.getExtracurricularActivitiesDashboardInputModel.returns(finishedPanel);

            applicantNotificationService.getEssayDashboardInputModels.returns([finishedPanel, finishedPanel]);
            applicantNotificationService.getLowGradeInformationDashboardInputModel.returns(finishedPanel);

            applicantNotificationService.getHideLowGradeInformation.returns(false);
        });

        describe("postFinalizeApplication", () => {
            describe('post with valid model', () => {
                var harrold = "harrolds";

                beforeEach(() => {
                    authRepo.post.returns(harrold);
                    result = service.postFinalizeApplication();
                });

                it('should call post.', () => {
                    sinon.assert.calledWith(authRepo.post, true, apiEndpoint);
                });

                it('should return whatever is returned from post.', () => {
                    expect(result).toBe(harrold);
                });
            });

            describe('post with invalid model', () => {
                beforeEach(() => {
                    applicantNotificationService.getTranscriptUploadedDashboardInputModel.returns(notFinishedPanel);
                    result = service.postFinalizeApplication();
                });

                it('should not call post.', () => {
                    sinon.assert.notCalled(authRepo.post);
                });
            });

            describe('resolve', () => {
                describe('failure.', () => {
                    beforeEach(() => {
                        service.resolveFinalizeApplication(errorResponse);
                    });

                    it('should let the user know that their attempt to finalize failed.', () => {
                        sinon.assert.calledWith(alertHelperService.addDangerAlert, "We were unable to finalize your application at this time. Please try again later.");
                    });

                    it('should not delete local storage token.', () => {
                        sinon.assert.notCalled(userInformationService.removeTokenFromLocalStorage);
                    });

                    it('should not log the user out of the system.', () => {
                        sinon.assert.notCalled(userInformationService.logOut);
                    });
                });

                describe('success.', () => {
                    beforeEach(() => {
                        service.resolveFinalizeApplication(successResponse);
                    });

                    it('should let the user know that their attempt to finalize succeeded.', () => {
                        sinon.assert.calledWith(alertHelperService.addSuccessAlert, "You have finalized your application to the Bulldog Scholarship.");
                    });

                    it('should delete local storage token.', () => {
                        sinon.assert.calledOnce(userInformationService.removeTokenFromLocalStorage);
                    });

                    it('should log the user out of the system.', () => {
                        sinon.assert.calledWith(userInformationService.logOut, "/Applicant/Finalized");
                    });
                });
            });
        });

        describe('canFinalizeEssay', () => {
            
            it('should return true when everything is success.', () => {
                expectWhatFromCanFinalizeEssay(true);
            });

            describe('lowGrades', () => {
                beforeEach(() => {
                    applicantNotificationService.getLowGradeInformationDashboardInputModel.returns(notFinishedPanel);
                });

                it("shouldn't need a low grade if it is hidden.", () => {
                    applicantNotificationService.getHideLowGradeInformation.returns(true);
                    expectWhatFromCanFinalizeEssay(true);
                });

                it('should need a low grade if it is not hidden.', () => {
                    applicantNotificationService.getHideLowGradeInformation.returns(false);
                    expectWhatFromCanFinalizeEssay(false);
                });
            });

            it('should return false if getExtracurricularActivitiesDashboardInputModel is something other than success.', () => {
                applicantNotificationService.getEssayDashboardInputModels.returns([finishedPanel, finishedPanel, notFinishedPanel]);
                expectWhatFromCanFinalizeEssay(false);
            });

            it('should return false if getExtracurricularActivitiesDashboardInputModel is something other than success.', () => {
                applicantNotificationService.getExtracurricularActivitiesDashboardInputModel.returns(notFinishedPanel);
                expectWhatFromCanFinalizeEssay(false);
            });

            it('should return false if getFamilyInformationDashboardInputModel is something other than success.', () => {
                applicantNotificationService.getFamilyInformationDashboardInputModel.returns(notFinishedPanel);
                expectWhatFromCanFinalizeEssay(false);
            });

            it('should return false if getReferenceDashboardInputModels is something other than success.', () => {
                applicantNotificationService.getReferenceDashboardInputModels.returns(notFinishedPanel);
                expectWhatFromCanFinalizeEssay(false);
            });

            it('should return false if getPersonalInformationDashboardInputModel is something other than success.', () => {
                applicantNotificationService.getPersonalInformationDashboardInputModel.returns(notFinishedPanel);
                expectWhatFromCanFinalizeEssay(false);
            });
            
            it('should return false if getContactInformationDashboardInputModel is something other than success.', () => {
                applicantNotificationService.getContactInformationDashboardInputModel.returns(notFinishedPanel);
                expectWhatFromCanFinalizeEssay(false);
            });

            it('should return false if getTranscriptUploadedDashboardInputModel is something other than success.', () => {
                applicantNotificationService.getTranscriptUploadedDashboardInputModel.returns(notFinishedPanel);
                expectWhatFromCanFinalizeEssay(false);
            });

            it('should return false if getAcademicInformationDashboardInputModel is something other than success.', () => {
                applicantNotificationService.getAcademicInformationDashboardInputModel.returns(notFinishedPanel);
                expectWhatFromCanFinalizeEssay(false);
            });

            function expectWhatFromCanFinalizeEssay(bool: boolean) {
                if (bool) {
                    expect(service.canFinalizeEssay()).toBeTruthy();
                } else {
                    expect(service.canFinalizeEssay()).toBeFalsy();
                }
            }
        });
    });
} 