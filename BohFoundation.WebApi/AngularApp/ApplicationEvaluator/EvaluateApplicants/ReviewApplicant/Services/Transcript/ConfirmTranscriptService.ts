module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.Transcript {
    export interface IConfirmTranscriptService {
        getTranscriptFromServer(transcriptReferences: Dtos.Applicant.Academic.TranscriptBlobReferenceModel): ng.resource.IResource<any>;
        resolveGetTranscriptFromServer(serverResponse: Common.Models.ServerResponseModel): void;

        postTranscriptConfirmation(transcriptConfirmation: Dtos.ApplicationEvaluator.EvaluatingApplicants.ConfirmTranscriptModel): ng.resource.IResource<any>;
        resolvePostTranscriptConfirmation(serverResponse:Common.Models.ServerResponseModel): void;
    }

    export class ConfirmTranscriptService implements IConfirmTranscriptService {
        private getApiEndpoint  = "/api/applicationevaluator/evaluatingapplicants/transcript/geturi/blobcontainername/";
        private postApiEndpoint = "/api/applicationevaluator/evaluatingapplicants/transcript/confirm";
        private typeOfData = "transcript";

        static $inject = ['AuthRequiredRepository', 'GenericResolver', "$window"];
        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver,
            private $window: ng.IWindowService) { }

        getTranscriptFromServer(transcriptReferences: Dtos.Applicant.Academic.TranscriptBlobReferenceModel): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.createGetUri(transcriptReferences));
        }

        resolveGetTranscriptFromServer(serverResponse: Common.Models.ServerResponseModel): void {
            var response = this.genericResolver.genericGetResolver(this.typeOfData, serverResponse);
            if (serverResponse.success) {
                this.$window.open(response);
            }
        }

        postTranscriptConfirmation(transcriptConfirmation: Dtos.ApplicationEvaluator.EvaluatingApplicants.ConfirmTranscriptModel): ng.resource.IResource<any> {
            return this.authRequiredRepository.post(transcriptConfirmation, this.postApiEndpoint);
        }

        resolvePostTranscriptConfirmation(serverResponse: Common.Models.ServerResponseModel): void {
            this.genericResolver.genericPostResolver(this.typeOfData, serverResponse);
        }

        private createGetUri(transcriptBlobReferenceModel: Dtos.Applicant.Academic.TranscriptBlobReferenceModel) {
            var splitArray = transcriptBlobReferenceModel.referenceToTranscriptPdf.split(".");

            var extension = splitArray[1];
            var referenceToPdfWithoutExtension = splitArray[0];

            return this.getApiEndpoint + transcriptBlobReferenceModel.blobContainerName + "/referencetotranscriptwithoutfileextension/" + referenceToPdfWithoutExtension + "/extension/" + extension;
        }
    }
}

module BohFoundation.Main {
    Register.ApplicationEvaluator.service("ConfirmTranscriptService", ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.Transcript.ConfirmTranscriptService);
}