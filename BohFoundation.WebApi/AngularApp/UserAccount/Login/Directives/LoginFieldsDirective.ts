module BohFoundation.Main {
    'use strict';
    Register.UserManagement.directive('loginFields', () => {
        return {
            restrict: 'E',
            templateUrl: '/AngularApp/UserAccount/Login/Templates/Directives/LoginFields.html'
        };
    });
} 