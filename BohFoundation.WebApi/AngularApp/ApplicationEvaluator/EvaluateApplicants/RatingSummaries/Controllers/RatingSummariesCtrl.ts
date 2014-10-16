module BohFoundation.ApplicationEvaluator.EvaluateApplicants.RatingSummaries.Controllers {
    export interface IRatingSummariesScope extends ng.IScope{
        getTop5Applicants(): Array<Dtos.ApplicationEvaluator.RatingSummary.TopApplicantRatingSummaryModel>;

        gotoApplicant(index:number): void;
    }

    export interface IRatingSummaries {
        $scope: IRatingSummariesScope;
    }

    export class RatingSummariesCtrl implements IRatingSummaries{
        static $inject = ['$scope', "RatingSummariesService"];
        constructor(
            public $scope: IRatingSummariesScope,
            private ratingSummariesService: Services.IRatingSummariesService) {

            this.initialize();

            $scope.getTop5Applicants = () => {
                return this.ratingSummariesService.getTop5Applicants();
            };

            $scope.gotoApplicant = (index: number) => {
                this.ratingSummariesService.gotoApplicant(index);
            };
        }

        private initialize() {
            var resource = this.ratingSummariesService.getRatingSummariesFromServer();
            resource.$promise.then(data => {
                 this.resolveGet(data);
            }, () => { this.resolveGet(null); });
        }

        private resolveGet(data) {
            var success = true;
            if (data == null) {
                success = false;
            }
            this.ratingSummariesService.resolveGetRatingSummariesFromServer(new Common.Models.ServerResponseModel(data, success));
        }
    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.controller("RatingSummariesCtrl", ApplicationEvaluator.EvaluateApplicants.RatingSummaries.Controllers.RatingSummariesCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/ApplicationEvaluator/RatingsSummaries', {
                    templateUrl: '/AngularApp/ApplicationEvaluator/EvaluateApplicants/RatingSummaries/Templates/RatingSummaries.html',
                    controller: 'RatingSummariesCtrl',
                    publicAccess: false,
                    title: 'Rating Summaries'
                });
            })]);
}