module BohFoundation.Reference.LetterOfRecommendation.Anon.Spec.Controllers {
    describe('RecommendAnonCtrl', () => {
        var recommendAnonService, routeParams, scope;
        var letterOfRecommendationGuid = 'guid';
        var cssInjector;
        var data = { somestuff: 12390 };

        var q, modal, fakeModal;
        var deferred, promise, resource;
        var deferred2, promise2, resource2;

        beforeEach(inject(($rootScope, $q) => {
            recommendAnonService = sinon.stub({
                setGuidOfLetterOfRecommendation: () => {},
                getInformationForReferenceFormModel: () => { },

                getLetterOfRecommendationInformation: () => { },
                resolveGetLetterOfRecommendationInformation: () => { },

                postReferencePersonalInformation: () => { },
                resolvePostReferencePersonalInformation: () => {},

                postLetterOfRecommendation: () => { },
                resolvePostLetterOfRecommendation: () => { },
            });

            routeParams = sinon.stub({
                letterOfRecommendationGuid: letterOfRecommendationGuid
            });

            modal = sinon.stub({
                open: () => { }
            });

            cssInjector = sinon.stub({
                add: () => { }
            });

            q = $q;
            scope = $rootScope;

            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };

            deferred2 = q.defer();
            promise2 = deferred2.promise;
            resource2 = { $promise: promise2 };

            recommendAnonService.postLetterOfRecommendation.returns(resource2);
            recommendAnonService.postReferencePersonalInformation.returns(resource2);
            recommendAnonService.getLetterOfRecommendationInformation.returns(resource);
            new Reference.LetterOfRecommendation.Anon.Controllers.RecommendAnonCtrl(scope, routeParams, recommendAnonService, modal, cssInjector);
        }));

        describe('successful construction', () => {
            beforeEach(() => {
                deferred.resolve(data);
                scope.$apply();
            });

            describe('getInformationForReferenceFormModel', () => {
                beforeEach(() => {
                    recommendAnonService.getInformationForReferenceFormModel.returns(letterOfRecommendationGuid);
                });

                it('should return whatever is in the service.', () => {
                    expect(scope.getInformationForReferenceFormModel()).toBe(letterOfRecommendationGuid);
                });
            });

            describe('mustCompletePersonalInformation', () => {
                it('should return true when there is a null on the lastUpdatedPhoneNumber field of the getIinformationForReferenceFormModel.', () => {
                    recommendAnonService.getInformationForReferenceFormModel.returns(new Dtos.Reference.Anonymous.InformationForReferenceFormModel(null, null, null, null));
                    expect(scope.mustCompletePersonalInformation()).toBeTruthy();
                });

                it('should return true when there getInformationFOrRefrenceFormModel is undefined.', () => {
                    recommendAnonService.getInformationForReferenceFormModel.returns(undefined);
                    expect(scope.mustCompletePersonalInformation()).toBeTruthy();
                });

                it('should not return a true if lastUpdatePhoneNumber field is not of the getIinformationForReferenceFormModel.', () => {
                    recommendAnonService.getInformationForReferenceFormModel.returns(new Dtos.Reference.Anonymous.InformationForReferenceFormModel(null, null, null, new Date));
                    expect(scope.mustCompletePersonalInformation()).toBeFalsy();
                });
            });

            describe('postLetterOfRecommendation', () => {
                describe('is processing', () => {
                    beforeEach(() => {
                        scope.processing = true;
                        scope.postLetterOfRecommendation();
                    });

                    it('should not call the service.', () => {
                        sinon.assert.notCalled(recommendAnonService.postLetterOfRecommendation);
                    });
                });

                describe('it isnt processing', () => {
                    var letterOfRecommendation = "this";
                    beforeEach(() => {
                        scope.letterOfRecommendation = letterOfRecommendation;
                        scope.postLetterOfRecommendation();
                    });

                    it('should call service.', () => {
                        sinon.assert.calledWith(recommendAnonService.postLetterOfRecommendation, letterOfRecommendation);
                    });
                    
                    it('should set processing to true.', () => {
                        expect(scope.processing).toBeTruthy();
                    });

                    describe('failure', () => {
                        beforeEach(() => {
                            deferred2.reject();
                            scope.$apply();
                        });

                        it('should call resolve with a false.', () => {
                            sinon.assert.calledWith(recommendAnonService.resolvePostLetterOfRecommendation, new Common.Models.ServerResponseModel(null, false));
                        });

                        it('should set processing to false.', () => {
                            expect(scope.processing).toBeFalsy();
                        });
                    });

                    describe('success', () => {
                        beforeEach(() => {
                            deferred2.resolve();
                            scope.$apply();
                        });

                        it('should call resolve with a true.', () => {
                            sinon.assert.calledWith(recommendAnonService.resolvePostLetterOfRecommendation, new Common.Models.ServerResponseModel(null, true));
                        });

                        it('should set processing to true.', () => {
                            expect(scope.processing).toBeTruthy();
                        });
                    });
                });
            });

            describe('editPersonalInformation', () => {
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

                describe('processing is true', () => {
                    beforeEach(() => {
                        scope.processing = true;
                        scope.editPersonalInformation();
                    });

                    it('should not call modal open.', () => {
                        sinon.assert.notCalled(modal.open);
                    });
                });

                describe('processing is false', () => {
                    describe('no name on informationForForm', () => {
                        beforeEach(() => {
                            recommendAnonService.getInformationForReferenceFormModel.returns(new Dtos.Reference.Anonymous.InformationForReferenceFormModel(null, null, null, null));
                            scope.editPersonalInformation();
                        });

                        it('should open the modal.', () => {
                            sinon.assert.calledOnce(modal.open);
                        });

                        it('should create a fresh referencePersonalInformationModel.', () => {
                            expect(scope.referencePersonalInformationModel).toMatch(new Dtos.Reference.Anonymous.ReferencePersonalInformationModel());
                        });
                    });

                    describe('a name on informationForForm', () => {
                        var name;

                        beforeEach(() => {
                            name = new Dtos.Person.NameModel("first", "last");
                            recommendAnonService.getInformationForReferenceFormModel.returns(new Dtos.Reference.Anonymous.InformationForReferenceFormModel(null, null, name, new Date));
                            scope.editPersonalInformation();
                        });

                        it('should open the modal.', () => {
                            sinon.assert.calledOnce(modal.open);
                        });

                        it('should create a referencePersonalInformationModel with the name on it.', () => {
                            expect(scope.referencePersonalInformationModel.name).toBe(name);
                        });

                        describe('cancelled operation', () => {
                            beforeEach(() => {
                                scope.modalInstance.dismiss();
                            });

                            it('should set referencePersonalInformationModel to null.', () => {
                                expect(scope.referencePersonalInformationModel).toBeNull();
                            });

                            it('should not call the post service.', () => {
                                sinon.assert.notCalled(recommendAnonService.postReferencePersonalInformation);
                            });

                            it('should have only called getLetterOfRecommendationInformation once.', () => {
                                sinon.assert.calledOnce(recommendAnonService.getLetterOfRecommendationInformation);
                            });

                            it('should set processing to false.', () => {
                                expect(scope.processing).toBeFalsy();
                            });
                        });

                        describe('successful operation', () => {
                            var model;
                            beforeEach(() => {
                                model = new Dtos.Reference.Anonymous.ReferencePersonalInformationModel(name, "as");
                                scope.modalInstance.close(model);
                            });

                            it('should call the post service with the thing returned from the modal.', () => {
                                sinon.assert.calledWith(recommendAnonService.postReferencePersonalInformation);
                            });

                            it('should set processing to true.', () => {
                                expect(scope.processing).toBeTruthy();
                            });

                            describe('failure', () => {
                                beforeEach(() => {
                                    deferred2.reject();
                                    scope.$apply();
                                });

                                it('should pass on a failure to the resolve method.', () => {
                                    sinon.assert.calledWith(recommendAnonService.resolvePostReferencePersonalInformation, new Common.Models.ServerResponseModel(null, false));
                                });

                                it('should call getLetterOfRecommendationInformation twice.', () => {
                                    sinon.assert.calledOnce(recommendAnonService.getLetterOfRecommendationInformation);
                                });
                            });

                            describe('success', () => {
                                beforeEach(() => {
                                    deferred2.resolve();
                                    scope.$apply();
                                });

                                it('should pass on a failure to the resolve method.', () => {
                                    sinon.assert.calledWith(recommendAnonService.resolvePostReferencePersonalInformation, new Common.Models.ServerResponseModel(null, true));
                                });

                                it('should call getLetterOfRecommendationInformation twice.', () => {
                                    sinon.assert.calledTwice(recommendAnonService.getLetterOfRecommendationInformation);
                                });
                            });
                        });
                    });
                });
            });
        });

        describe('constructor', () => {
            it('should call setGuidOfLetterOfRecommendation with guid.', () => {
                sinon.assert.calledWith(recommendAnonService.setGuidOfLetterOfRecommendation, letterOfRecommendationGuid);
            });

            it('should call getLetterOfRecommendationInformation.', () => {
                sinon.assert.calledOnce(recommendAnonService.getLetterOfRecommendationInformation);
            });

            it('should call getLetter after setGuidOfLetter.', () => {
                sinon.assert.callOrder(recommendAnonService.setGuidOfLetterOfRecommendation, recommendAnonService.getLetterOfRecommendationInformation);
            });

            it('should call cssInjector with link to fontawesome.', () => {
                sinon.assert.calledWith(cssInjector.add, "https://maxcdn.bootstrapcdn.com/font-awesome/4.1.0/css/font-awesome.min.css");
            });

            getLetterOfRecommendationChain();
        });

        function getLetterOfRecommendationChain() {
            it('should set processing to true.', () => {
                expect(scope.processing).toBeTruthy();
            });

            describe('ajax failure', () => {
                beforeEach(() => {
                    deferred.reject();
                    scope.$apply();
                });

                it('should call resolveGetLetterOfRecommendationInformation.', () => {
                    sinon.assert.calledWith(recommendAnonService.resolveGetLetterOfRecommendationInformation, new Common.Models.ServerResponseModel(null, false));
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

                it('should call resolveGetLetterOfRecommendationInformation.', () => {
                    sinon.assert.calledWith(recommendAnonService.resolveGetLetterOfRecommendationInformation, new Common.Models.ServerResponseModel(data, true));
                });
            });
        }
    });
}