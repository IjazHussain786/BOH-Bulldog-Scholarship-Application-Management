module BohFoundation.Admin.LetterOfRecommendation.Spec.Services {
    describe("AdminLetterOfRecommendationService", () => {
        var apiEndpoint = "/api/admin/letterofrecommendation/getguid";
        var typeOfData = "letter of recommendation id";
        var service, authRepo, errorResponse, successResponse, genericResolver, $location, result;

        beforeEach(() => {
            successResponse = new Common.Models.ServerResponseModel(null, true);
            errorResponse = new Common.Models.ServerResponseModel(null, false);

            var commonStubs = new TestHelpers.CommonStubs();

            authRepo = commonStubs.createAuthRepo();
            genericResolver = commonStubs.createGenericResolver();
            $location = commonStubs.create$Location();

            service = new LetterOfRecommendation.Services.AdminLetterOfRecommendationService(authRepo, $location, genericResolver);
        });

        describe('getGuidSentToReference', () => {
            it('should return whatever is on the private propery.', () => {
                service.guidSentToReference = apiEndpoint;
                expect(service.getGuidSentToReference()).toBe(apiEndpoint);
            });
        });

        describe('goToReferenceForm', () => {
            it('should not call location if canGoTo is false.', () => {
                service.guidSentToReference = undefined;
                service.goToReferenceForm();
                sinon.assert.notCalled($location.path);
            });

            it('should call location with to the anon reference ctrl.', () => {
                service.guidSentToReference = new Dtos.Admin.References.GuidSentToReferenceModel(typeOfData, null, null);
                service.goToReferenceForm();
                sinon.assert.calledWith($location.path, "/Reference/LetterOfRecommendation/Anon/" + typeOfData);
            });
        });

        describe('canGoToReference', () => {
            it('should default to false.', () => {
                service.guidSentToReference = undefined;
                expect(service.canGoToReference()).toBeFalsy();
            });

            it('should default to false.', () => {
                service.guidSentToReference = new Dtos.Admin.References.GuidSentToReferenceModel(undefined, false, null);
                expect(service.canGoToReference()).toBeFalsy();
            });

            it('should return true if guidSentToReference.guid is defined.', () => {
                service.guidSentToReference = new Dtos.Admin.References.GuidSentToReferenceModel("1123412", false, null);
                expect(service.canGoToReference()).toBeTruthy();
            });

            it('should return false if getGuidSentToReferenceModel().guid is null.', () => {
                service.guidSentToReference = new Dtos.Admin.References.GuidSentToReferenceModel(null, false, null);
                expect(service.canGoToReference()).toBeFalsy();
            });

            it('should return false if getGuidSentToReferenceModel().guid is 00000000-0000-0000-0000-000000000000.', () => {
                service.guidSentToReference = new Dtos.Admin.References.GuidSentToReferenceModel('00000000-0000-0000-0000-000000000000', false, null);
                expect(service.canGoToReference()).toBeFalsy();
            });
        });

        describe('getGuidSentToReferenceFromServer', () => {
            var applicantEmail = "afsdkj@aol.com";
            var referenceEmail = "adfsj021@gmail.com";
            var modelToSend;

            beforeEach(() => {
                modelToSend = new Dtos.Admin.References.GetLetterOfRecommendationGuidModel(applicantEmail, referenceEmail);
                authRepo.post.returns(typeOfData);
                result = service.getGuidSentToReferenceFromServer(modelToSend);
            });

            it('should call auth repo with correct uri.', () => {
                sinon.assert.calledWith(authRepo.post, modelToSend, apiEndpoint);
            });

            it('should return whatever the repo returns.', () => {
                expect(result).toBe(typeOfData);
            });
        });

        describe('resolveGetGuidSentToReferenceFromServer', () => {
            var serverMessage = "message";
            beforeEach(() => {
                genericResolver.genericGetResolver.returns(apiEndpoint);
                service.resolveGetGuidSentToReferenceFromServer(serverMessage);
            });

            it('should call genericGetResolver with type of data and the object passed in.', () => {
                sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, serverMessage);
            });

            it('should put whatever returns form the generic resolve on the guidSentToReference field.', () => {
                expect(service.guidSentToReference).toBe(apiEndpoint);
            });
        });
    });
} 