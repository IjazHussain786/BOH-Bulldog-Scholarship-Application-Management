module BohFoundation.Admin.LetterOfRecommendation.Spec.Controllers {
    describe("CreateGetLetterOfRecommendationGuidModelModalCtrl", () => {

        var scope, modalInstance, getLetterOfRecommendationGuidModel;

        beforeEach(inject(($rootScope) => {
            modalInstance = new TestHelpers.FakeModal().createModalInstanceStub();
            scope = $rootScope;

            getLetterOfRecommendationGuidModel = new Dtos.Admin.References.GetLetterOfRecommendationGuidModel();

            new LetterOfRecommendation.Controllers.CreateGetLetterOfRecommendationGuidModelModalCtrl(scope, modalInstance, getLetterOfRecommendationGuidModel);
        }));

        it('should set the model on scope.', () => {
            expect(scope.getLetterOfRecommendationGuidModel).toBe(getLetterOfRecommendationGuidModel);
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
                    sinon.assert.calledWith(modalInstance.close, getLetterOfRecommendationGuidModel);
                });
            });
        });
    });
} 