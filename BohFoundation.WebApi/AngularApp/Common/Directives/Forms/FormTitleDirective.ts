module BohFoundation.Main {
    Register.Common.directive('formTitle', () => {
         return {
            restrict: 'E',
            templateUrl: '/AngularApp/Common/Templates/Directives/Forms/FormTitleTemplate.html',
            scope: {
                title: '@title'
            }
        }
     });
}