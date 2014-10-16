module BohFoundation.Main {
    Register.Common.directive('lastUpdatedStaticField', () => {
         return {
            restrict: 'E',
             templateUrl: '/AngularApp/Common/Templates/Directives/Forms/LastUpdatedStaticFieldTemplate.html',
            scope: {
                updated: '=lastUpdated'
            }
        }
     });
}