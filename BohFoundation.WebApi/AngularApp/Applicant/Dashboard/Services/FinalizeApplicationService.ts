 module BohFoundation.Applicant.Dashboard.Services {
     export interface IFinalizeApplicationService {
         postFinalizeApplication(): ng.resource.IResource<any>;
         resolveFinalizeApplication(serverResponse: Common.Models.ServerResponseModel): void;

         canFinalizeEssay(): boolean;
     }

     export class FinalizeApplicationService implements IFinalizeApplicationService {
         private finalizeApiEndpoint = '/api/applicant/finalize';

         static $inject = ['AuthRequiredRepository', 'AlertHelperService', 'ApplicantNotificationService', 'UserInformationService'];
         constructor(
             private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
             private alertHelperService: Common.Services.UiServices.IAlertHelperServices,
             private applicantNotificationService: Applicant.Dashboard.Services.IApplicantNotificationService,
             private userInformationService: Common.Services.UserInformation.IUserInformationService) { }


         postFinalizeApplication(): ng.resource.IResource<any> {
             if (this.canFinalizeEssay()) {
                 return this.authRequiredRepository.post(true, this.finalizeApiEndpoint);
             }
         }

         resolveFinalizeApplication(serverResponse: Common.Models.ServerResponseModel): void {
             if (serverResponse.success) {
                 this.alertHelperService.addSuccessAlert("You have finalized your application to the Bulldog Scholarship.");
                 this.userInformationService.removeTokenFromLocalStorage();
                 this.userInformationService.logOut("/Applicant/Finalized");
             } else {
                 this.alertHelperService.addDangerAlert("We were unable to finalize your application at this time. Please try again later.");
             }
         }

         canFinalizeEssay(): boolean {
             if (this.allSuccessStyles()) {
                 return true;
             } else {
                 return false;
             }
         }

         private allSuccessStyles() {
             return this.isSuccessStyle(this.applicantNotificationService.getPersonalInformationDashboardInputModel())
                 && this.isSuccessStyle(this.applicantNotificationService.getContactInformationDashboardInputModel())
                 && this.isSuccessStyle(this.applicantNotificationService.getTranscriptUploadedDashboardInputModel())
                 && this.isSuccessStyle(this.applicantNotificationService.getAcademicInformationDashboardInputModel())
                 && this.isSuccessStyle(this.applicantNotificationService.getReferenceDashboardInputModels())
                 && this.isSuccessStyle(this.applicantNotificationService.getFamilyInformationDashboardInputModel())
                 && this.isSuccessStyle(this.applicantNotificationService.getExtracurricularActivitiesDashboardInputModel())
                 && this.areEssaysSuccesses()
                 && this.lowGradeSuccess();
         }

         private isSuccessStyle(dashboardInputModel: Models.ApplicantDashboardListItemInputModel) {
             if (dashboardInputModel.buttonStyle == Common.Enums.StyleEnum.Success) {
                 return true;
             } else {
                 return false;
             }
         }

         private areEssaysSuccesses() {
             var success = true;
             angular.forEach(this.applicantNotificationService.getEssayDashboardInputModels(), (model) => {
                 if (success) {
                     success = this.isSuccessStyle(model);
                 }
             });
             return success;
         }

         private lowGradeSuccess() {
             if (this.applicantNotificationService.getHideLowGradeInformation()) {
                 return true;
             } else {
                 return this.isSuccessStyle(this.applicantNotificationService.getLowGradeInformationDashboardInputModel());
             }
         }
     }
 }

 module BohFoundation.Main {
     Register.Applicant.service("FinalizeApplicationService", Applicant.Dashboard.Services.FinalizeApplicationService);
 }