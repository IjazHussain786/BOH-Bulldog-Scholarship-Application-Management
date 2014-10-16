 module BohFoundation.Applicant.References.Spec.Controllers {
     describe('ApplicantReferenceModalCtrl', () => {
         var modalInstance, scope: References.Controllers.IApplicantReferenceModalCtrlScope, referenceInputModel;

         beforeEach(inject(($rootScope) => {
             modalInstance = sinon.stub({
                 close: () => { },
                 dismiss: () => { }
             });

             scope = $rootScope;

             referenceInputModel = new Dtos.Applicant.References.ApplicantReferenceInputModel();

             new References.Controllers.ApplicantReferenceModalCtrl(scope, modalInstance,referenceInputModel);
         }));

         it('should set the model on scope.', () => {
             expect(scope.refernceInputModelInModal).toBe(referenceInputModel);
         });

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
                 it('should call close.', () => {
                     form.$valid = true;
                     scope.ok(form);
                     sinon.assert.calledWith(modalInstance.close, referenceInputModel);
                 });
             });
         });
     });
 }