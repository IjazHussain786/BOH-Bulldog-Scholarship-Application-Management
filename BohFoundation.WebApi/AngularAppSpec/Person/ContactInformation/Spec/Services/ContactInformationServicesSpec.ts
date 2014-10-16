module BohFoundation.Person.ContactInformation.Spec.Services {
    describe('ContactInformationServices', () => {
        var contactInformationApiEndpoint = "/api/person/contactinformation";
        var authRepo, result, errorResponse, userInformationService;
        var service;
        var whatInfo = "contact", genericResolver, successResponse;

        beforeEach(() => {
            errorResponse = new Common.Models.ServerResponseModel(null, false);
            successResponse = new Common.Models.ServerResponseModel(null, true);
            var commonStubs = new TestHelpers.CommonStubs();

            genericResolver = commonStubs.createGenericResolver();

            authRepo = commonStubs.createAuthRepo();

            userInformationService = commonStubs.getUserInformationServiceStub();

            userInformationService.getUserEmail.returns(whatInfo);

            service = new ContactInformation.Services.ContactInformationServices(authRepo, userInformationService, genericResolver);
        });

        describe('getContactInformationModel', () => {
            it('should return whatever is on the contactInformationModel.', () => {
                service.contactInformationModel = errorResponse;
                expect(service.getContactInformationModel()).toBe(errorResponse);
            });
        });

        describe('getContactInformation', () => {
            describe('get', () => {
                beforeEach(() => {
                    authRepo.get.returns(contactInformationApiEndpoint);
                    result = service.getContactInformation();
                });

                it('should call authRepo with endpoint string.', () => {
                    sinon.assert.calledWith(authRepo.get, contactInformationApiEndpoint);
                });

                it('should return what authRepo get returns.', () => {
                    expect(result).toBe(contactInformationApiEndpoint);
                });
            });

            describe('resolve', () => {
                describe('error', () => {
                    beforeEach(() => {
                        service.resolveGetContactInformation(errorResponse);
                    });

                    it('should call genericGetResolver.', () => {
                        sinon.assert.calledWith(genericResolver.genericGetResolver, whatInfo, errorResponse);
                    });
                });

                describe('success', () => {
                    describe('first time', () => {
                        beforeEach(() => {
                            genericResolver.genericGetResolver.returns(null);
                            service.resolveGetContactInformation(successResponse);
                        });

                        it('should call genericGetResolver.', () => {
                            sinon.assert.calledWith(genericResolver.genericGetResolver, whatInfo, successResponse);
                        });

                        describe('contactInformationModel', () => {
                            it('should be defined.', () => {
                                expect(service.contactInformationModel).toBeDefined();
                            });

                            it('should have an undefined phoneNumber.', () => {
                                expect(service.contactInformationModel.phoneInformation).toBeUndefined();
                            });

                            describe("email", () => {
                                it('should call getUsersEmail.', () => {
                                    sinon.assert.calledOnce(userInformationService.getUserEmail);
                                });

                                it('should have an undefined emailAddress.', () => {
                                    expect(service.contactInformationModel.emailAddress).toBe(whatInfo);
                                });
                            });

                            it('should have an undefined lastUpdated.', () => {
                                expect(service.contactInformationModel.lastUpdated).toBeUndefined();
                            });

                            describe('address', () => {
                                it('should be defined.', () => {
                                    expect(service.contactInformationModel.address).toBeDefined();
                                });

                                it('should have an undefined streetAddress1.', () => {
                                    expect(service.contactInformationModel.address.streetAddress1).toBeUndefined();
                                });

                                it('should have an undefined city.', () => {
                                    expect(service.contactInformationModel.address.city).toBeUndefined();
                                });

                                it('should have an undefined state.', () => {
                                    expect(service.contactInformationModel.address.state).toBeUndefined();
                                });

                                it('should have an undefined zipCode.', () => {
                                    expect(service.contactInformationModel.address.zipCode).toBeUndefined();
                                });

                                it('should have an undefined lastUpdated.', () => {
                                    expect(service.contactInformationModel.address.lastUpdated).toBeUndefined();
                                });

                                it('should have an undefined streetAddress2.', () => {
                                    expect(service.contactInformationModel.address.streetAddress2).toBeUndefined();
                                });
                            });
                        });
                    });
                    describe('update', () => {
                        var street1 = "street1";
                        var street2 = "street2";
                        var city = "citt";
                        var state = "state";
                        var zip = 139040;
                        var lastUpdatedAddress = new Date(2019, 1, 3);
                        var lastUpdatedContact = new Date(2019, 1, 4);
                        var phoneNumber = 1093894981;
                        var timeOfDay = Common.Enums.TimeOfDayEnum.Morning;
                        var email = "1293@aiod.con";

                        var phoneInformation = new Dtos.Person.PhoneInformationModel(phoneNumber, timeOfDay);
                        var address = new Dtos.Person.AddressModel(street1, city, state, zip, lastUpdatedAddress, street2);
                        var contact = new Dtos.Person.ContactInformationModel(address, email, phoneInformation, lastUpdatedContact);
                        
                        beforeEach(() => {
                            genericResolver.genericGetResolver.returns(contact);
                            service.resolveGetContactInformation(successResponse);
                        });

                        it('should call genericGetResolver.', () => {
                            sinon.assert.calledWith(genericResolver.genericGetResolver, whatInfo, successResponse);
                        });

                        describe('contactInformationModel', () => {
                            it('should be defined.', () => {
                                expect(service.contactInformationModel).toBeDefined();
                            });

                            it('should have an phoneNumber.', () => {
                                expect(service.contactInformationModel.phoneInformation.phoneNumber).toBe(phoneNumber);
                            });

                            it('should have an timeOfDay.', () => {
                                expect(service.contactInformationModel.phoneInformation.bestTimeToContactByPhone).toBe(timeOfDay);
                            });

                            it('should have an emailAddress.', () => {
                                expect(service.contactInformationModel.emailAddress).toBe(email);
                            });

                            it('should have an lastUpdated.', () => {
                                expect(service.contactInformationModel.lastUpdated).toBe(lastUpdatedContact);
                            });

                            describe('address', () => {
                                it('should be defined.', () => {
                                    expect(service.contactInformationModel.address).toBeDefined();
                                });

                                it('should have an streetAddress1.', () => {
                                    expect(service.contactInformationModel.address.streetAddress1).toBe(street1);
                                });

                                it('should have an city.', () => {
                                    expect(service.contactInformationModel.address.city).toBe(city);
                                });

                                it('should have an state.', () => {
                                    expect(service.contactInformationModel.address.state).toBe(state);
                                });

                                it('should have an zipCode.', () => {
                                    expect(service.contactInformationModel.address.zipCode).toBe(zip);
                                });

                                it('should have an lastUpdated.', () => {
                                    expect(service.contactInformationModel.address.lastUpdated).toBe(lastUpdatedAddress);
                                });

                                it('should have an streetAddress2.', () => {
                                    expect(service.contactInformationModel.address.streetAddress2).toBe(street2);
                                });

                                describe('no street address2', () => {
                                    it('should have an undefined streetaddress2.', () => {
                                        contact.address.streetAddress2 = undefined;
                                        genericResolver.genericGetResolver.returns(contact);
                                        service.resolveGetContactInformation(successResponse);
                                        expect(service.contactInformationModel.address.streetAddress2).toBeUndefined();
                                        expect(service.contactInformationModel.emailAddress).toBe(email);
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });

        describe('postContactInformation', () => {
            var address = new Dtos.Person.AddressModel();
            var contactInformation = new Dtos.Person.ContactInformationModel(address);
            describe('post', () => {
                beforeEach(() => {
                    authRepo.post.returns(address);
                    result = service.postContactInformation(contactInformation);
                });

                it('should call authRepo post.', () => {
                    sinon.assert.calledWith(authRepo.post, contactInformation, contactInformationApiEndpoint);
                });

                it('should call return what post returns.', () => {
                    expect(result).toBe(address);
                });
            });

            describe('resolve', () => {
                describe('failure', () => {
                    it('should pass whatever is passed in to the resolver with the correct information type to the generic resolver.', () => {
                        service.resolvePostContactInformation(errorResponse);
                        sinon.assert.calledWith(genericResolver.genericPostResolver, whatInfo, errorResponse);
                    });
                });

                describe('success', () => {
                    beforeEach(() => {
                        service.contactInformationModel = contactInformation;
                        service.resolvePostContactInformation(successResponse);
                    });

                    it('should set the lastUpdated to now.', () => {
                        var date = new Date();
                        expect(Math.abs(date.getMilliseconds() - service.contactInformationModel.lastUpdated.getMilliseconds())).toBeLessThan(2);
                    });

                    it('should set the lastUpdated on address to now.', () => {
                        var date = new Date();
                        expect(Math.abs(date.getMilliseconds() - service.contactInformationModel.address.lastUpdated.getMilliseconds())).toBeLessThan(2);
                    });

                    it('should pass whatever is passed in to the resolver with the correct information type to the generic resolver.', () => {
                        sinon.assert.calledWith(genericResolver.genericPostResolver, whatInfo, successResponse);
                    });
                });
            });
        });

    });
}