module BohFoundation.Applicant.Finalized.Controllers {
    export interface IFinalizedCtrl {
        $scope: IFinalizedCtrlScope
    }


    export interface IFinalizedCtrlScope extends ng.IScope {
        
    }

    export class FinalizedCtrl implements IFinalizedCtrl {
        
        static $inject = ['$scope'];

        constructor(
            public $scope: IFinalizedCtrlScope) {
        }

    }
    
} 

module BohFoundation.Main {
    Register.Applicant.controller("FinalizedCtrl", Applicant.Finalized.Controllers.FinalizedCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/Applicant/Finalized', {
                    templateUrl: '/AngularApp/Applicant/Finalized/Templates/FinalizedTemplate.html',
                    controller: 'FinalizedCtrl',
                    publicAccess: true,
                    title: 'Finalized'
                });
            })]);
}