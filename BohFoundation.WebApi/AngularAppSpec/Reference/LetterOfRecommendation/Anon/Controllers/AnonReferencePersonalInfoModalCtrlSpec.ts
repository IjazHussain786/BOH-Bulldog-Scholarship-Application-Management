module BohFoundation.Reference.LetterOfRecommendation.Anon.Spec.Controllers {
    describe('AnonReferencePersonalInfoModalCtrl', () => {
        var modalInstance, scope: Anon.Controllers.IAnonReferencePersonalInfoModalCtrlScope, referencePersonalInformationModel;

        beforeEach(inject(($rootScope) => {
            modalInstance = sinon.stub({
                close: () => { },
                dismiss: () => { }
            });

            scope = $rootScope;

            referencePersonalInformationModel = new Dtos.Reference.Anonymous.ReferencePersonalInformationModel();

            new Anon.Controllers.AnonReferencePersonalInfoModalCtrl(scope, modalInstance, referencePersonalInformationModel);
        }));

        it('should have five items in the bestTimeToContactByPhone.', () => {
            expect(scope.bestTimeToContactByPhone.length).toBe(5);
        });

        describe('construction', () => {
            it('should set the referencePersonalInformationModel on scope.', () => {
                expect(scope.referencePersonalInformationModel).toBe(referencePersonalInformationModel);
            });
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
                    sinon.assert.calledWith(modalInstance.close, referencePersonalInformationModel);
                });
            });
        });
    });
} 