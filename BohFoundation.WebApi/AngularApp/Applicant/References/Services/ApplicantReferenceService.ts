module BohFoundation.Applicant.References.Services {
    export interface IApplicantReferenceService {
        getSubmittedReferencesFromServer(): ng.resource.IResource<any>;
        resolveGetSubmittedReference(serverResponse: Common.Models.ServerResponseModel): void;
        postReferenceRequest(model: Dtos.Applicant.References.ApplicantReferenceInputModel): ng.resource.IResource<any>;
        resolvePostReferenceRequest(serverResponse: Common.Models.ServerResponseModel): void;  

        getArrayOfReferenceModels(): Array<Dtos.Applicant.References.ReferenceModel>;
    }

    export class ApplicantReferenceService implements IApplicantReferenceService {
        private apiEndpoint = "/api/applicant/references"; 
        private typeOfData = "reference";
        private arrayOfReferenceModels: Array<Dtos.Applicant.References.ReferenceModel> = [];

        static $inject = ['AuthRequiredRepository', 'GenericResolver'];

        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }

        getSubmittedReferencesFromServer(): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.apiEndpoint);
        }

        resolveGetSubmittedReference(serverResponse: Common.Models.ServerResponseModel): void {
            this.arrayOfReferenceModels = this.genericResolver.genericGetResolver(this.typeOfData, serverResponse);
        }

        postReferenceRequest(model: Dtos.Applicant.References.ApplicantReferenceInputModel): ng.resource.IResource<any>{
            return this.authRequiredRepository.post(model, this.apiEndpoint);
        }

        resolvePostReferenceRequest(serverResponse: Common.Models.ServerResponseModel): void {
            this.genericResolver.genericPostResolver(this.typeOfData, serverResponse);
        }

        getArrayOfReferenceModels(): Array<Dtos.Applicant.References.ReferenceModel> {
            return this.arrayOfReferenceModels;
        }
    } 
 }

module BohFoundation.Main {
    Register.Applicant.service("ApplicantReferenceService", Applicant.References.Services.ApplicantReferenceService);
}