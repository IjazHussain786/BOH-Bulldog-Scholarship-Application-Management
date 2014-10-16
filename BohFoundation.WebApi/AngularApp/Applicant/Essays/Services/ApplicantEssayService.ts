module BohFoundation.Applicant.Essays.Services {
    export interface IApplicantEssayService {
        getEssayFromServer(essayTopicId: number): ng.resource.IResource<any>;
        resolveGetEssayFromServer(serverResponse: Common.Models.ServerResponseModel): void;
        postEssay(essayModelToSave: Dtos.Applicant.Essay.EssayModel): ng.resource.IResource<any>;
        resolvePostEssay(serverResponse: Common.Models.ServerResponseModel): void;
        
        setArrayOfEssayNotificationsModelFromNotificationService(array: Array<Dtos.Applicant.Notifications.EssayNotificationsModel>): void;

        getEssayModel(essayTopicId: number): Dtos.Applicant.Essay.EssayModel;
        
    }

    export class ApplicantEssayService implements IApplicantEssayService {

        private apiEndpoint = "/api/applicant/essay";
        private typeOfData = "essay";
        private arrayOfEssayNotificationModels: Array<Dtos.Applicant.Notifications.EssayNotificationsModel> = [];
        private currentEssayModelFromServer: Dtos.Applicant.Essay.EssayModel;
        private getEssayFromServerHasBeenResolved = false;

        static $inject = ['AuthRequiredRepository', '$location', 'GenericResolver'];

        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private $location: ng.ILocationService,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }

        getEssayFromServer(essayTopicId: number): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.apiEndpoint + "/" + essayTopicId);
        }

        resolveGetEssayFromServer(serverResponse: Common.Models.ServerResponseModel): void {
            this.currentEssayModelFromServer = this.genericResolver.genericGetResolver(this.typeOfData, serverResponse);
            if (serverResponse.success) {
                this.getEssayFromServerHasBeenResolved = true;
            } 
        }

        postEssay(essayModelToSave: Dtos.Applicant.Essay.EssayModel): ng.resource.IResource<any> {
            return this.authRequiredRepository.post(essayModelToSave, this.apiEndpoint);
        }

        resolvePostEssay(serverResponse: Common.Models.ServerResponseModel): void {
            this.genericResolver.genericPostResolver(this.typeOfData, serverResponse);
        }

        setArrayOfEssayNotificationsModelFromNotificationService(array: Array<Dtos.Applicant.Notifications.EssayNotificationsModel>): void {
            this.arrayOfEssayNotificationModels = array;
        }
        
        getEssayModel(essayTopicId: number) : Dtos.Applicant.Essay.EssayModel {
            if (this.currentEssayModelFromServer == null || this.currentEssayModelFromServer == undefined) {
                return this.createEssayModelFromNotifications(essayTopicId);
            }
            return this.currentEssayModelFromServer;
        }

        private createEssayModelFromNotifications(essayTopicId: number) {
            var indexOfItem = this.arrayObjectIndexOf(this.arrayOfEssayNotificationModels, essayTopicId);
            if (indexOfItem > -1) { 
                var essayNotificationModel: Dtos.Applicant.Notifications.EssayNotificationsModel = this.arrayOfEssayNotificationModels[indexOfItem];
                return new Dtos.Applicant.Essay.EssayModel(essayNotificationModel.essayPrompt, essayTopicId);
            }else if (!this.getEssayFromServerHasBeenResolved) {
                return new Dtos.Applicant.Essay.EssayModel();
            }
            this.forwardToDashboardIfNoEssayFromServerAndArrayOfEssayNotificationModelsIsBlank();
        }

        //todo: add to a utilities class.
        private arrayObjectIndexOf(myArray: Array<any>, searchTerm: any) {
            return myArray.map(e => e.essayTopicId).indexOf(searchTerm);
        }

        private forwardToDashboardIfNoEssayFromServerAndArrayOfEssayNotificationModelsIsBlank() {
            this.$location.path("/Applicant");
        }

    }
}

module BohFoundation.Main {
    Register.Applicant.service("ApplicantEssayService", Applicant.Essays.Services.ApplicantEssayService);
}