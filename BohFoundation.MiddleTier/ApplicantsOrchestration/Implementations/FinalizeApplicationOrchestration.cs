using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;
using BohFoundation.MiddleTier.ApplicantsOrchestration.Interfaces;

namespace BohFoundation.MiddleTier.ApplicantsOrchestration.Implementations
{
    public class FinalizeApplicationOrchestration : IFinalizeApplicationOrchestration
    {
        private readonly IChangeApplicantToFinalizedApplicantService _changeApplicantToFinalizedApplicant;
        private readonly IApplicantMetadataRepository _metadataRepository;

        public FinalizeApplicationOrchestration(IChangeApplicantToFinalizedApplicantService changeApplicantToFinalizedApplicant, IApplicantMetadataRepository metadataRepository)
        {
            _changeApplicantToFinalizedApplicant = changeApplicantToFinalizedApplicant;
            _metadataRepository = metadataRepository;
        }

        public void FinalizeApplication()
        {
            //validate
            _changeApplicantToFinalizedApplicant.FlipApplicant();
            _metadataRepository.FinalizeApplication();
        }
    }
}
