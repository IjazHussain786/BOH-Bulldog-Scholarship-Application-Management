module BohFoundation.Applicant.Extracurriculars.Spec.Controllers {
    describe("ExtracurricularsCtrl", () => {

        var scope, q, modal, service;

        var deferred, promise, resource;
        var deferred2, promise2, resource2;
        var errorResponse, data = "2031kf";

        beforeEach(inject(($rootScope, $q) => {
            var commonStubs = new TestHelpers.CommonStubs();

            errorResponse = new Common.Models.ServerResponseModel(null, false);

            q = $q;
            scope = $rootScope;

            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };

            deferred2 = q.defer();
            promise2 = deferred2.promise;
            resource2 = { $promise: promise2 };

            modal = commonStubs.getModalStub();

            service = sinon.stub({
                getExtracurricularActivitiesModel: () => {},

                getExtracurricularsFromServer: () => {},
                resolveGetExtracurricularsFromServer: () => {},

                postExtracurriculars: () => {},
                resolvePostExtracurriculars: () => { },

                addActivity: () => { },
                editActivity: () => { },
                deleteActivity: () => { },

                addJob: () => { },
                editJob: () => { },
                deleteJob: () => { },

                getChangedButNotSaved: () => {}
            });

            service.postExtracurriculars.returns(resource2);
            service.getExtracurricularsFromServer.returns(resource);
            new Extracurriculars.Controllers.ExtracurricularsCtrl(scope, modal, service);
        }));

        describe('successful construction', () => {
            var model;
            
            beforeEach(() => {
                deferred.resolve(data);
                scope.$apply();
                model = new Dtos.Applicant.Extracurricular.ExtracurricularActivitiesModel(false, false);
            });

            describe('getExtracurricularActivitiesModel', () => {
                beforeEach(() => {
                    service.getExtracurricularActivitiesModel.returns(data);
                });

                it('should return whatever the service returns.', () => {
                    expect(scope.getExtracurricularActivitiesModel()).toBe(data);
                });
            });

            describe('getChangedButNotSaved', () => {
                beforeEach(() => {
                    service.getChangedButNotSaved.returns(data);
                });

                it('should return whatever the service returns.', () => {
                    expect(scope.getChangedButNotSaved()).toBe(data);
                });
            });

            describe('deleteActivity', () => {
                it('should pass the number to the delete service.', () => {
                    scope.deleteActivity(2);
                    sinon.assert.calledWith(service.deleteActivity, 2);
                });
            });

            describe('editActivity', () => {
                var activityModel;

                beforeEach(() => {
                    activityModel = new Dtos.Applicant.Extracurricular.ActivityModel();
                    model.activities = [1, 2, 3, activityModel];
                    service.getExtracurricularActivitiesModel.returns(model);
                });

                describe('processing', () => {
                    beforeEach(() => {
                        scope.processing = true;
                        scope.editActivity(3);
                    });

                    it('should not be able to call open.', () => {
                        sinon.assert.notCalled(modal.open);
                    });

                    it('should have an undefined currentActivityModel.', () => {
                        expect(scope.currentActivityModel).toBeUndefined();
                    });
                });

                describe('not processing', () => {
                    var fakeModal;

                    beforeEach(() => {
                        fakeModal = new TestHelpers.FakeModal().createFakeModel();
                        modal.open.returns(fakeModal);

                        scope.processing = false;
                        scope.editActivity(3);
                    });

                    it('should call modal open.', () => {
                        sinon.assert.calledOnce(modal.open);
                    });

                    it('should put whatever is in the service on the currentActivityModel.', () => {
                        expect(scope.currentActivityModel).toBe(activityModel);
                    });

                    describe('cancel operation', () => {
                        beforeEach(() => {
                            scope.modalInstance.dismiss();
                        });

                        it('should not call editActivity.', () => {
                            sinon.assert.notCalled(service.editActivity);
                        });
                    });

                    describe('ok operation', () => {
                        beforeEach(() => {
                            scope.modalInstance.close(model);
                        });

                        it('should call editActivity.', () => {
                            sinon.assert.calledWith(service.editActivity, model, 3);
                        });

                        it('should not call addActivity.', () => {
                            sinon.assert.notCalled(service.addActivity);
                        });
                    });
                });
            });

            describe('addActivity', () => {
                describe('processing', () => {
                    beforeEach(() => {
                        scope.processing = true;
                        scope.addActivity();
                    });

                    it('should not be able to call open.', () => {
                        sinon.assert.notCalled(modal.open);
                    });

                    it('should have an undefined currentActivityModel.', () => {
                        expect(scope.currentActivityModel).toBeUndefined();
                    });
                });

                describe('not processing', () => {
                    var fakeModal;

                    beforeEach(() => {
                        fakeModal = new TestHelpers.FakeModal().createFakeModel();
                        modal.open.returns(fakeModal);

                        scope.processing = false;
                        scope.addActivity();
                    });

                    it('should call modal open.', () => {
                        sinon.assert.calledOnce(modal.open);
                    });

                    it('should put whatever is in the service on the currentActivityModel.', () => {
                        expect(scope.currentActivityModel).toBeDefined();
                    });

                    describe('cancel operation', () => {
                        beforeEach(() => {
                            scope.modalInstance.dismiss();
                        });

                        it('should not call addActivity.', () => {
                            sinon.assert.notCalled(service.addActivity);
                        });
                    });

                    describe('ok operation', () => {
                        beforeEach(() => {
                            scope.modalInstance.close(model);
                        });

                        it('should call addActivity.', () => {
                            sinon.assert.calledWith(service.addActivity, model);
                        });

                        it('should not call editActivity.', () => {
                            sinon.assert.notCalled(service.editActivity);
                        });
                    });
                });
            });

            describe('addJob', () => {
                describe('processing', () => {
                    beforeEach(() => {
                        scope.processing = true;
                        scope.addJob();
                    });

                    it('should not be able to call open.', () => {
                        sinon.assert.notCalled(modal.open);
                    });

                    it('should have an undefined currentJobModel.', () => {
                        expect(scope.currentJobModel).toBeUndefined();
                    });
                });

                describe('not processing', () => {
                    var fakeModal;

                    beforeEach(() => {
                        fakeModal = new TestHelpers.FakeModal().createFakeModel();
                        modal.open.returns(fakeModal);
                        
                        scope.processing = false;
                        scope.addJob();
                    });

                    it('should call modal open.', () => {
                        sinon.assert.calledOnce(modal.open);
                    });

                    it('should put whatever is in the service on the currentJobModel.', () => {
                        expect(scope.currentJobModel).toBeDefined();
                    });

                    describe('cancel operation', () => {
                        beforeEach(() => {
                            scope.modalInstance.dismiss();
                        });

                        it('should not call addJob.', () => {
                            sinon.assert.notCalled(service.addJob);
                        });
                    });

                    describe('ok operation', () => {
                        beforeEach(() => {
                            scope.modalInstance.close(model);
                        });

                        it('should call addJob.', () => {
                            sinon.assert.calledWith(service.addJob, model);
                        });

                        it('should not call editJob.', () => {
                            sinon.assert.notCalled(service.editJob);
                        });
                    });
                });
            });

            describe('editJob', () => {
                var jobModel;

                beforeEach(() => {
                    jobModel = new Dtos.Applicant.Extracurricular.JobModel();
                    model.jobs = [1, 2, 3, jobModel];
                    service.getExtracurricularActivitiesModel.returns(model);
                });

                describe('processing', () => {
                    beforeEach(() => {
                        scope.processing = true;
                        scope.editJob(3);
                    });

                    it('should not be able to call open.', () => {
                        sinon.assert.notCalled(modal.open);
                    });

                    it('should have an undefined currentJobModel.', () => {
                        expect(scope.currentJobModel).toBeUndefined();
                    });
                });

                describe('not processing', () => {
                    var fakeModal;

                    beforeEach(() => {
                        fakeModal = new TestHelpers.FakeModal().createFakeModel();
                        modal.open.returns(fakeModal);

                        scope.processing = false;
                        scope.editJob(3);
                    });

                    it('should call modal open.', () => {
                        sinon.assert.calledOnce(modal.open);
                    });

                    it('should put whatever is in the service on the currentJobModel.', () => {
                        expect(scope.currentJobModel).toBe(jobModel);
                    });

                    describe('cancel operation', () => {
                        beforeEach(() => {
                            scope.modalInstance.dismiss();
                        });

                        it('should not call editJob.', () => {
                            sinon.assert.notCalled(service.editJob);
                        });
                    });

                    describe('ok operation', () => {
                        beforeEach(() => {
                            scope.modalInstance.close(model);
                        });

                        it('should call addJob.', () => {
                            sinon.assert.calledWith(service.editJob, model, 3);
                        });

                        it('should not call addJob.', () => {
                            sinon.assert.notCalled(service.addJob);
                        });
                    });
                });
            });

            describe('deleteJob', () => {
                it('should pass the number to the delete service.', () => {
                    scope.deleteJob(3);
                    sinon.assert.calledWith(service.deleteJob, 3);
                });
            });

            describe('saveExtracurriculars', () => {
                beforeEach(() => {
                    service.getExtracurricularActivitiesModel.returns(model);
                });

                describe('ableToSave false', () => {
                    beforeEach(() => {
                        model.paidWork = true;
                        scope.saveExtracurriculars();
                    });

                    it('should not change processing to true.', () => {
                        expect(scope.processing).toBeFalsy();
                    });

                    it('should not call post.', () => {
                        sinon.assert.notCalled(service.postExtracurriculars);
                    });
                });

                describe('ableToSave true', () => {
                    beforeEach(() => {
                        scope.saveExtracurriculars();
                    });

                    it('should set processing to true.', () => {
                        expect(scope.processing).toBeTruthy();
                    });

                    it('should call post.', () => {
                        sinon.assert.calledWith(service.postExtracurriculars, model);
                    });

                    describe('failure', () => {
                        beforeEach(() => {
                            deferred2.reject();
                            scope.$apply();
                        });

                        it('should flip processing.', () => {
                            expect(scope.processing).toBeFalsy();
                        });

                        it('should call resolvePost.', () => {
                            sinon.assert.calledWith(service.resolvePostExtracurriculars, new Common.Models.ServerResponseModel(null, false));
                        });

                        it('should set successfullySaved to false.', () => {
                            expect(scope.successfullySaved).toBeFalsy();
                        });
                    });

                    describe('success', () => {
                        beforeEach(() => {
                            deferred2.resolve();
                            scope.$apply();
                        });
                        
                        it('should set successfullySaved to true.', () => {
                            expect(scope.successfullySaved).toBeTruthy();
                        });

                        it('should call resolvePost.', () => {
                            sinon.assert.calledWith(service.resolvePostExtracurriculars, new Common.Models.ServerResponseModel(null, true));
                        });

                        it('should flip processing.', () => {
                            expect(scope.processing).toBeFalsy();
                        });
                    });
                });
            });

            describe('ableToSave', () => {
                describe('able to save', () => {
                    it('should return true when jobs and nonPaidActivities are false.', () => {
                        scope.processing = false;
                        ableToSave();
                    });

                    it('should return true when jobs is true and it has an item on the array.', () => {
                        model.paidWork = true;
                        model.jobs = [1];
                        ableToSave();
                    });


                    it('should return true when activities is true and it has an item on the array.', () => {
                        model.hasNonPaidActivities = true;
                        model.activities = [1];
                        ableToSave();
                    });

                    function ableToSave() {
                        service.getExtracurricularActivitiesModel.returns(model);
                        expect(scope.ableToSave()).toBeTruthy();
                    }
                });

                describe('not able to save', () => {
                    describe("processing", () => {
                        it('shouble be false when processing is true', () => {
                            scope.processing = true;
                            notAbleToSaveAssert();
                        });
                    });

                    describe('paid work true', () => {
                        beforeEach(() => {
                            scope.processing = false;
                            model.paidWork = true;
                        });

                        it('shouble be false when job array is undefined.', () => {
                            notAbleToSaveAssert();
                        });

                        it('shouble be false when job array is empty.', () => {
                            model.jobs = [];
                            notAbleToSaveAssert();
                        });
                    });

                    describe('nonpaid activities true', () => {
                        beforeEach(() => {
                            scope.processing = false;
                            model.hasNonPaidActivities = true;
                        });

                        it('shouble be false when activities array is undefined.', () => {
                            notAbleToSaveAssert();
                        });

                        it('shouble be false when activities array is empty.', () => {
                            model.activities = [];
                            notAbleToSaveAssert();
                        });
                    });

                    function notAbleToSaveAssert() {
                        service.getExtracurricularActivitiesModel.returns(model);
                        expect(scope.ableToSave()).toBeFalsy();
                    };
                });
            });
        });

        describe('constructor', () => {
            it('should call get from the service.', () => {
                sinon.assert.calledOnce(service.getExtracurricularsFromServer);
            });

            it('should have a true processing.', () => {
                expect(scope.processing).toBeTruthy();
            });

            describe('ajax failure', () => {
                beforeEach(() => {
                    deferred.reject();
                    scope.$apply();
                });

                it('should call resolve from the service.', () => {
                    sinon.assert.calledWith(service.resolveGetExtracurricularsFromServer, errorResponse);
                });

                it('should have a true processing.', () => {
                    expect(scope.processing).toBeTruthy();
                });
            });

            describe('ajax success', () => {
                beforeEach(() => {
                    deferred.resolve(data);
                    scope.$apply();
                });

                it('should call resolve with the data and true.', () => {
                    sinon.assert.calledWith(service.resolveGetExtracurricularsFromServer, new Common.Models.ServerResponseModel(data, true));
                });

                it('should have a false processing.', () => {
                    expect(scope.processing).toBeFalsy();
                });
            });
        });
    });
} 