using System;
using System.Collections.ObjectModel;
using System.Linq;
using BohFoundation.AdminsRepository.Repositories.Implementation;
using BohFoundation.Domain.Dtos.Admin.References;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.EntityFrameworkModels.References;
using BohFoundation.EntityFrameworkBaseClass;
using BohFoundation.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.AdminsRepository.Tests.IntegrationTests
{
    [TestClass]
    public class GetLetterOfRecommendationGuidRepositoryIntegrationTests
    {
        private static GetLetterOfRecommendationGuidRepository _getLORGuidRepo;

        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            SetUp();
            GetWithWrongApplicantEmail();
            GetWithWrongReferenceEmail();
            HappyPath();
            HappyPathWithAlreadyCompleted();
        }

        private static void SetUp()
        {
            TestHelpersCommonFields.InitializeFields();
            TestHelpersCommonFakes.InitializeFakes();
            
            ApplicantsEmail = TestHelpersCommonFields.EmailDynamic();
            ReferencesEmail = TestHelpersCommonFields.EmailDynamic();
            BadEmail = TestHelpersCommonFields.EmailDynamic();
            GuidSentToReference = Guid.NewGuid();

            _getLORGuidRepo = new GetLetterOfRecommendationGuidRepository(TestHelpersCommonFields.DatabaseName, TestHelpersCommonFakes.ClaimsInformationGetters);

            FakeOutPeopleAndLetterOfRecommendation();
        }
        
        private static void FakeOutPeopleAndLetterOfRecommendation()
        {
            var reference = new Reference
            {
                Person = new Person
                {
                    ContactInformation = new ContactInformation
                    {
                        EmailAddress = ReferencesEmail,
                        LastUpdated = DateTime.UtcNow
                    },
                    DateCreated = DateTime.UtcNow,
                    Guid = Guid.NewGuid()
                }
            };

            var applicant = new Applicant
            {
                Person = new Person
                {
                    ContactInformation = new ContactInformation
                    {
                        EmailAddress = ApplicantsEmail,
                        LastUpdated = DateTime.UtcNow
                    },
                    DateCreated = DateTime.UtcNow,
                    Guid = Guid.NewGuid()
                },
                LettersOfRecommendation = new Collection<LetterOfRecommendation>
                {
                    new LetterOfRecommendation
                    {
                        GuidSentToReference = GuidSentToReference,
                        Reference = reference
                    }
                }
            };

            using (var context = GetRootContext())
            {
                context.Applicants.Add(applicant);
                context.SaveChanges();
            }
        }

        private static string ApplicantsEmail { get; set; }
        private static string ReferencesEmail { get; set; }
        private static string BadEmail { get; set; }
        private static Guid GuidSentToReference { get; set; }

        private static void GetWithWrongApplicantEmail()
        {
            GetWithWrongApplicantEmailResult =
                _getLORGuidRepo.GetLetterOfRecommendationGuid(new GetLetterOfRecommendationGuidDto
                {
                    ApplicantsEmailAddress = BadEmail,
                    ReferencesEmailAddress = ReferencesEmail
                });
        }

        private static GuidSentToReferenceDto GetWithWrongApplicantEmailResult { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void GetLetterOfRecommendationGuidRepository_GetLetterOfRecommendationGuid_WrongApplicantEmail_Should_Return_WrongApplicantEmailErrorMessage()
        {
            Assert.AreEqual("No applicant with that email address is in the db.", GetWithWrongApplicantEmailResult.ErrorMessage);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetLetterOfRecommendationGuidRepository_GetLetterOfRecommendationGuid_WrongApplicantEmail_Should_Return_ZerosGuid()
        {
            Assert.AreEqual(new Guid(), GetWithWrongApplicantEmailResult.GuidSentToReference);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetLetterOfRecommendationGuidRepository_GetLetterOfRecommendationGuid_WrongApplicantEmail_Should_False_Alread_Updated()
        {
            Assert.IsFalse(GetWithWrongApplicantEmailResult.AlreadyUpdated);
        }

        private static void GetWithWrongReferenceEmail()
        {
            GetWithWrongReferenceEmailResult =
                _getLORGuidRepo.GetLetterOfRecommendationGuid(new GetLetterOfRecommendationGuidDto
                {
                    ApplicantsEmailAddress = ApplicantsEmail,
                    ReferencesEmailAddress = BadEmail
                });
        }

        private static GuidSentToReferenceDto GetWithWrongReferenceEmailResult { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void GetLetterOfRecommendationGuidRepository_GetLetterOfRecommendationGuid_WrongReferenceEmail_Should_Return_WrongReferenceEmailErrorMessage()
        {
            Assert.AreEqual("That applicant doesn't have a letter of recommendation started from that reference.", GetWithWrongReferenceEmailResult.ErrorMessage);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetLetterOfRecommendationGuidRepository_GetLetterOfRecommendationGuid_WrongReferenceEmail_Should_Return_ZerosGuid()
        {
            Assert.AreEqual(new Guid(), GetWithWrongReferenceEmailResult.GuidSentToReference);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetLetterOfRecommendationGuidRepository_GetLetterOfRecommendationGuid_WrongReferenceEmail_Should_False_Alread_Updated()
        {
            Assert.IsFalse(GetWithWrongReferenceEmailResult.AlreadyUpdated);
        }

        private static void HappyPath()
        {
            HappyPathResult = _getLORGuidRepo.GetLetterOfRecommendationGuid(new GetLetterOfRecommendationGuidDto
            {
                ReferencesEmailAddress = ReferencesEmail,
                ApplicantsEmailAddress = ApplicantsEmail
            });
        }

        private static GuidSentToReferenceDto HappyPathResult { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void GetLetterOfRecommendationGuidRepository_GetLetterOfRecommendationGuid_HappyPath_Should_Return_null_ErrorMessage()
        {
            Assert.IsNull(HappyPathResult.ErrorMessage);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetLetterOfRecommendationGuidRepository_GetLetterOfRecommendationGuid_HappyPath_Should_Return_CorrectGuid()
        {
            Assert.AreEqual(GuidSentToReference, HappyPathResult.GuidSentToReference);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetLetterOfRecommendationGuidRepository_GetLetterOfRecommendationGuid_HappyPath_Should_False_Alread_Updated()
        {
            Assert.IsFalse(HappyPathResult.AlreadyUpdated);
        }

        private static void HappyPathWithAlreadyCompleted()
        {
            using (var context = GetRootContext())
            {
                var letterOfRecommendation =
                    context.LetterOfRecommendations.First(
                        x =>
                            x.Reference.Person.ContactInformation.EmailAddress == ReferencesEmail &&
                            x.Applicant.Person.ContactInformation.EmailAddress == ApplicantsEmail);

                letterOfRecommendation.LastUpdated = DateTime.UtcNow;
                context.SaveChanges();
            }

            HappyPathWithAlreadyCompletedResult =
                _getLORGuidRepo.GetLetterOfRecommendationGuid(new GetLetterOfRecommendationGuidDto
                {
                    ApplicantsEmailAddress = ApplicantsEmail,
                    ReferencesEmailAddress = ReferencesEmail
                });
        }

        private static GuidSentToReferenceDto HappyPathWithAlreadyCompletedResult { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void GetLetterOfRecommendationGuidRepository_GetLetterOfRecommendationGuid_HappyPathWithAlreadyCompleted_Should_Return_null_ErrorMessage()
        {
            Assert.IsNull(HappyPathWithAlreadyCompletedResult.ErrorMessage);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetLetterOfRecommendationGuidRepository_GetLetterOfRecommendationGuid_HappyPathWithAlreadyCompleted_Should_Return_CorrectGuid()
        {
            Assert.AreEqual(GuidSentToReference, HappyPathWithAlreadyCompletedResult.GuidSentToReference);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetLetterOfRecommendationGuidRepository_GetLetterOfRecommendationGuid_HappyPathWithAlreadyCompleted_Should_True_Alread_Updated()
        {
            Assert.IsTrue(HappyPathWithAlreadyCompletedResult.AlreadyUpdated);
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
