module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.GetEssayAndLettersOfRecommendation {
    export interface IGetEssayForEvaluatorService {

        getEssay(): string;
        getEssayFromServer(letterOfRecommendationKey: Dtos.Common.AzureQueuryObjects.AzureTableStorageEntityKeyModel): ng.resource.IResource<any>;
        resolveEssayFromServer(serverMessage: Common.Models.ServerResponseModel): void;

    }

    export class GetEssayForEvaluatorService implements IGetEssayForEvaluatorService {
        static $inject = ['AuthRequiredRepository', 'GenericResolver'];
        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }

        private apiEndpoint = "/api/applicationevaluator/evaluatingapplicants/essay/partitionkey/";
        private essayDto: Dtos.Applicant.Essay.EssayModel;

        getEssay(): string {
            if (this.essayDto == undefined) {
                return null;
            }
            return this.essayDto.essay;
        }

        getEssayFromServer(essayKey: Dtos.Common.AzureQueuryObjects.AzureTableStorageEntityKeyModel): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.makeUri(essayKey));
        }

        resolveEssayFromServer(serverMessage: Common.Models.ServerResponseModel): void {
            this.essayDto = this.genericResolver.genericGetResolver("essay", serverMessage);
        }

        private makeUri(azureTableStorageEntityKeyModel: Dtos.Common.AzureQueuryObjects.AzureTableStorageEntityKeyModel) {
            return this.apiEndpoint + azureTableStorageEntityKeyModel.partitionKey + "/rowkey/" + azureTableStorageEntityKeyModel.rowKey;
        }
    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.service("GetEssayForEvaluatorService", ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.GetEssayAndLettersOfRecommendation.GetEssayForEvaluatorService);
} 