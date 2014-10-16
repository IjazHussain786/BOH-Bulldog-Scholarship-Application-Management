module BohFoundation.Applicant.Dashboard.Services {
    export interface IApplicantNotificationService {
        getNotifications(): ng.resource.IResource<any>;
        resolveGetNotifications(serverResponseModel: Common.Models.ServerResponseModel): void;
        getDeadlineDate(): Date;

        getPersonalInformationDashboardInputModel(): Models.ApplicantDashboardListItemInputModel;
        getContactInformationDashboardInputModel(): Models.ApplicantDashboardListItemInputModel;
        getTranscriptUploadedDashboardInputModel(): Models.ApplicantDashboardListItemInputModel;
        getAcademicInformationDashboardInputModel(): Models.ApplicantDashboardListItemInputModel;
        getLowGradeInformationDashboardInputModel(): Models.ApplicantDashboardListItemInputModel;
        getEssayDashboardInputModels(): Array<Models.ApplicantDashboardListItemInputModel>;
        getReferenceDashboardInputModels(): Models.ApplicantDashboardListItemInputModel;
        getFamilyInformationDashboardInputModel(): Models.ApplicantDashboardListItemInputModel;
        getExtracurricularActivitiesDashboardInputModel() : Models.ApplicantDashboardListItemInputModel;

        getHideLowGradeInformation(): boolean;
    }   
    
    export class ApplicantNotificationService implements IApplicantNotificationService {

        private applicantNotificationsApiEndpoint = '/api/applicant/notifications';
        private deadlineDate: Date;

        private personalInformationDashboardInputModel: Models.ApplicantDashboardListItemInputModel; 
        private contactInformationDashboardInputModel: Models.ApplicantDashboardListItemInputModel;
        private transciptUploadedDashboardInputModel: Models.ApplicantDashboardListItemInputModel;
        private academicInformationDashboardInputModel: Models.ApplicantDashboardListItemInputModel;
        private lowGradeDashboadInputModel: Models.ApplicantDashboardListItemInputModel;
        private essayDashboardInputModels: Array<Models.ApplicantDashboardListItemInputModel> = [];
        private referenceDashboardInputModels: Models.ApplicantDashboardListItemInputModel;
        private familyInformationDashboardInputModel: Models.ApplicantDashboardListItemInputModel;
        private extracurricularActivitiesDashboardInputModel: Models.ApplicantDashboardListItemInputModel;

        private hideLowGradeInformation: boolean;

        static $inject = ['AuthRequiredRepository', 'AlertHelperService', 'ApplicantEssayService'];
        constructor(private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private alertHelperService: Common.Services.UiServices.IAlertHelperServices,
            private applicantEssayService: Essays.Services.IApplicantEssayService) {

            this.personalInformationDashboardInputModel = new Models.ApplicantDashboardListItemInputModel("Personal Information", "/Applicant/PersonalInformation");
            this.contactInformationDashboardInputModel = new Models.ApplicantDashboardListItemInputModel("Contact Information", "/Person/ContactInformation");
            this.transciptUploadedDashboardInputModel = new Models.ApplicantDashboardListItemInputModel("Upload Transcript", "/Applicant/Transcript");
            this.academicInformationDashboardInputModel = new Models.ApplicantDashboardListItemInputModel("Academic Information", "/Applicant/AcademicInformation");
            this.lowGradeDashboadInputModel = new Models.ApplicantDashboardListItemInputModel("Low Grades Explanations", "/Applicant/LowGrades");
            this.referenceDashboardInputModels = new Models.ApplicantDashboardListItemInputModel("References", "/Applicant/References");
            this.familyInformationDashboardInputModel = new Models.ApplicantDashboardListItemInputModel("Family Information", "/Applicant/FamilyInformation");
            this.extracurricularActivitiesDashboardInputModel = new Models.ApplicantDashboardListItemInputModel("Extracurriculars", "/Applicant/Extracurriculars");
        }

        getNotifications() {
            return this.authRequiredRepository.get(this.applicantNotificationsApiEndpoint);
        }

        resolveGetNotifications(serverResponseModel: Common.Models.ServerResponseModel): void {
            if (serverResponseModel.success) {
                this.createDashboardModels(serverResponseModel.dataFromServer);
            } else {
                this.alertHelperService.addDangerAlert("There was a problem getting your notifications.");
            }
        }
        
        getHideLowGradeInformation() {
            return this.hideLowGradeInformation;
        }

        getPersonalInformationDashboardInputModel(): Models.ApplicantDashboardListItemInputModel {
            return this.personalInformationDashboardInputModel;
        }

        getContactInformationDashboardInputModel(): Models.ApplicantDashboardListItemInputModel {
            return this.contactInformationDashboardInputModel;
        }

        getTranscriptUploadedDashboardInputModel() {
            return this.transciptUploadedDashboardInputModel;
        }

        getAcademicInformationDashboardInputModel() {
            return this.academicInformationDashboardInputModel;
        }

        getLowGradeInformationDashboardInputModel() {
            return this.lowGradeDashboadInputModel;
        }

        getEssayDashboardInputModels() {
            return this.essayDashboardInputModels;
        }

        getReferenceDashboardInputModels() {
            return this.referenceDashboardInputModels;
        }

        getFamilyInformationDashboardInputModel() {
            return this.familyInformationDashboardInputModel;
        }

        getExtracurricularActivitiesDashboardInputModel() {
            return this.extracurricularActivitiesDashboardInputModel;
        }

        getDeadlineDate() {
            
            return this.deadlineDate;
        }

        private createDashboardModels(dataFromServer: Dtos.Applicant.ApplicantNotificationsModel): void {
            this.createDashboardModel(dataFromServer.lastUpdatedPersonalInformation, this.personalInformationDashboardInputModel);
            this.createDashboardModel(dataFromServer.lastUpdatedContactInformation, this.contactInformationDashboardInputModel);
            this.createDashboardModel(dataFromServer.lastUpdatedTranscriptUpload, this.transciptUploadedDashboardInputModel);
            this.createDashboardModel(dataFromServer.lastUpdatedAcademicInformation, this.academicInformationDashboardInputModel);
            this.createDashboardModel(dataFromServer.lastUpdatedFamilyInformation, this.familyInformationDashboardInputModel);
            this.createDashboardModel(dataFromServer.lastUpdatedExtracurriculars, this.extracurricularActivitiesDashboardInputModel);

            this.createLowGradeDashboardModel(dataFromServer.lowGradeNotificationInformation);
            this.createEssayDashboardModels(dataFromServer.essayNotifications);

            this.pushEssayNotificationsOntoEssayService(dataFromServer.essayNotifications);
            this.createReferenceDashboardInputModel(dataFromServer.applicantReferenceCounts);

            this.deadlineDate = dataFromServer.deadlineDate;
        }

        private createDashboardModel(date: Date, modelUpdating : Models.ApplicantDashboardListItemInputModel) {
            if (date == null) {
                modelUpdating.buttonStyle = Common.Enums.StyleEnum.Warning;
            } else {
                modelUpdating.buttonStyle = Common.Enums.StyleEnum.Success;
                modelUpdating.lastUpdated = date;
            }
        }

        private createLowGradeDashboardModel(lowGradeNotificationInformationModelFromServer: Dtos.Applicant.Notifications.ILowGradeNotificationInformationModel) {
            var lowGradeNotificationModel = new Dtos.Applicant.Notifications.LowGradeNotificationInformationModel(lowGradeNotificationInformationModelFromServer.numberOfLowGradesInformationSaved, lowGradeNotificationInformationModelFromServer.gpa, lowGradeNotificationInformationModelFromServer.lastUpdatedLowGrade);

            this.setHideLowGradeInformation(lowGradeNotificationModel);
            this.setLowGradeStyling(lowGradeNotificationModel);
        }

        private setHideLowGradeInformation(lowGradeNotificationInformationModel: Dtos.Applicant.Notifications.ILowGradeNotificationInformationModel) {
            if (lowGradeNotificationInformationModel.gpa == null || lowGradeNotificationInformationModel.numberOfLowGradeExplainationsNeeded.totalExplanationsNeeded == 0) {
                this.hideLowGradeInformation = true;
            } else {
                this.hideLowGradeInformation = false;
            }
        }

        private setLowGradeStyling(lowGradeNotificationModel: Dtos.Applicant.Notifications.ILowGradeNotificationInformationModel) {
            if (!this.hideLowGradeInformation) {
                if (lowGradeNotificationModel.lowGradesNeededOutstanding > 0) {
                    this.lowGradeDashboadInputModel.buttonStyle = Common.Enums.StyleEnum.Warning;
                } else {
                    this.lowGradeDashboadInputModel.buttonStyle = Common.Enums.StyleEnum.Success;
                }

                if(lowGradeNotificationModel.lastUpdatedLowGrade != null) {
                    this.lowGradeDashboadInputModel.lastUpdated = lowGradeNotificationModel.lastUpdatedLowGrade;
                }
            }
        }

        private createEssayDashboardModels(essayNotifications: Array<Dtos.Applicant.Notifications.EssayNotificationsModel>) {
            this.essayDashboardInputModels = [];
            if (essayNotifications.length > 0) {
                angular.forEach(essayNotifications, essayNotification => this.addEssayDashboardInputModel(essayNotification));
            }
            
        }

        private addEssayDashboardInputModel(essayNotification: Dtos.Applicant.Notifications.EssayNotificationsModel) {
            this.essayDashboardInputModels.push(this.createEssayDashboardItem(essayNotification));
        }

        private createEssayDashboardItem(essayNotification: Dtos.Applicant.Notifications.EssayNotificationsModel) {
            var dashboardModel = new Models.ApplicantDashboardListItemInputModel(essayNotification.titleOfEssay, "/Applicant/Essay/" + essayNotification.essayTopicId);
            if (essayNotification.revisionDateTime == null) {
                dashboardModel.buttonStyle = Common.Enums.StyleEnum.Warning;
            } else {
                dashboardModel.buttonStyle = Common.Enums.StyleEnum.Success;
                dashboardModel.lastUpdated = essayNotification.revisionDateTime;
            }
            return dashboardModel;
        }

        private pushEssayNotificationsOntoEssayService(essayNotifications: Array<Dtos.Applicant.Notifications.EssayNotificationsModel>) {
            this.applicantEssayService.setArrayOfEssayNotificationsModelFromNotificationService(essayNotifications);
        }

        private createReferenceDashboardInputModel(applicantReferenceCountsModel: Dtos.Applicant.Notifications.ApplicantReferenceCountsModel) {
            if (applicantReferenceCountsModel.numberOfReferencesRecieved == 3) {
                this.referenceDashboardInputModels.buttonStyle = Common.Enums.StyleEnum.Success;
            } else if (applicantReferenceCountsModel.numberOfReferenceInvitationsSent == 3) {
                this.referenceDashboardInputModels.buttonStyle = Common.Enums.StyleEnum.Info;
            } else {
                this.referenceDashboardInputModels.buttonStyle = Common.Enums.StyleEnum.Warning;
            }

            if(applicantReferenceCountsModel.lastUpdated != null) {
                this.referenceDashboardInputModels.lastUpdated = applicantReferenceCountsModel.lastUpdated;
            }
        }
    } 
}

module BohFoundation.Main {
    Register.Applicant.service("ApplicantNotificationService", Applicant.Dashboard.Services.ApplicantNotificationService);
}