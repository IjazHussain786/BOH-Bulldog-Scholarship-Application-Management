 module BohFoundation.Person.ContactInformation.Controllers {
     export interface IContactInformationInputCtrlScope extends ng.IScope {
         processing: boolean;
         modalInstance: ng.ui.bootstrap.IModalServiceInstance;
         successfullySaved: boolean;
         contactInformationModelForModal: Dtos.Person.ContactInformationModel;

         getContactInformationModel(): Dtos.Person.ContactInformationModel;
         editContactInformation(): void;
     }

     export interface IContactInformationInputCtrl {
         $scope: IContactInformationInputCtrlScope;
     }

     export class ContactInformationInputCtrl implements IContactInformationInputCtrl {
         static $inject = ['$scope', 'ContactInformationServices', '$modal'];
         constructor(
             public $scope: IContactInformationInputCtrlScope,
             private contactInformationServices: Services.IContactInformationServices,
             private $modal: ng.ui.bootstrap.IModalService) {

             this.getContactInformationFromServer();

             $scope.editContactInformation = () => {
                 if (!$scope.processing) {
                    this.openModal();
                }
             };

             $scope.getContactInformationModel = () => {
                 return this.contactInformationServices.getContactInformationModel();
             };
         }

         private getContactInformationFromServer() {
             this.setProcessingToTrue();
             var resource = this.contactInformationServices.getContactInformation();
             resource.$promise.then(
                 (data) => {
                      this.resolveGetContactInformation(new Common.Models.ServerResponseModel(data, true));
                 },
                 () => {
                     this.resolveGetContactInformation(new Common.Models.ServerResponseModel(null, false));
                 });
         }

         private flipProcessing() {
             this.$scope.processing = !this.$scope.processing;
         }

         private setProcessingToTrue() {
             this.$scope.processing = true;
         }

         private resolveGetContactInformation(serverResponseModel: Common.Models.ServerResponseModel) {
             this.contactInformationServices.resolveGetContactInformation(serverResponseModel);
             if (serverResponseModel.success) {
                 this.flipProcessing();
             }
         }

         private postContactInformation(contactInformationModel: Dtos.Person.ContactInformationModel) {
             this.setProcessingToTrue();
             var resource = this.contactInformationServices.postContactInformation(contactInformationModel);
             resource.$promise.then(() => {
                 this.resolvePostContactInformation(true);
             }, () => {
                 this.resolvePostContactInformation(false);
             });
         }

         private resolvePostContactInformation(success: boolean) {
             this.contactInformationServices.resolvePostContactInformation(new Common.Models.ServerResponseModel(null, success));
             if (success) {
                 this.$scope.successfullySaved = true;
                 this.getContactInformationFromServer();
             } else {
                 this.flipProcessing();
             }
         }

         private openModal() {
             this.$scope.contactInformationModelForModal = this.contactInformationServices.getContactInformationModel();
             this.$scope.modalInstance = this.$modal.open({
                 templateUrl: "/AngularApp/Person/ContactInformation/Templates/AddContactInformationModal.html",
                 controller: "ContactInformationModalInputCtrl",
                 resolve: {
                     contactInformationModel: () => this.$scope.contactInformationModelForModal
                 }
             });

             this.$scope.modalInstance.result.then(
                 modelFromModal => {
                     this.postContactInformation(modelFromModal);
                 },
                 () => {

                 });
         }
     }
 }

module BohFoundation.Main {
    Register.Person.controller("ContactInformationInputCtrl", Person.ContactInformation.Controllers.ContactInformationInputCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/Person/ContactInformation', {
                    templateUrl: '/AngularApp/Person/ContactInformation/Templates/ContactInformation.html',
                    controller: 'ContactInformationInputCtrl',
                    publicAccess: false,
                    title: 'Contact Information'
                });
            })]);
}