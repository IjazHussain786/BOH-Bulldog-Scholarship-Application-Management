using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BohFoundation.AzureStorage.BlobStorageStreamProvider.Interfaces
{
    public interface IBlobHelper
    {
        Task<CloudBlobContainer> GetBlobContainer(string containerName);
    }
}