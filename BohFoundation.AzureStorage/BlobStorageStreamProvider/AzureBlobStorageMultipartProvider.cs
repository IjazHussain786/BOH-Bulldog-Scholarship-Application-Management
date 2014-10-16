using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BohFoundation.AzureStorage.BlobStorageStreamProvider
{
    public class AzureBlobStorageMultipartProvider : MultipartFileStreamProvider
    {
        private CloudBlobContainer _container;
       
        public string Reference { get; set; }
        public string Container { get; set; }

        public AzureBlobStorageMultipartProvider(CloudBlobContainer container, string rootPath)
            : base(rootPath)
        {
            Initialize(container);
        }

        public AzureBlobStorageMultipartProvider(CloudBlobContainer container, string rootPath, int bufferSize)
            : base(rootPath, bufferSize)
        {
            Initialize(container);
        }

        public override async Task ExecutePostProcessingAsync()
        {
            // Upload the files asynchronously to azure blob storage and remove them from local disk when done
            foreach (MultipartFileData fileData in FileData)
            {
                // Get the blob name from the Content-Disposition header if present
                var blobName = GetBlobName(fileData);

                // Retrieve reference to a blob
                var blob = _container.GetBlockBlobReference(blobName);

                // Pick content type if present
                blob.Properties.ContentType = fileData.Headers.ContentType != null ?
                    fileData.Headers.ContentType.ToString() : "application/octet-stream";

                // Upload content to blob storage
                using (FileStream fStream = new FileStream(fileData.LocalFileName, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, useAsync: true))
                {
                    await Task.Factory.FromAsync(blob.BeginUploadFromStream, blob.EndUploadFromStream, fStream, state: null);
                }

                // Delete local file
                File.Delete(fileData.LocalFileName);

                Container = _container.Name;
                Reference = blobName;
            }

            await base.ExecutePostProcessingAsync();
        }

        private void Initialize(CloudBlobContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            _container = container;
        }

        private static string GetBlobName(MultipartFileData fileData)
        {
            string blobName = null;
            ContentDispositionHeaderValue contentDisposition = fileData.Headers.ContentDisposition;
            if (contentDisposition != null)
            {
                try
                {
                    blobName = Path.GetFileName(contentDisposition.Name.Trim('"'));
                }
                catch
                { }
            }

            return blobName ?? Path.GetFileName(fileData.LocalFileName);
        }
    }
}