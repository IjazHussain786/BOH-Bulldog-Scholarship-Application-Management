module BohFoundation.Applicant.Dashboard.Spec.Services {
    describe('ApplicantNotificationService', () => {
        var alertHelperService, authRequiredRepository, applicantEssayService;
        var applicantNotificationsApiEndpoint = '/api/applicant/notifications';
        var service;
        var result;

        beforeEach(() => {
            alertHelperService = sinon.stub({
                addDangerAlert:()=>{}
            });

            authRequiredRepository = sinon.stub({
                get:()=>{}
            });

            applicantEssayService = sinon.stub({
                setArrayOfEssayNotificationsModelFromNotificationService: () => {}
            });

            service = new Dashboard.Services.ApplicantNotificationService(authRequiredRepository, alertHelperService, applicantEssayService);
        });

        describe('construction', () => {
            it('essayDashboardInputModels should be defined.', () => {
                expect(service.essayDashboardInputModels).toBeDefined();
            });

            it('essayDashboardInputModels should be a blank array.', () => {
                expect(service.essayDashboardInputModels.length).toBe(0);
            });
        });

        describe('getEssayDashboardInputModels', () => {
            var bank = 1293090123;
            beforeEach(() => {
                service.essayDashboardInputModels = bank;
            });

            it('should return whatever is on the essayDashboardInputModels.', () => {
                expect(service.getEssayDashboardInputModels()).toBe(bank);
            });
        });

        describe('getNotifications', () => {
            beforeEach(() => {
                authRequiredRepository.get.returns(applicantNotificationsApiEndpoint);
                result = service.getNotifications();
            });

            it('should call authRequiredRepo.', () => {
                sinon.assert.calledWith(authRequiredRepository.get, applicantNotificationsApiEndpoint);
            });

            it('should return what authRepo returns.', () => {
                expect(result).toBe(applicantNotificationsApiEndpoint);
            });
        });

        describe('getFamilyInformationDashboardInputModel', () => {
            var zoeDoggy = "zoe doggy";
            beforeEach(() => {
                service.extracurricularActivitiesDashboardInputModel = zoeDoggy;
                result = service.getExtracurricularActivitiesDashboardInputModel();
            });

            it('should return whatever is on the field.', () => {
                expect(result).toBe(zoeDoggy);
            });
        });

        describe('getFamilyInformationDashboardInputModel', () => {
            var zoeDoggy = "zoe doggy";
            beforeEach(() => {
                service.familyInformationDashboardInputModel = zoeDoggy;
                result = service.getFamilyInformationDashboardInputModel();
            });

            it('should return whatever is on the field.', () => {
                expect(result).toBe(zoeDoggy);
            });
        });
        
        describe('getPersonalInformationDashboardInputModel', () => {
            var zoeDoggy = "zoe doggy";
            beforeEach(() => {
                service.personalInformationDashboardInputModel = zoeDoggy;
                result = service.getPersonalInformationDashboardInputModel();
            });

            it('should return whatever is on the field.', () => {
                expect(result).toBe(zoeDoggy);
            });
        });

        describe('getTranscriptUploadedDashboardInputModel', () => {
            var item = "1298098189";
            beforeEach(() => {
                service.transciptUploadedDashboardInputModel = item;
                result = service.getTranscriptUploadedDashboardInputModel();
            });

            it('should return whatever is on the field.', () => {
                expect(result).toBe(item);
            });
        });

        describe('getContactInformationDashboardInputModel', () => {
            var zoeDoggy = "zoe doggy2";
            beforeEach(() => {
                service.contactInformationDashboardInputModel = zoeDoggy;
                result = service.getContactInformationDashboardInputModel();
            });

            it('should return whatever is on the field.', () => {
                expect(result).toBe(zoeDoggy);
            });
        });

        describe('getAcademicInformationDashboardInputModel', () => {
            var zoeDoggy = "ad1213213";
            beforeEach(() => {
                service.academicInformationDashboardInputModel = zoeDoggy;
                result = service.getAcademicInformationDashboardInputModel();
            });

            it('should return whatever is on the field.', () => {
                expect(result).toBe(zoeDoggy);
            });
        });

        describe('getLowGradeInformationDashboardInputModel', () => {
            var zoeDoggy = "asd1122312";
            beforeEach(() => {
                service.lowGradeDashboadInputModel = zoeDoggy;
                result = service.getLowGradeInformationDashboardInputModel();
            });

            it('should return whatever is on the field.', () => {
                expect(result).toBe(zoeDoggy);
            });
        });

        describe('getHideLowGradeInformation', () => {
            var zoeDoggy = "asd3121414";
            beforeEach(() => {
                service.hideLowGradeInformation = zoeDoggy;
                result = service.getHideLowGradeInformation();
            });

            it('should return whatever is on the field.', () => {
                expect(result).toBe(zoeDoggy);
            });
        });

        describe('getReferenceDashboardInputModels', () => {
            var zoeDoggy = "asdasfsadf";
            beforeEach(() => {
                service.referenceDashboardInputModels = zoeDoggy;
                result = service.getReferenceDashboardInputModels();
            });

            it('should return whatever is on the field.', () => {
                expect(result).toBe(zoeDoggy);
            });
        });

        describe('getDeadlineDate', () => {
            describe('defined', () => {
                var zoeDoggy = "asdasfsadf";
                beforeEach(() => {
                    service.deadlineDate = zoeDoggy;
                    result = service.getDeadlineDate();
                });

                it('should return whatever is on the field.', () => {
                    expect(result).toBe(zoeDoggy);
                });
            });
        });

        describe('resolveGetNotifications', () => {
            var serverResponse;

            describe('failure', () => {
                beforeEach(() => {
                    serverResponse = new Common.Models.ServerResponseModel(null, false);
                    service.resolveGetNotifications(serverResponse);
                });

                it('should call alertHelperService.', () => {
                    sinon.assert.calledWith(alertHelperService.addDangerAlert, "There was a problem getting your notifications.");
                });
            });

            describe('success', () => {
                var objectFromServer;
                var lowGradeInformationModelFromServer;
                var date, date2, date3, date4, date5, date6, date7, date8;
                var applicantReferenceCountsModel;

                date = new Date();
                date2 = new Date(2019, 3, 4);
                date3 = new Date(2011, 3, 9);
                date4 = new Date(2013, 9, 12);
                date5 = new Date(2019, 11, 10);
                date6 = new Date(2013, 2, 10);
                date7 = new Date(2000, 1, 20);
                date8 = new Date(2004, 2, 21);

                describe('deadlineDate', () => {
                    beforeEach(() => {
                        applicantReferenceCountsModel = new Dtos.Applicant.Notifications.ApplicantReferenceCountsModel(0, 0);
                        lowGradeInformationModelFromServer = { gpa: null, numberOfLowGradesInformationSaved: 0, lastUpdatedLowGrade: null };
                        objectFromServer = new Dtos.Applicant.ApplicantNotificationsModel(null, null, null, null, lowGradeInformationModelFromServer, [], applicantReferenceCountsModel, null, null, date8);
                        serverResponse = new Common.Models.ServerResponseModel(objectFromServer, true);
                        service.resolveGetNotifications(serverResponse);
                    });

                    it('should have a deadline date that is the same as date8.', () => {
                        expect(service.deadlineDate).toBe(date8);
                    });
                });

                describe('null personal information', () => {
                    beforeEach(() => {
                        applicantReferenceCountsModel = new Dtos.Applicant.Notifications.ApplicantReferenceCountsModel(0, 0);
                        lowGradeInformationModelFromServer = { gpa: null, numberOfLowGradesInformationSaved: 0, lastUpdatedLowGrade: null };
                        objectFromServer = new Dtos.Applicant.ApplicantNotificationsModel(null, null, null, null, lowGradeInformationModelFromServer, [], applicantReferenceCountsModel, null, null);
                        serverResponse = new Common.Models.ServerResponseModel(objectFromServer, true);
                        service.resolveGetNotifications(serverResponse);
                    });

                    describe('extracurricularActivitiesDashboardInputModel', () => {
                        it('should have a warning style.', () => {
                            expect(service.extracurricularActivitiesDashboardInputModel.buttonStyle).toBe(Common.Enums.StyleEnum.Warning);
                        });

                        it('should have a title.', () => {
                            expect(service.extracurricularActivitiesDashboardInputModel.title).toBe("Extracurriculars");
                        });

                        it('should have a link.', () => {
                            expect(service.extracurricularActivitiesDashboardInputModel.link).toBe("/Applicant/Extracurriculars");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.extracurricularActivitiesDashboardInputModel.lastUpdated).toBeUndefined();
                        });
                    });

                    describe('familyInformationDashboardInputModel', () => {
                        it('should have a warning style.', () => {
                            expect(service.familyInformationDashboardInputModel.buttonStyle).toBe(Common.Enums.StyleEnum.Warning);
                        });

                        it('should have a title.', () => {
                            expect(service.familyInformationDashboardInputModel.title).toBe("Family Information");
                        });

                        it('should have a link.', () => {
                            expect(service.familyInformationDashboardInputModel.link).toBe("/Applicant/FamilyInformation");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.familyInformationDashboardInputModel.lastUpdated).toBeUndefined();
                        });
                    });

                    describe('personalInformationDashbordInputModel', () => {
                        it('should have a warning style.', () => {
                            expect(service.personalInformationDashboardInputModel.buttonStyle).toBe(Common.Enums.StyleEnum.Warning);
                        });

                        it('should have a title.', () => {
                            expect(service.personalInformationDashboardInputModel.title).toBe("Personal Information");
                        });

                        it('should have a link.', () => {
                            expect(service.personalInformationDashboardInputModel.link).toBe("/Applicant/PersonalInformation");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.personalInformationDashboardInputModel.lastUpdated).toBeUndefined();
                        });
                    });

                    describe('contactInformationDashbordInputModel', () => {
                        it('should have a warning style.', () => {
                            expect(service.contactInformationDashboardInputModel.buttonStyle).toBe(Common.Enums.StyleEnum.Warning);
                        });

                        it('should have a title.', () => {
                            expect(service.contactInformationDashboardInputModel.title).toBe("Contact Information");
                        });

                        it('should have a link.', () => {
                            expect(service.contactInformationDashboardInputModel.link).toBe("/Person/ContactInformation");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.contactInformationDashboardInputModel.lastUpdated).toBeUndefined();
                        });
                    });

                    describe('contactInformationDashbordInputModel', () => {
                        it('should have a warning style.', () => {
                            expect(service.transciptUploadedDashboardInputModel.buttonStyle).toBe(Common.Enums.StyleEnum.Warning);
                        });

                        it('should have a title.', () => {
                            expect(service.transciptUploadedDashboardInputModel.title).toBe("Upload Transcript");
                        });

                        it('should have a link.', () => {
                            expect(service.transciptUploadedDashboardInputModel.link).toBe("/Applicant/Transcript");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.transciptUploadedDashboardInputModel.lastUpdated).toBeUndefined();
                        });
                    });

                    describe('academicInformationDashboardInputModel', () => {
                        it('should have a warning style.', () => {
                            expect(service.academicInformationDashboardInputModel.buttonStyle).toBe(Common.Enums.StyleEnum.Warning);
                        });

                        it('should have a title.', () => {
                            expect(service.academicInformationDashboardInputModel.title).toBe("Academic Information");
                        });

                        it('should have a link.', () => {
                            expect(service.academicInformationDashboardInputModel.link).toBe("/Applicant/AcademicInformation");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.academicInformationDashboardInputModel.lastUpdated).toBeUndefined();
                        });
                    });

                    describe('essayDashboardInputModels', () => {
                        it('should have a blank array.', () => {
                            expect(service.essayDashboardInputModels.length).toBe(0);
                        });
                    });
                });

                describe('all return things', () => {
                    var prompt = "prompt", essayNotification1, essayNotification2, title = "title";
                    var arrayOfEssayNotifications;

                    beforeEach(() => {
                        essayNotification1 = new Dtos.Applicant.Notifications.EssayNotificationsModel(prompt + 1, title + 1, 1, null);
                        essayNotification2 = new Dtos.Applicant.Notifications.EssayNotificationsModel(prompt + 2, title + 2, 2, date);
                        service.essayDashboardInputModels = ["1"];

                        lowGradeInformationModelFromServer = { gpa: 4.0, numberOfLowGradesInformationSaved: 0, lastUpdatedLowGrade: date5 };
                        arrayOfEssayNotifications = [essayNotification1, essayNotification2];
                        objectFromServer = new Dtos.Applicant.ApplicantNotificationsModel(date, date2, date3, date4, lowGradeInformationModelFromServer, arrayOfEssayNotifications, applicantReferenceCountsModel, date6, date7);
                        serverResponse = new Common.Models.ServerResponseModel(objectFromServer, true);

                        service.resolveGetNotifications(serverResponse);
                    });

                    describe('extracurricularActivitiesDashboardInputModel', () => {
                        it('should have a success style.', () => {
                            expect(service.extracurricularActivitiesDashboardInputModel.buttonStyle).toBe(Common.Enums.StyleEnum.Success);
                        });

                        it('should have a title.', () => {
                            expect(service.extracurricularActivitiesDashboardInputModel.title).toBe("Extracurriculars");
                        });

                        it('should have a link.', () => {
                            expect(service.extracurricularActivitiesDashboardInputModel.link).toBe("/Applicant/Extracurriculars");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.extracurricularActivitiesDashboardInputModel.lastUpdated).toBe(date7);
                        });
                    });

                    describe('essayDashboardInputModels', () => {
                        describe('first one without a revisionDateTime', () => {
                            it('should have a warning style.', () => {
                                expect(service.essayDashboardInputModels[0].buttonStyle).toBe(Common.Enums.StyleEnum.Warning);
                            });

                            it('should have a title.', () => {
                                expect(service.essayDashboardInputModels[0].title).toBe(title+1);
                            });

                            it('should have a link.', () => {
                                expect(service.essayDashboardInputModels[0].link).toBe("/Applicant/Essay/1");
                            });

                            it('should have a lastUpdated.', () => {
                                expect(service.essayDashboardInputModels[0].lastUpdated).toBe(undefined);
                            });

                            it('should call setArrayOfEssayNotificationsModelFromNotificationService.', () => {
                                sinon.assert.calledWith(applicantEssayService.setArrayOfEssayNotificationsModelFromNotificationService, arrayOfEssayNotifications);
                            });
                        });

                        describe('second one with a revisionDateTime', () => {
                            it('should have a success style.', () => {
                                expect(service.essayDashboardInputModels[1].buttonStyle).toBe(Common.Enums.StyleEnum.Success);
                            });

                            it('should have a title.', () => {
                                expect(service.essayDashboardInputModels[1].title).toBe(title + 2);
                            });

                            it('should have a link.', () => {
                                expect(service.essayDashboardInputModels[1].link).toBe("/Applicant/Essay/2");
                            });

                            it('should have a lastUpdated.', () => {
                                expect(service.essayDashboardInputModels[1].lastUpdated).toBe(date);
                            });
                        });
                    });

                    describe('personalInformationDashbordInputModel', () => {
                        it('should have a success style.', () => {
                            expect(service.familyInformationDashboardInputModel.buttonStyle).toBe(Common.Enums.StyleEnum.Success);
                        });

                        it('should have a title.', () => {
                            expect(service.familyInformationDashboardInputModel.title).toBe("Family Information");
                        });

                        it('should have a link.', () => {
                            expect(service.familyInformationDashboardInputModel.link).toBe("/Applicant/FamilyInformation");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.familyInformationDashboardInputModel.lastUpdated).toBe(date6);
                        });
                    });

                    describe('personalInformationDashbordInputModel', () => {
                        it('should have a success style.', () => {
                            expect(service.personalInformationDashboardInputModel.buttonStyle).toBe(Common.Enums.StyleEnum.Success);
                        });

                        it('should have a title.', () => {
                            expect(service.personalInformationDashboardInputModel.title).toBe("Personal Information");
                        });

                        it('should have a link.', () => {
                            expect(service.personalInformationDashboardInputModel.link).toBe("/Applicant/PersonalInformation");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.personalInformationDashboardInputModel.lastUpdated).toBe(date);
                        });
                    });

                    describe('contactInformationDashbordInputModel', () => {
                        it('should have a success style.', () => {
                            expect(service.contactInformationDashboardInputModel.buttonStyle).toBe(Common.Enums.StyleEnum.Success);
                        });

                        it('should have a title.', () => {
                            expect(service.contactInformationDashboardInputModel.title).toBe("Contact Information");
                        });

                        it('should have a link.', () => {
                            expect(service.contactInformationDashboardInputModel.link).toBe("/Person/ContactInformation");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.contactInformationDashboardInputModel.lastUpdated).toBe(date2);
                        });
                    });

                    describe('contactInformationDashbordInputModel', () => {
                        it('should have a success style.', () => {
                            expect(service.transciptUploadedDashboardInputModel.buttonStyle).toBe(Common.Enums.StyleEnum.Success);
                        });

                        it('should have a title.', () => {
                            expect(service.transciptUploadedDashboardInputModel.title).toBe("Upload Transcript");
                        });

                        it('should have a link.', () => {
                            expect(service.transciptUploadedDashboardInputModel.link).toBe("/Applicant/Transcript");
                        }); 

                        it('should have a lastUpdated.', () => {
                            expect(service.transciptUploadedDashboardInputModel.lastUpdated).toBe(date3);
                        }); 
                    });

                    describe('academicInformationDashboardInputModel', () => {
                        it('should have a success style.', () => {
                            expect(service.academicInformationDashboardInputModel.buttonStyle).toBe(Common.Enums.StyleEnum.Success);
                        });

                        it('should have a title.', () => {
                            expect(service.academicInformationDashboardInputModel.title).toBe("Academic Information");
                        });

                        it('should have a link.', () => {
                            expect(service.academicInformationDashboardInputModel.link).toBe("/Applicant/AcademicInformation");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.academicInformationDashboardInputModel.lastUpdated).toBe(date4);
                        });
                    });

                    describe('lowGradeDashboadInputModel', () => {
                        it('should have a warning style.', () => {
                            expect(service.lowGradeDashboadInputModel.buttonStyle).toBeUndefined();
                        });

                        it('should have a title.', () => {
                            expect(service.lowGradeDashboadInputModel.title).toBe("Low Grades Explanations");
                        });

                        it('should have a link.', () => {
                            expect(service.lowGradeDashboadInputModel.link).toBe("/Applicant/LowGrades");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.lowGradeDashboadInputModel.lastUpdated).toBeUndefined();
                        });

                        it('should hide the low grade information item.', () => {
                            expect(service.hideLowGradeInformation).toBeTruthy();
                        });
                    });
                });

                describe('referenceDashboardInputModels', () => {
                    describe('two zeros', () => {
                        beforeEach(() => {
                            howManyReferences(0, 0);
                        });

                        correctUrlAndName();

                        it('should have a warning style.', () => {
                            expect(service.referenceDashboardInputModels.buttonStyle).toBe(Common.Enums.StyleEnum.Warning);
                        });
                    });

                    describe('two twos', () => {
                        beforeEach(() => {
                            howManyReferences(2, 2);
                        });

                        correctUrlAndName();

                        it('should have a warning style.', () => {
                            expect(service.referenceDashboardInputModels.buttonStyle).toBe(Common.Enums.StyleEnum.Warning);
                        });
                    });

                    describe('three send two recieved', () => {
                        beforeEach(() => {
                            howManyReferences(3, 2);
                        });

                        correctUrlAndName();

                        it('should have a info style.', () => {
                            expect(service.referenceDashboardInputModels.buttonStyle).toBe(Common.Enums.StyleEnum.Info);
                        });
                    });

                    describe('two threes', () => {
                        beforeEach(() => {
                            howManyReferences(3, 3);
                        });

                        correctUrlAndName();

                        it('should have a success style.', () => {
                            expect(service.referenceDashboardInputModels.buttonStyle).toBe(Common.Enums.StyleEnum.Success);
                        });
                    });

                    describe("defined last updated", () => {
                        beforeEach(() => {
                            howManyReferences(1, 1, date2);
                        });

                        it('should have date equal to whatever was passed in.', () => {
                            expect(service.referenceDashboardInputModels.lastUpdated).toBe(date2);
                        });
                    });

                    function correctUrlAndName() {
                        it('should have a title.', () => {
                            expect(service.referenceDashboardInputModels.title).toBe("References");
                        });

                        it('should have a link.', () => {
                            expect(service.referenceDashboardInputModels.link).toBe("/Applicant/References");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.referenceDashboardInputModels.lastUpdated).toBeUndefined();
                        });
                    }

                    function howManyReferences(sent:number, recieved:number, lastUpdated = null) {
                        applicantReferenceCountsModel = new Dtos.Applicant.Notifications.ApplicantReferenceCountsModel(sent, recieved, lastUpdated);
                        lowGradeInformationModelFromServer = { gpa: null, numberOfLowGradesInformationSaved: 0, lastUpdatedLowGrade: null };
                        objectFromServer = new Dtos.Applicant.ApplicantNotificationsModel(null, null, null, null, lowGradeInformationModelFromServer, [], applicantReferenceCountsModel);
                        serverResponse = new Common.Models.ServerResponseModel(objectFromServer, true);
                        service.resolveGetNotifications(serverResponse);
                    }
                });

                describe('lowGradeDashboadInputModel', () => {
                    describe('academic info not present.', () => {
                        beforeEach(() => {
                            lowGradeInformationModelFromServer = { gpa: null, numberOfLowGradesInformationSaved: 0, lastUpdatedLowGrade: null };
                            objectFromServer = new Dtos.Applicant.ApplicantNotificationsModel(null, null, null, null, lowGradeInformationModelFromServer, [], applicantReferenceCountsModel);
                            serverResponse = new Common.Models.ServerResponseModel(objectFromServer, true);
                            service.resolveGetNotifications(serverResponse);
                        });

                        it('should have a warning style.', () => {
                            expect(service.lowGradeDashboadInputModel.buttonStyle).toBeUndefined();
                        });

                        it('should have a title.', () => {
                            expect(service.lowGradeDashboadInputModel.title).toBe("Low Grades Explanations");
                        });

                        it('should have a link.', () => {
                            expect(service.lowGradeDashboadInputModel.link).toBe("/Applicant/LowGrades");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.lowGradeDashboadInputModel.lastUpdated).toBeUndefined();
                        });

                        it('should hide the low grade information item.', () => {
                            expect(service.hideLowGradeInformation).toBeTruthy();
                        });
                    });

                    describe('high gpa', () => {
                        beforeEach(() => {
                            lowGradeInformationModelFromServer = { gpa: 4.0, numberOfLowGradesInformationSaved: 0, lastUpdatedLowGrade: date3 };
                            objectFromServer = new Dtos.Applicant.ApplicantNotificationsModel(null, null, null, null, lowGradeInformationModelFromServer, [], applicantReferenceCountsModel);
                            serverResponse = new Common.Models.ServerResponseModel(objectFromServer, true);
                            service.resolveGetNotifications(serverResponse);
                        });

                        it('should have a warning style.', () => {
                            expect(service.lowGradeDashboadInputModel.buttonStyle).toBeUndefined();
                        });

                        it('should have a title.', () => {
                            expect(service.lowGradeDashboadInputModel.title).toBe("Low Grades Explanations");
                        });

                        it('should have a link.', () => {
                            expect(service.lowGradeDashboadInputModel.link).toBe("/Applicant/LowGrades");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.lowGradeDashboadInputModel.lastUpdated).toBeUndefined();
                        });

                        it('should hide the low grade information item.', () => {
                            expect(service.hideLowGradeInformation).toBeTruthy();
                        });
                    });

                    describe('low gpa not completed', () => {
                        beforeEach(() => {
                            lowGradeInformationModelFromServer = { gpa: 1.0, numberOfLowGradesInformationSaved: 0, lastUpdatedLowGrade: date3 };
                            objectFromServer = new Dtos.Applicant.ApplicantNotificationsModel(null, null, null, null, lowGradeInformationModelFromServer, [], applicantReferenceCountsModel);
                            serverResponse = new Common.Models.ServerResponseModel(objectFromServer, true);
                            service.resolveGetNotifications(serverResponse);
                        });

                        it('should have a warning style.', () => {
                            expect(service.lowGradeDashboadInputModel.buttonStyle).toBe(Common.Enums.StyleEnum.Warning);
                        });

                        it('should have a title.', () => {
                            expect(service.lowGradeDashboadInputModel.title).toBe("Low Grades Explanations");
                        });

                        it('should have a link.', () => {
                            expect(service.lowGradeDashboadInputModel.link).toBe("/Applicant/LowGrades");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.lowGradeDashboadInputModel.lastUpdated).toBe(date3);
                        });

                        it('should hide the low grade information item.', () => {
                            expect(service.hideLowGradeInformation).toBeFalsy();
                        });
                    });

                    describe('low gpa not completed with null last updated', () => {
                        beforeEach(() => {
                            lowGradeInformationModelFromServer = { gpa: 1.0, numberOfLowGradesInformationSaved: 0, lastUpdatedLowGrade: null };
                            objectFromServer = new Dtos.Applicant.ApplicantNotificationsModel(null, null, null, null, lowGradeInformationModelFromServer, [], applicantReferenceCountsModel);
                            serverResponse = new Common.Models.ServerResponseModel(objectFromServer, true);
                            service.resolveGetNotifications(serverResponse);
                        });

                        it('should have a warning style.', () => {
                            expect(service.lowGradeDashboadInputModel.buttonStyle).toBe(Common.Enums.StyleEnum.Warning);
                        });

                        it('should have a title.', () => {
                            expect(service.lowGradeDashboadInputModel.title).toBe("Low Grades Explanations");
                        });

                        it('should have a link.', () => {
                            expect(service.lowGradeDashboadInputModel.link).toBe("/Applicant/LowGrades");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.lowGradeDashboadInputModel.lastUpdated).toBeUndefined();
                        });

                        it('should hide the low grade information item.', () => {
                            expect(service.hideLowGradeInformation).toBeFalsy();
                        });
                    });

                    describe('low gpa not completed with null last updated', () => {
                        beforeEach(() => {
                            lowGradeInformationModelFromServer = { gpa: 1.0, numberOfLowGradesInformationSaved: 5, lastUpdatedLowGrade: date };
                            objectFromServer = new Dtos.Applicant.ApplicantNotificationsModel(null, null, null, null, lowGradeInformationModelFromServer, [], applicantReferenceCountsModel);
                            serverResponse = new Common.Models.ServerResponseModel(objectFromServer, true);
                            service.resolveGetNotifications(serverResponse);
                        });

                        it('should have a success style.', () => {
                            expect(service.lowGradeDashboadInputModel.buttonStyle).toBe(Common.Enums.StyleEnum.Success);
                        });

                        it('should have a title.', () => {
                            expect(service.lowGradeDashboadInputModel.title).toBe("Low Grades Explanations");
                        });

                        it('should have a link.', () => {
                            expect(service.lowGradeDashboadInputModel.link).toBe("/Applicant/LowGrades");
                        });

                        it('should have a lastUpdated.', () => {
                            expect(service.lowGradeDashboadInputModel.lastUpdated).toBe(date);
                        });

                        it('should hide the low grade information item.', () => {
                            expect(service.hideLowGradeInformation).toBeFalsy();
                        });
                    });
                });
            });
        });
    });
}