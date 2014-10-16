module BohFoundation.Admin.EditEssayTopic.Services {
    export interface IModifyEssayTopicGraduatingYearService {
        postAddYearToTopic(model: Dtos.Admin.EssayTopics.EditEssayTopicByGraduatingClassModel): ng.resource.IResource<any>;
        resolvePostAddYearToTopic(serverResponse: Common.Models.ServerResponseModel): void;

        deleteYearToTopic(model: Dtos.Admin.EssayTopics.EditEssayTopicByGraduatingClassModel): any;
        resolveDeleteYearToTopic(serverResponse: Common.Models.ServerResponseModel): void;
    }

    export class ModifyEssayTopicGraduatingYearService implements IModifyEssayTopicGraduatingYearService {
        private apiEndpoint = "/api/admin/essaytopic/graduatingyear";
        private typeOfData = "update graduating class essay topics";

        static $inject = ['AuthRequiredRepository', 'GenericResolver'];

        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }

        postAddYearToTopic(model: Dtos.Admin.EssayTopics.EditEssayTopicByGraduatingClassModel): ng.resource.IResource<any> {
            return this.authRequiredRepository.post(model, this.apiEndpoint);
        }

        resolvePostAddYearToTopic(serverResponse: Common.Models.ServerResponseModel): void {
            this.genericResolver.genericPostResolver(this.typeOfData, serverResponse);
        }

        deleteYearToTopic(model: Dtos.Admin.EssayTopics.EditEssayTopicByGraduatingClassModel): any {
            return this.authRequiredRepository.delete(this.apiEndpoint + "/" + model.essayId + "/" + model.classYear);
        } 

        resolveDeleteYearToTopic(serverResponse: Common.Models.ServerResponseModel): void {
            this.resolvePostAddYearToTopic(serverResponse);
        }
    }
}

module BohFoundation.Main {
    Register.Admin.service("ModifyEssayTopicGraduatingYearService", Admin.EditEssayTopic.Services.ModifyEssayTopicGraduatingYearService);
}