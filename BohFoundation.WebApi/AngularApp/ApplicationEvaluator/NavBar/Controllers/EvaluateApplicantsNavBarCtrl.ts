module BohFoundation.ApplicationEvaluator.NavBar.Controllers {
    export interface IEvaluateApplicantsNavBarCtrl {
        $scope: IEvaluateApplicantsNavBarScope;
    }

    export interface IEvaluateApplicantsNavBarScope extends ng.IScope {
        navCollapsed: boolean;

        clickToggleButton(): void;
        clickLinkButton(link: string): void;

        getFullName(): string;
        loggedIn(): boolean;
        logOut(): void;
        remembered(): boolean;
        isAdmin(): boolean;

        totalAdminNotifications(): number;

        getNumberOfApplicantsNotYetRated(): number;
    } 

    export class EvaluateApplicantsNavBarCtrl implements IEvaluateApplicantsNavBarCtrl {

        static $inject = ['$scope', 'UserInformationService', '$location', 'AdminNotificationService', 'GetAllApplicantsService'];

        constructor(
            public $scope: IEvaluateApplicantsNavBarScope,
            private userInformationService: Common.Services.UserInformation.IUserInformationService,
            private $location: ng.ILocationService,
            private adminNotificationService: Admin.Services.IAdminNotificationService,
            private getAllApplicantsService: ApplicationEvaluator.EvaluateApplicants.GetAllApplicants.Services.IGetAllApplicantsService) {

            this.getAdminNotifications();

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

            $scope.isAdmin = () => {
                return userInformationService.isAdmin();
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

            $scope.totalAdminNotifications = () => {
                return this.totalAdminNotifications();
            };

            $scope.getNumberOfApplicantsNotYetRated = () => {
                return this.getNumberOfApplicantsNotYetRated();
            };
        }

        private totalAdminNotifications(): number {
            if (!this.userInformationService.isAdmin()) {
                return 0;
            }
            return this.adminNotificationService.getNumberOfTotalNotifications();
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



        private getAdminNotifications() {
             if (this.userInformationService.isAdmin()) {
                 var resource = this.adminNotificationService.getNotifications();
                 resource.$promise.then(
                 (data) => {
                     this.resolveGetAdminNotification(data);
                 },
                 () => {
                     this.resolveGetAdminNotification(null);
                 }
                );
             }
        }

        private resolveGetAdminNotification(data) {
            var success = true;
            if (data == null) {
                success = false;
            }
            this.adminNotificationService.resolveNotifications(new Common.Models.ServerResponseModel(data, success));
        }

        private getNumberOfApplicantsNotYetRated(): number {
             return this.getAllApplicantsService.getNumberOfApplicantsNotYetRated();
        }
    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.controller("EvaluateApplicantsNavBarCtrl", ApplicationEvaluator.NavBar.Controllers.EvaluateApplicantsNavBarCtrl);
}