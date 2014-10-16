 module BohFoundation.Main {
     Register.BohFoundation.config(['$routeProvider', '$locationProvider', (($routeProvider, $locationProvider) => {
         $locationProvider.html5Mode(true);
         $routeProvider.otherwise({ redirectTo: '/' });
     })]);

     Register.BohFoundation.config([
         'cssInjectorProvider', ((cssInjectorProvider) => {
             cssInjectorProvider.setSinglePageMode(true);
         })
     ]);
 }