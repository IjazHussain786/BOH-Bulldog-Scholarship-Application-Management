module BohFoundation.Applicant.ManageAccount.Controllers {
    export class ManageAccountApplicantCtrl {
        
    }
}

module BohFoundation.Main {
    Register.Applicant.controller('ManageAccountApplicantCtrl', Applicant.ManageAccount.Controllers.ManageAccountApplicantCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/Applicant/ManageAccount', {
                    templateUrl: '/AngularApp/Applicant/ManageAccount/Templates/ManageUser.html',
                    controller: 'ManageAccountApplicantCtrl',
                    title: 'Manage User'
                });
            })]);
}