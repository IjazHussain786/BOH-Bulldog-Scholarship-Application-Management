module BohFoundation.Main {
    'use strict';
    Register.UserManagement.directive('loginScreen', () => {
        return {
            restrict: 'E',
            templateUrl: '/AngularApp/UserAccount/Login/Templates/Directives/LoginScreen.html',
            controller: 'LoginCtrl'
        };
    });
}