module BohFoundation.ApplicationEvaluator.Dashboard.Controllers {
    export interface IEvaluateApplicantsDashboardCtrl {
        $scope: IEvaluateApplicantsDashboardCtrlScope;
    }

    export interface IEvaluateApplicantsDashboardCtrlScope {
        
    }

    export class EvaluateApplicantsDashboardCtrl implements IEvaluateApplicantsDashboardCtrl {
        static $inject = ['$scope'];
        constructor(
            public $scope: IEvaluateApplicantsDashboardCtrlScope) { }
    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.controller('EvaluateApplicantsDashboardCtrl', ApplicationEvaluator.Dashboard.Controllers.EvaluateApplicantsDashboardCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/ApplicationEvaluator', {
                    templateUrl: '/AngularApp/ApplicationEvaluator/Dashboard/Templates/EvaluateApplicantsDashboard.html',
                    controller: 'EvaluateApplicantsDashboardCtrl',
                    publicAccess: false,
                    title: 'Application Evaluation Dashboard'
                });
            })]);
}