module BohFoundation.Main {
    'use strict';
    Register.Admin.directive('adminNavBar', () => {
        return {
            restrict: 'E',
            templateUrl: '/AngularApp/Admin/NavBar/Templates/Directives/AdminNavBar.html',
            controller: 'AdminNavBarCtrl'
        };
    });
} 