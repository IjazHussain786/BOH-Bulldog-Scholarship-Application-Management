using System;
using BohFoundation.Domain.Claims;
using BohFoundation.MembershipProvider.Repositories.Interfaces;
using BohFoundation.MembershipProvider.Tests.UnitTests.CommonStaticItemsForTests;
using BohFoundation.MembershipProvider.UserManagement.Manage.Implementation;
using BohFoundation.TestHelpers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.MembershipProvider.Tests.UnitTests.UserManagement.Manage
{
    [TestClass]
    public class ChangeApplicantToFinalizedApplicantServiceTests
    {
        private ChangeApplicantToFinalizedApplicantService _changeApplicantToFinalizedApplicantService;
        private IMembershipRebootCustomQueries _customQueries;

        [TestInitialize]
        public void Initialize()
        {
            Guid = Guid.NewGuid();
            Guid2 = Guid.NewGuid();

            MembershipProviderCommonFakes.CreateNewInstances();

            _customQueries = A.Fake<IMembershipRebootCustomQueries>();

            A.CallTo(() => MembershipProviderCommonFakes.ClaimsInformationGetters.GetUsersGuid()).Returns(Guid2);

            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.GetByID(A<Guid>.Ignored)).Returns(MembershipProviderCommonFakes.RelationalUserAccount);

            _changeApplicantToFinalizedApplicantService = new ChangeApplicantToFinalizedApplicantService(MembershipProviderCommonFakes.UserAccountService, MembershipProviderCommonFakes.ClaimsInformationGetters, _customQueries);
        }

        private Guid Guid { get; set; }
        private Guid Guid2 { get; set; }

        [TestMethod]
        public void ChangeApplicantToFinalizedApplicantService_FlipApplicantWithGuid_Should_GetByID()
        {
            FlipApplicantWithPassedInGuid();
            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.GetByID(Guid)).MustHaveHappened();
        }

        [TestMethod]
        public void ChangeApplicantToFinalizedApplicantService_FlipApplicantWithGuid_It_Should_Add_FinalizedApplicant_Claim()
        {
            FlipApplicantWithPassedInGuid();
            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.AddClaim(Guid, ClaimsNames.ApplicantAfterFinalization, true.ToString())).MustHaveHappened();
        }

        [TestMethod]
        public void ChangeApplicantToFinalizedApplicantService_FlipApplicantWithGuid_It_Should_Call_Custom_Queries()
        {
            FlipApplicantWithPassedInGuid();
            A.CallTo(() => _customQueries.RemoveApplicantClaim(0)).MustHaveHappened();
        }

        private void FlipApplicantWithPassedInGuid()
        {
            _changeApplicantToFinalizedApplicantService.FlipApplicant(Guid);
        }


        [TestMethod]
        public void ChangeApplicantToFinalizedApplicantService_FlipApplicantWithoutGuid_Should_GetByID()
        {
            FlipApplicantWithoutPassedInGuid();
            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.GetByID(Guid2)).MustHaveHappened();
        }

        [TestMethod]
        public void ChangeApplicantToFinalizedApplicantService_FlipApplicantWithoutGuid_It_Should_Add_FinalizedApplicant_Claim()
        {
            FlipApplicantWithoutPassedInGuid();
            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.AddClaim(Guid2, ClaimsNames.ApplicantAfterFinalization, true.ToString())).MustHaveHappened();
        }

        [TestMethod]
        public void ChangeApplicantToFinalizedApplicantService_FlipApplicantWithoutGuid_It_Should_Call_Custom_Queries()
        {
            FlipApplicantWithoutPassedInGuid();
            A.CallTo(() => _customQueries.RemoveApplicantClaim(0)).MustHaveHappened();
        }

        private void FlipApplicantWithoutPassedInGuid()
        {
            _changeApplicantToFinalizedApplicantService.FlipApplicant();
        }

    }
}
