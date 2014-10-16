 module BohFoundation.Admin.Dashboard.Spec.Controllers {
     describe('AdminDashboardCtrl', () => {
         var scope;
         var location;
         var adminNotificationService;

         beforeEach(inject(($rootScope) => {
             scope = $rootScope;

             location = sinon.stub({
                 path: () =>{}
             });

             adminNotificationService = sinon.stub({
                 getNumberOfPendingApplicationEvaluators: () =>{ }
             });

             new Admin.Dashboard.Controllers.AdminDashboardCtrl(scope, location, adminNotificationService);
         }));

         describe('clickLinkButton', () => {
             var link = "asdfkjn39";

             beforeEach(() => {
                 scope.clickLinkButton(link);
             });

             it('should call location path with the link.', () => {
                 sinon.assert.calledWith(location.path, link);
             });
         });

         describe('getNumberOfPendingApplicationEvaluators', () => {
             var pendingApplicationEvaluators = 19802;
             var result;

             beforeEach(() => {
                adminNotificationService.getNumberOfPendingApplicationEvaluators.returns(pendingApplicationEvaluators);
                result = scope.getNumberOfPendingApplicationEvaluators();
             });

             it('should call adminNotificationService for this number.', () => {
                 sinon.assert.calledOnce(adminNotificationService.getNumberOfPendingApplicationEvaluators);
             });

             it('should return the number adminNotification spits out.', () => {
                 expect(result).toBe(pendingApplicationEvaluators);
             });
         });

     });
 }