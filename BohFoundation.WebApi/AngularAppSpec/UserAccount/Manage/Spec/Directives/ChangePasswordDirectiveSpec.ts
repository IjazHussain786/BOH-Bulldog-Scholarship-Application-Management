module BohFoundation.UserAccount.Manage.Spec.Directives {
    describe('ChangePasswordDirective', () => {


        var el, scope, form;

        var oldPassword = 'oldPassword';
        var newPassword = 'newPassword';

        beforeEach(module('BohFoundation.UserManagement'));
        beforeEach(module('ng-Templates'));

        beforeEach(inject(($compile, $rootScope) => {
            scope = $rootScope;

            scope.changePasswordModel = {};
            scope.processing = false;
            scope.passwordScore = {};
            scope.passwordScore.score = 2;

            el = angular.element('<change-password></change-password>');

            $compile(el)(scope);

            digest();

            form = scope.changePasswordForm;
        }));

        it('should be a pristine form.', () => {
            expect(form.$pristine).toBeTruthy();
        });

        it('should not be valid.', () => {
            expect(form.$valid).toBeFalsy();
        });

        it('should not have a valid newPassword.', () => {
            expect(form.oldPassword.$valid).toBeFalsy();
        });

        it('should not have a valid oldPassword.', () => {
            expect(form.newPassword.$valid).toBeFalsy();
        });

        it('should have a disabled Change Email Address Button.', () => {
            expect(el.html()).toContain('disabled="disabled"');
        });

        describe('oldPassword field', () => {
            beforeEach(() => {
                form.oldPassword.$setViewValue(oldPassword);
                digest();
            });

            it('should set the value on scope.', () => {
                expect(scope.changePasswordModel.oldPassword).toBe(oldPassword);
            });

            it('should have a valid oldPassword field.', () => {
                expect(form.oldPassword.$valid).toBeTruthy();
            });

            it('should not be a pristine form.', () => {
                expect(form.$pristine).toBeFalsy();
            });

            it('should not be a valid form.', () => {
                expect(form.$valid).toBeFalsy();
            });

            it('should disable the submit button.', () => {
                expect(el.html()).toContain('disabled="disabled"');
            });
        });

        describe('newPassword field', () => {
            beforeEach(() => {
                form.newPassword.$setViewValue(newPassword);
                digest();
            });

            it('should set the value on scope.', () => {
                expect(scope.changePasswordModel.newPassword).toBe(newPassword);
            });

            it('should have a valid newPassword field.', () => {
                expect(form.newPassword.$valid).toBeTruthy();
            });

            it('should not be a pristine form.', () => {
                expect(form.$pristine).toBeFalsy();
            });

            it('should not be a valid form.', () => {
                expect(form.$valid).toBeFalsy();
            });

            it('should disable the submit button.', () => {
                expect(el.html()).toContain('disabled="disabled"');
            });
        });

        describe('both fields set', () => {

            beforeEach(() => {
                form.newPassword.$setViewValue(newPassword);
                form.oldPassword.$setViewValue(oldPassword);
                digest();
            });

            it('should not be a pristine form.', () => {
                expect(form.$pristine).toBeFalsy();
            });

            it('should be a valid form.', () => {
                expect(form.$valid).toBeTruthy();
            });

            it('should disable the submit button.', () => {
                expect(el.html()).toContain('disabled="disabled"');
            });

            describe('with good new password', () => {
                beforeEach(() => {
                    scope.passwordScore.score = 3;
                    digest();
                });

                it('should disable the submit button.', () => {
                    expect(el.html()).toNotContain('disabled="disabled"');
                });
            });
        });

        function digest() {
            scope.$digest();
        }
    });
}  