module BohFoundation.ApplicationEvaluator.EvaluateApplicants.RatingSummaries.Spec.Controllers {
    describe("CurrentApplicationCtrl", () => {

        var scope, q, ratingSummariesService, $routeParams;

        var deferred, promise, resource;
        var data = "data";
        var ctrl;

        beforeEach(inject(($rootScope, $q) => {
            q = $q;
            scope = $rootScope;

            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };

            ratingSummariesService = sinon.stub({
                getRatingSummariesFromServer:() => {},
                resolveGetRatingSummariesFromServer: () => { },
                getTop5Applicants: () => { },
                gotoApplicant: () => { }
            });

            ratingSummariesService.getRatingSummariesFromServer.returns(resource);


            new RatingSummaries.Controllers.RatingSummariesCtrl(scope, ratingSummariesService);
        }));

        describe('construction', () => {
            describe('getRatingSummariesFromServer', () => {
                describe('get', () => {
                    it('should call the get method.', () => {
                        sinon.assert.called(ratingSummariesService.getRatingSummariesFromServer);
                    });
                });

                describe('resolve', () => {
                    describe('success', () => {
                        beforeEach(() => {
                            deferred.resolve(data);
                            scope.$apply();
                        });

                        it('should call resolve.', () => {
                            sinon.assert.calledWith(ratingSummariesService.resolveGetRatingSummariesFromServer, new Common.Models.ServerResponseModel(data, true));
                        }); 
                    });

                    describe('failure', () => {
                        beforeEach(() => {
                            deferred.reject();
                            scope.$apply();  
                        });

                        it('should call resolve.', () => {
                            sinon.assert.calledWith(ratingSummariesService.resolveGetRatingSummariesFromServer, new Common.Models.ServerResponseModel(null, false));
                        });
                    });
                });
            });
        });

        describe('successful construction', () => {
            beforeEach(() => {
                deferred.resolve(data);
                scope.$apply();
            });

            describe('gotoApplicant', () => {
                it('should pass the index to the service.', () => {
                    var index = 10239;
                    scope.gotoApplicant(index);
                    sinon.assert.calledWith(ratingSummariesService.gotoApplicant, index);
                });
            });

            describe('get methods', () => {
                describe('getTop5Applicants', () => {
                    it('should return whatever the service returns.', () => {
                        ratingSummariesService.getTop5Applicants.returns(data);
                        expect(scope.getTop5Applicants()).toBe(data);
                    });
                });
            });
        });
    });
} 