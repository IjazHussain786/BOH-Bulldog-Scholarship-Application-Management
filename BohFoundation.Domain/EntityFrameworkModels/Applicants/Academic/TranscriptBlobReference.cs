using System;

namespace BohFoundation.Domain.EntityFrameworkModels.Applicants.Academic
{
    public class TranscriptBlobReference
    {
        public int Id { get; set; }
        public string ReferenceToTranscriptPdf { get; set; }
        public string BlobContainerName { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual AcademicInformation AcademicInformation { get; set; }
    }
}