module BohFoundation.Admin.LetterOfRecommendation.Spec.Controllers {
    describe("AdminLetterOfRecommendationCtrl", () => {

        var scope, q, modal, service;

        var deferred, promise, resource;

        beforeEach(inject(($rootScope, $q) => {
            var commonStubs = new TestHelpers.CommonStubs();

            q = $q;
            scope = $rootScope;

            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };

            modal = commonStubs.getModalStub();

            service = sinon.stub({
                getGuidSentToReference:() => {},
                getGuidSentToReferenceFromServer: () => { },
                resolveGetGuidSentToReferenceFromServer: () => { },
                canGoToReference: () => { },
                goToReferenceForm: () => {}    
            });

            service.getGuidSentToReferenceFromServer.returns(resource);
            new LetterOfRecommendation.Controllers.AdminLetterOfRecommendationCtrl(scope, modal, service);
        }));

        describe('construction', () => {
            it('should default processing to false.', () => {
                expect(scope.processing).toBeFalsy();
            });
        });

        describe('getGuidSentToReferenceModel', () => {
            it('should return whatever the service returns.', () => {
                service.getGuidSentToReference.returns(modal);
                expect(scope.getGuidSentToReferenceModel()).toBe(modal);
            });
        });

        describe('canGoToReference', () => {
            it('should return whatever canGoToReference returns.', () => {
                service.canGoToReference.returns(promise);
                expect(scope.canGoToReference()).toBe(promise);
            });
        });

        describe('goToReferenceForm', () => {
            describe("processing is true", () => {
                it('should not call gotoReferenceForm.', () => {
                    scope.processing = true;
                    scope.goToReferenceForm();
                    sinon.assert.notCalled(service.goToReferenceForm);
                });
            });

            describe('processing is false', () => {
                it('should call gotoReferenceForm.', () => {
                    scope.processing = false;
                    scope.goToReferenceForm();
                    sinon.assert.calledOnce(service.goToReferenceForm);
                });
            });
        });

        describe('getGuidSentToReferenceModelFromServer', () => {
            var fakeModal;
            beforeEach(() => {
                fakeModal = new TestHelpers.FakeModal().createFakeModel();

                modal.open.returns(fakeModal);
            });

            describe('processing is true', () => {
                beforeEach(() => {
                    scope.processing = true;
                    scope.getGuidSentToReferenceModelFromServer();
                });

                it('should not create a new modelSentToServer.', () => {
                    sinon.assert.notCalled(modal.open);
                });

                it('should have a modelSentToServer as undefined.', () => {
                    expect(scope.modelSentToServer).toBeUndefined();
                });
            });

            describe('processing is false', () => {
                beforeEach(() => {
                    scope.processing = false;
                    scope.getGuidSentToReferenceModelFromServer();
                });

                it('should open the modal.', () => {
                    sinon.assert.calledOnce(modal.open);
                });

                it('should create a new modelSentToServer.', () => {
                    expect(scope.modelSentToServer).toBeDefined();
                });

                describe('cancel modal', () => {
                    beforeEach(() => {
                        scope.modalInstance.dismiss();
                    });

                    it('should not call the get method.', () => {
                        sinon.assert.notCalled(service.getGuidSentToReferenceFromServer);
                    });

                    it('should set modelSentToServer to be undefined.', () => {
                        expect(scope.modelSentToServer).toBeUndefined();
                    });
                });

                describe('ok modal', () => {
                    var modelFromModal;
                    var appsEmail = "afsd", refEmail = "asdf902";
                    beforeEach(() => {
                        modelFromModal = new Dtos.Admin.References.GetLetterOfRecommendationGuidModel(appsEmail, refEmail);
                        scope.modalInstance.close(modelFromModal);
                    });

                    it('should set processing to true.', () => {
                        expect(scope.processing).toBeTruthy();
                    });

                    it('should set modelSentToServer to be what is returned.', () => {
                        expect(scope.modelSentToServer).toBe(modelFromModal);
                    });

                    it('should call getGuidSentToReferenceFromServer.', () => {
                        sinon.assert.calledWith(service.getGuidSentToReferenceFromServer, modelFromModal);
                    });

                    describe('success', () => {
                        beforeEach(() => {
                            deferred.resolve(appsEmail);
                            scope.$apply();
                        });

                        it('should set processing to false.', () => {
                            expect(scope.processing).toBeFalsy();
                        });

                        it('should call resolve with true and data.', () => {
                            sinon.assert.calledWith(service.resolveGetGuidSentToReferenceFromServer, new Common.Models.ServerResponseModel(appsEmail, true));
                        });
                    });

                    describe('failure', () => {
                        beforeEach(() => {
                            deferred.reject();
                            scope.$apply();
                        });

                        it('should set processing to false.', () => {
                            expect(scope.processing).toBeFalsy();
                        });

                        it('should call resolve with false and null.', () => {
                            sinon.assert.calledWith(service.resolveGetGuidSentToReferenceFromServer, new Common.Models.ServerResponseModel(null, false));
                        });
                    });
                });
            });
        });
    });
} 