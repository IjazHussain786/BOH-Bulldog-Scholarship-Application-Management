﻿ module BohFoundation.Applicant.Essays.Spec.Controllers {
     describe('ApplicantEditEssayModalCtrl', () => {
         var modalInstance, scope: Essays.Controllers.IApplicantEditEssayModalCtrlScope, essayTopicModel;
         var prompt = "prompt", topicId = 123;

         beforeEach(inject(($rootScope) => {
             modalInstance = sinon.stub({
                 close: () => { },
                 dismiss: () => { }
             });

             scope = $rootScope;

             essayTopicModel = new Dtos.Applicant.Essay.EssayModel(prompt, topicId);

             new Essays.Controllers.ApplicantEditEssayModalCtrl(scope, modalInstance, essayTopicModel);
         }));

         it('should set the model on scope.', () => {
             expect(scope.essayModelInModal).toBe(essayTopicModel);
         });

         describe('cancel', () => {
             it('should call dismiss.', () => {
                 scope.cancel();
                 sinon.assert.calledOnce(modalInstance.dismiss);
             });
         });

         describe("ok", () => {
             describe('invalid form', () => {
                 it('should not call close.', () => {
                     scope.essayModelInModal.essay = "ESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAYESSAY";
                     scope.ok();
                     sinon.assert.notCalled(modalInstance.close);
                 });
             });
             describe('valid form', () => {
                 it('should call close with object.', () => {
                     scope.essayModelInModal.essay = "ESSAYESSAYESSAYESSAYE";
                     scope.ok();
                     sinon.assert.calledWith(modalInstance.close, essayTopicModel);
                 });
             });
         });
     });
 }