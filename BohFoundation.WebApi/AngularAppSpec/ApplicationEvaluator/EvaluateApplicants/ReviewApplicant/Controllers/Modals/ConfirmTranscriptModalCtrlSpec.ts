module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Spec.Controllers.Modals {
    describe("CreateRatingModalCtrl", () => {

        var modalInstance, scope: ReviewApplicant.Controllers.Modals.IConfirmTranscriptModalCtrlScope, confirmTranscriptModel;

        beforeEach(inject(($rootScope) => {
            modalInstance = new TestHelpers.FakeModal().createModalInstanceStub();

            scope = $rootScope;

            confirmTranscriptModel = new Dtos.ApplicationEvaluator.EvaluatingApplicants.ConfirmTranscriptModel("ap", null);

            new ReviewApplicant.Controllers.Modals.ConfirmTranscriptModalCtrl(scope, modalInstance, confirmTranscriptModel);
        }));

        it('should set the model on scope.', () => {
            expect(scope.confirmTranscriptModel).toBe(confirmTranscriptModel);
        });

        describe('getRatingsArray', () => {
            var result;

            beforeEach(() => {
                result = scope.getRatingsArray();
            });

            it('should have 10 items in the array.', () => {
                expect(result.length).toBe(10);
            });

            it('should have APlus as the First item.', () => {
                expect(result[0]).toBe(Common.Enums.RatingEnum.APlus);
            });

            it('should have F as the last item.', () => {
                expect(result[9]).toBe(Common.Enums.RatingEnum.F);
            });
        });

        describe('cancel', () => {
            it('should call dismiss.', () => {
                scope.cancel();
                sinon.assert.calledOnce(modalInstance.dismiss);
            });
        });

        describe('ok', () => {
            it('should call close.', () => {
                scope.ok(false);
                sinon.assert.calledWith(modalInstance.close, confirmTranscriptModel);
            });

            it('should change the confirmTranscriptModel to false.', () => {
                scope.ok(false);
                expect(confirmTranscriptModel.informationMatchesTranscriptPdf).toBeFalsy();
            });

            it('should change the confirmTranscriptModel to true.', () => {
                scope.ok(true);
                expect(confirmTranscriptModel.informationMatchesTranscriptPdf).toBeTruthy();
            });
        });
    });
}  
