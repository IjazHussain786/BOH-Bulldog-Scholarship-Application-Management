 module BohFoundation.Person.ContactInformation.Controllers {
     export interface IContactInformationModalInputCtrl {
         $scope: IContactInformationModalInputCtrlScope;
     }

     export interface IContactInformationModalInputCtrlScope extends ng.IScope{
         contactInformationModel: Dtos.Person.ContactInformationModel;
         bestTimeToContactByPhone: Array<Common.Enums.TimeOfDayEnum>;

         ok(form: ng.IFormController): void;
         cancel(): void;
     }        

     export class ContactInformationModalInputCtrl implements IContactInformationModalInputCtrl {

         static $inject = ['$scope', '$modalInstance', 'contactInformationModel'];

         constructor(
             public $scope: IContactInformationModalInputCtrlScope,
             private $modalInstance: ng.ui.bootstrap.IModalServiceInstance,
             private contactInformationModel: Dtos.Person.ContactInformationModel) {

             $scope.contactInformationModel = contactInformationModel;

             $scope.cancel = () => {
                 $modalInstance.dismiss();
             };

             $scope.ok = (form: ng.IFormController) => {
                 this.ok(form);
             };

             $scope.bestTimeToContactByPhone =
             [
                 Common.Enums.TimeOfDayEnum.Morning,
                 Common.Enums.TimeOfDayEnum.Noon,
                 Common.Enums.TimeOfDayEnum.Afternoon,
                 Common.Enums.TimeOfDayEnum.Evening,
                 Common.Enums.TimeOfDayEnum.Anytime
             ];
         }

         private ok(formController: ng.IFormController) {
             if (formController.$valid) {
                 this.$modalInstance.close(this.$scope.contactInformationModel);
             }
         }
     }
 }

module BohFoundation.Main {
    Register.Person.controller("ContactInformationModalInputCtrl", Person.ContactInformation.Controllers.ContactInformationModalInputCtrl);
}