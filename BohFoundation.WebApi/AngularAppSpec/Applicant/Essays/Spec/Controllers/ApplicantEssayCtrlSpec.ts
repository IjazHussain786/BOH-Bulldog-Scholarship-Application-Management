module BohFoundation.Applicant.Essays.Spec.Controllers {
    describe('ApplicantEssayCtrl', () => {
        var scope, $routeParams;
        var applicantEssayService;
        var data = { somestuff: 12390 };
        var essayTopicId = 1039;
        var essayTopicIdString = "1039";

        var q, modal;
        var deferred, promise, resource;
        var deferred2, promise2, resource2;
        var originalEssayModel, prompt = "prompt";

        var cssInjector;

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
                open: () => { }
            });

            $routeParams = sinon.stub({
                essayTopicId: essayTopicIdString
            });

            cssInjector = sinon.stub({
                add:()=>{}
            });

            applicantEssayService = sinon.stub({
                getEssayFromServer: () => { },
                resolveGetEssayFromServer: () => { },
                postEssay: () => { },
                resolvePostEssay: () => { },
                getEssayModel: () => { }
            });

            originalEssayModel = new Dtos.Applicant.Essay.EssayModel(prompt, essayTopicId);

            applicantEssayService.getEssayFromServer.returns(resource);
            applicantEssayService.postEssay.returns(resource2);
            applicantEssayService.getEssayModel.returns(originalEssayModel);
            new Essays.Controllers.ApplicantEssayCtrl(scope, $routeParams, applicantEssayService, modal, cssInjector);
        }));

        describe('construction', () => {
            beforeEach(() => {
                
            });

            it('should call cssInjector with link to fontawesome.', () => {
                sinon.assert.calledWith(cssInjector.add, "https://maxcdn.bootstrapcdn.com/font-awesome/4.1.0/css/font-awesome.min.css"); 
            });

            it('should set the essayTopicId on scope.', () => {
                expect(scope.essayTopicId).toBe(essayTopicId);
            });

            it('should set the essay to the one on the service.', () => {
                expect(scope.essayModel).toBe(originalEssayModel);
            });

            it('should call getEssayModal with the topicId.', () => {
                sinon.assert.calledWith(applicantEssayService.getEssayModel, essayTopicId);
            });

            it('should get essay from server.', () => {
                sinon.assert.calledWith(applicantEssayService.getEssayFromServer, essayTopicId);
            });

            describe('ajax failure', () => {
                it('should call resolveGetEssayFromServicer.', () => {
                    deferred.reject();
                    scope.$apply();
                    sinon.assert.calledWith(applicantEssayService.resolveGetEssayFromServer, new Common.Models.ServerResponseModel(null, false));
                });

                it('should not call getEssayModel.', () => {
                    deferred.reject();
                    scope.$apply();
                    sinon.assert.calledOnce(applicantEssayService.getEssayModel);
                });
            });

            describe('ajax success', () => {
                beforeEach(() => {
                    applicantEssayService.getEssayModel.returns(data);
                    deferred.resolve(data);
                    scope.$apply();
                });

                it('should call resolveGetEssayFromServicer.', () => {
                    sinon.assert.calledWith(applicantEssayService.resolveGetEssayFromServer, new Common.Models.ServerResponseModel(data, true));
                });

                it('should call getEssayModel.', () => {
                    sinon.assert.calledTwice(applicantEssayService.getEssayModel);
                });

                it('should set getEssayModel on scope.', () => {
                    expect(scope.essayModel).toBe(data);
                });
            });
        });

        describe('successful construction', () => {
            var fakeModal;

            beforeEach(() => {
                deferred.resolve(data);
                scope.$apply();
            });

            describe('edit essay', () => {
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

                describe('editEssay', () => {
                    var newEssayModel;
                    var essay = "essayessayessayessayessayessayessayessayessayessayessay";

                    beforeEach(() => {
                        scope.essayModel.essay = "original";
                        newEssayModel = new Dtos.Applicant.Essay.EssayModel(prompt, essayTopicId, essay);
                        scope.editEssay();
                    });

                    it('should call open modal.', () => {
                        sinon.assert.calledOnce(modal.open);
                    });

                    it('should set scope.originalEssayModel with the essay model on scope.', () => {
                        expect(scope.originalEssay).toBe(originalEssayModel.essay);
                    });
                    
                    describe('canceled operation', () => {
                        beforeEach(() => {
                            scope.essayModel = newEssayModel;
                            scope.modalInstance.dismiss();
                        });

                        it('should set essayModel.essay to the orginal essay.', () => {
                            expect(scope.essayModel.essay).toBe(originalEssayModel.essay);
                        });

                        it('should not call the update service.', () => {
                            sinon.assert.notCalled(applicantEssayService.postEssay);
                        });

                        it('should have a false attemptedToSave.', () => {
                            expect(scope.attemptedToSave).toBeFalsy();
                        });
                    });

                    describe('successful operation', () => {
                        beforeEach(() => {
                            scope.modalInstance.close(newEssayModel);
                        });

                        it('should have the new object on scope.', () => {
                            expect(scope.essayModel).toBe(newEssayModel);
                        });

                        it('should call post.', () => {
                            sinon.assert.calledWith(applicantEssayService.postEssay, newEssayModel);
                        });

                        it('should have a true attemptedToSave.', () => {
                            expect(scope.attemptedToSave).toBeTruthy();
                        });

                        describe('failure', () => {
                            beforeEach(() => {
                                deferred2.reject();
                                scope.$apply();
                            });

                            it('it should pass the failure on to the resolvePost.', () => {
                                sinon.assert.calledWith(applicantEssayService.resolvePostEssay, new Common.Models.ServerResponseModel(null, false));
                            });

                            it('should not change the model.', () => {
                                expect(scope.essayModel).toBe(newEssayModel);
                            });

                            it('should set successfullySaved to false.', () => {
                                expect(scope.successfullySaved).toBeFalsy();
                            });

                            it('should have a true attemptedToSave.', () => {
                                expect(scope.attemptedToSave).toBeTruthy();
                            });
                        });

                        describe('success', () => {
                            beforeEach(() => {
                                deferred2.resolve();
                                scope.$apply();
                            });

                            it('it should pass the success on to the resolvePost.', () => {
                                sinon.assert.calledWith(applicantEssayService.resolvePostEssay, new Common.Models.ServerResponseModel(null, true));
                            });

                            it('should try to get new array.', () => {
                                expect(scope.essayModel.revisionDateTime).toBeDefined();
                            });

                            it('should set successfullySaved to true.', () => {
                                expect(scope.successfullySaved).toBeTruthy();
                            });

                            it('should have a true attemptedToSave.', () => {
                                expect(scope.attemptedToSave).toBeTruthy();
                            });
                        });
                    });
                    
                });
            });
        });
    });
}

