module BohFoundation.Common.Spec.Services.Resolvers {
    describe('GenericResolver', () => {
        var alertHelperService, result, errorResponse, successResponse;
        var service;
        var type = "TYEPS";
        var serverMessage, data;

        beforeEach(() => {
            errorResponse = new Common.Models.ServerResponseModel(null, false);

            data = "asfdjkhn";

            serverMessage = new Common.Models.ServerMessageModel(data);

            successResponse = new Common.Models.ServerResponseModel(serverMessage, true);
            
            alertHelperService = sinon.stub({
                addSuccessAlert: () => {},
                addDangerAlert: () => {},
                addGenericInformationGetError: () => {},
                addGenericInformationPostSuccess: () => {},
                addGenericInformationPostError: () => {}
            });

            service = new Common.Services.Resolvers.GenericResolver(alertHelperService);
        });

        describe('genericPostResolver', () => {
            describe('error', () => {
                beforeEach(() => {
                    service.genericPostResolver(type, errorResponse);
                });

                it('should send call genericError.', () => {
                    sinon.assert.calledWith(alertHelperService.addGenericInformationPostError, type);
                });
            });

            describe('success', () => {
                beforeEach(() => {
                    service.genericPostResolver(type, successResponse);
                });

                it('should call addGenericPostSuccess.', () => {
                    sinon.assert.calledOnce(alertHelperService.addGenericInformationPostSuccess);
                });
            });
        });

        describe('genericGetResolver', () => {
            describe('error', () => {
                beforeEach(() => {
                    result = service.genericGetResolver(type, errorResponse);
                });

                it('should call genericGetError', () => {
                    sinon.assert.calledWith(alertHelperService.addGenericInformationGetError, type);
                });

                it('should return false.', () => {
                    expect(result).toBeNull();
                });
            });

            describe('post', () => {
                beforeEach(() => {
                    result = service.genericGetResolver(type, successResponse);
                });

                it('should return whatever is on the data part.', () => {
                    expect(result).toBe(data);
                });
            });
        });
    });
} 