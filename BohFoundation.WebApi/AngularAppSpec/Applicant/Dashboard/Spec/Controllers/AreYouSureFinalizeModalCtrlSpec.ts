module BohFoundation.Applicant.Dashboard.Spec.Controllers{
    describe("AreYouSureFinalizeModalCtrl", () => {

        var scope, modalInstance;

        beforeEach(inject(($rootScope, $q) => {
            modalInstance = new TestHelpers.FakeModal().createModalInstanceStub();

            scope = $rootScope;

            new Applicant.Dashboard.Controllers.AreYouSureFinalizeModalCtrl(scope, modalInstance);
        }));

        describe('cancel', () => {
            it('should call dismiss.', () => {
                scope.cancel();
                sinon.assert.calledOnce(modalInstance.dismiss);
            });
        });

        describe('ok', () => {
            it('should call close.', () => {
                scope.ok();
                sinon.assert.calledOnce(modalInstance.close);
            });
        });
    });
} 