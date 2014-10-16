module BohFoundation.Applicant.NavBar.Controllers {
    export interface IApplicantNavBarCtrl {
        $scope: IApplicantNavBarCtrlScope;
    }

    export interface IApplicantNavBarCtrlScope extends ng.IScope {
        navCollapsed: boolean;

        clickToggleButton(): void;
        clickLinkButton(link: string): void;

        getFullName(): string;
        loggedIn(): boolean;
        logOut(): void;
        remembered():boolean;
    }

    export class ApplicantNavBarCtrl implements IApplicantNavBarCtrl {

        static $inject = ['$scope','UserInformationService', '$location'];
        constructor(public $scope: IApplicantNavBarCtrlScope, private userInformationService: Common.Services.UserInformation.IUserInformationService,
            private $location: ng.ILocationService) {

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
    }
}

module BohFoundation.Main {
    Register.Applicant.controller('ApplicantNavBarCtrl', Applicant.NavBar.Controllers.ApplicantNavBarCtrl);
}