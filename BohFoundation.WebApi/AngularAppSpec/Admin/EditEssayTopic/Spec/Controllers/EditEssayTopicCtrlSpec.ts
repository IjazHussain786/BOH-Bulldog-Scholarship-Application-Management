// It should get the essays on construction. 
// It should refresh essays after changes. 

module BohFoundation.Admin.EditEssayTopic.Spec.Controllers {
    describe('EditEssayTopicCtrl', () => {
        var scope, q, createAndGetEssayTopicsService, modifyEssayTopicGraduatingYearService, modal;
        var deferred, promise, resource;
        var deferred2, promise2, resource2;
        var data = { somestuff: 12390 };
        var essayTopicModel1, essayTopicModel2;
        var titleOfEssay = "title", prompt = "prompt", revision1 = new Date(), revision2 = new Date(2013, 2, 10), lastAuthor1, lastAuthor2;
        var fullArray;

        beforeEach(inject(($rootScope, $q) => {
            q = $q;
            scope = $rootScope;

            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };

            deferred2 = q.defer();
            promise2 = deferred2.promise;
            resource2 = { $promise: promise2 };

            createAndGetEssayTopicsService = sinon.stub({
                postCreateOrModifyEssayTopic: () => {},
                resolvePostCreateOrModifyEssayTopic: () => {},
                getEssayTopicsFromServer: () =>{},
                resolveGetEssayTopics: () => {},
                getEssayTopicArray: () =>{ }
            });

            modifyEssayTopicGraduatingYearService = sinon.stub({
                postAddYearToTopic: () => { },
                resolvePostAddYearToTopic: () => { },
                deleteYearToTopic: () => { },
                resolveDeleteYearToTopic: () => {}
            });

            modal = sinon.stub({
                open: () => { }
            });

            essayTopicModel1 = new Dtos.Admin.EssayTopics.EssayTopicModel(1, titleOfEssay + 1, prompt + 1, revision1, [], lastAuthor1);
            essayTopicModel2 = new Dtos.Admin.EssayTopics.EssayTopicModel(2, titleOfEssay + 2, prompt + 2, revision2, [], lastAuthor2);

            fullArray = [essayTopicModel1, essayTopicModel2];

            createAndGetEssayTopicsService.postCreateOrModifyEssayTopic.returns(resource2);
            createAndGetEssayTopicsService.getEssayTopicsFromServer.returns(resource);
            modifyEssayTopicGraduatingYearService.postAddYearToTopic.returns(resource2);
            modifyEssayTopicGraduatingYearService.deleteYearToTopic.returns(resource2);
            new EditEssayTopic.Controllers.EditEssayTopicCtrl(scope, createAndGetEssayTopicsService, modifyEssayTopicGraduatingYearService, modal);
        }));

        describe('construction', () => {
            it('should call getEssayTopicsFromServer.', () => {
                callGetEssayTopicsFromServer(1);
            });
            
            describe('failure', () => {
                beforeEach(() => {
                    deferred.reject();
                    scope.$apply();
                });

                it('should call resolve with a false success.', () => {
                    sinon.assert.calledWith(createAndGetEssayTopicsService.resolveGetEssayTopics, new Common.Models.ServerResponseModel(null, false));
                });

            });

            describe('success', () => {
                beforeEach(() => {
                    deferred.resolve(data);
                    scope.$apply();
                });

                it('should call resolve with a true success.', () => {
                    sinon.assert.calledWith(createAndGetEssayTopicsService.resolveGetEssayTopics, new Common.Models.ServerResponseModel(data, true));
                });
            });
        });

        describe('getEssayTopics', () => {
            it('should return whatever the service returns.', () => {
                createAndGetEssayTopicsService.getEssayTopicArray.returns(data);
                expect(scope.getEssayTopics()).toBe(data);
            });
        });

        describe('successful construction', () => {
            beforeEach(() => {
                deferred.resolve(data);
                scope.$apply();
            });

            it('should call getTopicsFromServer once.', () => {
                sinon.assert.calledOnce(createAndGetEssayTopicsService.getEssayTopicsFromServer);
            });


            describe('graduating class add and delete', () => {
                var yearToAddOrDelete = 210394;
                var idOfItem = 123;

                beforeEach(() => {
                    scope.yearToAddOrDelete = yearToAddOrDelete;
                });

                describe('delete year', () => {
                    beforeEach(() => {
                        scope.deleteYearFromEssayTopic(idOfItem);
                    });

                    it('should call the delete method with the new object.', () => {
                        sinon.assert.calledWith(modifyEssayTopicGraduatingYearService.deleteYearToTopic, new Dtos.Admin.EssayTopics.EditEssayTopicByGraduatingClassModel(idOfItem, yearToAddOrDelete))
                    });

                    describe('failure', () => {
                        beforeEach(() => {
                            deferred2.reject();
                            scope.$apply();
                        });

                        it('should pass the failure on to the resolve service.', () => {
                            sinon.assert.calledWith(modifyEssayTopicGraduatingYearService.resolveDeleteYearToTopic, new Common.Models.ServerResponseModel(null, false));
                        });
                    });

                    describe('success', () => {
                        beforeEach(() => {
                            deferred2.resolve();
                            scope.$apply();
                        });

                        it('should pass the failure on to the resolve service.', () => {
                            sinon.assert.calledWith(modifyEssayTopicGraduatingYearService.resolveDeleteYearToTopic, new Common.Models.ServerResponseModel(null, true));
                        });

                        it('should call getArrayFromServer.', () => {
                            callGetEssayTopicsFromServer(2);
                        });
                    });
                });

                describe('add year', () => {
                    beforeEach(() => {
                        scope.addYearToEssayTopic(idOfItem);
                    });

                    it('should call the service post method with the new object.', () => {
                        sinon.assert.calledWith(modifyEssayTopicGraduatingYearService.postAddYearToTopic, new Dtos.Admin.EssayTopics.EditEssayTopicByGraduatingClassModel(idOfItem, yearToAddOrDelete));
                    });

                    describe('failure', () => {
                        beforeEach(() => {
                            deferred2.reject();
                            scope.$apply();
                        });

                        it('should pass the failure on to the resolve service.', () => {
                            sinon.assert.calledWith(modifyEssayTopicGraduatingYearService.resolvePostAddYearToTopic, new Common.Models.ServerResponseModel(null, false));
                        });
                    });

                    describe('success', () => {
                        beforeEach(() => {
                            deferred2.resolve();
                            scope.$apply();
                        });

                        it('should pass the failure on to the resolve service.', () => {
                            sinon.assert.calledWith(modifyEssayTopicGraduatingYearService.resolvePostAddYearToTopic, new Common.Models.ServerResponseModel(null, true));
                        });

                        it('should call getArrayFromServer.', () => {
                            callGetEssayTopicsFromServer(2);
                        });
                    });
                });
            });

            describe('add and edit essay topics', () => {
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

                    scope.createAndModifyEssayTopicModel = null;
                });

                describe('addEssayTopic', () => {
                    beforeEach(() => {
                        scope.addNewEssayTopic();
                    });

                    describe('createAndModifyEssayTopic on scope', () => {
                        it('should have the right title.', () => {
                            expect(scope.createAndModifyEssayTopicModel.titleOfEssay).toBeUndefined();
                        });

                        it('should have the right prompt.', () => {
                            expect(scope.createAndModifyEssayTopicModel.essayPrompt).toBeUndefined();
                        });

                        it('should have the right id.', () => {
                            expect(scope.createAndModifyEssayTopicModel.id).toBeUndefined();
                        });
                    });

                    shouldCallAllItemsAfterModal();
                });

                describe('editEssayTopic', () => {
                    beforeEach(() => {
                        createAndGetEssayTopicsService.getEssayTopicArray.returns(fullArray);
                        scope.editEssayTopic(1);
                    });
                    
                    describe('createAndModifyEssayTopic on scope', () => {
                        it('should have the right title.', () => {
                            expect(scope.createAndModifyEssayTopicModel.titleOfEssay).toBe(titleOfEssay + 2);
                        });

                        it('should have the right prompt.', () => {
                            expect(scope.createAndModifyEssayTopicModel.essayPrompt).toBe(prompt + 2);
                        });

                        it('should have the right id.', () => {
                            expect(scope.createAndModifyEssayTopicModel.id).toBe(2);
                        });
                    });

                    shouldCallAllItemsAfterModal();
                });

                function shouldCallAllItemsAfterModal() {
                    it('should call open modal.', () => {
                        sinon.assert.calledOnce(modal.open);
                    });

                    describe('canceled operation', () => {
                        beforeEach(() => {
                            scope.modalInstance.dismiss();
                        });

                        it('should set createAndModifyEssayTopicModel to null.', () => {
                            expect(scope.createAndModifyEssayTopicModel).toBeNull();
                        });

                        it('should not call the update service.', () => {
                            sinon.assert.notCalled(createAndGetEssayTopicsService.postCreateOrModifyEssayTopic);
                        });
                    });

                    describe('successful operation', () => {
                        beforeEach(() => {
                            scope.modalInstance.close(essayTopicModel1);
                        });

                        it('should call postCreateOrModifyEssayTopic.', () => {
                            sinon.assert.calledWith(createAndGetEssayTopicsService.postCreateOrModifyEssayTopic, essayTopicModel1);
                        });

                        describe('failure', () => {
                            beforeEach(() => {
                                deferred2.reject();
                                scope.$apply();
                            });

                            it('should set the model to null.', () => {
                                expect(scope.createAndModifyEssayTopicModel).toBeNull();
                            });

                            it('should not try to get new arrays.', () => {
                                callGetEssayTopicsFromServer(1);
                            });
                        });

                        describe('success', () => {
                            beforeEach(() => {
                                deferred2.resolve();
                                scope.$apply();
                            });

                            it('should set the model to null.', () => {
                                expect(scope.createAndModifyEssayTopicModel).toBeNull();
                            });

                            it('should try to get new array.', () => {
                                callGetEssayTopicsFromServer(2);
                            });
                        });
                    });
                }
            });
        });

        function callGetEssayTopicsFromServer(howManyTimes: number) {
            if (howManyTimes == 1) {
                sinon.assert.calledOnce(createAndGetEssayTopicsService.getEssayTopicsFromServer);
            } else if (howManyTimes == 2) {
                sinon.assert.calledTwice(createAndGetEssayTopicsService.getEssayTopicsFromServer);
            }
        }
    });
}