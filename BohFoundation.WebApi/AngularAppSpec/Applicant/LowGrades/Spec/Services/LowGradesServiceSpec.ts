module BohFoundation.Applicant.LowGrades.Spec.Services {
    describe('LowGradesService', () => {
        var typeOfData = "low grade";
        var lowGradesApiEndpoint = "/api/applicant/lowgrades";
        var service;
        var authRepo, result, errorResponse;
        var emptyArray = [];
        var fullArray, lowGradeOne, lowGradeTwo, date;
        var gpa = 2.0;
        var lowGradesWithGpaDto: Dtos.Applicant.Academic.LowGradesWithGpaModel;
        var modelFromServer;
        var grade = "grade", classTitle = "class", explanation = "explanation";
        var successResponse, genericResolver;
        var commonStubs;

        beforeEach(() => {
            commonStubs = new TestHelpers.CommonStubs();
            date = new Date(2013, 10, 1);
            lowGradeOne = new Dtos.Applicant.Academic.LowGradeModel(grade + 0, classTitle+0, false, Common.Enums.YearOfHighSchool.Freshman, explanation + 0, date);
            lowGradeTwo = new Dtos.Applicant.Academic.LowGradeModel(grade + 1, classTitle + 1, true, Common.Enums.YearOfHighSchool.Sophomore, explanation + 1);
            fullArray = [lowGradeOne, lowGradeTwo];

            modelFromServer = { data: null };

            errorResponse = new Common.Models.ServerResponseModel(null, false);
            successResponse = new Common.Models.ServerResponseModel(null, true);

            genericResolver = commonStubs.createGenericResolver();

            authRepo = commonStubs.createAuthRepo();

            service = new LowGrades.Services.LowGradesService(authRepo, genericResolver);
        });

        describe('getMethods', () => {
            describe('getLowGradesArray', () => {
                it('should return whatever is on that property.', () => {
                    service.lowGradesArray = date;
                    expect(service.getLowGradesArray()).toBe(date);
                });
            });

            describe('getLowGradeNotificationInformationModel', () => {
                it('should return whatever is on that property.', () => {
                    service.lowGradeNotificationsInformationModel = date;
                    expect(service.getLowGradeNotificationInformationModel()).toBe(date);
                });

                it('should return a getLowGradeNotificationsInformation with defaults if it is undefined.', () => {
                    service.lowGradeNotificationsInformationModel = undefined;
                    expect(service.getLowGradeNotificationInformationModel()).toMatch(new Dtos.Applicant.Notifications.LowGradeNotificationInformationModel(0, 0.0));
                });
            });

            describe('getArraySameAsServer', () => {
                it('should return whatever is on that property.', () => {
                    service.arraySameAsServer = true;
                    expect(service.getArraySameAsServer()).toBeTruthy();
                });
            });
        });

        describe('crud on array methods', () => {
            beforeEach(() => {
                service.arraySameAsServer = true;
                service.lowGradesArray = fullArray;
                service.lowGradeNotificationsInformationModel = new Dtos.Applicant.Notifications.LowGradeNotificationInformationModel(2, gpa, date);
            });

            describe('deleteLowGrade', () => {
                beforeEach(() => {
                    service.deleteLowGrade(0);
                });

                it('should have one item in the array.', () => {
                    expect(service.lowGradesArray.length).toBe(1);
                });

                it('should have a flag for not being synced.', () => {
                    arraySameAsServer(false);
                });

                it('should have item1 in position 0.', () => {
                    expect(service.lowGradesArray[0]).toBe(lowGradeTwo);
                });

                it('should have 4 items outstanding.', () => {
                    expect(service.lowGradeNotificationsInformationModel.lowGradesNeededOutstanding).toBe(4);
                });

                it('should have just updated.', () => {
                    var localDate = new Date();
                    expect(Math.abs(localDate.getMilliseconds() - service.lowGradeNotificationsInformationModel.lastUpdatedLowGrade.getMilliseconds())).toBeLessThan(2);
                });
            });

            describe('modifyLowGrade', () => {
                beforeEach(() => {
                    service.modifyLowGrade(1, lowGradeOne);
                });

                it('should have one item in the array.', () => {
                    expect(service.lowGradesArray.length).toBe(2);
                });

                it('should have a flag for not being synced.', () => {
                    arraySameAsServer(false);
                });

                it('should have item1 in position 1.', () => {
                    expect(service.lowGradesArray[1]).toBe(lowGradeOne);
                });

                it('should have item1 in position 0.', () => {
                    expect(service.lowGradesArray[0]).toBe(lowGradeOne);
                });
            });

            describe('addLowGrade', () => {
                var lowGradeThree;
                beforeEach(() => {
                    lowGradeThree = new Dtos.Applicant.Academic.LowGradeModel;
                    service.addLowGrade(lowGradeThree);
                });

                it('should have one item in the array.', () => {
                    expect(service.lowGradesArray.length).toBe(3);
                });

                it('should have a flag for not being synced.', () => {
                    arraySameAsServer(false);
                });

                it('should have item3 in position 2.', () => {
                    expect(service.lowGradesArray[2]).toBe(lowGradeThree);
                });

                it('should have item1 in position 0.', () => {
                    expect(service.lowGradesArray[0]).toBe(lowGradeOne);
                });

                it('should have item2 in position 1.', () => {
                    expect(service.lowGradesArray[1]).toBe(lowGradeTwo);
                });

                it('should have 2 items outstanding.', () => {
                    expect(service.lowGradeNotificationsInformationModel.lowGradesNeededOutstanding).toBe(2);
                });

                it('should have just updated.', () => {
                    var localDate = new Date();
                    expect(Math.abs(localDate.getMilliseconds() - service.lowGradeNotificationsInformationModel.lastUpdatedLowGrade.getMilliseconds())).toBeLessThan(2);
                });
            });
        });

        describe('postLowGrades', () => {
            beforeEach(() => {
                service.lowGradesArray = fullArray;
                service.lowGradeNotificationsInformationModel = new Dtos.Applicant.Notifications.LowGradeNotificationInformationModel(3, gpa, date);
            });

            describe('post', () => {
                beforeEach(() => {
                    authRepo.post.returns(lowGradesApiEndpoint);
                    result = service.postLowGrades();
                });

                it('should call authrepo with array and endpoin info.', () => {
                    sinon.assert.calledWith(authRepo.post, fullArray, lowGradesApiEndpoint);
                });

                it('should return what authRepo post returns.', () => {
                    expect(result).toBe(lowGradesApiEndpoint); 
                });
            });

            describe('resolve', () => {
                describe('failure', () => {
                    it('should pass whatever is passed in to the resolver with the correct information type to the generic resolver.', () => {
                        service.resolvePostLowGrades(errorResponse);
                        sinon.assert.calledWith(genericResolver.genericPostResolver, typeOfData, errorResponse);
                    });
                });

                describe('success', () => {
                    beforeEach(() => {
                        service.resolvePostLowGrades(successResponse);
                    });

                    it('should set same as server to true.', () => {
                        expect(service.arraySameAsServer).toBeTruthy();
                    });

                    it('should pass whatever is passed in to the resolver with the correct information type to the generic resolver.', () => {
                        sinon.assert.calledWith(genericResolver.genericPostResolver, typeOfData, successResponse);
                    });

                    it('should set the lastUpdated to now.', () => {
                        var localDate = new Date();
                        expect(Math.abs(localDate.getMilliseconds() - service.lowGradeNotificationsInformationModel.lastUpdatedLowGrade.getMilliseconds())).toBeLessThan(2);
                    });
                });
            });
        });

        describe('getLowGrades', () => {
            describe('get', () => {
                beforeEach(() => {
                    authRepo.get.returns(typeOfData);
                    result = service.getLowGrades();
                });

                it('should call authRepo with endpoint string.', () => {
                    sinon.assert.calledWith(authRepo.get, lowGradesApiEndpoint);
                });

                it('should return what authRepo get returns.', () => {
                    expect(result).toBe(typeOfData);
                });
            });

            describe('resolve', () => {
                describe('error', () => {
                    beforeEach(() => {
                        service.resolveGetLowGrades(errorResponse);
                    });

                    it('should call genericGetResolver.', () => {
                        sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, errorResponse);
                    });
                });

                describe('success', () => {
                    var serverResponseModel;

                    describe('first time', () => {
                        beforeEach(() => {
                            lowGradesWithGpaDto = new Dtos.Applicant.Academic.LowGradesWithGpaModel(gpa, emptyArray);
                            modelFromServer.data = lowGradesWithGpaDto;
                            serverResponseModel = new Common.Models.ServerResponseModel(modelFromServer, true);
                            genericResolver.genericGetResolver.returns(modelFromServer.data);
                            service.resolveGetLowGrades(serverResponseModel);
                        });

                        it('should call genericGetResolver.', () => {
                            sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, serverResponseModel);
                        });

                        it('should have an empty array on ', () => {
                            expect(service.lowGradesArray).toBe(emptyArray);
                        });

                        it('should have a true arraySameAsServer.', () => {
                            arraySameAsServer(true);
                        });

                        describe('should set LowGradesNotificationInformationModel', () => {
                            it('should set Gpa.', () => {
                                expect(service.lowGradeNotificationsInformationModel.gpa).toBe(gpa);
                            });

                            it('should set number of items in array.', () => {
                                expect(service.lowGradeNotificationsInformationModel.numberOfLowGradesInformationSaved).toBe(0);
                            });

                            it('should set number of items outstanding.', () => {
                                expect(service.lowGradeNotificationsInformationModel.lowGradesNeededOutstanding).toBe(5);
                            });

                            it('should set number of items needed.', () => {
                                expect(service.lowGradeNotificationsInformationModel.numberOfLowGradeExplainationsNeeded.totalExplanationsNeeded).toBe(5);
                            });

                            it('should have a date from the first array object.', () => {
                                expect(service.lowGradeNotificationsInformationModel.lastUpdatedLowGrade).toBeUndefined();
                            });
                        });
                    });

                    describe('second time', () => {
                        beforeEach(() => {
                            lowGradesWithGpaDto = new Dtos.Applicant.Academic.LowGradesWithGpaModel(gpa, fullArray);
                            modelFromServer.data = lowGradesWithGpaDto;
                            serverResponseModel = new Common.Models.ServerResponseModel(modelFromServer, true);
                            genericResolver.genericGetResolver.returns(modelFromServer.data);
                            service.resolveGetLowGrades(serverResponseModel);
                        });

                        it('should call genericGetResolver.', () => {
                            sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, serverResponseModel);
                        });

                        it('should have an empty array on ', () => {
                            expect(service.lowGradesArray).toBe(fullArray);
                        });

                        it('should have an have item one on zero index.', () => {
                            expect(service.lowGradesArray[0]).toBe(lowGradeOne);
                        });

                        it('should have an have item two on one index.', () => {
                            expect(service.lowGradesArray[1]).toBe(lowGradeTwo);
                        });

                        it('should have a true arraySameAsServer.', () => {
                            arraySameAsServer(true);
                        });

                        describe('should set LowGradesNotificationInformationModel', () => {
                            it('should set Gpa.', () => {
                                expect(service.lowGradeNotificationsInformationModel.gpa).toBe(gpa);
                            });

                            it('should set number of items in array.', () => {
                                expect(service.lowGradeNotificationsInformationModel.numberOfLowGradesInformationSaved).toBe(2);
                            });

                            it('should set number of items outstanding.', () => {
                                expect(service.lowGradeNotificationsInformationModel.lowGradesNeededOutstanding).toBe(3);
                            });

                            it('should set number of items needed.', () => {
                                expect(service.lowGradeNotificationsInformationModel.numberOfLowGradeExplainationsNeeded.totalExplanationsNeeded).toBe(5);
                            });

                            it('should have a date from the first array object.', () => {
                                expect(service.lowGradeNotificationsInformationModel.lastUpdatedLowGrade).toBe(date);
                            });
                        });
                    });
                });
            });
        });
        function arraySameAsServer(isSame: boolean) {
            expect(service.arraySameAsServer).toBe(isSame);
        }
    });
}