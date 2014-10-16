module BohFoundation.Applicant.FamilyInformation.Services {
    export interface IFamilyInformationService {
        getFamilyInformationModel(): Dtos.Applicant.Family.FamilyInformationModel;

        getFamilyInformationFromServer(): ng.resource.IResource<any>;
        resolveGetFamilyInformationFromServer(serverResponse: Common.Models.ServerResponseModel): void;

        postFamilyInformation(familyInformation: Dtos.Applicant.Family.FamilyInformationModel): ng.resource.IResource<any>;
        resolvePostFamilyInformation(serverResponse: Common.Models.ServerResponseModel): void;
    }

    export class FamilyInformationService implements IFamilyInformationService {
        private apiEndpoint = "/api/applicant/familyinformation";
        private typeOfData = "family information";
        private familyInformationModel: Dtos.Applicant.Family.FamilyInformationModel;

        static $inject = ['AuthRequiredRepository','GenericResolver'];

        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }

        getFamilyInformationModel(): Dtos.Applicant.Family.FamilyInformationModel {
            return this.familyInformationModel;
        }

        getFamilyInformationFromServer() {
            return this.authRequiredRepository.get(this.apiEndpoint);
        }

        resolveGetFamilyInformationFromServer(serverResponse: Common.Models.ServerResponseModel): void {
            this.familyInformationModel = this.genericResolver.genericGetResolver(this.typeOfData, serverResponse);
        }

        postFamilyInformation(familyInformation: Dtos.Applicant.Family.FamilyInformationModel): any {
            return this.authRequiredRepository.post(familyInformation, this.apiEndpoint);
        }

        resolvePostFamilyInformation(serverResponse: Common.Models.ServerResponseModel): void {
            this.genericResolver.genericPostResolver(this.typeOfData, serverResponse);
        }

    }
    
} 

module BohFoundation.Main {
    Register.Applicant.service("FamilyInformationService", Applicant.FamilyInformation.Services.FamilyInformationService);
}