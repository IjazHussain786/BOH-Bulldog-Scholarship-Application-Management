using System;
using BohFoundation.ApplicantsRepository.DbContext;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.ApplicantsRepository.Repositories.Implementations
{
    public class ApplicantsRepositoryBase
    {
        internal readonly string DbConnection;
        private readonly IClaimsInformationGetters _claimsInformationGetters;
        internal readonly Guid ApplicantGuid;

        public ApplicantsRepositoryBase(string dbConnection, IClaimsInformationGetters claimsInformationGetters)
        {
            DbConnection = dbConnection;
            _claimsInformationGetters = claimsInformationGetters;
            ApplicantGuid = claimsInformationGetters.GetUsersGuid();
        }

        internal int GetApplicantsGraduatingClassYear()
        {
            return _claimsInformationGetters.GetApplicantsGraduatingYear();
        }

        protected ApplicantRepositoryDbContext GetApplicantsDbContext()
        {
            return new ApplicantRepositoryDbContext(DbConnection);
        }
    }
}
