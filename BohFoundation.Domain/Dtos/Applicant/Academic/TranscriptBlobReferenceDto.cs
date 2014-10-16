using System;

namespace BohFoundation.Domain.Dtos.Applicant.Academic
{
    public class TranscriptBlobReferenceDto
    {
        public string ReferenceToTranscriptPdf { get; set; }
        public string BlobContainerName { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
