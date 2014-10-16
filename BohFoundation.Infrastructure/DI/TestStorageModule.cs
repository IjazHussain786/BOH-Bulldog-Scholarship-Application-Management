using BohFoundation.Domain.Infrastructure;
using BohFoundation.Infrastructure.DI.Helpers;
using Ninject.Modules;

namespace BohFoundation.Infrastructure.DI
{
    public class TestStorageModule 
    {
        public bool LocalTest
        {
            get { return IsLocalTest.LocalTest; }
        }

        public INinjectModule GetTestStorageModule()
        {
            if (LocalTest)
            {
                const string localTest = "LocalTest";
                const string azureStorageLocalTest = "AzureStorageLocalTest";

                StorageModuleHelpers.InitializeDb(localTest);

                return new RepositoryModule(localTest, azureStorageLocalTest, false);
            }
            const string cloudTest = "CloudTest";
            const string azureStorageCloudTest = "AzureStorageCloudTest";

            StorageModuleHelpers.InitializeDb(cloudTest);

            return new RepositoryModule(cloudTest, azureStorageCloudTest, false);
        }
    }
}
