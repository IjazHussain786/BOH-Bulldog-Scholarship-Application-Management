module BohFoundation.ApplicationEvaluator.NavBar.Spec.Controllers {
    describe('EvaluateApplicantsNavBar', () => {

        var scope: NavBar.Controllers.IEvaluateApplicantsNavBarScope;
        var userInformationService;
        var usernameString = 'username';
        var $location;
        var adminNotificationService;
        var getAllApplicantsService;

        beforeEach(inject(($rootScope) => {
            scope = $rootScope;

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
                getNumberOfTotalNotifications: () => { },
                getNotifications: () => { },
                resolveNotifications: () => {}
            });

            getAllApplicantsService = sinon.stub({
                getNumberOfApplicantsNotYetRated: () => {}
            });

            new NavBar.Controllers.EvaluateApplicantsNavBarCtrl(scope, userInformationService, $location, adminNotificationService, getAllApplicantsService);
        }));

        describe('getNumberOfApplicantsNotYetRated', () => {
            it('should return whatever the service returns', () => {
                getAllApplicantsService.getNumberOfApplicantsNotYetRated.returns(10);
                expect(scope.getNumberOfApplicantsNotYetRated()).toBe(10);
            });
        });
        

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

        describe('isAdmin()', () => {

            it('should call isAdmin.', () => {
                isAdmin();
                expect(userInformationService.isAdmin.calledTwice).toBeTruthy();
            });

            it("should return false.", () => {
                userInformationService.isAdmin.returns(false);
                expect(isAdmin()).toBeFalsy();
            });

            it('should return true.', () => {
                userInformationService.isAdmin.returns(true);
                expect(isAdmin()).toBeTruthy();
            });

            function isAdmin() {
                return scope.isAdmin();
            }
        });

        describe('totalAdminNotifications', () => {
            var result;
            describe('not admin', () => {
                beforeEach(() => {
                    userInformationService.isAdmin.returns(false);
                    result = scope.totalAdminNotifications();
                });

                it('should return 0.', () => {
                    expect(result).toBe(0);
                });

                it('should not call getNumberOfTotalNotifications.', () => {
                    sinon.assert.notCalled(adminNotificationService.getNumberOfTotalNotifications);
                });
            });

            describe('is admin', () => {
                var returnValue = 193;
                
                beforeEach(() => {
                    userInformationService.isAdmin.returns(true);
                    adminNotificationService.getNumberOfTotalNotifications.returns(returnValue);
                    result = scope.totalAdminNotifications();
                });

                it('should return 0.', () => {
                    expect(result).toBe(returnValue);
                });

                it('should call getNumberOfTotalNotifications.', () => {
                    sinon.assert.calledOnce(adminNotificationService.getNumberOfTotalNotifications);
                });
            });
        });

        describe('on construction admin notifications', () => {
            describe('not admin', () => {
                beforeEach(() => {
                    userInformationService.isAdmin.returns(false);
                    new NavBar.Controllers.EvaluateApplicantsNavBarCtrl(scope, userInformationService, $location, adminNotificationService, getAllApplicantsService);
                });

                it('should not call getNotifications.', () => {
                    sinon.assert.notCalled(adminNotificationService.getNotifications);
                });
            });

            describe('is admin', () => {
                var q: ng.IQService, deferred, promise: ng.IPromise<any>, resource, returnedErrorData;
                var errorMessage = "errorMessage";
                var data = { j: 123, am: 923 };

                beforeEach(inject(($q) => {
                    q = $q;
                    deferred = q.defer();
                    promise = deferred.promise;
                    resource = { $promise: promise };
                    returnedErrorData = { data: { message: errorMessage } };
                    
                    adminNotificationService.getNotifications.returns(resource);
                    userInformationService.isAdmin.returns(true);
                    new NavBar.Controllers.EvaluateApplicantsNavBarCtrl(scope, userInformationService, $location, adminNotificationService, getAllApplicantsService);
                }));

                it('should call getNotifications.', () => {
                    sinon.assert.called(adminNotificationService.getNotifications);
                });

                it('should call resolve notifications with data and true success if it resolves.', () => {
                    deferred.resolve(data);
                    scope.$apply();
                    sinon.assert.calledWith(adminNotificationService.resolveNotifications, new Common.Models.ServerResponseModel(data, true));
                });


                it('should call resolve notifications with data and false success if it rejects.', () => {
                    deferred.reject();
                    scope.$apply();
                    sinon.assert.calledWith(adminNotificationService.resolveNotifications, new Common.Models.ServerResponseModel(null, false));
                });
            });
        });
    });
} 