using System;
using System.Linq;
using BohFoundation.ApplicantsRepository.Repositories.Implementations;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.EntityFrameworkBaseClass;
using BohFoundation.TestHelpers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.ApplicantsRepository.Tests.IntegrationTests
{
    [TestClass]
    public class ApplicantMetadataRepositoryIntegrationTests
    {
        private static ApplicantMetadataRepository _applicantMetadataRepo;

        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            TestHelpersCommonFields.InitializeFields();
            TestHelpersCommonFakes.InitializeFakes();

            CreateApplicant();

            _applicantMetadataRepo = new ApplicantMetadataRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters);
            
            FinalizeApplicant();
        }

        private static void CreateApplicant()
        {
            ApplicantGuid = Guid.NewGuid();
            var applicant = new Applicant
            {
                Person = new Person
                {
                    DateCreated = DateTime.UtcNow,
                    Guid = ApplicantGuid
                }
            };

            using (var context = GetRootContext())
            {
                context.Applicants.Add(applicant);
                context.SaveChanges();
            }

            A.CallTo(() => TestHelpersCommonFakes.ClaimsInformationGetters.GetUsersGuid()).Returns(ApplicantGuid);

        }

        private static Guid ApplicantGuid { get; set; }

        private static void FinalizeApplicant()
        {
            _applicantMetadataRepo.FinalizeApplication();

            using (var context = GetRootContext())
            {
                ResultOfFinalize = context.People.First(person => person.Guid == ApplicantGuid).Applicant.Metadata;
            }
        }

        private static ApplicantMetadata ResultOfFinalize { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void ApplicantMetadataRepository_FinalizeApplication_Should_Finalize()
        {
            Assert.IsTrue(ResultOfFinalize.ApplicationFinalized);
        }

        [TestCategory("Integration"), TestMethod]
        public void ApplicantMetadataRepository_FinalizeApplication_Should_HavePositiveId()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(ResultOfFinalize.Id);
        }

        [TestCategory("Integration"), TestMethod]
        public void ApplicantMetadataRepository_FinalizeApplication_Should_False_Finalist()
        {
            Assert.IsFalse(ResultOfFinalize.Finalist);
        }

        [TestCategory("Integration"), TestMethod]
        public void ApplicantMetadataRepository_FinalizeApplication_Should_False_SelectionNonSelectionLetter()
        {
            Assert.IsFalse(ResultOfFinalize.AcceptanceNonSelectionLetterSent);
        }
        #region Utilities

        private static DatabaseRootContext GetRootContext()
        {
            return new DatabaseRootContext(TestHelpersCommonFields.DatabaseName);
        }

        [ClassCleanup]
        public static void CleanDb()
        {

        }

        #endregion
    }
}