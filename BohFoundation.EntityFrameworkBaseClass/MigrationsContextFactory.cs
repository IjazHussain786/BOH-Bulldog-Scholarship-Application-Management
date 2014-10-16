using System.Data.Entity.Infrastructure;

namespace BohFoundation.EntityFrameworkBaseClass
{
    public class MigrationsContextFactory : IDbContextFactory<DatabaseRootContext>
    {
        public DatabaseRootContext Create()
        {
            return new DatabaseRootContext("ProductionDb");

            //ProductionDb
            //CloudTest
            //LocalTest

            //Add-Migration ITSNAME
            //Update-Database
        }
    }
}
