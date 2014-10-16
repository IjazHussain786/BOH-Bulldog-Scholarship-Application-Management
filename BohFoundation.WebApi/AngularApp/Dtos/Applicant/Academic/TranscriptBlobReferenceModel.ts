module BohFoundation.Dtos.Applicant.Academic {
    export class TranscriptBlobReferenceModel {
        constructor(
            public referenceToTranscriptPdf?: string,
            public blobContainerName?: string,
            public lastUpdated?: Date) {
        }
    }
} 