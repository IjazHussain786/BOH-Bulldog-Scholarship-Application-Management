 module BohFoundation.Admin.NavBar.Controllers {
     export interface IAdminNavBarCtrlScope extends ng.IScope {
         navCollapsed: boolean;

         clickToggleButton(): void;
         clickLinkButton(link: string): void;

         getFullName(): string;
         loggedIn(): boolean;
         logOut(): void;
         remembered(): boolean;

         getTotalNotifications(): number;
     }

     export interface IAdminNavBarCtrl {
         $scope:IAdminNavBarCtrlScope;
     }

     export class AdminNavBarCtrl implements IAdminNavBarCtrl {
         static $inject = ['$scope', 'UserInformationService', '$location', 'AdminNotificationService'];

         constructor(public $scope: IAdminNavBarCtrlScope,
             private userInformationService: Common.Services.UserInformation.IUserInformationService,
             private $location: ng.ILocationService,
             private adminNotificationService: Admin.Services.IAdminNotificationService) {

             this.getNotifications();

             $scope.navCollapsed = true;

             $scope.remembered = () => {
                 return userInformationService.getRemembered();
             };

             $scope.loggedIn = () => {
                 return userInformationService.getLoggedIn();
             };

             $scope.getFullName = () => {
                 return userInformationService.getFullName();
             };

             $scope.clickToggleButton = () => {
                 this.changeNavCollapsedBoolean();
             };

             $scope.clickLinkButton = (link: string) => {
                 this.clickLinkButton(link);
             };

             $scope.logOut = () => {
                 this.userInformationService.logOut("");
                 this.clickLinkButton('/');
             };

             $scope.getTotalNotifications = () => {
                 return adminNotificationService.getNumberOfTotalNotifications();
             };
         }

         private changeNavCollapsedBoolean() {
             this.$scope.navCollapsed = !this.$scope.navCollapsed;
         }

         private clickLinkButton(link: string) {
             if (this.$scope.navCollapsed != true) {
                 this.changeNavCollapsedBoolean();
             }
             this.gotoLink(link);
         }

         private gotoLink(link: string) {
             if (link != null) {
                 this.$location.path(link);
             }
         }

         private getNotifications() {
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
     }
 }

module BohFoundation.Main {
    Register.Admin.controller("AdminNavBarCtrl", Admin.NavBar.Controllers.AdminNavBarCtrl);
}