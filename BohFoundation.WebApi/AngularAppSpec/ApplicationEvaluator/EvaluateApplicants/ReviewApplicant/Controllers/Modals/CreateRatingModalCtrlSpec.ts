module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Spec.Controllers.Modals {
    describe("CreateRatingModalCtrl", () => {

        var modalInstance, scope: ReviewApplicant.Controllers.Modals.ICreateRatingModalCtrlScope, genericRatingModel;

        beforeEach(inject(($rootScope) => {
            modalInstance = new TestHelpers.FakeModal().createModalInstanceStub();

            scope = $rootScope;

            genericRatingModel = new Dtos.Common.GenericRatingModel(Common.Enums.RatingEnum.B, "asfjk1234");

            new ReviewApplicant.Controllers.Modals.CreateRatingModalCtrl(scope, modalInstance, genericRatingModel);
        }));

        it('should set the model on scope.', () => {
            expect(scope.genericRatingModel).toBe(genericRatingModel);
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
                    sinon.assert.calledWith(modalInstance.close, genericRatingModel);
                });
            });
        });
    });
}  

