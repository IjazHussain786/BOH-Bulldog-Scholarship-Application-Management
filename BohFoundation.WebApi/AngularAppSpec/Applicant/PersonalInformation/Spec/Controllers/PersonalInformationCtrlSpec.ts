module BohFoundation.Applicant.PersonalInformation.Spec.Controllers {
    describe("PersonalInformationCtrl", () => {
        var scope, personalInformationService, q;
        var fullName = "fullname";
        var deferred, promise, resource;
        var deferred2, promise2, resource2;
        var data = { somestuff: 12390 };
        var personalInfo1 = { son: 10 };
        var personalInfo2 = { foalk: 100 };
        var date;

        beforeEach(inject(($rootScope, $q) => {
            q = $q;
            scope = $rootScope;

            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };

            deferred2 = q.defer();
            promise2 = deferred2.promise;
            resource2 = { $promise: promise2 };

            personalInformationService = sinon.stub({
                fullName: fullName,
                applicantPersonalInformation: personalInfo1,
                getPersonalInformation: () => { },
                resolveGetPersonalInformation: () => { },
                postPersonalInformation: () => { },
                resolvePostPersonalInformation: () =>{ }
            });

            date = new Date();

            personalInformationService.getPersonalInformation.returns(resource);
            new PersonalInformation.Controllers.PersonalInformationCtrl(scope, personalInformationService);
        }));

        describe('construction', () => {
            it('should call personalInformationService get.',()=> {
                sinon.assert.calledOnce(personalInformationService.getPersonalInformation);
            });

            it('should set processing as true.', () => {
                expect(scope.processing).toBeTruthy();
            });

            it('should set datePickerSettings to an instance of datePickerSettings.', () => {
                expect(scope.datePickerSettings).toBeDefined();
            });

            describe('failure', () => {
                beforeEach(() => {
                    deferred.reject();
                    scope.$apply();
                });

                it('should call resolve with a false success.', () => {
                    sinon.assert.calledWith(personalInformationService.resolveGetPersonalInformation, new Common.Models.ServerResponseModel(null, false));
                });

                it('should maintain a true processing.', () => {
                    expect(scope.processing).toBeTruthy();
                });
            });

            describe('success', () => {
                beforeEach(() => {
                    deferred.resolve(data);
                    scope.$apply();
                });

                it('should call resolve with a true success.', () => {
                    sinon.assert.calledWith(personalInformationService.resolveGetPersonalInformation, new Common.Models.ServerResponseModel(data, true));
                });

                it('should flip the processing bit to false.', () => {
                    expect(scope.processing).toBeFalsy();
                });

                it('should set scopes full name to the full name of the service.', () => {
                    expect(scope.fullName).toBe(fullName);
                });

                it('should set scopes ApplicantPersonInformationModel to the one on service.', () => {
                    expect(scope.applicantPersonalInformation).toBe(personalInfo1);
                });
            });
        });

        describe('postPersonalInformatioin', () => {
            var form;

            beforeEach(() => {

                form = sinon.stub({
                    $valid:{}
                });

                deferred.resolve(data);
                scope.$apply();
            });

            describe('validation', () => {
                describe('process', () => {
                    beforeEach(() => {
                        scope.processing = true;
                        form.$valid = true;
                        scope.applicantPersonalInformation.birthdate = date;
                        scope.postApplicantPersonalInformation(form);
                    });

                    it('should not call postPersonalInformation.', () => {
                        sinon.assert.notCalled(personalInformationService.postPersonalInformation);
                    });
                });

                describe('form invalid', () => {
                    beforeEach(() => {
                        scope.processing = false;
                        form.$valid = false;
                        scope.applicantPersonalInformation.birthdate = date;
                        scope.postApplicantPersonalInformation(form);
                    });

                    it('should not call postPersonalInformation.', () => {
                        sinon.assert.notCalled(personalInformationService.postPersonalInformation);
                    });

                    it('should not set processing to true.', () => {
                        expect(scope.processing).toBeFalsy();
                    });
                });

                describe('no birthdate', () => {

                    beforeEach(() => {
                        scope.processing = false;
                        form.$valid = true;
                        scope.applicantPersonalInformation.birthdate = undefined;
                        scope.postApplicantPersonalInformation(form);
                    });

                    it('should not call postPersonalInformation.', () => {
                        sinon.assert.notCalled(personalInformationService.postPersonalInformation);
                    });

                    it('should not set processing to true.', () => {
                        expect(scope.processing).toBeFalsy();
                    });
                });

                describe('wrong birthdate', () => {

                    beforeEach(() => {
                        scope.processing = false;
                        form.$valid = true;
                        scope.applicantPersonalInformation.birthdate = "tasion";
                        scope.postApplicantPersonalInformation(form);
                    });

                    it('should not call postPersonalInformation.', () => {
                        sinon.assert.notCalled(personalInformationService.postPersonalInformation);
                    });

                    it('should not set processing to true.', () => {
                        expect(scope.processing).toBeFalsy();
                    });
                });
            });

            describe('valid', () => {
                beforeEach(() => {
                    scope.processing = false;
                    form.$valid = true;
                    scope.applicantPersonalInformation.birthdate = date;
                    personalInformationService.postPersonalInformation.returns(resource2);
                    scope.postApplicantPersonalInformation(form);
                });

                it('should flip the processing bit.', () => {
                    expect(scope.processing).toBeTruthy();
                });

                it('should call postPersonalInfomration.', () => {
                    sinon.assert.calledWith(personalInformationService.postPersonalInformation, personalInfo1);
                });

                describe('success', () => {
                    beforeEach(() => {
                        personalInformationService.applicantPersonalInformation = personalInfo2;
                        deferred2.resolve();
                        scope.$apply();
                    });

                    it('should call resolvePost.', () => {
                        sinon.assert.calledWith(personalInformationService.resolvePostPersonalInformation, new Common.Models.ServerResponseModel(null, true));
                    });

                    it('should flip processing.', () => {
                        expect(scope.processing).toBeFalsy();
                    });

                    it('should set the model on scope equal to the new model.', () => {
                        expect(scope.applicantPersonalInformation).toBe(personalInfo2);
                    });
                });

                describe('failure', () => {
                    beforeEach(() => {
                        deferred2.reject();
                        scope.$apply();
                    });

                    it('should call resolvePost.', () => {
                        sinon.assert.calledWith(personalInformationService.resolvePostPersonalInformation, new Common.Models.ServerResponseModel(null, false));
                    });

                    it('should flip processing.', () => {
                        expect(scope.processing).toBeFalsy();
                    });

                    it('should not set the model on scope equal to the new model.', () => {
                        expect(scope.applicantPersonalInformation).toBe(personalInfo1);
                    });
                });
            });
        });
    });
} 