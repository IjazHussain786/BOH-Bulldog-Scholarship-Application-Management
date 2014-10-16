module BohFoundation.Person.ContactInformation.Spec.Controllers {

    describe('ContactInformationModalInputCtrl', () => {
        var modalInstance, scope: ContactInformation.Controllers.IContactInformationModalInputCtrlScope, contactInformationModel;

        beforeEach(inject(($rootScope) => {
            modalInstance = new TestHelpers.FakeModal().createModalInstanceStub();

            scope = $rootScope; 

            contactInformationModel = new Dtos.Person.ContactInformationModel();

            new ContactInformation.Controllers.ContactInformationModalInputCtrl(scope, modalInstance, contactInformationModel);
        }));

        it('should set the model on scope.', () => {
            expect(scope.contactInformationModel).toBe(contactInformationModel);
        });

        it('should have 5 items on bestTimeToContactByPhone.', () => {
            expect(scope.bestTimeToContactByPhone.length).toBe(5);
        });

        describe('cancel', () => {
            it('should call dismiss.', () => {
                scope.cancel();
                sinon.assert.calledOnce(modalInstance.dismiss);
            });
        });

        describe('ok', () => {
            var form;

            beforeEach(() => {
                form = sinon.stub({
                    $valid: {}
                });
            });

            describe('invalid form', () => {
                it('should not call close.', () => {
                    form.$valid = false;
                    scope.ok(form);
                    sinon.assert.notCalled(modalInstance.close);
                });
            });

            describe('valid form', () => {
                it('should call close.', () => {
                    form.$valid = true;
                    scope.ok(form);
                    sinon.assert.calledWith(modalInstance.close, contactInformationModel);
                });
            });
        });
    });
}