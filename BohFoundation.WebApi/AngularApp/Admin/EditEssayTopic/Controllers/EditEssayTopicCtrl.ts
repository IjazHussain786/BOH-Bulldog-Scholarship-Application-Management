 module BohFoundation.Admin.EditEssayTopic.Controllers {
     export interface IEditEssayTopicCtrlScope extends ng.IScope {
         getEssayTopics(): Array<Dtos.Admin.EssayTopics.EssayTopicModel>;

         createAndModifyEssayTopicModel: Dtos.Admin.EssayTopics.CreateAndModifyEssayTopicModel;
         addNewEssayTopic(): void;
         editEssayTopic(index: number): void;
         modalInstance: any;

         yearToAddOrDelete: number;
         addYearToEssayTopic(id: number): void;
         deleteYearFromEssayTopic(id: number): void;
     }

     export interface IEditEssayTopicCtrl {
         $scope: IEditEssayTopicCtrlScope;
     }

     export class EditEssayTopicCtrl implements IEditEssayTopicCtrl {
         
         static $inject = ['$scope', 'CreateAndGetEssayTopicsService','ModifyEssayTopicGraduatingYearService', '$modal'];
         constructor(public $scope: IEditEssayTopicCtrlScope,
             private createAndGetEssayTopicsService: Services.ICreateAndGetEssayTopics,
             private modifyEssayTopicGraduatingYearService: Services.IModifyEssayTopicGraduatingYearService,
             private $modal: any) {

             this.getEssayTopicsFromServer();

             this.$scope.getEssayTopics = () : Array<Dtos.Admin.EssayTopics.EssayTopicModel> => {
                 return this.createAndGetEssayTopicsService.getEssayTopicArray();
             }

             this.$scope.editEssayTopic = (index: number): void =>{
                 this.editEssayTopic(index);
             }

             this.$scope.addNewEssayTopic = (): void => {
                 this.addEssayTopic();
             }

             this.$scope.addYearToEssayTopic = (id : number) => {
                 this.addYearToEssayTopic(id);
             }

             this.$scope.deleteYearFromEssayTopic = (id: number) => {
                 this.deleteYearFromEssayTopic(id);
             }
         }

         private getEssayTopicsFromServer() {
             var resource = this.createAndGetEssayTopicsService.getEssayTopicsFromServer();
             resource.$promise.then((data) => {
                 this.resolveGet(new Common.Models.ServerResponseModel(data, true));
             }, () => {
                 this.resolveGet(new Common.Models.ServerResponseModel(null, false));
             });
         }

         private resolveGet(model: Common.Models.ServerResponseModel) {
             this.createAndGetEssayTopicsService.resolveGetEssayTopics(model);
         }

         private editEssayTopic(index: number) {
             var topicToModify : Dtos.Admin.EssayTopics.EssayTopicModel = this.$scope.getEssayTopics()[index];
             this.$scope.createAndModifyEssayTopicModel = new Dtos.Admin.EssayTopics.CreateAndModifyEssayTopicModel(topicToModify.titleOfEssay, topicToModify.essayPrompt, topicToModify.id);
             this.openModal();
         }

         private addEssayTopic() {
             this.$scope.createAndModifyEssayTopicModel = new Dtos.Admin.EssayTopics.CreateAndModifyEssayTopicModel();
             this.openModal();
         }

         private openModal() {
             this.$scope.modalInstance = this.$modal.open({
                 templateUrl: '/AngularApp/Admin/EditEssayTopic/Templates/CreateAndModifyEssayTopicModal.html',
                 controller: 'CreateAndModifyEssayTopicModal',
                 resolve: {
                     createAndModifyEssayTopicModel: () => this.$scope.createAndModifyEssayTopicModel
                 }
             });

             this.$scope.modalInstance.result.then(
                 dataFromModal => {
                     this.addOrUpdateEssayTopic(dataFromModal);
                 },
                 () => {
                    this.cleanUpAfterModal();
             });
         }

         private cleanUpAfterModal() {
             this.$scope.createAndModifyEssayTopicModel = null;
         }

         private addOrUpdateEssayTopic(dataFromModal: Dtos.Admin.EssayTopics.CreateAndModifyEssayTopicModel) {
             var resource = this.createAndGetEssayTopicsService.postCreateOrModifyEssayTopic(dataFromModal);
             resource.$promise.then(() => {
                 this.cleanUpAfterModal();
                 this.getEssayTopicsFromServer();
             }, () => {
                 this.cleanUpAfterModal();
             });
         }

         private addYearToEssayTopic(id: number) {
             var resource = this.modifyEssayTopicGraduatingYearService.postAddYearToTopic(this.createModifyOrDeleteObject(id));
             resource.$promise.then(
                 () => {
                     this.resolvePostAddYearToEssayTopic(new Common.Models.ServerResponseModel(null, true));
                     this.getEssayTopicsFromServer();
                 },
             () => { this.resolvePostAddYearToEssayTopic(new Common.Models.ServerResponseModel(null, false)); });
         }

         private createModifyOrDeleteObject(id: number) {
             return new Dtos.Admin.EssayTopics.EditEssayTopicByGraduatingClassModel(id, this.$scope.yearToAddOrDelete);
         }

         private resolvePostAddYearToEssayTopic(model: Common.Models.ServerResponseModel) {
             this.modifyEssayTopicGraduatingYearService.resolvePostAddYearToTopic(model);
         }

         private deleteYearFromEssayTopic(id: number) {
             var resource = this.modifyEssayTopicGraduatingYearService.deleteYearToTopic(this.createModifyOrDeleteObject(id));
             resource.$promise.then(
                 () => {
                     this.resolveDeleteYearFromEssayTopic(new Common.Models.ServerResponseModel(null, true));
                     this.getEssayTopicsFromServer();
                 },
                 () => { this.resolveDeleteYearFromEssayTopic(new Common.Models.ServerResponseModel(null, false)); });
         }

         private resolveDeleteYearFromEssayTopic(model: Common.Models.ServerResponseModel) {
             this.modifyEssayTopicGraduatingYearService.resolveDeleteYearToTopic(model);
         }
     }
 }
module BohFoundation.Main {
    Register.Admin.controller('EditEssayTopicCtrl', Admin.EditEssayTopic.Controllers.EditEssayTopicCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/Admin/EditEssayTopics', {
                    templateUrl: '/AngularApp/Admin/EditEssayTopic/Templates/EditEssayTopic.html',
                    controller: 'EditEssayTopicCtrl',
                    publicAccess: false,
                    title: 'Edit Essay Topics'
                });
            })]);
}