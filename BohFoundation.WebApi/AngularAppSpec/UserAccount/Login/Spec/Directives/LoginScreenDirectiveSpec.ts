module BohFoundation.UserAccount.Login.Spec.Directives {
    describe('LoginFormDirective', () => {
        var email = "email@email.com";
        var password = "password";
        var keepMeLoggedIn = true;

        var scope, el, form;

        beforeEach(module('BohFoundation.UserManagement'));
        beforeEach(module('ng-Templates'));

        beforeEach(inject(($compile, _$rootScope_) => {
            scope = _$rootScope_;

            scope.loginModel = {};
            scope.loginModel.email = null;
            scope.loginModel.password = null;
            scope.loginModel.keepMeLoggedIn = false;

            el = angular.element('<login-screen></login-screen>');

            $compile(el)(scope);
            
            digest();

            form = scope.loginForm;

        }));

        it('should have a null email.', () => {
            expect(scope.loginModel.email).toBeNull();
        });

        it('should have a null password.', () => {
            expect(scope.loginModel.password).toBeNull();
        });

        it('should have a false keepMeLoggedIn.', () => {
            expect(scope.loginModel.keepMeLoggedIn).toBeFalsy();
        });

        it('should be a pristine form.', () => {
            expect(form.$pristine).toBeTruthy();
        });

        it('should not be valid.', () => {
            expect(form.$valid).toBeFalsy();
        });

        it('should not have a valid emailOrUsername.', () => {
            expect(form.email.$valid).toBeFalsy();
        });

        it('should not have a valid password.', () => {
            expect(form.password.$valid).toBeFalsy();
        });

        it('should not have a valid rememberMe.', () => {
            expect(form.remember.$valid).toBeTruthy();
        });

        describe('emailOrUsername field', () => {
            beforeEach(() => {
                form.email.$setViewValue(email);
                digest();
            });

            it('should not be a valid form.', () => {
                expect(form.$valid).toBeFalsy();
            });

            it('should not be a pristine form.', () => {
                expect(form.$pristine).toBeFalsy();
            });

            it('should be a valid field.', () => {
                expect(form.email.$valid).toBeTruthy();
            });

            it('should have transmitted the value to scope.', () => {
                expect(scope.loginModel.email).toBe(email);
            });
        });

        describe('password field', () => {
            beforeEach(() => {
                form.password.$setViewValue(password);
                digest();
            });

            it('should not be a valid form.', () => {
                expect(form.$valid).toBeFalsy();
            });

            it('should not be a pristine form.', () => {
                expect(form.$pristine).toBeFalsy();
            });

            it('should be a valid field.', () => {
                expect(form.password.$valid).toBeTruthy();
            });

            it('should have transmitted the value to scope.', () => {
                expect(scope.loginModel.password).toBe(password);
            });
        });

        describe('remember field', () => {
            beforeEach(() => {
                form.remember.$setViewValue(keepMeLoggedIn);
                digest();
            });

            it('should not be a valid form.', () => {
                expect(form.$valid).toBeFalsy();
            });

            it('should not be a pristine form.', () => {
                expect(form.$pristine).toBeFalsy();
            });

            it('should be a valid field.', () => {
                expect(form.remember.$valid).toBeTruthy();
            });

            it('should have transmitted the value to scope.', () => {
                expect(scope.loginModel.keepMeLoggedIn).toBe(keepMeLoggedIn);
            });
        });

        describe('filled out form', () => {
            beforeEach(() => {
                form.email.$setViewValue(email);
                form.password.$setViewValue(password);
                form.remember.$setViewValue(keepMeLoggedIn);
                digest();
            });

            it('should be a valid form.', () => {
                expect(form.$valid).toBeTruthy();
            });
        });

        function digest() {
            scope.$digest();
        }

    });

} 