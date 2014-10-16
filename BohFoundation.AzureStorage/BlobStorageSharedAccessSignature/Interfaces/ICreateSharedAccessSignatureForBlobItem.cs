using System.Threading.Tasks;
using BohFoundation.Domain.Dtos.Applicant.Academic;

namespace BohFoundation.AzureStorage.BlobStorageSharedAccessSignature.Interfaces
{
    public interface ICreateSharedAccessSignatureForBlobItem
    {
        Task<string> GetApplicantsTranscriptUri(TranscriptBlobReferenceDto dto);
    }
}
