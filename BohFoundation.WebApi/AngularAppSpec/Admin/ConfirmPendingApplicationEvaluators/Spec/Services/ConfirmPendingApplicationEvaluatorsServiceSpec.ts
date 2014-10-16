module BohFoundation.Admin.ConfirmPendingApplicationEvaluators.Spec.Services {
    describe('ConfirmPendingApplicationEvaluatorsService', () => {
        var service: ConfirmPendingApplicationEvaluators.Services.IConfirmPendingApplicationEvaluatorsService;
        var authRequiredRepo, alertHelper;
        var getPendingApplicationEvaluatorsApiEndpoint = "/api/admin/inviteconfirmapplicationevaluator/pendingevaluators";
        var result;
        var firstName = "firstName";
        var lastName = "lastName";
        var email = "email";

        beforeEach(() => {

            authRequiredRepo = sinon.stub({
                get: () => { },
                post: () => { }
            });

            alertHelper = sinon.stub({
                addDangerAlert: () => { },
                addSuccessAlert: () => { }
            });

            service = new ConfirmPendingApplicationEvaluators.Services.ConfirmPendingApplicationEvaluatorsService(authRequiredRepo, alertHelper);
        });

        it('should have a empty array on listOfPendingApplicationEvaluators.', () => {
            expect(service.listOfPendingApplicationEvaluators.length).toBe(0);
        });

        describe('getPendingapplicationEvaluators', () => {
            beforeEach(() => {
                authRequiredRepo.get.returns(authRequiredRepo);
                result = service.getPendingApplicationEvaluators();
            });

            it('should call authrequired get.', () => {
                sinon.assert.calledWith(authRequiredRepo.get, getPendingApplicationEvaluatorsApiEndpoint);
            });

            it('should call authrequired get.', () => {
                expect(result).toBe(authRequiredRepo);
            });
        });

        describe('resolveGetPendingApplicationEvaluators', () => {
            var listOfPeopleFromServer;
            var numberOfItems = 10;
            
            beforeEach(() => {
                listOfPeopleFromServer = createListOfPeopleFromServer(numberOfItems);
            });

            describe('false success', () => {
                beforeEach(() => {
                    var serverResponse = new Common.Models.ServerResponseModel({}, false);
                    service.resolveGetPendingApplicationEvaluators(serverResponse);
                });

                it('should call addDanger with an error message.', () => {
                    sinon.assert.calledWith(alertHelper.addDangerAlert, "There was an error getting the pending application evaluators. Try again later.");
                });
            });

            describe('success', () => {
                beforeEach(() => {
                    var serverResponse = new Common.Models.ServerResponseModel(listOfPeopleFromServer, true);
                    service.resolveGetPendingApplicationEvaluators(serverResponse);
                });

                it('should set listOfPendingApplicationEvaluators to the object returned.', () => {
                    expect(service.listOfPendingApplicationEvaluators.length).toBe(numberOfItems);
                });

                it('should transform the list from the server into one with confirmed.', () => {
                    for (var i = 0; i < numberOfItems; i++) {
                        expect(service.listOfPendingApplicationEvaluators[i].confirmed).toBe(ConfirmPendingApplicationEvaluators.Models.ConfirmRejectedOrNotEvaluated.NotEvaluated);
                    }
                });

                it('should keep the firstName.', () => {
                    for (var i = 0; i < numberOfItems; i++) {
                        expect(service.listOfPendingApplicationEvaluators[i].firstName).toBe(firstName + i);
                    }
                });

                it('should keep the lastName.', () => {
                    for (var i = 0; i < numberOfItems; i++) {
                        expect(service.listOfPendingApplicationEvaluators[i].lastName).toBe(lastName + i);
                    }
                });

                it('should keep the email.', () => {
                    for (var i = 0; i < numberOfItems; i++) {
                        expect(service.listOfPendingApplicationEvaluators[i].emailAddress).toBe(email + i);
                    }
                });
            });

            function createListOfPeopleFromServer(numberOfPeople:number) {
                var list = [];
                for (var i = 0; i < numberOfPeople; i++) {
                    list.push(new Common.Models.PersonModel(firstName + i, lastName + i, email + i));
                }
                return list;
            }
        });

        describe('confirmOrRejectEvaluator', () => {
            var confirmApplicationEvaluatorApiEndpoint = "/api/admin/inviteconfirmapplicationevaluator/confirm";

            beforeEach(() => {
                authRequiredRepo.post.returns(email);
                populateListOfPendingApplicationEvaluators(10);
            });

            describe('confirm', () => {
                beforeEach(() => {
                    result = service.confirmOrRejectEvaluator(3, true, true);
                });

                it('should return what authRequired repo returns.', () => {
                    expect(result).toBe(email);
                });

                it('should flag the selected item as confirmed.', () => {
                    expect(service.listOfPendingApplicationEvaluators[3].confirmed).toBe(Models.ConfirmRejectedOrNotEvaluated.Confirmed);
                });

                it('should call post with information.', () => {
                    sinon.assert.calledWith(authRequiredRepo.post, new Dtos.Admin.ConfirmApplicationEvaluatorModel(email + 3, true, true), confirmApplicationEvaluatorApiEndpoint);
                });
            });
            
            describe('reject', () => {
                beforeEach(() => {
                    result = service.confirmOrRejectEvaluator(6, false, false);
                });

                it('should return what authRequired repo returns.', () => {
                    expect(result).toBe(email);
                });

                it('should flag the selected item as confirmed.', () => {
                    expect(service.listOfPendingApplicationEvaluators[6].confirmed).toBe(Models.ConfirmRejectedOrNotEvaluated.Rejected);
                });

                it('should call post with information.', () => {
                    sinon.assert.calledWith(authRequiredRepo.post, new Dtos.Admin.ConfirmApplicationEvaluatorModel(email + 6, false, false), confirmApplicationEvaluatorApiEndpoint);
                });
            });

            function populateListOfPendingApplicationEvaluators(numberOfPeople: number) {
                var list = [];
                for (var i = 0; i < numberOfPeople; i++) {
                    list.push(new Models.PendingApplicationEvaluatorModel(firstName + i, lastName + i, email + i, Models.ConfirmRejectedOrNotEvaluated.NotEvaluated));
                }
                service.listOfPendingApplicationEvaluators = list;
            }
        });

        describe('resolveConfirmOrRejectEvaluator', () => {
            it('should call alerts success on a success.', () => {
                service.resolveConfirmOrRejectEvaluator(true);
                sinon.assert.calledWith(alertHelper.addSuccessAlert, "You successfully confirmed or rejected a application evaluator.");
            });

            it('should call alerts danger on a failure.', () => {
                service.resolveConfirmOrRejectEvaluator(false);
                sinon.assert.calledWith(alertHelper.addDangerAlert, "There was an error confirming or rejecting an application evaluator. Please try again later.");
            });
        });
    });
}