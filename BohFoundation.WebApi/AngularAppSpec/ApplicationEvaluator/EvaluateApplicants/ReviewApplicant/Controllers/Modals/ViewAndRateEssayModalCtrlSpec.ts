module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Controllers.Spec.Modals {
    describe("ViewAndRateEssayModalCtrl", () => {

        var q, modalInstance, scope: ReviewApplicant.Controllers.Modals.IViewAndRateEssayModalCtrlScope, essaySummary, getEssayService;

        var keyValues, partitionKey = "part", rowkey = "row";
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

            essaySummary = new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.Essay.EssaySummaryModel(null, null, null, null, null, keyValues);
            
            getEssayService = sinon.stub({
                getEssay:()=> {},
                getEssayFromServer: () => { },
                resolveEssayFromServer: () => { }
            });

            getEssayService.getEssayFromServer.returns(resource);
            new Controllers.Modals.ViewAndRateEssayModalCtrl(scope, modalInstance, essaySummary, getEssayService);
        }));

        it('should set the model on scope.', () => {
            expect(scope.essaySummary).toBe(essaySummary);
        });

        describe('getRatingsArray', () => {
            var result;

            beforeEach(() => {
                result = scope.getRatingsArray();
            }); 

            it('should have 10 items in the array.', () => {
                expect(result.length).toBe(10);
            });

            it('should have APlus as the First item.', () => {
                expect(result[0]).toBe(Common.Enums.RatingEnum.APlus);
            });

            it('should have F as the last item.', () => {
                expect(result[9]).toBe(Common.Enums.RatingEnum.F);
            });
        });

        describe('construction', () => {
            describe('getEssay', () => {
                describe('get', () => {
                    it('should call the get method with the keys.', () => {
                        sinon.assert.calledWith(getEssayService.getEssayFromServer, keyValues);
                    });
                });

                describe('resolve', () => {
                    describe('success', () => {
                        beforeEach(() => {
                            deferred.resolve(data);
                            scope.$apply();
                        });

                        it('should call resolve.', () => {
                            sinon.assert.calledWith(getEssayService.resolveEssayFromServer, new Common.Models.ServerResponseModel(data, true));
                        });
                    });

                    describe('failure', () => {
                        beforeEach(() => {
                            deferred.reject();
                            scope.$apply();
                        });

                        it('should call resolve.', () => {
                            sinon.assert.calledWith(getEssayService.resolveEssayFromServer, new Common.Models.ServerResponseModel(null, false));
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

            describe('ok', () => {
                var form;

                beforeEach(() => {
                    form = sinon.stub({
                        $valid: {}
                    });
                });

                describe('invalid form', () => {
                    it('should not call close.', () => {
                        form.$valid = false;
                        scope.ok(form);
                        sinon.assert.notCalled(modalInstance.close);
                    });
                });

                describe('valid form', () => {
                    var rating;

                    it('should call close.', () => {
                        rating = new Dtos.Common.GenericRatingModel(Common.Enums.RatingEnum.A, "good stuff.");

                        form.$valid = true;
                        var validEssay = new Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.Essay.EssaySummaryModel(1, "a", "123", 123, rating);
                        scope.essaySummary = validEssay;
                        scope.ok(form);
                        
                        sinon.assert.calledWith(modalInstance.close, new Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate.EssayRatingModel(1, rating));
                    });
                });
            });

            describe('getLetterOfRecommendation', () => {
                var essay = "essay";

                it('should return the letter of recommendation from service', () => {
                    getEssayService.getEssay.returns(essay);
                    expect(scope.getEssay()).toBe(essay);
                });
            });
        });
    });
}  