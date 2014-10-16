 module BohFoundation.Admin.EditEssayTopic.Controllers {
     export interface ICreateAndModifyEssayTopicModalScope extends ng.IScope {
         createAndModifyEssayTopicModal: Dtos.Admin.EssayTopics.CreateAndModifyEssayTopicModel;

         ok(form: ng.IFormController): void;
         cancel(): void;
     }

     export interface ICreateAndModifyEssayTopicModal {
         $scope: ICreateAndModifyEssayTopicModalScope
     }
     
     export class CreateAndModifyEssayTopicModal implements ICreateAndModifyEssayTopicModal {
         static $inject = ['$scope', '$modalInstance', 'createAndModifyEssayTopicModel'];
         constructor(public $scope: ICreateAndModifyEssayTopicModalScope,
             private $modalInstance,
             private createAndModifyEssayTopicModel) {

             $scope.createAndModifyEssayTopicModal = createAndModifyEssayTopicModel;

             $scope.ok = (form: ng.IFormController) => {
                 if (form.$valid) {
                     $modalInstance.close($scope.createAndModifyEssayTopicModal);
                 }
             };

             $scope.cancel = () => {
                 $modalInstance.dismiss();
             };
         }
     }
 }

module BohFoundation.Main {
    Register.Admin.controller('CreateAndModifyEssayTopicModal', Admin.EditEssayTopic.Controllers.CreateAndModifyEssayTopicModal);
}