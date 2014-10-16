module BohFoundation.Applicant.FamilyInformation.Spec.Controllers {
    describe("FamilyInformationCtrl", () => {

        var scope, q, modal, service;

        var deferred, promise, resource;
        var deferred2, promise2, resource2;

        var data = "data";
        var data2 = "asfjknjnk";

        var errorResponse;

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
                getFamilyInformationModel: () => { },
                getFamilyInformationFromServer: () => { },
                resolveGetFamilyInformationFromServer: () => { },
                postFamilyInformation: () => { },
                resolvePostFamilyInformation: () => { }
            });

            service.getFamilyInformationFromServer.returns(resource);
            service.postFamilyInformation.returns(resource2);
            new FamilyInformation.Controllers.FamilyInformationCtrl(scope, modal, service);
        }));

        describe('constructor', () => {
            it('should call get from the service.', () => {
                sinon.assert.calledOnce(service.getFamilyInformationFromServer);
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
                    sinon.assert.calledWith(service.resolveGetFamilyInformationFromServer, errorResponse);
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
                    sinon.assert.calledWith(service.resolveGetFamilyInformationFromServer, new Common.Models.ServerResponseModel(data, true));
                });

                it('should have a false processing.', () => {
                    expect(scope.processing).toBeFalsy();
                });
            });
        });

        describe('successful construction', () => {
            beforeEach(() => {
                deferred.resolve(data);
                scope.$apply();
            });

            describe('editFamilyInformation', () => {
                describe('processing', () => {
                    beforeEach(() => {
                        scope.processing = true;
                        scope.editFamilyInformation();
                    });

                    it('should not be able to call open.', () => {
                        sinon.assert.notCalled(modal.open);
                    });

                    it('should have an undefined newFamilyInformationInputModel.', () => {
                        expect(scope.newFamilyInformationInputModel).toBeUndefined();
                    });
                });

                describe('not processing', () => {
                    var fakeModal;

                    beforeEach(() => {
                        fakeModal = new TestHelpers.FakeModal().createFakeModel();
                        modal.open.returns(fakeModal);

                        service.getFamilyInformationModel.returns(data);
                        scope.processing = false;
                        scope.editFamilyInformation();
                    });

                    it('should call modal open.', () => {
                        sinon.assert.calledOnce(modal.open);
                    });

                    it('should put whatever is in the service on the newFamilyInformationInputModel.', () => {
                        expect(scope.newFamilyInformationInputModel).toBe(data);
                    });

                    describe('cancel operation', () => {
                        beforeEach(() => {
                            scope.modalInstance.dismiss();
                        });

                        it('should not call post.', () => {
                            sinon.assert.notCalled(service.postFamilyInformation);
                        });

                        it('should not call get from the service again.', () => {
                            sinon.assert.calledOnce(service.getFamilyInformationFromServer);
                        });

                        it('should set successfullySaved to true.', () => {
                            expect(scope.successfullySaved).toBeFalsy();
                        });
                    });

                    describe('ok operation', () => {
                        beforeEach(() => {
                            scope.modalInstance.close(data2);
                        });

                        it('should set processing to true.', () => {
                            expect(scope.processing).toBeTruthy();
                        });

                        it('should call post.', () => {
                            sinon.assert.calledWith(service.postFamilyInformation, data2);
                        });

                        describe('success', () => {
                            beforeEach(() => {
                                deferred2.resolve();
                                scope.$apply();
                            });

                            it('should call resolvePost.', () => {
                                sinon.assert.calledWith(service.resolvePostFamilyInformation, new Common.Models.ServerResponseModel(null, true));
                            });

                            it('should call get again.', () => {
                                sinon.assert.calledTwice(service.getFamilyInformationFromServer);
                            });

                            it('should set successfullySaved to true.', () => {
                                expect(scope.successfullySaved).toBeTruthy();
                            });

                            it('should set processing to true.', () => {
                                expect(scope.successfullySaved).toBeTruthy();
                            });
                        });

                        describe('failure', () => {
                            beforeEach(() => {
                                deferred2.reject();
                                scope.$apply();
                            });

                            it('should call resolvePost.', () => {
                                sinon.assert.calledWith(service.resolvePostFamilyInformation, new Common.Models.ServerResponseModel(null, false));
                            });

                            it('should flip processing.', () => {
                                expect(scope.processing).toBeFalsy();
                            });

                            it('should set successfullySaved to false.', () => {
                                expect(scope.successfullySaved).toBeFalsy();
                            });
                        });
                    });
                });
            });

            describe('getFamilyInformationModel', () => {
                it('should return whatever the server returns.', () => {
                    service.getFamilyInformationModel.returns(data);
                    expect(scope.getFamilyInformationModel()).toBe(data);
                });
            });
        });
    });
} 