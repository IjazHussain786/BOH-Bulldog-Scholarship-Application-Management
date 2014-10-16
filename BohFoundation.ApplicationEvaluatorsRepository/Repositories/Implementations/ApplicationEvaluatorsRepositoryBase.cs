using System;
using BohFoundation.ApplicationEvaluatorsRepository.DbContext;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.ApplicationEvaluatorsRepository.Repositories.Implementations
{
    public class ApplicationEvaluatorsRepositoryBase
    {
        internal readonly string DbConnection;
        internal readonly Guid ApplicationEvaluatorsGuid;

        public ApplicationEvaluatorsRepositoryBase(string dbConnection, IClaimsInformationGetters claimsInformationGetters)
        {
            DbConnection = dbConnection;
            ApplicationEvaluatorsGuid = claimsInformationGetters.GetUsersGuid();
        }

        protected ApplicationEvaluatorRepositoryDbContext GetApplicationEvaluatorRepositoryDbContext()
        {
            return new ApplicationEvaluatorRepositoryDbContext(DbConnection);
        }
    }
}
