module BohFoundation.Admin.LetterOfRecommendation.Services {
    export interface IAdminLetterOfRecommendationService {
        getGuidSentToReference(): Dtos.Admin.References.GuidSentToReferenceModel;

        getGuidSentToReferenceFromServer(model: Dtos.Admin.References.GetLetterOfRecommendationGuidModel): ng.resource.IResource<any>;
        resolveGetGuidSentToReferenceFromServer(serverMessage: Common.Models.ServerResponseModel): void;

        canGoToReference(): boolean;
        goToReferenceForm(): void;
    }

    export class AdminLetterOfRecommendationService implements IAdminLetterOfRecommendationService {
        private pathToAnonReference = '/Reference/LetterOfRecommendation/Anon/';
        private apiEndpoint = "/api/admin/letterofrecommendation/getguid";
        private typeOfData = "letter of recommendation id";

        private guidSentToReference: Dtos.Admin.References.GuidSentToReferenceModel;

        static $inject = ['AuthRequiredRepository', '$location', 'GenericResolver'];

        constructor(
            private authRequiredRepo: Common.Repositories.IAuthRequiredRepository,
            private $location: ng.ILocationService,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) {
        }

        getGuidSentToReference(): Dtos.Admin.References.GuidSentToReferenceModel {
            return this.guidSentToReference;
        }

        getGuidSentToReferenceFromServer(model: Dtos.Admin.References.GetLetterOfRecommendationGuidModel): ng.resource.IResource<any> {
            return this.authRequiredRepo.post(model, this.apiEndpoint);
        }

        resolveGetGuidSentToReferenceFromServer(serverMessage: Common.Models.ServerResponseModel): void {
            this.guidSentToReference = this.genericResolver.genericGetResolver(this.typeOfData, serverMessage);
        }

        canGoToReference(): boolean {
            if (this.guidSentToReference != undefined) {
                if (this.guidSentToReference.guidSentToReference != null) {
                    if (this.guidSentToReference.guidSentToReference != '00000000-0000-0000-0000-000000000000') {
                        return true;
                    }
                }
            }
            return false;
        }

        goToReferenceForm(): void { 
            if (this.canGoToReference()) {
                this.$location.path(this.pathToAnonReference + this.guidSentToReference.guidSentToReference);
            }
        }
    }
} 

module BohFoundation.Main {
    Register.Admin.service("AdminLetterOfRecommendationService", Admin.LetterOfRecommendation.Services.AdminLetterOfRecommendationService);
}