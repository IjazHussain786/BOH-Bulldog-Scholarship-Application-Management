 module BohFoundation.Admin.ConfirmPendingApplicationEvaluators.Controllers {
     'use strict';
     export interface IConfirmPendingApplicationEvaluatorsCtrlScope extends ng.IScope {
         pendingEvaluators: Array<Models.PendingApplicationEvaluatorModel>;

         confirmOrRejectEvaluator(index: number, confirm: boolean, createAdmin: boolean): void;
     }

     export interface IConfirmPendingApplicationEvaluatorsCtrl {
        $scope: IConfirmPendingApplicationEvaluatorsCtrlScope;
     }

     export class ConfirmPendingApplicationEvaluatorsCtrl implements IConfirmPendingApplicationEvaluatorsCtrl {
         static $inject = ['$scope', 'ConfirmPendingApplicationEvaluatorsService', 'AdminNotificationService'];
         constructor(public $scope: IConfirmPendingApplicationEvaluatorsCtrlScope,
             private confirmPendingApplicationEvaluatorsService: Services.IConfirmPendingApplicationEvaluatorsService,
             private adminNotificationService: Admin.Services.IAdminNotificationService) {

             this.getPendingEvaluators();

             $scope.confirmOrRejectEvaluator = (index: number, confirm: boolean, createAdmin: boolean) => {
                 this.confirmOrReject(index, confirm, createAdmin);
             };
         }

         private confirmOrReject(index: number, confirm: boolean, createAdmin: boolean) {
             var resource = this.confirmPendingApplicationEvaluatorsService.confirmOrRejectEvaluator(index, confirm, createAdmin);
             this.refreshPendingEvaluators();
             resource.$promise.then(
                 () => { this.successOnConfirmOrReject(); },
                 () => { this.confirmPendingApplicationEvaluatorsService.resolveConfirmOrRejectEvaluator(false); });
         }

         private successOnConfirmOrReject() {
             this.confirmPendingApplicationEvaluatorsService.resolveConfirmOrRejectEvaluator(true);
             var resource = this.adminNotificationService.getNotifications();
             var model;
             resource.$promise.then(
             (data) => {
                 model = new Common.Models.ServerResponseModel(data, true);
                 this.adminNotificationService.resolveNotifications(model);
             },
             (data) => {
                 model = new Common.Models.ServerResponseModel(data, false);
                 this.adminNotificationService.resolveNotifications(model);
             });
         }


         private getPendingEvaluators() {
             var resource = this.confirmPendingApplicationEvaluatorsService.getPendingApplicationEvaluators();
             resource.$promise.then(
                 (data) => {
                     this.resolveGetPendingApplicationEvaluators(new Common.Models.ServerResponseModel(data.data, true));
                     this.refreshPendingEvaluators();
                 },
                 () => {
                      this.resolveGetPendingApplicationEvaluators(new Common.Models.ServerResponseModel(null, false));
                 });
         }

         private resolveGetPendingApplicationEvaluators(serverResponseModel: Common.Models.ServerResponseModel): void {
             this.confirmPendingApplicationEvaluatorsService.resolveGetPendingApplicationEvaluators(serverResponseModel);
         }

         private refreshPendingEvaluators() {
             this.$scope.pendingEvaluators = this.confirmPendingApplicationEvaluatorsService.listOfPendingApplicationEvaluators;
         }
     }
 }

module BohFoundation.Main {
    Register.Admin.controller("ConfirmPendingApplicationEvaluatorsCtrl", Admin.ConfirmPendingApplicationEvaluators.Controllers.ConfirmPendingApplicationEvaluatorsCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/Admin/ConfirmPendingApplicationEvaluators', {
                    templateUrl: '/AngularApp/Admin/ConfirmPendingApplicationEvaluators/Templates/ConfirmPendingApplicationEvaluators.html',
                    controller: 'ConfirmPendingApplicationEvaluatorsCtrl',
                    publicAccess: false,
                    title: 'Confirm Pending Application Evaluators'
                });
            })]);
}