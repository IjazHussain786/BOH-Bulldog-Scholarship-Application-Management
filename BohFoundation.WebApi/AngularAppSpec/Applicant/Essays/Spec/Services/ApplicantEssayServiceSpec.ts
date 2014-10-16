module BohFoundation.Applicant.Essays.Spec.Services {
    describe('ApplicantEssayService', () => {
        var apiEndpoint = "/api/applicant/essay";
        var typeOfData = "essay";
        var essayTopicId = 19123;
        var service;
        var authRepo, result, errorResponse, modelFromServer;
        var $location, genericResolver, successResponse;

        beforeEach(() => {
            successResponse = new Common.Models.ServerResponseModel(null, true);
            modelFromServer = { data: null };

            errorResponse = new Common.Models.ServerResponseModel(null, false);

            authRepo = new TestHelpers.CommonStubs().createAuthRepo();

            genericResolver = new TestHelpers.CommonStubs().createGenericResolver();

            $location = new TestHelpers.CommonStubs().create$Location();

            service = new Essays.Services.ApplicantEssayService(authRepo, $location, genericResolver);
        });

        describe('constructor', () => {
            it('should have an empty arrayOfEssayNotificationModels.', () => {
                expect(service.arrayOfEssayNotificationModels.length).toBe(0);
            });
        });

        describe('setArrayOfEssayNotificationsModelFromNotificationService', () => {
            it('should set whatever come in to the arrayOfEssayNotificationModels field.', () => {
                var dog = "hund";
                service.setArrayOfEssayNotificationsModelFromNotificationService(dog);
                expect(service.arrayOfEssayNotificationModels).toBe(dog);
            });
        });

        describe('getEssayModel', () => {
            describe('currentEssayModelFromServer is', () => {
                var essayNotificationModel1, essayNotificationModel2, essayNotificationModel3, essayNotificationModel4;
                var prompt = "prompt", title = "title";

                beforeEach(() => {
                    essayNotificationModel1 = new Dtos.Applicant.Notifications.EssayNotificationsModel(prompt + 1, title + 1, 3);
                    essayNotificationModel2 = new Dtos.Applicant.Notifications.EssayNotificationsModel(prompt + 2, title + 2, 6);
                    essayNotificationModel3 = new Dtos.Applicant.Notifications.EssayNotificationsModel(prompt + 3, title + 3, 9);
                    essayNotificationModel4 = new Dtos.Applicant.Notifications.EssayNotificationsModel(prompt + 4, title + 4, 10);

                    service.arrayOfEssayNotificationModels = [essayNotificationModel1, essayNotificationModel2, essayNotificationModel3, essayNotificationModel4];
                });

                describe('null', () => {
                    beforeEach(() => {
                        service.currentEssayModelFromServer = null;
                        result = service.getEssayModel(9);
                    });

                    it('should return an essayModel without revisionDate.', () => {
                        expect(result.revisionDateTime).toBeUndefined();
                    });

                    it('should return an essayModel without essay.', () => {
                        expect(result.essay).toBeUndefined();
                    });

                    it('should return an essayModel with correct prompt.', () => {
                        expect(result.essayPrompt).toBe(prompt + 3);
                    });

                    it('should return an essayModel with essayTopicId.', () => {
                        expect(result.essayTopicId).toBe(9);
                    });

                    it('should not hit path.', () => {
                        sinon.assert.notCalled($location.path);
                    });
                });

                describe('undefined', () => {
                    beforeEach(() => {
                        result = service.getEssayModel(3);
                    });

                    it('should return an essayModel without revisionDate.', () => {
                        expect(result.revisionDateTime).toBeUndefined();
                    });

                    it('should return an essayModel without essay.', () => {
                        expect(result.essay).toBeUndefined();
                    });

                    it('should return an essayModel with correct prompt.', () => {
                        expect(result.essayPrompt).toBe(prompt + 1);
                    });

                    it('should return an essayModel with essayTopicId.', () => {
                        expect(result.essayTopicId).toBe(3);
                    });

                    it('should not hit path.', () => {
                        sinon.assert.notCalled($location.path);
                    });
                });

                describe('not in array and getEssayFromServerHasBeenResolved is true', () => {
                    beforeEach(() => {
                        service.getEssayFromServerHasBeenResolved = true;
                        result = service.getEssayModel(32);
                    });

                    it('should hit path to dashboard.', () => {
                        sinon.assert.calledWith($location.path, "/Applicant");
                    });
                });


                describe('not in array and getEssayFromServerHasBeenResolved is false', () => {
                    beforeEach(() => {
                        result = service.getEssayModel(32);
                    });

                    it('should not hit path.', () => {
                        sinon.assert.notCalled($location.path);
                    });

                    it('should return an essayModel without revisionDate.', () => {
                        expect(result.revisionDateTime).toBeUndefined();
                    });

                    it('should return an essayModel without essay.', () => {
                        expect(result.essay).toBeUndefined();
                    });

                    it('should return an essayModel with undefined prompt.', () => {
                        expect(result.essayPrompt).toBeUndefined();
                    });

                    it('should return an essayModel with undefined essayTopicId.', () => {
                        expect(result.essayTopicId).toBeUndefined();
                    });
                });

                describe('instantiated', () => {
                    beforeEach(() => {
                        service.currentEssayModelFromServer = typeOfData;
                        result = service.getEssayModel(3);
                    });

                    it('should return whatever is on currentEssayModelFromServer.', () => {
                        expect(result).toBe(typeOfData);
                    });

                    it('should not hit path.', () => {
                        sinon.assert.notCalled($location.path);
                    });
                });
            });
        });

        describe('getEssayFromServer', () => {
            describe('get', () => {
                beforeEach(() => {
                    authRepo.get.returns(typeOfData);
                    result = service.getEssayFromServer(essayTopicId);
                });

                it('should call authRepo with endpoint + essayTopicId string.', () => {
                    sinon.assert.calledWith(authRepo.get, apiEndpoint + "/" + essayTopicId);
                });

                it('should return what authRepo get returns.', () => {
                    expect(result).toBe(typeOfData);
                });
            });

            describe('resolve', () => {
                var serverResponseWithModel;

                describe('error', () => {
                    beforeEach(() => {
                        service.resolveGetEssayFromServer(errorResponse);
                    });

                    it('should call genericInformationGetError.', () => {
                        sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, errorResponse);
                    });

                    it('should have get essayFromServerHasBeenResolved To False.', () => {
                        expect(service.getEssayFromServerHasBeenResolved).toBeFalsy();
                    });
                });

                describe('first time', () => {
                    beforeEach(() => {
                        genericResolver.genericGetResolver.returns(null);
                        modelFromServer.data = null;
                        serverResponseWithModel = new Common.Models.ServerResponseModel(modelFromServer, true);
                        service.resolveGetEssayFromServer(serverResponseWithModel);
                    });

                    it('should call the resolver.', () => {
                        sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, serverResponseWithModel);
                    });

                    it('should set the essayTopicArray.', () => {
                        expect(service.currentEssayModelFromServer).toBeNull();
                    });

                    it('should have get essayFromServerHasBeenResolved To True.', () => {
                        expect(service.getEssayFromServerHasBeenResolved).toBeTruthy();
                    });
                });

                describe('with essay', () => {
                    beforeEach(() => {
                        genericResolver.genericGetResolver.returns(typeOfData);
                        modelFromServer.data = typeOfData;
                        serverResponseWithModel = new Common.Models.ServerResponseModel(modelFromServer, true);
                        service.resolveGetEssayFromServer(serverResponseWithModel);
                    });

                    it('should call the resolver.', () => {
                        sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, serverResponseWithModel);
                    });

                    it('should set the essayTopicArray.', () => {
                        expect(service.currentEssayModelFromServer).toBe(typeOfData);
                    });

                    it('should have get essayFromServerHasBeenResolved To True.', () => {
                        expect(service.getEssayFromServerHasBeenResolved).toBeTruthy();
                    });
                });
            });
        });

        describe('postEssay', () => {
            var model;

            beforeEach(() => {
                model = "model";
            });

            describe('post', () => {
                beforeEach(() => {
                    authRepo.post.returns(typeOfData);
                    result = service.postEssay(model);
                });

                it('should call auth repo.', () => {
                    sinon.assert.calledWith(authRepo.post, model, apiEndpoint);
                });

                it('should return whatever authrepo returns.', () => {
                    expect(result).toBe(typeOfData);
                });
            });

            describe('resolve', () => {
                it('should pass whatever is passed in to the resolver with the correct information type to the generic resolver.', () => {
                    service.resolvePostEssay(successResponse);
                    sinon.assert.calledWith(genericResolver.genericPostResolver, typeOfData, successResponse);
                });
            });
        });
    });
}