module BohFoundation.UserAccount.PasswordStrength.Spec.Directives {
    describe('PasswordFieldAndMeterDirective', () => {

        var el, scope;

        beforeEach(module('BohFoundation.UserManagement'));
        beforeEach(module('ng-Templates'));

        beforeEach(inject(($compile, _$rootScope_) => {
            scope = _$rootScope_;
            scope.passwordScore = {};

            el = angular.element('<password-field-and-meter></password-field-and-meter>');

            $compile(el)(scope);

            digest();
        }));


        describe('field checkmark', () => {
            it('should default to not checked.', () => {
                expect(el.html()).toContain('class="glyphicon glyphicon-remove form-control-feedback"');
            });

            it('should not have the success checkmark.', () => {
                expect(el.html()).toNotContain('class="glyphicon form-control-feedback glyphicon-ok"');
            });

            describe('password score is more than 2', () => {
                beforeEach(() => {
                    scope.passwordScore.score = 3;
                    digest();
                });

                it('should switch to a success.', () => {
                    expect(el.html()).toContain('class="glyphicon form-control-feedback glyphicon-ok"');
                });

                it('should not have the not checked class.', () => {
                    expect(el.html()).toNotContain('class="glyphicon glyphicon-remove form-control-feedback"');
                });
            });
        });

        function digest() {
            scope.$digest();
        }
    });
} 