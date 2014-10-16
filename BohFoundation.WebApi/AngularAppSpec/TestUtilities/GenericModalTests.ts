module TestHelpers.Asserts {
    export class GenericModalTests {
        constructor(public scope, public modelPassedIn, public modalInstance) {
        }

        runTests() {
            describe('cancel', () => {
                it('should call dismiss.', () => {
                    this.scope.cancel();
                    sinon.assert.calledOnce(this.modalInstance.dismiss);
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
                        this.scope.ok(form);
                        sinon.assert.notCalled(this.modalInstance.close);
                    });
                });

                describe('valid form', () => {
                    it('should call close.', () => {
                        form.$valid = true;
                        this.scope.ok(form);
                        sinon.assert.calledWith(this.modalInstance.close, this.modelPassedIn);
                    });
                });
            });
        }
    }
}