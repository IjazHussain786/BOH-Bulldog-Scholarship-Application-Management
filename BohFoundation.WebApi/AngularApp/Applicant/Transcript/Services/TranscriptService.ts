module BohFoundation.Applicant.Transcript.Services {
    export interface ITranscriptService {
        getWhenTranscriptWasLastUpdatedFromServer(): ng.resource.IResource<any>;
        resolveGetLastUpdatedTranscript(serverMessage: Common.Models.ServerResponseModel): void;

        getLastUpdated(): Date;
    }

    export class TranscriptService implements ITranscriptService {
        private apiEndpoint = '/api/applicant/transcript';
        private typeOfData = 'transcript';
        private lastUpdatedModel: Dtos.Common.LastUpdatedModel;

        static $inject = ['AuthRequiredRepository', 'GenericResolver'];
        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) {
            
        }

        getWhenTranscriptWasLastUpdatedFromServer(): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.apiEndpoint);
        }

        resolveGetLastUpdatedTranscript(serverMessage: Common.Models.ServerResponseModel): void {
            this.lastUpdatedModel = this.genericResolver.genericGetResolver(this.typeOfData, serverMessage);
        }

        getLastUpdated(): Date {
            if (this.lastUpdatedModel != undefined) {
                return this.lastUpdatedModel.lastUpdated;
            }
        }
    }   
}

module BohFoundation.Main {
    Register.Applicant.service("TranscriptService", Applicant.Transcript.Services.TranscriptService);
}