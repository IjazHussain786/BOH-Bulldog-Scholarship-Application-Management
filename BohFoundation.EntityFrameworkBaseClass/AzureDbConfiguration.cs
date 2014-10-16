using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace BohFoundation.EntityFrameworkBaseClass
{
    public class AzureDbConfiguration : DbConfiguration
    {
        public AzureDbConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient",() => new SqlAzureExecutionStrategy());
        }
    }
}
