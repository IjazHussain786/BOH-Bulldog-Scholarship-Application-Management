module BohFoundation.Applicant.Extracurriculars.Spec.Controllers {
    describe("JobModalCtrl", () => {

        var scope: Extracurriculars.Controllers.IJobModalCtrlScope, modalInstance, jobModel;

        beforeEach(inject(($rootScope) => {
            modalInstance = new TestHelpers.FakeModal().createModalInstanceStub();
            
            scope = $rootScope;

            jobModel = new Dtos.Applicant.Extracurricular.JobModel();

            new Extracurriculars.Controllers.JobModalCtrl(scope, modalInstance, jobModel);
        }));

        it('should set the model on scope.', () => {
            expect(scope.jobModel).toBe(jobModel);
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
                    sinon.assert.calledWith(modalInstance.close, jobModel);
                });
            });
        });
    });
} 