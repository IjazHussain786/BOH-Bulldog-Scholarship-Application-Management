module BohFoundation.Main {
    Register.Applicant.directive('applicantDashboardListItem', () => {
        return {
            restrict: 'E',
            templateUrl: '/AngularApp/Applicant/Dashboard/Templates/Directives/DashboardListItem.html',
            scope: {
                dashboardListItem:'=dashboardItem'
            }
        }; 
    });
}