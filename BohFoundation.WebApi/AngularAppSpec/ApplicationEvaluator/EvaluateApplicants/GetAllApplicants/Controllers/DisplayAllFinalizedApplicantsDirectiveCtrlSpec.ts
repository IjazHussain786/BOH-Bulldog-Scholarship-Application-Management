module BohFoundation.ApplicationEvaluator.EvaluateApplicants.GetAllApplicants.Spec.Controllers {
    describe("DisplayAllFinalizedApplicantsDirectiveCtrl", () => {

        var scope, q, service;
        var data = { data: "1239" };
        var deferred, promise, resource;

        beforeEach(inject(($rootScope, $q) => {
            q = $q;
            scope = $rootScope;

            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };

            service = sinon.stub({
                getFinalizedApplicantsFromServer:() => {},
                resolveGetFinalizedApplicants: () => { },
                getListOfApplicantSummaries: () => { },
                getClassYear: () => { },
                gotoApplicant: () => { },
                gotoRandomApplicant: () => { },
                canGotoSummaryOfRatings: () => { },
                gotoSummaryOfRatings: () => { }
            });

            service.getFinalizedApplicantsFromServer.returns(resource);
            new ApplicationEvaluator.EvaluateApplicants.GetAllApplicants.Controllers.DisplayAllFinalizedApplicantsDirectiveCtrl(scope, service);
        }));

        describe('constructor', () => {
            it('should call getFinalizedApplicantsFromServer', () => {
                sinon.assert.calledOnce(service.getFinalizedApplicantsFromServer);
            });

            describe('success', () => {
                
                beforeEach(() => {
                    deferred.resolve(data);
                    scope.$apply();
                });

                it('should call resolve.', () => {
                    sinon.assert.calledWith(service.resolveGetFinalizedApplicants, new Common.Models.ServerResponseModel(data, true));
                });
            });

            describe('failure', () => {
                beforeEach(() => {
                    deferred.reject();
                    scope.$apply();
                });

                it('should call resolve.', () => {
                    sinon.assert.calledWith(service.resolveGetFinalizedApplicants, new Common.Models.ServerResponseModel(null, false));
                });
            });
        });

        describe('successful construction', () => {
            beforeEach(() => {
                deferred.resolve(data);
                scope.$apply();
            });

            describe('gotoApplicant', () => {
                it('should pass the number to the service.', () => {
                    scope.gotoApplicant(1092);
                    sinon.assert.calledWith(service.gotoApplicant, 1092);
                });
            }); 

            describe('getApplicantSummaries', () => {
                it('should return whatever is returned from the service.', () => {
                    service.getListOfApplicantSummaries.returns(data);
                    expect(scope.getApplicantSummaries()).toBe(data);
                });
            });

            describe('getGraduatingYear', () => {
                it('should return whatever is returned from the service.', () => {
                    service.getClassYear.returns(123);
                    expect(scope.getGraduatingYear()).toBe(123);
                });
            });

            describe('gotoRandomApplicant', () => {
                it('should call the service.', () => {
                    scope.gotoRandomApplicant();
                    sinon.assert.calledOnce(service.gotoRandomApplicant);
                });
            });

            describe('gotoSummaryOfRatings', () => {
                it('should call the service.', () => {
                    scope.gotoSummaryOfRatings();
                    sinon.assert.calledOnce(service.gotoSummaryOfRatings);
                });
            });

            describe('canGotoSummaryOfRatings', () => {
                it('should return whatever the service returns.', () => {
                    service.canGotoSummaryOfRatings.returns(data);
                    expect(scope.canGotoSummaryOfRatings()).toBe(data);
                });
            });
        });
    });
} 