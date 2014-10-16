module BohFoundation.Main {
    'use strict';
    Register.ApplicationEvaluator.directive('displayAllFinalizedApplicants', () => {
        return {
            restrict: 'E',
            templateUrl: '/AngularApp/ApplicationEvaluator/EvaluateApplicants/GetAllApplicants/Templates/Directives/DisplayAllFinalizedApplicants.html',
            controller: 'DisplayAllFinalizedApplicantsDirectiveCtrl'
        };
    });
} 