using System.Linq;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.ApplicationEvaluatorsRepository.Repositories.Implementations
{
    public class ConfirmTranscriptRepository : ApplicationEvaluatorsRepositoryBase, IConfirmTranscriptRepository
    {
        public ConfirmTranscriptRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters) : base(dbConnection, claimsInformationGetters)
        {
        }

        public void ConfirmTranscript(ConfirmTranscriptDto confirmTranscript)
        {
            using (var context = GetApplicationEvaluatorRepositoryDbContext())
            {
                var metaData =
                    context.Applicants.First(applicant => applicant.Person.Guid == confirmTranscript.ApplicantsGuid).Metadata;

                metaData.TranscriptDoesNotMatchDatabaseValues   = !confirmTranscript.InformationMatchesTranscriptPdf;
                metaData.TranscriptMatchesDatabaseValues        = confirmTranscript.InformationMatchesTranscriptPdf;

                context.SaveChanges();
            }
        }
    }
}
