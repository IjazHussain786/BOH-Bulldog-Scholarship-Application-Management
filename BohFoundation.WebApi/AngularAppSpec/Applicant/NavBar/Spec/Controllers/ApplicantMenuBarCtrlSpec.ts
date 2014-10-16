module BohFoundation.Applicant.NavBar.Spec.Controllers {
    describe('ApplicantNavBarCtrl', () => {

        var scope: Applicant.NavBar.Controllers.IApplicantNavBarCtrlScope;
        var userInformationService;
        var usernameString = 'username';
        var $location;

        beforeEach(inject(($rootScope) => {
            scope = $rootScope;

            userInformationService = sinon.stub({
                logOut: () => {},
                getFullName: () => { },
                getLoggedIn: () => { },
                getRemembered: () => { }
            });

            $location = sinon.stub({
                path: () => { }
            });

            new NavBar.Controllers.ApplicantNavBarCtrl(scope, userInformationService, $location);
        }));

        it("should have navCollapsed as true by default.", () => {
            expect(scope.navCollapsed).toBeTruthy();
        });

        it("should switch bools when clickToggleButton() is triggered.", () => {
            expect(scope.navCollapsed).toBeTruthy();
            scope.clickToggleButton();
            expect(scope.navCollapsed).toBeFalsy();
            scope.clickToggleButton();
            expect(scope.navCollapsed).toBeTruthy();
        });

        it("should switch bools when clickLinkButton() is triggered when navCollapsed = false.", () => {
            expect(scope.navCollapsed).toBeTruthy();
            scope.navCollapsed = false;
            expect(scope.navCollapsed).toBeFalsy();
            scope.clickLinkButton(null);
            expect(scope.navCollapsed).toBeTruthy();
        });

        it("should not switch bools when clickLinkButton() is triggered when navCollapsed = true", () => {
            expect(scope.navCollapsed).toBeTruthy();
            scope.clickLinkButton(null);
            expect(scope.navCollapsed).toBeTruthy();
        });

        describe('getFullName()', () => {
            it('should call getUserName when userName() is called when userInformation.getLoggedIn comes back positive.', () => {
                expect(userInformationService.getFullName.calledOnce).toBeFalsy();
                getFullName();
                expect(userInformationService.getFullName.calledOnce).toBeTruthy();
            });

            it('should return some username.', () => {
                userInformationService.getFullName.returns(usernameString);
                expect(getFullName()).toBe(usernameString);
            });

            function getFullName() {
                return scope.getFullName();
            }
        });

        it('should getLoggedIn when Loggedin is called.', () => {
            expect(userInformationService.getLoggedIn.calledOnce).toBeFalsy();
            scope.loggedIn();
            expect(userInformationService.getLoggedIn.calledOnce).toBeTruthy();
        });

        describe('logout', () => {
            it('should call logOUt on userInfoService when logOut is called.', () => {
                expect(userInformationService.logOut.calledOnce).toBeFalsy();
                scope.logOut();
                sinon.assert.calledWith(userInformationService.logOut, "");
            });

            it('should call location with "/".', () => {
                scope.logOut();
                expect($location.path.calledWith('/')).toBeTruthy();
            });

        });

        describe('clickLinkButton', () => {
            it('should clickLinkButton when LogOut is called.', () => {
                scope.navCollapsed = false;
                scope.logOut();
                expect(scope.navCollapsed).toBeTruthy();
            });

            it('should not change navCollapsed if it is already truthy.', () => {
                scope.navCollapsed = true;
                scope.logOut();
                expect(scope.navCollapsed).toBeTruthy();
            });

            it('should push the link string to the $location service. If one is passed in.', () => {
                var link = "link";
                scope.clickLinkButton(link);
                expect($location.path.calledWith(link)).toBeTruthy();
            });

            it('should not call $location if passed a null.', () => {
                var link = null;
                scope.clickLinkButton(link);
                expect($location.path.called).toBeFalsy();
            });
        });

        describe('remembered()', () => {
            it('should call remembered() when userName() is called when userInformation.getLoggedIn comes back positive.', () => {
                expect(userInformationService.getRemembered.calledOnce).toBeFalsy();
                remembered();
                expect(userInformationService.getRemembered.calledOnce).toBeTruthy();
            });

            it("should return false if loggedIn is false.", () => {
                userInformationService.getLoggedIn.returns(false);
                expect(remembered()).toBeFalsy();
            });

            it('should return true when getLoggedInIsTrue if the user is remembered.', () => {
                userInformationService.getRemembered.returns(true);
                expect(remembered()).toBeTruthy();
            });

            function remembered() {
                return scope.remembered();
            }
        });

    });

}