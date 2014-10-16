module BohFoundation.ApplicationEvaluator.ManageAccount.Controllers {
    export class ManageAccountEvaluateApplicantsCtrl {

    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.controller('ManageAccountEvaluateApplicantsCtrl', ApplicationEvaluator.ManageAccount.Controllers.ManageAccountEvaluateApplicantsCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/ApplicationEvaluator/ManageAccount', {
                    templateUrl: '/AngularApp/ApplicationEvaluator/ManageAccount/Templates/ManageUser.html',
                    controller: 'ManageAccountEvaluateApplicantsCtrl',
                    title: 'Manage User'
                });
            })]);
} 