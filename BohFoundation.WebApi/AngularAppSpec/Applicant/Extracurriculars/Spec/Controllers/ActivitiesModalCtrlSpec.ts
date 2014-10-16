module BohFoundation.Applicant.Extracurriculars.Spec.Controllers {
    describe("ActivityModalCtrl", () => {

        var scope: Extracurriculars.Controllers.IActivityModalCtrlScope, modalInstance, activityModel;

        beforeEach(inject(($rootScope) => {
            modalInstance = new TestHelpers.FakeModal().createModalInstanceStub();

            scope = $rootScope;

            activityModel = new Dtos.Applicant.Extracurricular.ActivityModel();

            new Extracurriculars.Controllers.ActivityModalCtrl(scope, modalInstance, activityModel);
        }));

        it('should set the model on scope.', () => {
            expect(scope.activityModel).toBe(activityModel);
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
                    sinon.assert.calledWith(modalInstance.close, activityModel);
                });
            });
        });
    });
} 