module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Controllers.Spec.Modals {
    describe("ViewLetterOfRecommendationModalCtrl", () => {

        var q, modalInstance, scope: ReviewApplicant.Controllers.Modals.IViewLetterOfRecommendationModalCtrlScope, letterOfRecommendationSummary, getLetterOfRecommendationForEvaluatorService;

        var keyValues, partitionKey="part", rowkey = "row";
        var data = "dajkjnksad";
        var deferred, promise, resource;

        beforeEach(inject(($rootScope, $q) => {
            modalInstance = new TestHelpers.FakeModal().createModalInstanceStub();

            q = $q;
            scope = $rootScope;

            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };

            keyValues = new Dtos.Common.AzureQueuryObjects.AzureTableStorageEntityKeyModel(partitionKey, rowkey);

            letterOfRecommendationSummary = new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.LetterOfRecommendation.LetterOfRecommendationSummaryModel(null, null, keyValues);

            getLetterOfRecommendationForEvaluatorService = sinon.stub({
                getLetterOfRecommedation:()=>{},
                getLetterOfRecommendationFromServer: () => { },
                resolveGetLetterOfRecommendation: () => { }
            });

            getLetterOfRecommendationForEvaluatorService.getLetterOfRecommendationFromServer.returns(resource);
            new Controllers.Modals.ViewLetterOfRecommendationModalCtrl(scope, modalInstance, letterOfRecommendationSummary, getLetterOfRecommendationForEvaluatorService);
        }));

        it('should set the model on scope.', () => {
            expect(scope.letterOfRecommendationSummary).toBe(letterOfRecommendationSummary);
        });

        describe('construction', () => {
            describe('getLetterOfRecommendationFromServer', () => {
                describe('get', () => {
                    it('should call the get method with the keys.', () => {
                        sinon.assert.calledWith(getLetterOfRecommendationForEvaluatorService.getLetterOfRecommendationFromServer, keyValues);
                    });
                });

                describe('resolve', () => {
                    describe('success', () => {

                        beforeEach(() => {
                            deferred.resolve(data);
                            scope.$apply();
                        });

                        it('should call resolve.', () => {
                            sinon.assert.calledWith(getLetterOfRecommendationForEvaluatorService.resolveGetLetterOfRecommendation, new Common.Models.ServerResponseModel(data, true));
                        });
                    });

                    describe('failure', () => {
                        beforeEach(() => {
                            deferred.reject();
                            scope.$apply();
                        });

                        it('should call resolve.', () => {
                            sinon.assert.calledWith(getLetterOfRecommendationForEvaluatorService.resolveGetLetterOfRecommendation, new Common.Models.ServerResponseModel(null, false));
                        });
                    });
                });
            });
        });

        describe('successful construction', () => {
            describe('cancel', () => {
                it('should call dismiss.', () => {
                    scope.cancel();
                    sinon.assert.calledOnce(modalInstance.dismiss);
                });
            });

            describe('getLetterOfRecommendation', () => {
                var letterOfRecommendation = "letter";

                it('should return the letter of recommendation from service', () => {
                    getLetterOfRecommendationForEvaluatorService.getLetterOfRecommedation.returns(letterOfRecommendation);
                    expect(scope.getLetterOfRecommendation()).toBe(letterOfRecommendation);
                });
            });
        });
    });
}  