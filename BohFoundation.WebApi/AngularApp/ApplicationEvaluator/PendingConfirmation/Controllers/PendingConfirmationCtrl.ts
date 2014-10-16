module BohFoundation.ApplicationEvaluator.PendingConfirmation.Controllers {
    export class PendingConfirmationCtrl {
        static $inject = ['$scope'];
        constructor(public $scope) { }
    }
}


module BohFoundation.Main {
    Register.ApplicationEvaluator.controller('PendingConfirmationCtrl', ApplicationEvaluator.PendingConfirmation.Controllers.PendingConfirmationCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/ApplicationEvaluator/PendingConfirmation', {
                    templateUrl: '/AngularApp/ApplicationEvaluator/PendingConfirmation/Templates/PendingConfirmation.html',
                    controller: 'PendingConfirmationCtrl',
                    publicAccess: true,
                    title: 'Pending Confirmation'
                });
            })]);
}