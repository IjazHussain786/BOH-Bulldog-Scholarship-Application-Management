module BohFoundation.Reference.LetterOfRecommendation.Anon.Services {
    export interface IRecommendAnonService {
        setGuidOfLetterOfRecommendation(guid: string) : void;
        getInformationForReferenceFormModel(): Dtos.Reference.Anonymous.InformationForReferenceFormModel;

        getLetterOfRecommendationInformation(): ng.resource.IResource<any>;
        resolveGetLetterOfRecommendationInformation(serverMessage: Common.Models.ServerResponseModel): void;

        postReferencePersonalInformation(personalInformation: Dtos.Reference.Anonymous.ReferencePersonalInformationModel): ng.resource.IResource<any>;
        resolvePostReferencePersonalInformation(serverMessage: Common.Models.ServerResponseModel): void;

        postLetterOfRecommendation(letterOfRecommendation: string): ng.resource.IResource<any>;
        resolvePostLetterOfRecommendation(serverMessage: Common.Models.ServerResponseModel) : void;
    }

    export class RecommendAnonService implements IRecommendAnonService {
        private letterOfRecommendationApiEndpoint = "/api/references/anon/letterofrecommendation";
        private letterOfRecommendationInformationApiEndpoint = "/api/references/anon/letterofrecommendationinformation";
        private letterOfRecommendationInformationType = "letter of recommendation";
        private referencePersonalInformationType = "personal information";

        private guidOfLetterOfRecommendation: string;
        private informationForReferenceFormModel : Dtos.Reference.Anonymous.InformationForReferenceFormModel;

        static $inject = ['AllowAnonymousRepository', 'GenericResolver'];
        constructor(
            private allowAnonymousRepository: Common.Repositories.IAllowAnonymousRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }

        setGuidOfLetterOfRecommendation(guid: string): void {
            this.guidOfLetterOfRecommendation = guid;
        }
        
        getInformationForReferenceFormModel() {
            return this.informationForReferenceFormModel;
        }

        getLetterOfRecommendationInformation() {
            return this.allowAnonymousRepository.get(this.letterOfRecommendationInformationApiEndpoint + "/" + this.guidOfLetterOfRecommendation);
        }

        resolveGetLetterOfRecommendationInformation(serverMessage: Common.Models.ServerResponseModel): void {
            var dataFromServer = this.genericResolver.genericGetResolver(this.letterOfRecommendationInformationType, serverMessage);
            if (serverMessage.success) {
                this.informationForReferenceFormModel = dataFromServer;
            }
        }

        postReferencePersonalInformation(personalInformation: Dtos.Reference.Anonymous.ReferencePersonalInformationModel) {
            personalInformation.guidSentToReference = this.guidOfLetterOfRecommendation;
            return this.allowAnonymousRepository.post(personalInformation, this.letterOfRecommendationInformationApiEndpoint);
        }

        resolvePostReferencePersonalInformation(serverMessage: Common.Models.ServerResponseModel): void {
            this.genericResolver.genericPostResolver(this.referencePersonalInformationType, serverMessage);
        }

        postLetterOfRecommendation(letterOfRecommendation: string) {
            return this.allowAnonymousRepository.post(new Dtos.Reference.Anonymous.LetterOfRecommendationModel(letterOfRecommendation, this.guidOfLetterOfRecommendation), this.letterOfRecommendationApiEndpoint);
        }

        resolvePostLetterOfRecommendation(serverMessage: Common.Models.ServerResponseModel): void {
            this.genericResolver.genericPostResolver(this.letterOfRecommendationInformationType, serverMessage);
        }
    }
}

module BohFoundation.Main {
    Register.Reference.service('RecommendAnonService', Reference.LetterOfRecommendation.Anon.Services.RecommendAnonService);
}