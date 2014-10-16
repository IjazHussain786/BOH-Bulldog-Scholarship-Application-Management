module BohFoundation.Admin.Dashboard.Controllers {
    'use strict';
    export interface IAdminDashboardCtrlScope extends ng.IScope {
        clickLinkButton(link: string): void;
        getNumberOfPendingApplicationEvaluators():number;
    }

    export interface IAdminDashboardCtrl {
        $scope: IAdminDashboardCtrlScope; 
    }

    export class AdminDashboardCtrl implements IAdminDashboardCtrl {
        static $inject = ['$scope', '$location', 'AdminNotificationService'];
        constructor(public $scope: IAdminDashboardCtrlScope,
            private $location: ng.ILocationService,
            private adminNotificationService: Admin.Services.IAdminNotificationService) {

            $scope.clickLinkButton = (link: string) => {
                this.$location.path(link);
            };

            $scope.getNumberOfPendingApplicationEvaluators = () => {
                return adminNotificationService.getNumberOfPendingApplicationEvaluators();
            };
        }
    }
}

module BohFoundation.Main {
    Register.Admin.controller("AdminDashboardCtrl", Admin.Dashboard.Controllers.AdminDashboardCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/Admin', {
                    templateUrl: '/AngularApp/Admin/Dashboard/Templates/AdminDashboard.html',
                    controller: 'AdminDashboardCtrl',
                    publicAccess: false,
                    title: 'Admin Dashboard'
                });
            })]);
}