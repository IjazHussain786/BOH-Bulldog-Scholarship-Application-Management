module BohFoundation.Applicant.References.Spec.Controllers {
    describe('ApplicantReferenceCtrl', () => {
        var scope, applicantReferenceService;
        var data = { somestuff: 12390 };

        var q, modal, fakeModal;
        var deferred, promise, resource;
        var deferred2, promise2, resource2;

        beforeEach(inject(($rootScope, $q) => {
            q = $q;
            scope = $rootScope;

            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };

            deferred2 = q.defer();
            promise2 = deferred2.promise;
            resource2 = { $promise: promise2 };

            modal = sinon.stub({
                open: () => {}
            });

            applicantReferenceService = sinon.stub({
                getSubmittedReferencesFromServer: () => { },
                resolveGetSubmittedReference: () => { },
                postReferenceRequest: () => { },
                resolvePostReferenceRequest: () => { },
                getArrayOfReferenceModels: () => { }
            });

            applicantReferenceService.getSubmittedReferencesFromServer.returns(resource);
            applicantReferenceService.postReferenceRequest.returns(resource2);
            new References.Controllers.ApplicantReferenceCtrl(scope, applicantReferenceService, modal); 
        }));

        describe('successful construction', () => {
            beforeEach(() => {
                deferred.resolve(data);
                scope.$apply();
            });

            describe('ableToAddNewReference', () => {
                describe('true if', () => {
                    beforeEach(() => {
                        scope.processing = false;
                    });

                    it('should return true with two items in array.', () => {
                        applicantReferenceService.getArrayOfReferenceModels.returns(['1', '2']);
                        canAddNewReference();
                    });

                    it('should return true with zero items in array.', () => {
                        applicantReferenceService.getArrayOfReferenceModels.returns([]);
                        canAddNewReference();
                    });

                    function canAddNewReference() {
                        expect(scope.ableToAddNewReference()).toBeTruthy();
                    }
                });

                describe('false if', () => {
                    it('should return false if processing.', () => {
                        scope.processing = true;
                        applicantReferenceService.getArrayOfReferenceModels.returns(['1', '2']);
                        canNotAddNewReference();
                    });

                    it('should return false is the array is three items long.', () => {
                        scope.processing = false;
                        applicantReferenceService.getArrayOfReferenceModels.returns(['1', '2', '4']);
                        canNotAddNewReference();
                    });

                    it('should return false is the array is four items long.', () => {
                        scope.processing = false;
                        applicantReferenceService.getArrayOfReferenceModels.returns(['1', '2', '4', '4']);
                        canNotAddNewReference();
                    });

                    function canNotAddNewReference() {
                        expect(scope.ableToAddNewReference()).toBeFalsy();
                    }
                });
            });

            describe('add Reference', () => {
                beforeEach(() => {
                    applicantReferenceService.getArrayOfReferenceModels.returns(['1', '2']);
                    scope.processing = false;

                    fakeModal = {
                        result: {
                            then: function(confirmCallback, cancelCallback) {
                                //Store the callbacks for later when the user clicks on the OK or Cancel button of the dialog
                                this.confirmCallBack = confirmCallback;
                                this.cancelCallback = cancelCallback;
                            }
                        },
                        close: function(item) {
                            //The user clicked OK on the modal dialog, call the stored confirm callback with the selected item
                            this.result.confirmCallBack(item);
                        },
                        dismiss: function(type) {
                            //The user clicked cancel on the modal dialog, call the stored cancel callback
                            this.result.cancelCallback(type);
                        }
                    };

                    modal.open.returns(fakeModal);
                });

                describe("addReference", () => {
                    describe('ableToAddNewReferences is false', () => {
                        beforeEach(() => {
                            scope.processing = true;
                            scope.addReference();
                        });

                        it('should not open the modal.', () => {
                            sinon.assert.notCalled(modal.open);
                        });
                    });

                    describe('ableToAddNewReferences is true', () => {
                        beforeEach(() => {
                            scope.addReference();
                        });

                        it('should open the modal.', () => {
                            sinon.assert.calledOnce(modal.open);
                        });

                        it('should create a newReferenceInputModel on scope.', () => {
                            expect(scope.newReferenceInputModel).toEqual(new Dtos.Applicant.References.ApplicantReferenceInputModel());
                        });

                        describe('cancelled operation', () => {
                            beforeEach(() => {
                                scope.modalInstance.dismiss();
                            });

                            it('should set newReferenceInputModel to null.', () => {
                                expect(scope.newReferenceInputModel).toBeNull();
                            });

                            it('should not call the update service.', () => {
                                sinon.assert.notCalled(applicantReferenceService.postReferenceRequest);
                            });

                            it('should not call the get service only once.', () => {
                                sinon.assert.calledOnce(applicantReferenceService.getSubmittedReferencesFromServer);
                            });
                        });

                        describe('successful operation', () => {
                            var newReferenceInputModel;

                            beforeEach(() => {
                                newReferenceInputModel = new Dtos.Applicant.References.ApplicantReferenceInputModel("thomas", "as", "sf", "Asdfjk");
                                scope.modalInstance.close(newReferenceInputModel);
                            });

                            it('should have the new object on scope.', () => {
                                expect(scope.newReferenceInputModel).toBe(newReferenceInputModel);
                            });

                            it('should call post.', () => {
                                sinon.assert.calledWith(applicantReferenceService.postReferenceRequest, newReferenceInputModel);
                            });

                            it('should set processing to true.', () => {
                                expect(scope.processing).toBeTruthy();
                            });

                            describe('post failure', () => {
                                beforeEach(() => {
                                    deferred2.reject();
                                    scope.$apply();
                                });

                                it('it should pass the failure on to the resolvePost.', () => {
                                    sinon.assert.calledWith(applicantReferenceService.resolvePostReferenceRequest, new Common.Models.ServerResponseModel(null, false));
                                });

                                it('should not change the model.', () => {
                                    expect(scope.newReferenceInputModel).toBe(newReferenceInputModel);
                                });
                            });

                            describe('post success', () => {
                                beforeEach(() => {
                                    deferred2.resolve();
                                    scope.$apply();
                                });

                                it('it should pass the failure on to the resolvePost.', () => {
                                    sinon.assert.calledWith(applicantReferenceService.resolvePostReferenceRequest, new Common.Models.ServerResponseModel(null, true));
                                });

                                it('should flip the new model to null..', () => {
                                    expect(scope.newReferenceInputModel).toBeNull();
                                });

                                it('should call getSubmittedReferencesFromServer.', () => {
                                    sinon.assert.calledTwice(applicantReferenceService.getSubmittedReferencesFromServer);
                                });
                            });
                        });
                    });
                });
            });

            describe('getArrayOfReferenceModels', () => {
                it('should return whatever the service returns.', () => {
                    applicantReferenceService.getArrayOfReferenceModels.returns(data);
                    expect(scope.getArrayOfReferenceModels()).toBe(data);
                });
            });
        });

        describe('constructor', () => {
            it('should call getSubmittedReferencesFromServer.', () => {
                sinon.assert.calledOnce(applicantReferenceService.getSubmittedReferencesFromServer);
            });

            getSubmittedReferencesChain();
        });

        function getSubmittedReferencesChain() {
            it('should set processing to true.', () => {
                expect(scope.processing).toBeTruthy();
            });

            describe('ajax failure', () => {
                beforeEach(() => {
                    deferred.reject();
                    scope.$apply();
                });

                it('should call resolveGetSubmittedReference.', () => {
                    sinon.assert.calledWith(applicantReferenceService.resolveGetSubmittedReference, new Common.Models.ServerResponseModel(null, false));
                });

                it('should remain processing.', () => {
                    expect(scope.processing).toBeTruthy();
                });
            });

            describe('ajax success', () => {
                beforeEach(() => {
                    deferred.resolve(data);
                    scope.$apply();
                });

                it('should set processing to false.', () => {
                    expect(scope.processing).toBeFalsy();
                });

                it('should call resolveGetSubmittedReference.', () => {
                    sinon.assert.calledWith(applicantReferenceService.resolveGetSubmittedReference, new Common.Models.ServerResponseModel(data, true));
                });
            });
        }
    });
}