module BohFoundation.Applicant.FamilyInformation.Spec.Controllers {
    describe("FamilyInformationModalCtrl", () => {
        var modalInstance, scope: FamilyInformation.Controllers.IFamilyInformationModalCtrlScope, familyInformationModel;

        describe('FamilyInformationModalCtrl with null passed in', () => {
            mainBeforeEach(true);

            it('should set the new modal on scope.', () => {
                expect(scope.familyInformationModel).toNotBe(null);
            });
        });

        describe('pass in real model', () => {
            mainBeforeEach(false);

            it('should set the modal on scope.', () => {
                expect(scope.familyInformationModel).toBe(familyInformationModel);
            });

            it('should have an array of length 10 for yearly incomes.', () => {
                expect(scope.yearlyHouseholdIncomeRange.length).toBe(10);
            });

            it('should have an array of length 6 for degrees.', () => {
                expect(scope.highestAttainedDegreeInHome.length).toBe(6);
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
                        sinon.assert.calledWith(modalInstance.close, familyInformationModel);
                    });
                });
            });
        });

        function mainBeforeEach(useNullAsModel: boolean) {
            beforeEach(inject(($rootScope) => {
                modalInstance = new TestHelpers.FakeModal().createModalInstanceStub();

                scope = $rootScope;

                familyInformationModel = new Dtos.Applicant.Family.FamilyInformationModel();

                if (useNullAsModel) {
                    new FamilyInformation.Controllers.FamilyInformationModalCtrl(scope, modalInstance, null);
                } else {
                    new FamilyInformation.Controllers.FamilyInformationModalCtrl(scope, modalInstance, familyInformationModel);
                }
            }));
        }
    });
   
} 