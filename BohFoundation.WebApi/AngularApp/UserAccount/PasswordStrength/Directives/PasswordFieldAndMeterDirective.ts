module BohFoundation.Main {
    'use strict';
    Register.UserManagement.directive('passwordFieldAndMeter', () => {
        return {
            restrict: 'E',
            templateUrl: '/AngularApp/UserAccount/PasswordStrength/Templates/Directives/PasswordFieldAndMeter.html'
        };
    });
} 