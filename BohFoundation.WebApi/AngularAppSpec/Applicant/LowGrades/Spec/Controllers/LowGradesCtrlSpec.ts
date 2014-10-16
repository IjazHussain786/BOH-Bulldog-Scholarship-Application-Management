module BohFoundation.Applicant.LowGrades.Spec.Controllers {
    describe('LowGradesCtrl', () => {
        var scope, lowGradesService, q;
        var deferred, promise, resource;
        var deferred2, promise2, resource2;
        var data = { somestuff: 12390 };
        var lowGrades1 = { son: 10 };
        var lowGrades2 = { foalk: 100 };
        var modal, controller;
        var fakeLowGradeArray = [lowGrades1, lowGrades2];
        
        beforeEach(inject(($rootScope, $q) => {
            q = $q;
            scope = $rootScope;

            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };

            deferred2 = q.defer();
            promise2 = deferred2.promise;
            resource2 = { $promise: promise2 };

            lowGradesService = sinon.stub({
                getLowGrades: () => { },
                resolveGetLowGrades: () => { },
                postLowGrades: () => { },
                resolvePostLowGrades: () => { },
                getLowGradesArray: () => { },
                getArraySameAsServer: () => { },
                getLowGradeNotificationInformationModel: () => { },
                modifyLowGrade: () => { },
                addLowGrade: () => { },
                deleteLowGrade: () => { }
            });

            modal = sinon.stub({
                open: () => { }
            });

            lowGradesService.getLowGrades.returns(resource);
            lowGradesService.postLowGrades.returns(resource2);

            controller = new LowGrades.Controllers.LowGradesCtrl(scope, modal, lowGradesService);
        }));

        describe('construction', () => {
            it('should call getLowGrades.', () => {
                sinon.assert.calledOnce(lowGradesService.getLowGrades);
            });

            it('should set processing to true.', () => {
                expect(scope.processing).toBeTruthy();
            });

            describe('failure', () => {
                beforeEach(() => {
                    deferred.reject();
                    scope.$apply();
                });

                it('should call resolve with a false success.', () => {
                    sinon.assert.calledWith(lowGradesService.resolveGetLowGrades, new Common.Models.ServerResponseModel(null, false));
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
                    sinon.assert.calledWith(lowGradesService.resolveGetLowGrades, new Common.Models.ServerResponseModel(data, true));
                });

                it('should flip the processing bit to false.', () => {
                    expect(scope.processing).toBeFalsy();
                });
            });
        });

        describe('postLowGrades', () => {
            describe('validation', () => {
                describe('process', () => {
                    beforeEach(() => {
                        lowGradesService.getArraySameAsServer.returns(false);
                        scope.processing = true;
                        scope.postLowGrades();
                    });

                    it('should not call postLowGrades.', () => {
                        sinon.assert.notCalled(lowGradesService.postLowGrades);
                    });
                });

                describe('nothing has changed', () => {
                    beforeEach(() => {
                        lowGradesService.getArraySameAsServer.returns(true);
                        scope.processing = false;
                        scope.postLowGrades();
                    });

                    it('should not call postLowGrades.', () => {
                        sinon.assert.notCalled(lowGradesService.postLowGrades);
                    });

                    it('should not set processing to true.', () => {
                        expect(scope.processing).toBeFalsy();
                    });
                });
            });

            describe('valid', () => {
                beforeEach(() => {
                    lowGradesService.getArraySameAsServer.returns(false);
                    scope.processing = false;
                    scope.postLowGrades();
                });

                it('should flip the processing bit.', () => {
                    expect(scope.processing).toBeTruthy();
                });

                it('should call postContactInformation.', () => {
                    sinon.assert.calledOnce(lowGradesService.postLowGrades);
                });

                describe('success', () => {
                    beforeEach(() => {
                        deferred2.resolve();
                        scope.$apply();
                    });

                    it('should call resolvePost.', () => {
                        sinon.assert.calledWith(lowGradesService.resolvePostLowGrades, new Common.Models.ServerResponseModel(null, true));
                    });

                    it('should flip processing.', () => {
                        expect(scope.processing).toBeFalsy();
                    });
                });

                describe('failure', () => {
                    beforeEach(() => {
                        deferred2.reject();
                        scope.$apply();
                    });

                    it('should call resolvePost.', () => {
                        sinon.assert.calledWith(lowGradesService.resolvePostLowGrades, new Common.Models.ServerResponseModel(null, false));
                    });

                    it('should flip processing.', () => {
                        expect(scope.processing).toBeFalsy();
                    });
                });
            });
        });

        describe('items derived from notificationInfoModel', () => {
            var gpa = 2.3;
            var numberRemaining = 3;
            var totalNeeded = 5;
            var notificationModel;
            var date = new Date(2013,5,12);

            beforeEach(() => {
                notificationModel = new Dtos.Applicant.Notifications.LowGradeNotificationInformationModel(2, gpa, date);
                lowGradesService.getLowGradeNotificationInformationModel.returns(notificationModel);
            });
            


            describe('getGpa', () => {
                it('should return whatever the service returns.', () => {
                    expect(scope.getGpa()).toBe(gpa);
                });
            });

            describe('getNumberOfLowGradesTotalNeeded', () => {
                it('should return whatever the service returns.', () => {
                    expect(scope.getNumberOfLowGradesTotalNeeded()).toBe(totalNeeded);
                });
            });

            describe('getNumberOfLowGradesRemaining', () => {
                it('should return whatever the service returns.', () => {
                    expect(scope.getNumberOfLowGradesRemaining()).toBe(numberRemaining);
                });
            });

            describe('getLastUpdated', () => {
                it('should return whatever the service returns.', () => {
                    expect(scope.getLastUpdated()).toBe(date);
                });
            });
        });

        describe('getLowGradeArray', () => {
            it('should return whatever the service returns.', () => {
                lowGradesService.getLowGradesArray.returns(fakeLowGradeArray);
                expect(scope.getLowGradeArray()).toBe(fakeLowGradeArray);
            });
        });

        describe('getArraySameAsServer', () => {
            it('should return whatever the service returns.', () => {
                lowGradesService.getArraySameAsServer.returns(data);
                expect(scope.getArraySameAsServer()).toBe(data);
            });
        });

        describe('deleteLowGrade', () => {
            it('should call the service with the index passed in.', () => {
                scope.deleteLowGrade(1);
                sinon.assert.calledWith(lowGradesService.deleteLowGrade, 1);
            });
        });

        describe('data entry', () => {
            var fakeModal;

            beforeEach(() => {
                fakeModal = {
                    result: {
                        then: function (confirmCallback, cancelCallback) {
                            //Store the callbacks for later when the user clicks on the OK or Cancel button of the dialog
                            this.confirmCallBack = confirmCallback;
                            this.cancelCallback = cancelCallback;
                        }
                    },
                    close: function (item) {
                        //The user clicked OK on the modal dialog, call the stored confirm callback with the selected item
                        this.result.confirmCallBack(item);
                    },
                    dismiss: function (type) {
                        //The user clicked cancel on the modal dialog, call the stored cancel callback
                        this.result.cancelCallback(type);
                    }
                };

                modal.open.returns(fakeModal);
            });

            describe('editLowGrade', () => {
                beforeEach(() => {
                    lowGradesService.getLowGradesArray.returns(fakeLowGradeArray);
                    scope.newLowGradeModel = null;
                    scope.editLowGrade(1);
                });

                it('should set newLowGradeModel to the object referenced from the array.', () => {
                    expect(scope.newLowGradeModel).toBe(lowGrades2);
                });

                it('should set the type of operation to add.', () => {
                    expect(controller.typeOfOperation).toBe(Common.Enums.CrudEnum.Update);
                });

                it('should set the index to 1.', () => {
                    expect(controller.indexOfOperation).toBe(1);
                });

                it('should call open modal.', () => {
                    sinon.assert.calledOnce(modal.open);
                });

                describe('canceled operation', () => {
                    beforeEach(() => {
                        scope.modalInstance.dismiss();
                    });

                    it('should set newLowGradeModel to null.', () => {
                        expect(scope.newLowGradeModel).toBeNull();
                    });

                    it('should set typeOfOperation to null.', () => {
                        expect(controller.typeOfOperation).toBe(null);
                    });

                    it('should set the index to null.', () => {
                        expect(controller.indexOfOperation).toBeNull();
                    });
                });

                describe('success operation', () => {
                    beforeEach(() => {
                        scope.modalInstance.close(lowGrades1);
                    });

                    it('should pass that low grade object into the addLowGrade method of the service.', () => {
                        sinon.assert.calledWith(lowGradesService.modifyLowGrade, 1, lowGrades1);
                    });

                    it('should set newLowGradeModel to null.', () => {
                        expect(scope.newLowGradeModel).toBeNull();
                    });

                    it('should set typeOfOperation to null.', () => {
                        expect(controller.typeOfOperation).toBe(null);
                    });
                    
                    it('should set the index to null.', () => {
                        expect(controller.indexOfOperation).toBeNull();
                    });
                });
            });

            describe('addLowGrade', () => {
                beforeEach(() => {
                    scope.newLowGradeModel = null;
                    scope.addLowGrade();
                });

                it('should set newLowGradeModel to a new instance of the object.', () => {
                    expect(scope.newLowGradeModel).toMatch(new Dtos.Applicant.Academic.LowGradeModel);
                });

                it('should set the type of operation to add.', () => {
                    expect(controller.typeOfOperation).toBe(Common.Enums.CrudEnum.Add);
                });

                it('should call open modal.', () => {
                    sinon.assert.calledOnce(modal.open);
                });

                describe('canceled operation', () => {
                    beforeEach(() => {
                        scope.modalInstance.dismiss();
                    });

                    it('should set newLowGradeModel to null.', () => {
                        expect(scope.newLowGradeModel).toBeNull();
                    });

                    it('should set typeOfOperation to null.', () => {
                        expect(controller.typeOfOperation).toBeNull();
                    });
                });

                describe('success operation', () => {
                    beforeEach(() => {
                        scope.modalInstance.close(lowGrades1);
                    });

                    it('should pass that low grade object into the addLowGrade method of the service.', () => {
                        sinon.assert.calledWith(lowGradesService.addLowGrade, lowGrades1);
                    });

                    it('should set newLowGradeModel to null.', () => {
                        expect(scope.newLowGradeModel).toBeNull();
                    });

                    it('should set typeOfOperation to null.', () => {
                        expect(controller.typeOfOperation).toBeNull();
                    });
                });
            });
        });
    });
}