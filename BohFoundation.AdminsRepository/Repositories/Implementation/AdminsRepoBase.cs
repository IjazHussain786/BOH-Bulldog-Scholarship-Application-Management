using System;
using BohFoundation.AdminsRepository.DbContext;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.AdminsRepository.Repositories.Implementation
{
    public class AdminsRepoBase
    {
        internal readonly string DbConnection;
        private readonly IClaimsInformationGetters _claimsInformationGetters;
        internal readonly Guid AdminsGuid;
        
        public AdminsRepoBase(string dbConnection, IClaimsInformationGetters claimsInformationGetters)
        {
            DbConnection = dbConnection;
            _claimsInformationGetters = claimsInformationGetters;
            AdminsGuid = _claimsInformationGetters.GetUsersGuid();
        }

        protected AdminsRepositoryDbContext GetAdminsRepositoryDbContext()
        {
            return new AdminsRepositoryDbContext(DbConnection);
        }
    }
}
