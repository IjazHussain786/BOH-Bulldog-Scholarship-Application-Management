module BohFoundation.Applicant.Dashboard.Spec.Controllers {
    describe('ApplicantDashboardCtrl', () => {
        var q, scope: Dashboard.Controllers.IApplicantDashboardCtrlScope;
        var applicantNotificationService;
        var deferred, promise, resource;
        var result, modal, finalizeApplicationService;
        var deferred2, promise2, resource2;

        beforeEach(inject(($q, $rootScope) => {
            q = $q;
            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };

            deferred2 = q.defer();
            promise2 = deferred2.promise;
            resource2 = { $promise: promise2 };

            scope = $rootScope;

            applicantNotificationService = sinon.stub({
                getNotifications: () => { },
                resolveGetNotifications: () => { },
                getPersonalInformationDashboardInputModel: () => { },
                getContactInformationDashboardInputModel: () => { },
                getTranscriptUploadedDashboardInputModel: () => { },
                getAcademicInformationDashboardInputModel: () => { },
                getLowGradeInformationDashboardInputModel: () => { },
                getHideLowGradeInformation: () => { },
                getEssayDashboardInputModels: () => { },
                getReferenceDashboardInputModels: () => { },
                getFamilyInformationDashboardInputModel: () => { },
                getExtracurricularActivitiesDashboardInputModel: () => { },
                getDeadlineDate:()=>{}
            });


            modal = new TestHelpers.FakeModal().createModalStub();

            finalizeApplicationService = sinon.stub({
                postFinalizeApplication: () => {},
                resolveFinalizeApplication: () => { },
                canFinalizeEssay: () => { },
            });

            finalizeApplicationService.postFinalizeApplication.returns(resource2);
            applicantNotificationService.getNotifications.returns(resource);
            new Dashboard.Controllers.ApplicantDashboardCtrl(scope, applicantNotificationService, modal, finalizeApplicationService);
        }));

        describe('constructor', () => {
            it('should call getNotifications.', () => {
                sinon.assert.calledOnce(applicantNotificationService.getNotifications);
            });

            describe('success', () => {
                var data = { data: "1239" };
                beforeEach(() => {
                    deferred.resolve(data);
                    scope.$apply();
                });

                it('should call resolve.', () => {
                    sinon.assert.calledWith(applicantNotificationService.resolveGetNotifications, new Common.Models.ServerResponseModel(data, true));
                });
            });

            describe('failure', () => {
                beforeEach(() => {
                    deferred.reject();
                    scope.$apply();
                });

                it('should call resolve.', () => {
                    sinon.assert.calledWith(applicantNotificationService.resolveGetNotifications, new Common.Models.ServerResponseModel(null, false));
                });
            });
        });

        describe('successful constructor', () => {
            beforeEach(() => {
                deferred.resolve();
                scope.$apply();
            });


            describe('finalizeApplication', () => {
                var fakeModal;

                beforeEach(() => {
                    fakeModal = new TestHelpers.FakeModal().createFakeModel();

                    modal.open.returns(fakeModal);

                    scope.finalizeApplication();
                });

                it('should open the modal.', () => {
                    sinon.assert.calledOnce(modal.open);
                });

                describe('cancel modal', () => {
                    beforeEach(() => {
                        scope.modalInstance.dismiss();
                    });

                    it('should not call postFinalizeApplication.', () => {
                        sinon.assert.notCalled(finalizeApplicationService.postFinalizeApplication);
                    });
                });

                describe('ok modal', () => {
                    beforeEach(() => {
                        scope.modalInstance.close();
                    });

                    it('should call postFinalizeApplication.', () => {
                        sinon.assert.calledOnce(finalizeApplicationService.postFinalizeApplication);
                    });

                    describe('success', () => {
                        beforeEach(() => {
                            deferred2.resolve();
                            scope.$apply();
                        });

                        it('should call resolveFinalizeApplication.', () => {
                            sinon.assert.calledWith(finalizeApplicationService.resolveFinalizeApplication, new Common.Models.ServerResponseModel(null, true));
                        });
                    });

                    describe('failure', () => {
                        beforeEach(() => {
                            deferred2.reject();
                            scope.$apply();
                        });

                        it('should call resolveFinalizeApplication.', () => {
                            sinon.assert.calledWith(finalizeApplicationService.resolveFinalizeApplication, new Common.Models.ServerResponseModel(null, false));
                        });
                    });
                });
            });

            describe('canFinalizeApplication', () => {
                var itemToReturn = "asd132";
                beforeEach(() => {
                    finalizeApplicationService.canFinalizeEssay.returns(itemToReturn);
                    result = scope.canFinalizeApplication();
                });

                it('should return whatever the service returns.', () => {
                    expect(result).toBe(itemToReturn);
                });
            });

            describe('getExtracurricularActivitiesItem', () => {
                var itemToReturn = "asd12";
                beforeEach(() => {
                    applicantNotificationService.getExtracurricularActivitiesDashboardInputModel.returns(itemToReturn);
                    result = scope.getExtracurricularActivitiesItem();
                });

                it('should return whatever the service returns.', () => {
                    expect(result).toBe(itemToReturn);
                });
            });

            describe('getPersonalInfoDashboardItem', () => {
                var itemToReturn = "1909";
                beforeEach(() => {
                    applicantNotificationService.getPersonalInformationDashboardInputModel.returns(itemToReturn);
                    result = scope.getPersonalInfoDashboardItem();
                });

                it('should return whatever the service returns.', () => {
                    expect(result).toBe(itemToReturn);
                });
            });
            
            describe('getContactInfoDashboardItem', () => {
                var itemToReturn = "asd1";
                beforeEach(() => {
                    applicantNotificationService.getFamilyInformationDashboardInputModel.returns(itemToReturn);
                    result = scope.getFamilyInformationDashboardItem();
                });

                it('should return whatever the service returns.', () => {
                    expect(result).toBe(itemToReturn);
                });
            });

            describe('getContactInfoDashboardItem', () => {
                var itemToReturn = "asd1";
                beforeEach(() => {
                    applicantNotificationService.getContactInformationDashboardInputModel.returns(itemToReturn);
                    result = scope.getContactInfoDashboardItem();
                });

                it('should return whatever the service returns.', () => {
                    expect(result).toBe(itemToReturn);
                });
            });

            describe('getTransciptUploadedDashboardItem', () => {
                var itemToReturn = "1235143536236";
                beforeEach(() => {
                    applicantNotificationService.getTranscriptUploadedDashboardInputModel.returns(itemToReturn);
                    result = scope.getTransciptUploadedDashboardItem();
                });

                it('should return whatever the service returns.', () => {
                    expect(result).toBe(itemToReturn);
                });
            });

            describe('getAcademicInformationDashboardItem', () => {
                var itemToReturn = "adasd1";
                beforeEach(() => {
                    applicantNotificationService.getAcademicInformationDashboardInputModel.returns(itemToReturn);
                    result = scope.getAcademicInformationDashboardItem();
                });

                it('should return whatever the service returns.', () => {
                    expect(result).toBe(itemToReturn);
                });
            });

            describe('getLowGradeDashboardItem', () => {
                var itemToReturn = "asd11223123123123123";
                beforeEach(() => {
                    applicantNotificationService.getLowGradeInformationDashboardInputModel.returns(itemToReturn);
                    result = scope.getLowGradeDashboardItem();
                });

                it('should return whatever the service returns.', () => {
                    expect(result).toBe(itemToReturn);
                });
            });

            describe('getHideLowGrade', () => {
                var itemToReturn = true;
                beforeEach(() => {
                    applicantNotificationService.getHideLowGradeInformation.returns(itemToReturn);
                    result = scope.getHideLowGrade();
                });

                it('should return whatever the service returns.', () => {
                    expect(result).toBe(itemToReturn);
                });
            });

            describe('getEssayDashboardInputModels', () => {
                var itemToReturn = "Hidelsoakjn";
                beforeEach(() => {
                    applicantNotificationService.getEssayDashboardInputModels.returns(itemToReturn);
                    result = scope.getEssayDashboardInputModels();
                });

                it('should return whatever the service returns.', () => {
                    expect(result).toBe(itemToReturn);
                });
            });

            describe('getReferenceItem', () => {
                var itemToReturn = "asdasdasdds";
                beforeEach(() => {
                    applicantNotificationService.getReferenceDashboardInputModels.returns(itemToReturn);
                    result = scope.getReferenceItem();
                });

                it('should return whatever the service returns.', () => {
                    expect(result).toBe(itemToReturn); 
                });
            });

            describe('getDeadlineDate', () => {
                var itemToReturn = "asdasdasdds";
                beforeEach(() => {
                    applicantNotificationService.getDeadlineDate.returns(itemToReturn);
                    result = scope.getDeadlineDate();
                });

                it('should return whatever the service returns.', () => {
                    expect(result).toBe(itemToReturn);
                });
            });
        });
    });
}