using System;
using BohFoundation.Domain.Dtos.Applicant.Academic;
using BohFoundation.Domain.Dtos.Common;

namespace BohFoundation.ApplicantsRepository.Repositories.Interfaces
{
    public interface ITranscriptReferenceRepository
    {
        void UpsertTranscriptReference(TranscriptBlobReferenceDto transcriptBlobReference);
        LastUpdatedDto LastUpdatedTranscript();
    }
}
