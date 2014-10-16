 module BohFoundation.Admin.InviteApplicationEvaluator.Spec.Controllers {
     describe('InviteApplicationEvaluatorCtrl', () => {
         var q, scope;
         var authRequiredRepository, alertHelperService;
         var form;

         beforeEach(inject(($rootScope, $q) => {
             scope = $rootScope;
             q = $q;

             authRequiredRepository = sinon.stub({
                 post: () =>{ }
             });

             alertHelperService = sinon.stub({
                 addDangerAlert: () => { },
                 addSuccessAlert: () =>{ }
             });

             new InviteApplicationEvaluator.Controllers.InviteApplicationEvaluatorCtrl(scope, authRequiredRepository, alertHelperService);
         }));

         it('should have processing as false by default.', () => {
             expect(scope.processing).toBeFalsy();
         });

         describe('invitePerson', () => {
             describe('validation', () => {
                 describe('processing is true', () => {

                     beforeEach(() => {
                         form = { $valid: true };
                         scope.processing = true;
                         invitePerson(form);
                     });

                     it('should not call authRequired repository.', () => {
                         sinon.assert.notCalled(authRequiredRepository.post);
                     });
                 });

                 describe('form is not valid', () => {
                     beforeEach(() => {
                         form = { $valid: false };
                         scope.processing = false;
                         invitePerson(form);
                     });

                     it('should not call authRequired repository.', () => {
                         sinon.assert.notCalled(authRequiredRepository.post);
                     });

                     it('should not switch processing to true.', () => {
                         expect(scope.processing).toBeFalsy();
                     });
                 });
             });

             describe('everything valid', () => {
                 var sendEmailContact;
                 var deferred, promise: ng.IPromise<any>, resource;

                 beforeEach(() => {
                     form = {
                         $valid: true, $pristine: false, $dirty: true, $setPristine: () => { scope.inviteApplicationEvaluatorForm.$dirty = false, scope.inviteApplicationEvaluatorForm.$pristine = true; }
                     };

                     scope.inviteApplicationEvaluatorForm = form;
                     scope.processing = false;
                     sendEmailContact = new Dtos.Email.SendEmailContactModel("firstName", "lastName", "email@email.com");
                     scope.newSendEmailContact = sendEmailContact;

                     deferred = q.defer();
                     promise = deferred.promise;
                     resource = { $promise: promise };

                     authRequiredRepository.post.returns(resource);

                     invitePerson(form);
                 });

                 it('should switch processing to true.', () => {
                     expect(scope.processing).toBeTruthy();
                 });

                 it('should call authrequiredrepo with sendEmailContact object.', () => {
                     sinon.assert.calledWith(authRequiredRepository.post, sendEmailContact, '/api/admin/inviteconfirmapplicationevaluator/invite');
                 });

                 describe('successful call' , () => {

                     beforeEach(() => {
                         deferred.resolve();
                         scope.$apply();
                     });

                     it('should call alertHelper success.', () => {
                         sinon.assert.calledWith(alertHelperService.addSuccessAlert, "firstName lastName has been invited to start a evaluator's account!");
                     });

                     it('should switch processing to false.', () => {
                         expect(scope.processing).toBeFalsy();
                     });

                     it('should reset form.', () => {
                         expect(scope.inviteApplicationEvaluatorForm.$pristine).toBeTruthy();
                         expect(scope.inviteApplicationEvaluatorForm.$dirty).toBeFalsy();
                     });

                     it('should reset sendNewEmailContact.', () => {
                         expect(scope.newSendEmailContact).toBeUndefined();
                     });
                 });

                 describe('failure call', () => {

                     beforeEach(() => {
                         deferred.reject();
                         scope.$apply();
                     });

                     it('should call alertHelper danger.', () => {
                         sinon.assert.calledWith(alertHelperService.addDangerAlert, "There are an error sending the invite. Try again later.");
                     });

                     it('should switch processing to false.', () => {
                         expect(scope.processing).toBeFalsy();
                     });

                     it('should not reset form.', () => {
                         expect(scope.inviteApplicationEvaluatorForm.$pristine).toBeFalsy();
                         expect(scope.inviteApplicationEvaluatorForm.$dirty).toBeTruthy();
                     });

                     it('should not reset sendNewEmailContact.', () => {
                         expect(scope.newSendEmailContact).toBe(sendEmailContact);
                     });
                 });
             });

             function invitePerson(form) {
                 scope.invitePerson(form);
             }
         });
     });
 }