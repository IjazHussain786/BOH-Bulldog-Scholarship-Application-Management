 module BohFoundation.Admin.EditEssayTopic.Spec.Controllers {
     describe('CreateAndModifyEssayTopicModal', () => {
         var modalInstance, scope: EditEssayTopic.Controllers.ICreateAndModifyEssayTopicModalScope, createAndModifyEssayTopicModal;
         var form;

         beforeEach(inject(($rootScope) => {
             modalInstance = sinon.stub({
                 close: () => { },
                 dismiss: () => { }
             });

             form = sinon.stub({
                 $valid: false
             });
             
             scope = $rootScope;

             createAndModifyEssayTopicModal = new Dtos.Admin.EssayTopics.CreateAndModifyEssayTopicModel("t", "e", 1);

             new EditEssayTopic.Controllers.CreateAndModifyEssayTopicModal(scope, modalInstance, createAndModifyEssayTopicModal);
         }));

         it('should set the createAndModifyEssayTopicModal on scope.', () => {
             expect(scope.createAndModifyEssayTopicModal).toBe(createAndModifyEssayTopicModal);
         });

         describe("ok", () => {
             describe('invalid form', () => {
                 it('should not call close.', () => {
                     scope.ok(form);
                     sinon.assert.notCalled(modalInstance.close);
                 });
             });
             describe('valid form', () => {
                 it('should call close with object.', () => {
                     form.$valid = true;
                     scope.ok(form);
                     sinon.assert.calledWith(modalInstance.close, createAndModifyEssayTopicModal);
                 });
             });
         });

         describe('cancel', () => {
             it('should call dismiss.', () => {
                 scope.cancel();
                 sinon.assert.calledOnce(modalInstance.dismiss);
             });
         });
     });
 }