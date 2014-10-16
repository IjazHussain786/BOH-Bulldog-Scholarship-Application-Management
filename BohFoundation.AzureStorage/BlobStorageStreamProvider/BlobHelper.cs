using System;
using System.Configuration;
using System.Threading.Tasks;
using BohFoundation.AzureStorage.BlobStorageStreamProvider.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BohFoundation.AzureStorage.BlobStorageStreamProvider
{
    public class BlobHelper : IBlobHelper
    {
        private readonly string _dbConnection;

        public BlobHelper(string dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<CloudBlobContainer> GetBlobContainer(string containerName)
        {
            if (String.IsNullOrEmpty(containerName))
            {
                throw new ArgumentException("containerName");
            }

            // Retrieve storage account from connection-string
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings[_dbConnection].ConnectionString);

            // Create the blob client 
            var blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container. Note that container name must use lower case
            var container = blobClient.GetContainerReference(containerName.ToLowerInvariant());

            // Create options for communicating with the blob container.
            var options = new BlobRequestOptions();

            //// Create the container if it doesn't already exist
            //bool result = await Task.Factory.FromAsync<BlobRequestOptions, bool>(container.BeginCreateIfNotExists(), container.EndCreateIfNotExists, options, state: null);

            await container.CreateIfNotExistsAsync();

            return container;
        }
    }
}