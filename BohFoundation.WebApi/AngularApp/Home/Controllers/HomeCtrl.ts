module BohFoundation.Home.Controllers {
    'use strict';

    export class HomeCtrl {

        static $inject = ['$scope'];
        constructor(public $scope) {
        }
    }
}

module BohFoundation.Main {
    Register.BohFoundation.controller("HomeCtrl", Home.Controllers.HomeCtrl).config(['$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
        $routeProvider.when('/', {
            templateUrl: '/AngularApp/Home/Templates/HomeTemplate.html',
            controller: 'HomeCtrl',
            title: 'Home',
            publicAccess: true,
            preLoad: true
        });
    })]);
}