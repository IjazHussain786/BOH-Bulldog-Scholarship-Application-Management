 module BohFoundation.Admin.ConfirmPendingApplicationEvaluators.Spec.Controllers {
     describe('ConfirmPendingApplicationEvaluatorsCtrl', () => {

         var confirmPendingEvalService, scope: ConfirmPendingApplicationEvaluators.Controllers.IConfirmPendingApplicationEvaluatorsCtrlScope, adminNotificationService;
         var q;
         var fakeArray = ["turtles", "dogs", "bunnies"];
         var deferred, promise, resource;
         var deferred2, promise2, resource2;

         beforeEach(inject(($q, $rootScope) => {
             q = $q;

             deferred = q.defer();
             promise = deferred.promise;
             resource = { $promise: promise };

             deferred2 = q.defer();
             promise2 = deferred2.promise;
             resource2 = { $promise: promise2 };

             scope = $rootScope;

             confirmPendingEvalService = sinon.stub({
                 getPendingApplicationEvaluators: () => {},
                 resolveGetPendingApplicationEvaluators: () => {},
                 confirmOrRejectEvaluator: () => {},
                 resolveConfirmOrRejectEvaluator: () => {},
                 listOfPendingApplicationEvaluators: {}
             });

             adminNotificationService = sinon.stub({
                 getNotifications: () => { },
                 resolveNotifications: () => { }
             });

             confirmPendingEvalService.getPendingApplicationEvaluators.returns(resource);
             confirmPendingEvalService.confirmOrRejectEvaluator.returns(resource);
             adminNotificationService.getNotifications.returns(resource2);

             new ConfirmPendingApplicationEvaluators.Controllers.ConfirmPendingApplicationEvaluatorsCtrl(scope, confirmPendingEvalService, adminNotificationService);
         }));

         describe('constructor', () => {
             it('should call getPendingApplications', () => {
                 sinon.assert.calledOnce(confirmPendingEvalService.getPendingApplicationEvaluators);
             });

             describe('error', () => {
                 it('should call resolveGetPendingApplicationEvaluators with an error model.', () => {
                     deferred.reject();
                     scope.$apply();
                     sinon.assert.calledWith(confirmPendingEvalService.resolveGetPendingApplicationEvaluators, new Common.Models.ServerResponseModel(null, false));
                 });
             });

             describe('OK', () => {
                 beforeEach(() => {
                     confirmPendingEvalService.listOfPendingApplicationEvaluators = fakeArray;
                     deferred.resolve({ data: fakeArray });
                     scope.$apply();
                 });
                 
                 it('should call resolveGetPendingApplicationEvaluators with a success model.', () => {
                     sinon.assert.calledWith(confirmPendingEvalService.resolveGetPendingApplicationEvaluators, new Common.Models.ServerResponseModel(fakeArray, true));
                 });

                 it('should set pendingEvaluators equal to what is on the service.', () => {
                     expect(scope.pendingEvaluators).toBe(fakeArray);
                 });
             });
         });

         describe('confirmOrRejectEvaluator', () => {
             var index = 100;
             var newFakeArray = ['kittens', 'gerbals', 'mormons'];

             beforeEach(() => {
                 confirmPendingEvalService.listOfPendingApplicationEvaluators = newFakeArray;
                 scope.confirmOrRejectEvaluator(index, true, true);
             });

             it('should call confirmOrRejectEvaluator with the params.', () => {
                 sinon.assert.calledWith(confirmPendingEvalService.confirmOrRejectEvaluator, index, true, true);
             });

             it('should reset the list again.', () => {
                 expect(scope.pendingEvaluators).toBe(newFakeArray);
             });

             describe('success', () => {
                 
                 beforeEach(() => {
                     
                     deferred.resolve({ data: fakeArray });
                     scope.$apply();
                 });

                 it('should pass to the resolve service that there was a success.', () => {
                     sinon.assert.calledWith(confirmPendingEvalService.resolveConfirmOrRejectEvaluator, true);
                 });

                 describe('notification center update.', () => {
                     it('should call the notification center get method.', () => {
                         sinon.assert.calledOnce(adminNotificationService.getNotifications);
                     });

                     describe('success', () => {

                         it('should call resolve notifications with data and true success if it resolves.', () => {
                             deferred2.resolve(fakeArray);
                             scope.$apply();
                             sinon.assert.calledWith(adminNotificationService.resolveNotifications, new Common.Models.ServerResponseModel(fakeArray, true));
                         });
                     });

                     describe('failure', () => {
                         var errorMessage = 'message';

                         it('should call resolve notifications with data and false success if it rejects.', () => {
                             deferred2.reject(errorMessage);
                             scope.$apply();
                             sinon.assert.calledWith(adminNotificationService.resolveNotifications, new Common.Models.ServerResponseModel(errorMessage, false));
                         });
                     });
                 });
             });

             describe('failure', () => {
                 it('should pass the failure on to the resolve service.', () => {
                     deferred.reject();
                     scope.$apply();
                     sinon.assert.calledWith(confirmPendingEvalService.resolveConfirmOrRejectEvaluator, false);
                 });
             });
         });
     });
 }