 module BohFoundation.Applicant.LowGrades.Controllers {
    
     export interface ILowGradesCtrlScope extends ng.IScope {
         processing: boolean;

         getLowGradeArray(): Array<Dtos.Applicant.Academic.LowGradeModel>;
         getGpa(): number;
         getNumberOfLowGradesTotalNeeded(): number;
         getNumberOfLowGradesRemaining(): number;
         getLastUpdated(): Date;
         getArraySameAsServer():boolean;

         newLowGradeModel: Dtos.Applicant.Academic.LowGradeModel;
         
         addLowGrade(): void;
         editLowGrade(index: number): void;
         deleteLowGrade(index: number): void;

         postLowGrades(): void;
         modalInstance: any;
     }

     export interface ILowGradeCtrl {
         $scope: ILowGradesCtrlScope;
     }

     export class LowGradesCtrl implements ILowGradeCtrl{
        
         private typeOfOperation: Common.Enums.CrudEnum;
         private indexOfOperation: number;

         static $inject = ['$scope', '$modal', 'LowGradesService'];   
         constructor(public $scope: ILowGradesCtrlScope,
             private $modal: any,
             private lowGradesService: Services.ILowGradesService) {

             this.getLowGrades();

             $scope.getLowGradeArray = () => {
                 return this.lowGradesService.getLowGradesArray();
             };
             
             $scope.addLowGrade = () => {
                 this.addLowGrade();
             };

             $scope.editLowGrade = (index: number) => {
                 this.editLowGrade(index);
             };

             $scope.deleteLowGrade = (index: number) => {
                 this.lowGradesService.deleteLowGrade(index);
             };

             $scope.postLowGrades = () => {
                 if (!$scope.processing && !$scope.getArraySameAsServer()) {
                     this.postLowGrades();
                 }
             };

             $scope.getGpa = () => {
                 return this.getLowGradeNotificationInformationModel().gpa;
             }

             $scope.getNumberOfLowGradesTotalNeeded = () => {
                 return this.getLowGradeNotificationInformationModel().numberOfLowGradeExplainationsNeeded.totalExplanationsNeeded;
             }

             $scope.getNumberOfLowGradesRemaining = () => {
                 return this.getLowGradeNotificationInformationModel().lowGradesNeededOutstanding;
             }

             $scope.getLastUpdated = () => {
                 return this.getLowGradeNotificationInformationModel().lastUpdatedLowGrade;
             }

             $scope.getArraySameAsServer = () => {
                 return this.lowGradesService.getArraySameAsServer();
             }

         }

         private getLowGradeNotificationInformationModel(): Dtos.Applicant.Notifications.ILowGradeNotificationInformationModel {
             return this.lowGradesService.getLowGradeNotificationInformationModel();
         }

         private getLowGrades() {
             this.flipProcessing();
             var resource = this.lowGradesService.getLowGrades();
             resource.$promise.then((data) => {
                 this.resolveGetLowGrades(new Common.Models.ServerResponseModel(data, true));
                 this.flipProcessing();
             },() => {
                 this.resolveGetLowGrades(new Common.Models.ServerResponseModel(null, false));
             });
         }

         private resolveGetLowGrades(serverResponseModel: Common.Models.ServerResponseModel): void {
             this.lowGradesService.resolveGetLowGrades(serverResponseModel);
         }

         private flipProcessing() { this.$scope.processing = !this.$scope.processing; }

         private addLowGrade() {
             this.typeOfOperation = Common.Enums.CrudEnum.Add;
             this.$scope.newLowGradeModel = new Dtos.Applicant.Academic.LowGradeModel();
             this.openModal();
         }

         private editLowGrade(index: number) {
             this.typeOfOperation = Common.Enums.CrudEnum.Update;
             this.$scope.newLowGradeModel = this.$scope.getLowGradeArray()[index];
             this.indexOfOperation = index;
             this.openModal();
         }

         private openModal() {
             this.$scope.modalInstance = this.$modal.open({
                 templateUrl: '/AngularApp/Applicant/LowGrades/Templates/LowGradeModal.html',
                 controller: 'LowGradeModalCtrl',
                 resolve: {
                     lowGradeModel: () => this.$scope.newLowGradeModel
                 }
             });

             this.$scope.modalInstance.result.then(
                 newLowGradeModel => {
                     this.$scope.newLowGradeModel = newLowGradeModel;
                     this.takeActionFromSuccessfulModal();
                 },
                 () => {
                     this.cleanUpModelTypeOfOperationAndIndex();
                 });
         }

         private takeActionFromSuccessfulModal() {
             switch (this.typeOfOperation) {
                 case Common.Enums.CrudEnum.Add:
                     this.lowGradesService.addLowGrade(this.$scope.newLowGradeModel);
                     break;
                 case Common.Enums.CrudEnum.Update:
                     this.lowGradesService.modifyLowGrade(this.indexOfOperation, this.$scope.newLowGradeModel);
                     break;
             }
             this.cleanUpModelTypeOfOperationAndIndex();
         }

         private cleanUpModelTypeOfOperationAndIndex() {
             this.$scope.newLowGradeModel = null;
             this.typeOfOperation = null;
             this.indexOfOperation = null;
         }

         private postLowGrades() {
             this.flipProcessing();
             var resource = this.lowGradesService.postLowGrades();
             resource.$promise.then(() => {
                 this.resolvePost(true);
             }, () => {
                 this.resolvePost(false);
             });
         }

         private resolvePost(success: boolean) {
             this.lowGradesService.resolvePostLowGrades(new Common.Models.ServerResponseModel(null, success));
             this.flipProcessing();
         }
     }
 }

module BohFoundation.Main {
    Register.Applicant.controller('LowGradesCtrl', Applicant.LowGrades.Controllers.LowGradesCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when("/Applicant/LowGrades", {
                    templateUrl: '/AngularApp/Applicant/LowGrades/Templates/LowGrades.html',
                    controller: 'LowGradesCtrl',
                    publicAccess: false,
                    title: 'Low Grade Explanation'
                });
            })]);
}