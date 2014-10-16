module BohFoundation.Main {
    'use strict';
    Register.UserManagement.directive('manageUser', () => {
        return {
            restrict: 'E',
            templateUrl: '/AngularApp/UserAccount/Manage/Templates/Directives/ManageUser.html',
            controller: "ManageUserCtrl"
        };
    });
} 