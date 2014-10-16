using System;
using System.Linq;
using BohFoundation.ApplicantsRepository.DbContext;
using BohFoundation.ApplicantsRepository.Repositories.Implementations;
using BohFoundation.Domain.Dtos.Applicant.Academic;
using BohFoundation.Domain.Dtos.Applicant.Notifications;
using BohFoundation.Domain.Dtos.Common;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Academic;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.Enums;
using BohFoundation.PersonsRepository.Repositories.Implementations;
using BohFoundation.TestHelpers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.ApplicantsRepository.Tests.IntegrationTests
{
    [TestClass]
    public class TranscriptReferenceRepositoryIntegrationTests
    {
        private static ApplicantsesNotificationRepository _applicantsesNotificationRepo;
        private static TranscriptReferenceRepository _transcriptRepository;
        private static Guid Guid { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            TestHelpersCommonFields.InitializeFields();
            TestHelpersCommonFakes.InitializeFakes();
            Guid = TestHelpersCommonFields.GuidOne;

            CreateTranscriptReferenceRepository();
            AddPerson();
            GetFirstNotification();
            AddFirstTranscriptReference();
            UpdateTranscriptReference();
            GetSecondNotification();
        }
        
        #region Setup

        private static void AddPerson()
        {
            var name = new Name
            {
                FirstName = TestHelpersCommonFields.FirstName,
                LastName = TestHelpersCommonFields.LastName
            };
            var personsRepo = new CreatePersonRepository(TestHelpersCommonFields.DatabaseName);

            personsRepo.CreatePerson(Guid, name, MemberTypesEnum.Applicant);
        }

        private static void CreateTranscriptReferenceRepository()
        {
            A.CallTo(() => TestHelpersCommonFakes.ClaimsInformationGetters.GetUsersGuid())
                .Returns(Guid);

            _transcriptRepository = new TranscriptReferenceRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters);
            _applicantsesNotificationRepo = new ApplicantsesNotificationRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters, TestHelpersCommonFakes.DeadlineUtilities);

        }

        #endregion

        private static void GetFirstNotification()
        {
            FirstNotificationResult = _applicantsesNotificationRepo.GetApplicantNotifications();
            LastUpdatedTranscriptResult0 = _transcriptRepository.LastUpdatedTranscript();
        }

        private static LastUpdatedDto LastUpdatedTranscriptResult0 { get; set; }
        private static ApplicantNotificationsDto FirstNotificationResult { get; set; }

        
        [TestCategory("Integration")]
        [TestMethod]
        public void TranscriptReferenceRepository_UpsertTranscriptReference_FirstNotification_Should_Have_Null_LastUpdated()
        {
            Assert.IsNull(FirstNotificationResult.LastUpdatedTranscriptUpload);
        }

        [TestMethod, TestCategory("Integration")]
        public void TranscriptReferenceRepository_LastUpdatedTranscript_FirstNotification_Should_Have_Null_LastUpdated()
        {
            Assert.IsNull(LastUpdatedTranscriptResult0.LastUpdated);
        }

        #region AddFirstReference

        private static void AddFirstTranscriptReference()
        {
            Container = "container";
            ReferenceToTranscriptPdf = "ReferenceToTranscriptPdf";

            var dto = new TranscriptBlobReferenceDto
            {
                BlobContainerName = Container,
                ReferenceToTranscriptPdf = ReferenceToTranscriptPdf
            };

            _transcriptRepository.UpsertTranscriptReference(dto);

            using (var context = new ApplicantRepositoryDbContext(TestHelpersCommonFields.DatabaseName))
            {
                var result = context.People.First(person => person.Guid == Guid);
                FirstTransciptUpsertResult = result.Applicant.AcademicInformation.Transcript;
            }

            LastUpdatedTranscriptResult1 = _transcriptRepository.LastUpdatedTranscript();
        }

        private static TranscriptBlobReference FirstTransciptUpsertResult { get; set; }
        private static string ReferenceToTranscriptPdf { get; set; }
        private static string Container { get; set; }
        private static LastUpdatedDto LastUpdatedTranscriptResult1 { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void TranscriptReferenceRepository_UpsertTranscriptReference_FirstInsert_Should_Add_Reference()
        {
            Assert.AreEqual(ReferenceToTranscriptPdf, FirstTransciptUpsertResult.ReferenceToTranscriptPdf);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void TranscriptReferenceRepository_UpsertTranscriptReference_FirstInsert_Should_Add_Container()
        {
            Assert.AreEqual(Container, FirstTransciptUpsertResult.BlobContainerName);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void TranscriptReferenceRepository_UpsertTranscriptReference_FirstInsert_Should_Add_Id()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(FirstTransciptUpsertResult.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void TranscriptReferenceRepository_UpsertTranscriptReference_FirstInsert_Should_Add_LastUpdatedTime()
        {
            TestHelpersTimeAsserts.RecentTime(FirstTransciptUpsertResult.LastUpdated);
        }

        [TestMethod, TestCategory("Integration")]
        public void TranscriptReferenceRepository_LastUpdatedTranscript_FirstInsert_Should_Be_Same_As_From_FirstTranscriptUpsertResult()
        {
            Assert.AreEqual(FirstTransciptUpsertResult.LastUpdated, LastUpdatedTranscriptResult1.LastUpdated);
        }

        #endregion

        #region UpdateTranscript

        private static void UpdateTranscriptReference()
        {
            ContainerTwo = "container231";
            ReferenceToTranscriptPdfTwo = "ReferenceToTranscriptPdf1234";

            var dto = new TranscriptBlobReferenceDto
            {
                BlobContainerName = ContainerTwo,
                ReferenceToTranscriptPdf = ReferenceToTranscriptPdfTwo
            };

            _transcriptRepository.UpsertTranscriptReference(dto);

            using (var context = new ApplicantRepositoryDbContext(TestHelpersCommonFields.DatabaseName))
            {
                var result = context.People.First(person => person.Guid == Guid);
                SecondTranscriptReferenceResult = result.Applicant.AcademicInformation.Transcript;
            }
        }

        private static TranscriptBlobReference SecondTranscriptReferenceResult { get; set; }
        private static string ReferenceToTranscriptPdfTwo { get; set; }
        private static string ContainerTwo { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void TranscriptReferenceRepository_UpsertTranscriptReference_SecondInsert_Should_Change_Reference()
        {
            Assert.AreEqual(ReferenceToTranscriptPdfTwo, SecondTranscriptReferenceResult.ReferenceToTranscriptPdf);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void TranscriptReferenceRepository_UpsertTranscriptReference_SecondInsert_Should_Change_Container()
        {
            Assert.AreEqual(ContainerTwo, SecondTranscriptReferenceResult.BlobContainerName);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void TranscriptReferenceRepository_UpsertTranscriptReference_SecondInsert_Keep_Id()
        {
            Assert.AreEqual(FirstTransciptUpsertResult.Id, SecondTranscriptReferenceResult.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void TranscriptReferenceRepository_UpsertTranscriptReference_SecondInsert_Should_Add_New_LastUpdatedTime()
        {
            Assert.IsTrue(SecondTranscriptReferenceResult.LastUpdated >= FirstTransciptUpsertResult.LastUpdated);
        }

        #endregion
        
        #region SecondNotification

        private static void GetSecondNotification()
        {
            SecondNotificationResult = _applicantsesNotificationRepo.GetApplicantNotifications();
            LastUpdatedTranscriptResult2 = _transcriptRepository.LastUpdatedTranscript();
        }

        private static LastUpdatedDto LastUpdatedTranscriptResult2 { get; set; }
        private static ApplicantNotificationsDto SecondNotificationResult { get; set; }
        

        [TestCategory("Integration")]
        [TestMethod]
        public void
            TranscriptReferenceRepository_UpsertTranscriptReference_SecondNotification_Should_Have_LastUpdated_Equal_ToSecondUpdate
            ()
        {
            Assert.AreEqual(SecondTranscriptReferenceResult.LastUpdated,
                SecondNotificationResult.LastUpdatedTranscriptUpload);
        }

        [TestMethod, TestCategory("Integration")]
        public void TranscriptReferenceRepository_LastUpdatedTranscript_GetSecondNotification_Should_Be_Same_As_From_SecondTranscriptResult()
        {
            Assert.AreEqual(SecondTranscriptReferenceResult.LastUpdated, LastUpdatedTranscriptResult2.LastUpdated);
        }

        #endregion

    }
}
