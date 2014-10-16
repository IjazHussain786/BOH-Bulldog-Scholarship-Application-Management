module BohFoundation.Common.Spec.Services.UiServices {
    describe('AlertHelperServices', () => {

        var service: Common.Services.UiServices.IAlertHelperServices;
        var message = "message";
        var toaster;
        var whatInfo = "contact";

        beforeEach(() => {
            toaster = sinon.stub({
                pop: () => { }
            });

            service = new Common.Services.UiServices.AlertHelperService(toaster);
        });


        describe('addWarningAlert', () => {
            it('should add an alert with a message and type of warning.', () => {
                expect(toaster.pop.calledWith('warning', 'Warning...', message)).toBeFalsy();
                service.addWarningAlert(message);
                expect(toaster.pop.calledWith('warning', 'Warning...', message)).toBeTruthy();
            });
        });

        describe('addSuccessAlert', () => {
            it('should add an alert with a message and type of success.', () => {
                expect(toaster.pop.calledWith('success', 'Success!', message)).toBeFalsy();
                service.addSuccessAlert(message);
                expect(toaster.pop.calledWith('success', 'Success!', message)).toBeTruthy();
            });
        });

        describe('addSuccessAlert', () => {
            it('should add an alert with a message and type of success.', () => {
                expect(toaster.pop.calledWith('error', 'Drat!', message)).toBeFalsy();
                service.addDangerAlert(message);
                expect(toaster.pop.calledWith('error', 'Drat!', message)).toBeTruthy();
            });
        });

        describe('addGenericInformationPost', () => {
            it('should add a success with "You have successfully saved your information."', () => {
                service.addGenericInformationPostSuccess();
                sinon.assert.calledWith(toaster.pop, 'success', "Success!", "You have successfully saved your information.");
            });
        });

        describe('addGenericInformationPostError', () => {
            it('should call with message.', () => {
                service.addGenericInformationPostError(whatInfo);
                sinon.assert.calledWith(toaster.pop, 'error', "Drat!", "There was a problem saving your contact information. Please try again later.");
            });
        });

        describe('addGenericInformationGetError', () => {
            it('should call with message.', () => {
                service.addGenericInformationGetError(whatInfo);
                sinon.assert.calledWith(toaster.pop, 'error', "Drat!", "There was a problem getting your contact information.");
            });
        });

    });
} 