using System.Data.Entity;

namespace BohFoundation.EntityFrameworkBaseClass
{
    public class BaseContext<TContext> : DbContext where TContext : DbContext
    {
        static BaseContext()
        {
            DbConfiguration.SetConfiguration(new AzureDbConfiguration());
            Database.SetInitializer<TContext>(null);
        }

        protected BaseContext()
        {
            DbConfiguration.SetConfiguration(new AzureDbConfiguration());
        }

        protected BaseContext(string nameOfConnection)
            : base("name=" + nameOfConnection)
        {

        }
    }
}