using System;
using System.Threading.Tasks;
using BohFoundation.AzureStorage.BlobStorageSharedAccessSignature.Interfaces;
using BohFoundation.AzureStorage.BlobStorageStreamProvider.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Academic;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BohFoundation.AzureStorage.BlobStorageSharedAccessSignature.Implementation
{
    //todo: not unit tested
    public class CreateSharedAccessSignatureForBlobItem : ICreateSharedAccessSignatureForBlobItem
    {
        private readonly IBlobHelper _blobHelper;

        public CreateSharedAccessSignatureForBlobItem(IBlobHelper blobHelper)
        {
            _blobHelper = blobHelper;
        }

        public async Task<string> GetApplicantsTranscriptUri(TranscriptBlobReferenceDto dto)
        {
            var container = await _blobHelper.GetBlobContainer(dto.BlobContainerName);

            var reference = await container.GetBlobReferenceFromServerAsync(dto.ReferenceToTranscriptPdf);

            var policy = new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = DateTime.UtcNow,
                SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromSeconds(30),
                Permissions = SharedAccessBlobPermissions.Read
            };

            var transripturi = reference.Uri.AbsoluteUri;

            return transripturi + reference.GetSharedAccessSignature(policy);
        }
    }
}
