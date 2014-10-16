using BohFoundation.AzureStorage.BlobStorageSharedAccessSignature.Implementation;
using BohFoundation.AzureStorage.BlobStorageSharedAccessSignature.Interfaces;
using BohFoundation.AzureStorage.TableStorage.Implementations.Essay.Helpers;
using BohFoundation.AzureStorage.TableStorage.Interfaces.Essay.Helpers;
using Ninject.Modules;

namespace BohFoundation.AzureStorage
{
    public class AzureStorageModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEssayRowKeyGenerator>().To<EssayRowKeyGenerator>();
            Bind<ICreateSharedAccessSignatureForBlobItem>().To<CreateSharedAccessSignatureForBlobItem>();
        }
    }
}
