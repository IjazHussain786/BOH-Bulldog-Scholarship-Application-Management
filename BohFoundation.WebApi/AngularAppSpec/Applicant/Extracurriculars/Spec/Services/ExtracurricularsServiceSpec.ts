module BohFoundation.Applicant.Extracurriculars.Spec.Services {
    describe("ExtracurricularsService", () => {
        var apiEndpoint = "/api/applicant/extracurriculars";
        var typeOfData = "extracurricular activities";
        var service, authRepo, errorResponse, successResponse, genericResolver, resource, result;

        beforeEach(() => {
            successResponse = new Common.Models.ServerResponseModel(null, true);
            errorResponse = new Common.Models.ServerResponseModel(null, false);

            var commonStubs = new TestHelpers.CommonStubs();

            authRepo = commonStubs.createAuthRepo();
            genericResolver = commonStubs.createGenericResolver();

            resource = { a: { b: { c: 10 } } };

            service = new Extracurriculars.Services.ExtracurricularsService(authRepo, genericResolver);
        });

        describe('constructor', () => {
            it('should have an undefined extracurricularActivitiesModel.', () => {
                expect(service.extracurricularActivitiesModel).toBeUndefined();
            });

            it('should default changeButNotSaved to false.', () => {
                expect(service.changedButNotSaved).toBeFalsy();
            });
        });

        describe('getExtracurricularActivitiesModel', () => {
            it('should return whatever is on the service.', () => {
                service.extracurricularActivitiesModel = typeOfData;
                expect(service.getExtracurricularActivitiesModel()).toBe(typeOfData);
            });
        });

        describe('getChangedButNotSaved', () => {
            it('should return whatever is on the service.', () => {
                service.changedButNotSaved = typeOfData;
                expect(service.getChangedButNotSaved()).toBe(typeOfData);
            });
        });

        describe('editing arrays', () => {
            var newModel;
            var modelOnService;

            beforeEach(() => {
                modelOnService = new Dtos.Applicant.Extracurricular.ExtracurricularActivitiesModel();
                service.extracurricularActivitiesModel = modelOnService;
            });

            describe('activity array', () => {
                beforeEach(() => {
                    newModel = new Dtos.Applicant.Extracurricular.ActivityModel();
                });

                describe('add', () => {
                    it('should set changedButNotSaved to true.', () => {
                        service.addActivity(newModel);
                        expect(service.changedButNotSaved).toBeTruthy();
                    });

                    it('should add the model to an undefined array.', () => {
                        service.addActivity(newModel);
                        expect(service.extracurricularActivitiesModel.activities.length).toBe(1);
                    });

                    it('should add the model to an undefined array.', () => {
                        service.addActivity(newModel);
                        expect(service.extracurricularActivitiesModel.activities[0]).toBe(newModel);
                    });

                    it('should add it on defined array.', () => {
                        service.extracurricularActivitiesModel.activities = [1];
                        service.addActivity(newModel);
                        expect(service.extracurricularActivitiesModel.activities[1]).toBe(newModel);
                    });

                    it('should add it on defined array.', () => {
                        service.extracurricularActivitiesModel.activities = [1];
                        service.addActivity(newModel);
                        expect(service.extracurricularActivitiesModel.activities.length).toBe(2);
                    });
                });

                describe('edit', () => {
                    beforeEach(() => {
                        service.extracurricularActivitiesModel.activities = [1, 2, 3];
                    });

                    it('should replace the model.', () => {
                        editActivity(0);
                    });

                    it('should replace the model.', () => {
                        editActivity(1);
                    });

                    it('should replace the model.', () => {
                        editActivity(2);
                    });

                    function editActivity(index: number) {
                        service.editActivity(newModel, index);
                        expect(service.extracurricularActivitiesModel.activities.length).toBe(3);
                        expect(service.extracurricularActivitiesModel.activities[index]).toBe(newModel);
                        expect(service.changedButNotSaved).toBeTruthy();
                    }
                });

                describe('delete', () => {
                    beforeEach(() => {
                        service.extracurricularActivitiesModel.activities = [1, 2, 3];
                    });

                    it('should set changedButNotSaved to true.', () => {
                        service.deleteActivity(2);
                        expect(service.changedButNotSaved).toBeTruthy();
                    });

                    it('should have two items.', () => {
                        service.deleteActivity(2);
                        expect(service.extracurricularActivitiesModel.activities.length).toBe(2);
                    });

                    it('should have 1 in place 0.', () => {
                        service.deleteActivity(2);
                        expect(service.extracurricularActivitiesModel.activities[0]).toBe(1);
                    });

                    it('should have 2 in place 1.', () => {
                        service.deleteActivity(2);
                        expect(service.extracurricularActivitiesModel.activities[1]).toBe(2);
                    });

                    it('should have two items.', () => {
                        service.deleteActivity(1);
                        expect(service.extracurricularActivitiesModel.activities.length).toBe(2);
                    });

                    it('should have 1 in place 0.', () => {
                        service.deleteActivity(1);
                        expect(service.extracurricularActivitiesModel.activities[0]).toBe(1);
                    });

                    it('should have 2 in place 1.', () => {
                        service.deleteActivity(1);
                        expect(service.extracurricularActivitiesModel.activities[1]).toBe(3);
                    });

                    it('should have two items.', () => {
                        service.deleteActivity(0);
                        expect(service.extracurricularActivitiesModel.activities.length).toBe(2);
                    });

                    it('should have 1 in place 0.', () => {
                        service.deleteActivity(0);
                        expect(service.extracurricularActivitiesModel.activities[0]).toBe(2);
                    });

                    it('should have 2 in place 1.', () => {
                        service.deleteActivity(0);
                        expect(service.extracurricularActivitiesModel.activities[1]).toBe(3);
                    });
                });
            });

            describe('jobs array', () => {
                beforeEach(() => {
                    newModel = new Dtos.Applicant.Extracurricular.JobModel();
                });

                describe('add', () => {
                    it('should set changedButNotSaved to true.', () => {
                        addJob();
                        expect(service.changedButNotSaved).toBeTruthy();
                    });

                    it('should add the model to an undefined array.', () => {
                        addJob();
                        expect(service.extracurricularActivitiesModel.jobs.length).toBe(1);
                    });

                    it('should add the model to an undefined array.', () => {
                        addJob();
                        expect(service.extracurricularActivitiesModel.jobs[0]).toBe(newModel);
                    });

                    it('should add it on defined array.', () => {
                        service.extracurricularActivitiesModel.jobs = [1];
                        addJob();
                        expect(service.extracurricularActivitiesModel.jobs[1]).toBe(newModel);
                    });

                    it('should add it on defined array.', () => {
                        service.extracurricularActivitiesModel.jobs = [1];
                        addJob();
                        expect(service.extracurricularActivitiesModel.jobs.length).toBe(2);
                    });

                    function addJob() {
                        service.addJob(newModel);
                    }
                });

                describe('edit', () => {
                    beforeEach(() => {
                        service.extracurricularActivitiesModel.jobs = [1, 2, 3];
                    });

                    it('should replace the model.', () => {
                        editJobs(0);
                    });

                    it('should replace the model.', () => {
                        editJobs(1);
                    });

                    it('should replace the model.', () => {
                        editJobs(2);
                    });

                    function editJobs(index: number) {
                        service.editJob(newModel, index);
                        expect(service.extracurricularActivitiesModel.jobs.length).toBe(3);
                        expect(service.extracurricularActivitiesModel.jobs[index]).toBe(newModel);
                        expect(service.changedButNotSaved).toBeTruthy();
                    }
                });

                describe('delete', () => {
                    beforeEach(() => {
                        service.extracurricularActivitiesModel.jobs = [1, 2, 3];
                    });

                    it('should set changedButNotSaved to true.', () => {
                        service.deleteJob(2);
                        expect(service.changedButNotSaved).toBeTruthy();
                    });

                    it('should have two items.', () => {
                        service.deleteJob(2);
                        expect(service.extracurricularActivitiesModel.jobs.length).toBe(2);
                    });

                    it('should have 1 in place 0.', () => {
                        service.deleteJob(2);
                        expect(service.extracurricularActivitiesModel.jobs[0]).toBe(1);
                    });

                    it('should have 2 in place 1.', () => {
                        service.deleteJob(2);
                        expect(service.extracurricularActivitiesModel.jobs[1]).toBe(2);
                    });

                    it('should have two items.', () => {
                        service.deleteJob(1);
                        expect(service.extracurricularActivitiesModel.jobs.length).toBe(2);
                    });

                    it('should have 1 in place 0.', () => {
                        service.deleteJob(1);
                        expect(service.extracurricularActivitiesModel.jobs[0]).toBe(1);
                    });

                    it('should have 2 in place 1.', () => {
                        service.deleteJob(1);
                        expect(service.extracurricularActivitiesModel.jobs[1]).toBe(3);
                    });

                    it('should have two items.', () => {
                        service.deleteJob(0);
                        expect(service.extracurricularActivitiesModel.jobs.length).toBe(2);
                    });

                    it('should have 1 in place 0.', () => {
                        service.deleteJob(0);
                        expect(service.extracurricularActivitiesModel.jobs[0]).toBe(2);
                    });

                    it('should have 2 in place 1.', () => {
                        service.deleteJob(0);
                        expect(service.extracurricularActivitiesModel.jobs[1]).toBe(3);
                    });
                });
            });
        });

        describe('postExtracurriculars', () => {
            describe('post', () => {
                beforeEach(() => {
                    authRepo.post.returns(resource);
                    result = service.postExtracurriculars(typeOfData);
                });

                it('should call post with the endpoint and whatever is passed in.', () => {
                    sinon.assert.calledWith(authRepo.post, typeOfData, apiEndpoint);
                });

                it('should return whatever the post returns.', () => {
                    expect(result).toBe(resource);
                });
            });

            describe('resolve', () => {
                beforeEach(() => {
                    service.changedButNotSaved = true;
                    service.resolvePostExtracurriculars(successResponse);
                });

                it('should pass successResponse on to the genericResolver.', () => {
                    sinon.assert.calledWith(genericResolver.genericPostResolver, typeOfData, successResponse);
                });

                it('should set changedButNotSaved to false.', () => {
                    expect(service.changedButNotSaved).toBeFalsy();
                });
            });

            describe('resolve failure', () => {
                beforeEach(() => {
                    service.changedButNotSaved = true;
                    service.resolvePostExtracurriculars(errorResponse);
                });

                it('should pass errorResponse on to the genericResolver.', () => {
                    sinon.assert.calledWith(genericResolver.genericPostResolver, typeOfData, errorResponse);
                });

                it('should not set changedButNotSaved to false.', () => {
                    expect(service.changedButNotSaved).toBeTruthy();
                });
            });
        });

        describe('getExtracurricularsFromServer', () => {
            describe('get', () => {
                beforeEach(() => {
                    authRepo.get.returns(resource);
                    result = service.getExtracurricularsFromServer();
                });

                it('should call get with apiEndpoint.', () => {
                    sinon.assert.calledWith(authRepo.get, apiEndpoint);
                });

                it('should return whatever the get returns.', () => {
                    expect(result).toBe(resource);
                });
            });

            describe('resolve', () => {
                beforeEach(() => {
                    genericResolver.genericGetResolver.returns(apiEndpoint);
                    service.resolveGetExtracurricularsFromServer(errorResponse);
                });

                it('should keep extracurricularActivitiesModel to be undefined.', () => {
                    expect(service.extracurricularActivitiesModel).toBe(apiEndpoint);
                });

                it('should call generic resolver.', () => {
                    sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, errorResponse);
                });
            });

            describe('resolve with null', () => {
                beforeEach(() => {
                    genericResolver.genericGetResolver.returns(null);
                    service.resolveGetExtracurricularsFromServer(successResponse);
                });

                it('should keep extracurricularActivitiesModel not be null.', () => {
                    expect(service.extracurricularActivitiesModel).toNotBe(null);
                });

                it('should keep extracurricularActivitiesModel to be defined.', () => {
                    expect(service.extracurricularActivitiesModel).toBeDefined();
                });

                it('should keep hasNotPaidActivities to be false.', () => {
                    expect(service.extracurricularActivitiesModel.hasNonPaidActivities).toBeFalsy();
                });

                it('should keep paidWork to be false.', () => {
                    expect(service.extracurricularActivitiesModel.paidWork).toBeFalsy();
                });

                it('should call generic resolver.', () => {
                    sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, successResponse);
                });
            });
        });
    });
} 