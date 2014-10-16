 module BohFoundation.Applicant.AcademicInformation.Spec.Controller {
     describe('EditAcademicInformationModalCtrl', () => {
         var modalInstance, scope: AcademicInformation.Controllers.IEditAcademicInformationModalCtrlScope, academicInformationModel;

         beforeEach(inject(($rootScope) => {
             modalInstance = new TestHelpers.FakeModal().createModalInstanceStub();

             scope = $rootScope;

             academicInformationModel = new Dtos.Applicant.Academic.AcademicInformationModel(null);

             new AcademicInformation.Controllers.EditAcademicInformationModalCtrl(scope, modalInstance, academicInformationModel);
         }));

         it('should set the model on scope.', () => {
             expect(scope.academicInformationModel).toBe(academicInformationModel);
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
                     sinon.assert.calledWith(modalInstance.close, academicInformationModel);
                 });
             });
         });
     });
 }