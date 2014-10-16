using Ninject.Modules;

namespace BohFoundation.Infrastructure.DI
{
    public class ProductionStorageModule
    {
        public INinjectModule GetProductionModule()
        {
            const string productionDb = "ProductionDb";
            const string azureStorageProduction = "AzureBohFoundation";

            return new RepositoryModule(productionDb, azureStorageProduction, true);
        }
    }
}
