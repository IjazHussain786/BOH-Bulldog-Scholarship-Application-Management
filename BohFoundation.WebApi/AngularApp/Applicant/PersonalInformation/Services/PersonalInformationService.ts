 module BohFoundation.Applicant.PersonalInformation.Services {
     'use strict';
     export interface IPersonalInformationService {

         fullName: string;
         applicantPersonalInformation: Dtos.Applicant.PersonalInformation.ApplicantPersonalInformationModel;

         getPersonalInformation(): ng.resource.IResource<any>;
         resolveGetPersonalInformation(result: Common.Models.ServerResponseModel):void;

         postPersonalInformation(
             personalInformationModel: Dtos.Applicant.PersonalInformation.ApplicantPersonalInformationModel): any;

         resolvePostPersonalInformation(result: Common.Models.ServerResponseModel): void;
     }

     export class PersonalInformationService implements IPersonalInformationService {

         fullName: string;

         applicantPersonalInformation: Dtos.Applicant.PersonalInformation.ApplicantPersonalInformationModel;

         private typeOfInformation = "personal";
         private applicantPersonalInformationApiEndpoint = "/api/applicant/personalinformation";

         static $inject = ['AuthRequiredRepository', 'UserInformationService', 'GenericResolver'];

         constructor(private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
             private userInformationService: Common.Services.UserInformation.IUserInformationService,
             private genericResolver: Common.Services.Resolvers.IGenericResolver) {

             this.fullName = userInformationService.getFullName();
         }

         getPersonalInformation() {
             return this.authRequiredRepository.get(this.applicantPersonalInformationApiEndpoint);
         }

         resolveGetPersonalInformation(result: Common.Models.ServerResponseModel): void {
             var dataFromServer = this.genericResolver.genericGetResolver(this.typeOfInformation, result);
             if (result.success) {
                 this.getSuccessResult(dataFromServer);
             } 
         }

         postPersonalInformation(personalInformationModel: Dtos.Applicant.PersonalInformation.ApplicantPersonalInformationModel) {
             return this.authRequiredRepository.post(personalInformationModel, this.applicantPersonalInformationApiEndpoint);
         }

         resolvePostPersonalInformation(result: Common.Models.ServerResponseModel): void {
             if (result.success) {
                 this.postSuccessResult();
             }
             this.genericResolver.genericPostResolver(this.typeOfInformation, result);
         }

         private getSuccessResult(data: any) {
             var dataFromServer = data;
             if (dataFromServer == null) {
                 this.applicantPersonalInformation = new Dtos.Applicant.PersonalInformation.ApplicantPersonalInformationModel(this.userInformationService.getGraduatingYear());
             } else {
                 this.applicantPersonalInformation = new Dtos.Applicant.PersonalInformation.ApplicantPersonalInformationModel(dataFromServer.graduatingYear, dataFromServer.birthdate, dataFromServer.lastUpdated);
             }
         }

         private postSuccessResult() {
             this.applicantPersonalInformation.lastUpdated = new Date();
         }
     }
 }

module BohFoundation.Main {
    Register.Applicant.service("PersonalInformationService", Applicant.PersonalInformation.Services.PersonalInformationService);
}