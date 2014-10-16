module BohFoundation.Admin.EditEssayTopic.Services {
    export interface ICreateAndGetEssayTopics {
        postCreateOrModifyEssayTopic(model: Dtos.Admin.EssayTopics.CreateAndModifyEssayTopicModel): ng.resource.IResource<any>;
        resolvePostCreateOrModifyEssayTopic(serverResponse: Common.Models.ServerResponseModel): void;

        getEssayTopicsFromServer(): ng.resource.IResource<any>;
        resolveGetEssayTopics(serverResponse: Common.Models.ServerResponseModel): void;

        getEssayTopicArray(): Array<Dtos.Admin.EssayTopics.EssayTopicModel>;

    }

    export class CreateAndGetEssayTopicsService implements ICreateAndGetEssayTopics {
        private apiEndpoint = "/api/admin/essaytopic";
        private typeOfData = "essay topics";

        private essayTopicArray: Array<Dtos.Admin.EssayTopics.EssayTopicModel> = [];

        static $inject = ['AuthRequiredRepository', 'GenericResolver'];

        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }

        getEssayTopicArray(): Array<Dtos.Admin.EssayTopics.EssayTopicModel> {
            return this.essayTopicArray;
        }

        postCreateOrModifyEssayTopic(model: Dtos.Admin.EssayTopics.CreateAndModifyEssayTopicModel): ng.resource.IResource<any> {
            return this.authRequiredRepository.post(model, this.apiEndpoint);
        }

        resolvePostCreateOrModifyEssayTopic(serverResponse: Common.Models.ServerResponseModel): void {
            this.genericResolver.genericPostResolver(this.typeOfData, serverResponse);
        }

        getEssayTopicsFromServer(): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.apiEndpoint);
        }

        resolveGetEssayTopics(serverResponse: Common.Models.ServerResponseModel): void {
            this.essayTopicArray = this.genericResolver.genericGetResolver(this.typeOfData, serverResponse);
        }
    }
} 

module BohFoundation.Main {
    Register.Admin.service('CreateAndGetEssayTopicsService', Admin.EditEssayTopic.Services.CreateAndGetEssayTopicsService);
}