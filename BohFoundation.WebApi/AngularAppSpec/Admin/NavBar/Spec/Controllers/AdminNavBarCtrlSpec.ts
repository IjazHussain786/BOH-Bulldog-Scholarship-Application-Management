module BohFoundation.Admin.NavBar.Spec.Controllers {
    describe('EvaluateApplicantsNavBar', () => {

        var scope;
        var userInformationService;
        var usernameString = 'username';
        var $location;
        var adminNotificationService;
        var q: ng.IQService;

        var deferred, promise: ng.IPromise<any>, resource, returnedErrorData;
        var errorMessage = "errorMessage";
        var data = { j: 123, am: 923 };

        beforeEach(inject(($rootScope, $q) => {
            scope = $rootScope;

            q = $q;
            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };
            returnedErrorData = { data: { message: errorMessage } };

            userInformationService = sinon.stub({
                logOut: () => { },
                getFullName: () => { },
                getLoggedIn: () => { },
                getRemembered: () => { },
                isAdmin: () => { }
            });

            $location = sinon.stub({
                path: () => { }
            });

            adminNotificationService = sinon.stub({
                getNumberOfTotalNotifications:() =>{ }, 
                getNotifications: () => { },
                resolveNotifications: () => { }
            });

            adminNotificationService.getNotifications.returns(resource);
            new Admin.NavBar.Controllers.AdminNavBarCtrl(scope, userInformationService, $location, adminNotificationService);
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

        describe('construction', () => {
            beforeEach(() => {
                new Admin.NavBar.Controllers.AdminNavBarCtrl(scope, userInformationService, $location, adminNotificationService);
            });

            it('should call getNotifications.', () => {
                sinon.assert.called(adminNotificationService.getNotifications);
            });

            it('should call resolve notifications with data and true success if it resolves.', () => {
                deferred.resolve(data);
                scope.$apply();
                sinon.assert.calledWith(adminNotificationService.resolveNotifications, new Common.Models.ServerResponseModel(data, true));
            });


            it('should call resolve notifications with data and false success if it rejects.', () => {
                deferred.reject(errorMessage);
                scope.$apply();
                sinon.assert.calledWith(adminNotificationService.resolveNotifications, new Common.Models.ServerResponseModel(errorMessage, false));
            });
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
                expect(userInformationService.logOut.calledOnce).toBeTruthy();
            });

            it('should call location with "/".', () => {
                scope.logOut();
                expect($location.path.calledWith('/')).toBeTruthy();
            });

            it('should call logOUt on userInfoService when logOut is called with "".', () => {
                scope.logOut();
                sinon.assert.calledWith(userInformationService.logOut, "");
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

        describe('getTotalNotifications', () => {
            var result;
            var notifications = 193;

            beforeEach(() => {
                adminNotificationService.getNumberOfTotalNotifications.returns(notifications);
                result = scope.getTotalNotifications();
            });

            it('should return what getNumberOfTotalNotifications returns.', () => {
                expect(result).toBe(notifications);
            });

            it('should call getNumberOfTotalNotifications.', () => {
                sinon.assert.calledOnce(adminNotificationService.getNumberOfTotalNotifications);
            });
        });
    });

}  