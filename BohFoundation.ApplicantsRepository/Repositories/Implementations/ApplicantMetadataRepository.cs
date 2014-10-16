using System.Linq;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.ApplicantsRepository.Repositories.Implementations
{
    public class ApplicantMetadataRepository : ApplicantsRepositoryBase, IApplicantMetadataRepository
    {
        public ApplicantMetadataRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters) : base(dbConnection, claimsInformationGetters)
        {
        }

        public void FinalizeApplication()
        {
            var metadata = new ApplicantMetadata{ApplicationFinalized = true};
            using (var context = GetApplicantsDbContext())
            {
                var applicant = context.Applicants.First(applicants => applicants.Person.Guid == ApplicantGuid);
                applicant.Metadata = metadata;
                context.SaveChanges();
            }
        }
    }
}