module BohFoundation.Main {
    'use strict';
    Register.Applicant.directive('applicantNavBar', () => {
        return {
            restrict: 'E',
            templateUrl: '/AngularApp/Applicant/NavBar/Templates/Directives/ApplicantNavBar.html',
            controller: 'ApplicantNavBarCtrl'
        };
    });
}