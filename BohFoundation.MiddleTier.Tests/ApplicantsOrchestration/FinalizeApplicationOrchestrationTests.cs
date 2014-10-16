using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;
using BohFoundation.MiddleTier.ApplicantsOrchestration.Implementations;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.MiddleTier.Tests.ApplicantsOrchestration
{
    [TestClass]
    public class FinalizeApplicationOrchestrationTests
    {
        private FinalizeApplicationOrchestration _finalizeApplicantOrchestration;
        private IChangeApplicantToFinalizedApplicantService _changeApplicantToFinalizedService;
        private IApplicantMetadataRepository _metadataRepo;

        [TestInitialize]
        public void Initialize()
        {

            _metadataRepo = A.Fake<IApplicantMetadataRepository>();
            _changeApplicantToFinalizedService = A.Fake<IChangeApplicantToFinalizedApplicantService>();
            _finalizeApplicantOrchestration = new FinalizeApplicationOrchestration(_changeApplicantToFinalizedService, _metadataRepo);
        }

        [TestMethod]
        public void FinalizeApplicationOrchestration_Finalize_Calls_FlipApplicant()
        {
            FinalizeApplication();
            A.CallTo(() => _changeApplicantToFinalizedService.FlipApplicant()).MustHaveHappened();
        }

        [TestMethod]
        public void FinalizeApplicationOrchestration_Finalize_Calls_MetadataRepo()
        {
            FinalizeApplication();
            A.CallTo(() => _metadataRepo.FinalizeApplication()).MustHaveHappened();
        }

        private void FinalizeApplication()
        {
            _finalizeApplicantOrchestration.FinalizeApplication();
        }
    }
}
