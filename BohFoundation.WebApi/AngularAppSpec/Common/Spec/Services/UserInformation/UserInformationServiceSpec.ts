module BohFoundation.Common.Spec.Services.UserInformation {

    describe('UserInformationService', () => {

        var userInformationService: Common.Services.UserInformation.IUserInformationService;
        var userInformationToken: Models.UserInformationModel;
        var location;
        var accessToken = "asdfjkna983nasjf0";
        var email = "email@email.com";
        var firstName = "firstName";
        var lastName = "lastName";
        var sessionCookieService, localStorage;
        var rememberedOnDevice = true;
        var guid = 'guid';
        var date = new Date();
        var dateInFuture = new Date(date.getTime() + 600000);
        var dateInPast = new Date(date.getTime() - 600000);

        beforeEach(() => {
            sessionCookieService = sinon.stub({
                getUserInformationFromCookie: () => { },
                setCookie: () => { },
                deleteSessionCookie: ()=>{}
            });

            localStorage = sinon.stub({
                get: () => { },
                add: () => { },
                remove: () => { }
            });

            location = sinon.stub({
                path: ()=>{}
            });
            
            userInformationToken = new Common.Models.UserInformationModel(accessToken, email, firstName, lastName, rememberedOnDevice, guid, dateInFuture, true, false, false, false);
        });

        it('should have loggedIn defaulted to false.', () => {
            setUpTests(false);
            expect(userInformationService.getLoggedIn()).toBeFalsy();
        });

        it('should lastUrlVisitedBesidesLogin default to /.', ()=> {
            setUpTests(false);
            expect(userInformationService.lastUrlVisitedBesidesLogin).toBe('/');
        });

        describe('getUserInformationCookie returns token', ()=> {
            beforeEach(()=> {
                setUpTests(true);
            });

            it('should call sessionCookieService when constructed.', () => {
                expect(sessionCookieService.getUserInformationFromCookie.calledOnce).toBeTruthy();
            });

            it('should set loggedIn to true is sessionCookie is not null.', () => {
                expect(userInformationService.getLoggedIn()).toBeTruthy();
            });

            it('should set the field sessionCookie to the cookie.', () => {
                expect(userInformationService.userInfo).toBe(userInformationToken);
            });
        });

        describe('getLocalInfoFromLocalStorage returns token', ()=> {
            beforeEach(() => {
                expect(sessionCookieService.setCookie.calledWithExactly(userInformationToken)).toBeFalsy();
                expect(userInformationService).toBeNull();
                setUpTests(false, true);
            });

            it('should call createSessionCookie.', ()=> {
                expect(sessionCookieService.setCookie.calledWithExactly(userInformationToken)).toBeTruthy();
                expect(userInformationService.userInfo).toBe(userInformationToken);
                expect(userInformationService.getLoggedIn()).toBeTruthy();
            });
        });

        describe('getLocalInfoFromLocalStorage returns null.', ()=> {
            beforeEach(()=> {
                expect(userInformationService).toBeNull();
                setUpTests(false, false);
            });

            it('should maintain a false login', ()=> {
                expect(userInformationService.getLoggedIn()).toBeFalsy();
            });

            it('should maintain a undefined userInfo', () => {
                expect(userInformationService.userInfo).toBe(undefined);
            });
        });

        describe('non-constructor functions', () => {
            beforeEach(() => {
                expect(userInformationService).toBeNull();
                setUpTests(false, false);
            });

            describe('createSessionCookie', ()=> {
                it('should call sessionCookie Service with the object.', () => {
                    expect(sessionCookieService.setCookie.calledWithExactly(userInformationToken)).toBeFalsy();
                    createSession();
                    expect(sessionCookieService.setCookie.calledWithExactly(userInformationToken)).toBeTruthy();
                });

                it('should set the session cookie property to the input.', () => {
                    expect(userInformationService.userInfo).toBe(undefined);
                    createSession();
                    expect(userInformationService.userInfo).toBe(userInformationToken);
                });

                it('should set logged in to true.', () => {
                    expect(userInformationService.getLoggedIn()).toBeFalsy();
                    createSession();
                    expect(userInformationService.getLoggedIn()).toBeTruthy();
                });

                it('should save the toke to local if rememberMe true.', ()=> {
                    expect(localStorage.add.calledWithExactly(userInformationToken)).toBeFalsy();
                    createSession();
                    expect(localStorage.add.calledWithExactly(userInformationToken)).toBeTruthy();
                });

                it('should not save the token to loacl if rememberMe is false.', ()=> {
                    expect(localStorage.add.calledWithExactly(userInformationToken)).toBeFalsy();
                    userInformationToken.rememberedOnDevice = false;
                    createSession();
                    expect(localStorage.add.calledWithExactly(userInformationToken)).toBeFalsy();
                });

                function createSession() {
                    userInformationService.persistLogin(userInformationToken);
                }
            });

            describe("logOut",()=> {
                it('should call the delete function on the cookie service.', ()=> {
                    expect(sessionCookieService.deleteSessionCookie.calledOnce).toBeFalsy();
                    logOut();
                    expect(sessionCookieService.deleteSessionCookie.calledOnce).toBeTruthy();
                });

                it('should set the flag to not logged in.', () => {
                    userInformationService.loggedIn = true;
                    userInformationService.userInfo = userInformationToken;
                    expect(userInformationService.getLoggedIn()).toBeTruthy();
                    logOut();
                    expect(userInformationService.getLoggedIn()).toBeFalsy();
                });

                it('should set userInfo to null.', ()=> {
                    userInformationService.userInfo = userInformationToken;
                    expect(userInformationService.userInfo).toBe(userInformationToken);
                    logOut();
                    expect(userInformationService.userInfo).toBeNull();
                });

                it('should call location to the root path.', () => {
                    sinon.assert.notCalled(location.path);
                    logOut();
                    sinon.assert.calledWith(location.path, "");
                });


                it('should call location with whatever is passed in.', () => {
                    var url = "abot";
                    userInformationService.logOut(url);
                    sinon.assert.calledWith(location.path, url);
                });

                function logOut() {
                    userInformationService.logOut("");
                }

            });

            describe('removeTokenFromLocalStorage', ()=> {
                beforeEach(() => {
                    userInformationService.loggedIn = true;
                    userInformationService.userInfo = userInformationToken;
                });

                it('should call the local store to delete.', () => {
                    expect(localStorage.remove.calledOnce).toBeFalsy();
                    userInformationService.removeTokenFromLocalStorage();
                    expect(localStorage.remove.calledOnce).toBeTruthy();
                });

                it('should set userInfo.rememberedOnDevice to false.', () => {
                    expect(userInformationService.getRemembered()).toBeTruthy();
                    userInformationService.removeTokenFromLocalStorage();
                    expect(userInformationService.getRemembered()).toBeFalsy();
                });

                it('should reset the cookie with this new information.', () => {
                    userInformationService.removeTokenFromLocalStorage();
                    sinon.assert.calledOnce(sessionCookieService.setCookie);
                });

                it('should not call userInformationLocalStore.', () => {
                    userInformationService.removeTokenFromLocalStorage();
                    sinon.assert.notCalled(localStorage.add);
                });
            });

            describe('getLoggedIn', () => {
                beforeEach(()=> {
                    userInformationService.loggedIn = true;
                });

                it('getLoggedIn should return true.', () => {
                    userInformationService.userInfo = userInformationToken;
                    var result = userInformationService.getLoggedIn();
                    expect(result).toBeTruthy();
                });

                describe('token has expired', () => {
                    beforeEach(()=> {
                        userInformationToken.expireDate = dateInPast;
                        userInformationService.userInfo = userInformationToken;
                    });

                    it('should log the user out if the token has expired.', () => {
                        var result = userInformationService.getLoggedIn();
                        expect(result).toBeFalsy();
                    });

                    it('should call delete session cookie.', () => {
                        expect(sessionCookieService.deleteSessionCookie.calledOnce).toBeFalsy();
                        userInformationService.getLoggedIn();
                        expect(sessionCookieService.deleteSessionCookie.calledOnce).toBeTruthy();
                    });

                    it('should call remove token from local.', ()=> {
                        expect(localStorage.remove.calledOnce).toBeFalsy();
                        userInformationService.getLoggedIn();
                        expect(localStorage.remove.calledOnce).toBeTruthy();
                    });
                });


            });

            describe('getProperty Methods', ()=> {
                beforeEach(()=> {
                    userInformationService.userInfo = userInformationToken;
                    userInformationService.loggedIn = true;
                });

                it('getGraduatingYear should return graduating year.', () => {
                    userInformationService.userInfo.graduatingYear = 13;
                    expect(userInformationService.getGraduatingYear()).toBe(13);
                });

                it('getUserEmail should return email address.', ()=> {
                    var result = userInformationService.getUserEmail();
                    expect(result).toBe(email);
                });

                it('getUserAuthHeader should return a header formated token.', ()=> {
                    var result = userInformationService.getUserAuthHeader();
                    expect(result).toBe("Bearer " + accessToken);
                });
                
                it('getRemembered should return true.', () => {
                    var result = userInformationService.getRemembered();
                    expect(result).toBeTruthy();
                });

                it('getUserGuid should return guid.', ()=> {
                    var result = userInformationService.getUserGuid();
                    expect(result).toBe(guid);
                });

                it('getFirstName should return firstName.', () => {
                    var result = userInformationService.getFirstName();
                    expect(result).toBe(firstName);
                });

                it('getLastName should return lastName.', () => {
                    var result = userInformationService.getLastName();
                    expect(result).toBe(lastName);
                });

                it('getFullName should return firstname + " " + lastName.', () => {
                    var result = userInformationService.getFullName();
                    expect(result).toBe(firstName + " " + lastName);
                });

                it('getLastNameFirst should return lastname, " " firstname', () => {
                    var result = userInformationService.getLastnameFirst();
                    expect(result).toBe(lastName + ", " + firstName);
                });

                it('isApplicant should return value off userInfo.', () => {
                    userInformationService.userInfo.applicant = true;
                    var result = userInformationService.isApplicant();
                    expect(result).toBeTruthy();
                });

                it('isAdmin should return value off userInfo.', () => {
                    userInformationService.userInfo.admin = true;
                    var result = userInformationService.isAdmin();
                    expect(result).toBeTruthy();
                });

                it('isReference should return value off userInfo.', () => {
                    userInformationService.userInfo.reference = true;
                    var result = userInformationService.isReference();
                    expect(result).toBeTruthy();
                });

                it('isApplicationEvaluator should return value off userInfo.', () => {
                    userInformationService.userInfo.applicationEvaluator = true;
                    var result = userInformationService.isApplicationEvaluator();
                    expect(result).toBeTruthy();
                });

                describe('should try to return valid user info if user isnt logged in.', ()=> {
                    beforeEach(()=> {
                        userInformationService.loggedIn = false;
                    });

                    it('getRemembered should return false if loggedIn is false.', () => {
                        var result = userInformationService.getRemembered();
                        expect(result).toBeFalsy();
                    });

                    it('getUserEmail should return the noEmail.', () => {
                        var result = userInformationService.getUserEmail();
                        expect(result).toBe('noEmail');
                    });

                    it('getUserAuthHeader should return the noAuthHeader.', ()=> {
                        var result = userInformationService.getUserAuthHeader();
                        expect(result).toBe('noAuthHeader');
                    });

                    it('getUserGuid should return noGuid when not logged in.', ()=> {
                        var result = userInformationService.getUserGuid();
                        expect(result).toBe('00000000-0000-0000-0000-000000000000');
                    });

                    it('getLastnameFirst should return noLastnameFirst when not logged in.', () => {
                        var result = userInformationService.getLastnameFirst();
                        expect(result).toBe('noLastnameFirst');
                    });

                    it('getFirstName should return noFirstName when not logged in.', () => {
                        var result = userInformationService.getFirstName();
                        expect(result).toBe('noFirstName');
                    });

                    it('getLastName should return noLastName when not logged in.', () => {
                        var result = userInformationService.getLastName();
                        expect(result).toBe('noLastName');
                    });

                    it('getFullName should return noFullName when not logged in.', () => {
                        var result = userInformationService.getFullName();
                        expect(result).toBe('noFullName');
                    });

                    it('isApplicant should return false if not logged in.', () => {
                        userInformationService.userInfo.applicant = true;
                        var result = userInformationService.isApplicant();
                        expect(result).toBeFalsy();
                    });

                    it('isAdmin should return false if not logged in.', () => {
                        userInformationService.userInfo.admin = true;
                        var result = userInformationService.isAdmin();
                        expect(result).toBeFalsy();
                    });

                    it('isReference should return false if not logged in.', () => {
                        userInformationService.userInfo.reference = true;
                        var result = userInformationService.isReference();
                        expect(result).toBeFalsy();
                    });

                    it('isApplicationEvaluator should return false if not logged in.', () => {
                        userInformationService.userInfo.applicationEvaluator = true;
                        var result = userInformationService.isApplicationEvaluator();
                        expect(result).toBeFalsy();
                    });


                    it('isApplicationEvaluator should return false if not logged in.', () => {
                        userInformationService.userInfo.graduatingYear = 1345;
                        expect(userInformationService.getGraduatingYear()).toBe(0);
                    });
                });
            });
        });

        function setUpTests(sessionCookieComesBackGood: boolean, localStorageComesUpGood?:boolean) {
            if (sessionCookieComesBackGood) {
                sessionCookieService.getUserInformationFromCookie.returns(userInformationToken);
            } else {
                sessionCookieService.getUserInformationFromCookie.returns(null);
                if (localStorageComesUpGood) {
                    localStorage.get.returns(userInformationToken);
                } else {
                    localStorage.get.returns(null);
                }
            }

            userInformationService = new Common.Services.UserInformation.UserInformationService(sessionCookieService, localStorage, location);
        }

        afterEach(()=> {
            userInformationService = null;
        });
    });
}