 module BohFoundation.Main {
     'use strict';

     export class Register {
         public static Common: ng.IModule = angular.module('BohFoundation.Common', ['ngRoute', 'LocalStorageModule', 'ngResource', 'toaster', 'ui.keypress', 'ui.bootstrap', 'ngAnimate', 'chieffancypants.loadingBar', 'textAngular', 'angular.css.injector']);
         public static Applicant: ng.IModule = angular.module('BohFoundation.Applicant', ['BohFoundation.Common']);
         public static ApplicationEvaluator: ng.IModule = angular.module('BohFoundation.ApplicationEvaluator', ['BohFoundation.Common']);
         public static UserManagement: ng.IModule = angular.module('BohFoundation.UserManagement', ['BohFoundation.Common']);
         public static Admin: ng.IModule = angular.module('BohFoundation.Admin', ['BohFoundation.Common']);
         public static Person: ng.IModule = angular.module('BohFoundation.Person', ['BohFoundation.Common']); 
         public static Reference: ng.IModule = angular.module('BohFoundation.Reference', ['BohFoundation.Common']);
         public static BohFoundation: ng.IModule = angular.module('BohFoundation', ['BohFoundation.Common', 'BohFoundation.UserManagement', 'BohFoundation.Applicant', 'BohFoundation.ApplicationEvaluator', 'BohFoundation.Admin', 'BohFoundation.Person', 'BohFoundation.Reference']);   
     }
 }