using System;
using System.Linq;
using BohFoundation.EntityFrameworkBaseClass;

namespace BohFoundation.Infrastructure.DI.Helpers
{
    public static class StorageModuleHelpers
    {
        public static void InitializeDb(string nameOfConnection)
        {
            using (var dbContext = new DatabaseRootContext(nameOfConnection))
            {
                try
                {
                    dbContext.Users.Count();
                }
                catch (InvalidOperationException ex)
                {
                    if (ex.Message !=
                        "No connection string named '" + nameOfConnection +
                        "' could be found in the application config file.")
                    {
                        throw;
                    }
                }
            }
        }
    }

}