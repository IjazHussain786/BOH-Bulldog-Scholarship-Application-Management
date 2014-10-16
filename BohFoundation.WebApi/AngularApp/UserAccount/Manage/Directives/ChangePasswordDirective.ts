module BohFoundation.Main {
    'use strict';
    Register.UserManagement.directive('changePassword', () => {
        return {
            restrict: 'E',
            templateUrl: '/AngularApp/UserAccount/Manage/Templates/Directives/ChangePassword.html'
        };
    });
}