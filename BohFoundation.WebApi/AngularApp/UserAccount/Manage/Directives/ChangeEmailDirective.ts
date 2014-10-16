module BohFoundation.Main {
    'use strict';
    Register.UserManagement.directive('changeEmail', () => {
        return {
            restrict: 'E',
            templateUrl: '/AngularApp/UserAccount/Manage/Templates/Directives/ChangeEmail.html'
        };
    });
}