using System.Linq;
using BohFoundation.Domain.Claims;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.Domain.Enums;
using BohFoundation.MembershipProvider.UserManagement.Helpers.Implementation;
using BohFoundation.TestHelpers;
using BrockAllen.MembershipReboot;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.MembershipProvider.Tests.UnitTests.UserManagement.Helpers
{
    [TestClass]
    public class CreateUserClaimCollectionTests
    {
        private CreateUserClaimCollection _createUserClaimCollection;
        private UserClaimCollection Result { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            _createUserClaimCollection = new CreateUserClaimCollection();
        }

        [TestMethod]
        public void CreateUserClaimCollection_CreateClaimsCollection_ShouldReturn_ACollectionThatContains_FirstName_Applicant()
        {
            CreateApplicantClaimsCollection(MemberTypesEnum.Applicant);
            Assert.AreEqual(TestHelpersCommonFields.FirstName, Result.First(x => x.Type == ClaimsNames.FirstName).Value);
        }

        [TestMethod]
        public void CreateUserClaimCollection_CreateClaimsCollection_ShouldReturn_ACollectionThatContains_LastName_Applicant()
        {
            CreateApplicantClaimsCollection(MemberTypesEnum.Applicant);
            Assert.AreEqual(TestHelpersCommonFields.LastName, Result.First(x => x.Type == ClaimsNames.LastName).Value);
        }

        [TestMethod]
        public void CreateUserClaimCollection_CreateClaimsCollection_ShouldReturn_ACollectionThatContains_GraduatingYear_Applicant()
        {
            CreateApplicantClaimsCollection(MemberTypesEnum.Applicant);
            Assert.AreEqual(TestHelpersCommonFields.GraduatingYear.ToString(), Result.First(x => x.Type == ClaimsNames.GraduatingYear).Value);
        }

        [TestMethod]
        public void CreateUserClaimCollection_CreateClaimsCollection_ShouldReturn_ACollectionThatContains_Applicant()
        {
            CreateApplicantClaimsCollection(MemberTypesEnum.Applicant);
            Assert.AreEqual("True", Result.First(x => x.Type == ClaimsNames.Applicant).Value);
        }

        [TestMethod]
        public void CreateUserClaimCollection_CreateClaimsCollection_ShouldNotReturn_ACollectionThatContains_Applicant_ApplicationEvaluatorPendingConfirmation()
        {
            CreateApplicantClaimsCollection(MemberTypesEnum.Applicant);
            Assert.IsNull(Result.FirstOrDefault(x => x.Type == ClaimsNames.ApplicationEvaluatorPendingConfirmation));
        }

        [TestMethod]
        public void CreateUserClaimCollection_CreateClaimsCollection_ShouldReturn_ACollectionThatContains_FirstName_ApplicationEvaluator()
        {
            CreateApplicantClaimsCollection(MemberTypesEnum.PendingApplicationEvaluator);
            Assert.AreEqual(TestHelpersCommonFields.FirstName, Result.First(x => x.Type == ClaimsNames.FirstName).Value);
        }

        [TestMethod]
        public void CreateUserClaimCollection_CreateClaimsCollection_ShouldReturn_ACollectionThatContains_LastName_ApplicationEvaluator()
        {
            CreateApplicantClaimsCollection(MemberTypesEnum.PendingApplicationEvaluator);
            Assert.AreEqual(TestHelpersCommonFields.LastName, Result.First(x => x.Type == ClaimsNames.LastName).Value);
        }

        [TestMethod]
        public void CreateUserClaimCollection_CreateClaimsCollection_ShouldReturn_ACollectionThatContains_ApplicationEvaluatorPendingConfirmation()
        {
            CreateApplicantClaimsCollection(MemberTypesEnum.PendingApplicationEvaluator);
            Assert.AreEqual("True", Result.First(x => x.Type == ClaimsNames.ApplicationEvaluatorPendingConfirmation).Value);
        }

        [TestMethod]
        public void CreateUserClaimCollection_CreateClaimsCollection_ShouldNotReturn_ACollectionThatContains_Applicant__ApplicantEvaluator()
        {
            CreateApplicantClaimsCollection(MemberTypesEnum.PendingApplicationEvaluator);
            Assert.IsNull(Result.FirstOrDefault(x => x.Type == ClaimsNames.Applicant));
        }


        private void CreateApplicantClaimsCollection(MemberTypesEnum type)
        {
            var registerApplicationInputModel = new RegisterInputModel { EmailAddress = TestHelpersCommonFields.Email, FirstName = TestHelpersCommonFields.FirstName, LastName = TestHelpersCommonFields.LastName, GraduatingYear = TestHelpersCommonFields.GraduatingYear, Password = TestHelpersCommonFields.Password };

            Result = _createUserClaimCollection.CreateClaimsCollection(registerApplicationInputModel, type);
        }

    }
}
