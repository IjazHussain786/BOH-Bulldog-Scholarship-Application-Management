module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.GetEssayAndLettersOfRecommendation {
    export interface IGetLetterOfRecommendationForEvaluatorService {

        getLetterOfRecommedation(): string;
        getLetterOfRecommendationFromServer(letterOfRecommendationKey: Dtos.Common.AzureQueuryObjects.AzureTableStorageEntityKeyModel): ng.resource.IResource<any>;
        resolveGetLetterOfRecommendation(serverMessage: Common.Models.ServerResponseModel): void;

    }

    export class GetLetterOfRecommendationForEvaluatorService implements IGetLetterOfRecommendationForEvaluatorService {
        static $inject = ['AuthRequiredRepository', 'GenericResolver'];
        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }

        private apiEndpoint = "/api/applicationevaluator/evaluatingapplicants/displayapplication/letterofrecommendation/partitionkey/";
        private letterOfRecommendation: Dtos.Reference.Anonymous.LetterOfRecommendationModel;

        getLetterOfRecommedation() {
            if (this.letterOfRecommendation == undefined) {
                return null;
            }
            return this.letterOfRecommendation.letterOfRecommendation;
        }

        getLetterOfRecommendationFromServer(letterOfRecommendationKey: Dtos.Common.AzureQueuryObjects.AzureTableStorageEntityKeyModel): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.makeUri(letterOfRecommendationKey));
        }

        resolveGetLetterOfRecommendation(serverMessage: Common.Models.ServerResponseModel): void {
            this.letterOfRecommendation = this.genericResolver.genericGetResolver("letter of recommendation", serverMessage);
        }

        private makeUri(azureTableStorageEntityKeyModel: Dtos.Common.AzureQueuryObjects.AzureTableStorageEntityKeyModel) {
            return this.apiEndpoint + azureTableStorageEntityKeyModel.partitionKey + "/rowkey/" + azureTableStorageEntityKeyModel.rowKey;
        }
    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.service("GetLetterOfRecommendationForEvaluatorService", ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.GetEssayAndLettersOfRecommendation.GetLetterOfRecommendationForEvaluatorService);
} 