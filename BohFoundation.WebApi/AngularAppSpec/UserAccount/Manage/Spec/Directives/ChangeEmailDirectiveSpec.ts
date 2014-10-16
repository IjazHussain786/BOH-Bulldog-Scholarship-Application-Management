module BohFoundation.UserAccount.Manage.Spec.Directives {
    describe('ChangeEmailDirective', () => {

        var el, scope, form;
        var userEmail = 'userEmail';
        var newEmailInvalid = 'newEmailInvalid';
        var newEmailValid = 'newEmail@cooks.guru';

        beforeEach(module('BohFoundation.UserManagement'));
        beforeEach(module('ng-Templates'));

        beforeEach(inject(($compile, _$rootScope_) => {
            scope = _$rootScope_;
            scope.usersEmail = () => { return userEmail; }
             scope.changeEmailModel = {};
            scope.processing = false;

            el = angular.element('<change-email></change-email>');

            $compile(el)(scope);

            digest();

            form = scope.changeEmailForm;
        }));

        it('should have printed the user\'s email.', () => {
            expect(el.html()).toContain('Current Email Address: ' + userEmail);
        });

        it('should be a pristine form.', () => {
            expect(form.$pristine).toBeTruthy();
        });

        it('should not be valid.', () => {
            expect(form.$valid).toBeFalsy();
        });

        it('should not have a valid emailOrUsername.', () => {
            expect(form.newEmailAddress.$valid).toBeFalsy();
        });

        it('should have a disabled Change Email Address Button.', () => {
            expect(el.html()).toContain('disabled="disabled"');
        });


        describe('there has been a submission somewhere else', () => {
            beforeEach(() => {
                scope.processing = true;
                scope.changeEmailForm.$invalid = false;

                digest();
            });

            it('should disable the submit button.', () => {
                expect(el.html()).toContain('disabled="disabled"');
            });
        });

        describe('invalid email', () => {

            beforeEach(() => {
                form.newEmailAddress.$setViewValue(newEmailInvalid);
                digest();
            });

            it('should not set the value on scope.', () => {
                expect(scope.changeEmailModel.newEmail).toBeUndefined();
            });

            it('should not have a valid email field.', () => {
                expect(form.newEmailAddress.$valid).toBeFalsy();
            });

            it('should not be a pristine form.', () => {
                expect(form.$pristine).toBeFalsy();
            });

            it('should not be valid.', () => {
                expect(form.$valid).toBeFalsy();
            });

            it('should disable the submit button.', () => {
                expect(el.html()).toContain('disabled="disabled"');
            });
        });

        describe('valid inputs', () => {
            beforeEach(() => {
                form.newEmailAddress.$setViewValue(newEmailValid);
                digest();
            });

            it('should set the value on scope.', () => {
                expect(scope.changeEmailModel.newEmail).toBe(newEmailValid);
            });

            it('should have a valid email field.', () => {
                expect(form.newEmailAddress.$valid).toBeTruthy();
            });

            it('should not be a pristine form.', () => {
                expect(form.$pristine).toBeFalsy();
            });

            it('should be valid.', () => {
                expect(form.$valid).toBeTruthy();
            });

            it('should not disable the submit button.', () => {
                expect(el.html()).toNotContain('disabled="disabled"');
            });
        });

        function digest() {
            scope.$digest();
        }
    });
}