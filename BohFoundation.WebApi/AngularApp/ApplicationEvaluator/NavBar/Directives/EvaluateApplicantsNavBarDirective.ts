module BohFoundation.Main {
    'use strict';
    Register.UserManagement.directive('evaluateApplicantsNavBar', () => {
        return {
            restrict: 'E',
            templateUrl: '/AngularApp/ApplicationEvaluator/NavBar/Templates/Directives/EvaluateApplicantsNavBar.html',
            controller: 'EvaluateApplicantsNavBarCtrl'
        };
    });
} 