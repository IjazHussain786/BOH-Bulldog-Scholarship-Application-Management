using System;
using System.Collections.Generic;
using System.Linq;
using BohFoundation.ApplicantsRepository.Repositories.Implementations;
using BohFoundation.Domain.Dtos.Applicant.Notifications;
using BohFoundation.Domain.Dtos.Applicant.References;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Common;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.EntityFrameworkModels.References;
using BohFoundation.EntityFrameworkBaseClass;
using BohFoundation.TestHelpers;
using BohFoundation.Utilities.Context.Implementation;
using BohFoundation.Utilities.Context.Interfaces;
using EntityFramework.Extensions;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.ApplicantsRepository.Tests.IntegrationTests
{
    [TestClass]
    public class ApplicantsReferencesRepositoryIntegrationTests
    {
        private static ApplicantsReferencesRepository _applicantsReferencesRepository;
        private static ApplicantsesNotificationRepository _notificationRepo;
        private static IDeadlineUtilities _deadlineUtilities;
        private const string Relationship = "Relationship";
       
        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            Setup();
            GetReferencesFirstTime();
            AddReferenceFirstTime();
            GetReferencesSecondTime();
            AddReferenceSecondTime();
            GetReferencesThirdTime(); //Edit last modified situation.
        }

        private static void Setup()
        {
            TestHelpersCommonFields.InitializeFields();
            TestHelpersCommonFakes.InitializeFakes();
            CreateApplicant();

            _deadlineUtilities = A.Fake<IDeadlineUtilities>();

            Deadline = DateTime.UtcNow;
            
            A.CallTo(() => _deadlineUtilities.GetApplicantsDeadlineInUtc()).Returns(Deadline);
            A.CallTo(() => TestHelpersCommonFakes.ClaimsInformationGetters.GetUsersGuid()).Returns(ApplicantsGuid);

            _applicantsReferencesRepository = new ApplicantsReferencesRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters);

            _notificationRepo = new ApplicantsesNotificationRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters, _deadlineUtilities);
        }

        private static DateTime Deadline { get; set; }

        private static void CreateApplicant()
        {
            ApplicantsGuid = Guid.NewGuid();

            var random = new Random();


            var graduatingClass = new GraduatingClass
            {
                GraduatingYear = random.Next()
            };

            var applicant = new Applicant
            {
                Person = new Person { Guid = ApplicantsGuid, DateCreated = DateTime.UtcNow },
                ApplicantPersonalInformation =
                    new ApplicantPersonalInformation
                    {
                        GraduatingClass = graduatingClass,
                        Birthdate = DateTime.UtcNow,
                        LastUpdated = DateTime.UtcNow
                    }
            };

            using (var context = GetRootContext())
            {
                context.Applicants.Add(applicant);
                context.SaveChanges();
            }
        }

        private static Guid ApplicantsGuid { get; set; }

        #region GetReferenceFirstTime()

        private static void GetReferencesFirstTime()
        {
            GetReferenceResult1 = _applicantsReferencesRepository.GetReferences();
            GetNotificationResult1 = _notificationRepo.GetApplicantNotifications();
        }

        private static ApplicantNotificationsDto GetNotificationResult1 { get; set; }
        private static ICollection<ReferenceDto> GetReferenceResult1 { get; set; }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_GetReference_First_Time_Should_Return_NoItems()
        {
            Assert.AreEqual(0, GetReferenceResult1.Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationsRepository_References_First_Send_Should_Return_0()
        {
            Assert.AreEqual(0, GetNotificationResult1.ApplicantReferenceCounts.NumberOfReferenceInvitationsSent);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationsRepository_References_First_Recieved_Should_Return_0()
        {
            Assert.AreEqual(0, GetNotificationResult1.ApplicantReferenceCounts.NumberOfReferencesRecieved);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationsRepository_References_First_LastUpdated_Should_Return_Null()
        {
            Assert.IsNull(GetNotificationResult1.ApplicantReferenceCounts.LastUpdated);
        }

        #endregion

        #region AddReferenceFirstTime

        private static void AddReferenceFirstTime()
        {
            ReferenceOneEmailAddress = CreateRandomEmail();
            ReferenceOneGuidLink = Guid.NewGuid();

            var referenceInputDto = new ApplicantReferenceForEntityFrameworkDto
            {
                ReferenceEmail = ReferenceOneEmailAddress,
                GuidLink = ReferenceOneGuidLink,
                RelationshipToReference = Relationship + 1,
            };

            _applicantsReferencesRepository.AddReference(referenceInputDto);

            using (var context = GetRootContext())
            {
                var reference = context.People.First(
                    persons => persons.ContactInformation.EmailAddress == ReferenceOneEmailAddress);

                var applicant = context.People.First(person => person.Guid == ApplicantsGuid);

                AddApplicantReferencesResult1 = applicant.Applicant.LettersOfRecommendation.ToList();
                AddReferenceResult1 = reference.Reference.LettersOfRecommendation.ToList();

                var forceContactInfo = reference.Reference.Person.ContactInformation;
            }

            AddDateTime1 = DateTime.UtcNow;
        }

        private static DateTime AddDateTime1 { get; set; }
        private static List<LetterOfRecommendation> AddReferenceResult1 { get; set; }
        private static List<LetterOfRecommendation> AddApplicantReferencesResult1 { get; set; }


        private static Guid ReferenceOneGuidLink { get; set; }
        private static string ReferenceOneEmailAddress { get; set; }

        [TestMethod, TestCategory("Integration")]
        public void
            ApplicantsReferencesRepository_AddReference_First_Time_Should_SetTheProperValues_Reference_Applicant_Same()
        {
            Assert.AreSame(AddApplicantReferencesResult1[0], AddReferenceResult1[0]);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_AddReference_First_Time_Should_SetTheProperValues_Guid()
        {
            Assert.AreEqual(ReferenceOneGuidLink, AddReferenceResult1[0].GuidSentToReference);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_AddReference_First_Time_Should_SetTheProperValues_LastUpdated_Null()
        {
            Assert.IsNull(AddReferenceResult1[0].LastUpdated);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_AddReference_First_Time_Should_SetTheProperValues_KeyValue()
        {
            Assert.IsNull(AddReferenceResult1[0].LetterOfRecommendationRowKey);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_AddReference_First_Time_Should_Have_Positive_Id()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(AddReferenceResult1[0].Id);
        }

        [TestMethod, TestCategory("Integration")]
        public void
            ApplicantsReferencesRepository_AddReference_First_Time_Applicant_Should_HaveOne_LetterOfRecommentdation()
        {
            Assert.AreEqual(1, AddApplicantReferencesResult1.Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void
            ApplicantsReferencesRepository_AddReference_First_Time_Reference_Should_HaveOne_LetterOfRecommentdation()
        {
            Assert.AreEqual(1, AddReferenceResult1.Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_AddReference_First_Time_Reference_Should_EmailAddress()
        {
            Assert.AreEqual(ReferenceOneEmailAddress,
                AddApplicantReferencesResult1[0].Reference.Person.ContactInformation.EmailAddress);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_AddReference_First_Time_Reference_Should_Have_Persons_Guid()
        {
            Assert.AreNotEqual(new Guid(), AddReferenceResult1[0].Reference.Person.Guid);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_AddReference_First_Time_Reference_Should_Have_Persons_JustCreated()
        {
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(AddDateTime1,
                AddApplicantReferencesResult1[0].Reference.Person.DateCreated);
        }

        #endregion

        #region GetReferenceSecondTime

        private static void GetReferencesSecondTime()
        {
            GetReferenceResult2 = _applicantsReferencesRepository.GetReferences().ToList();
            GetNotificationResult2 = _notificationRepo.GetApplicantNotifications();
        }

        private static ApplicantNotificationsDto GetNotificationResult2 { get; set; }
        private static List<ReferenceDto> GetReferenceResult2 { get; set; }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationsRepository_References_Second_Send_Should_Return_1()
        {
            Assert.AreEqual(1, GetNotificationResult2.ApplicantReferenceCounts.NumberOfReferenceInvitationsSent);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationsRepository_References_Second_Recieved_Should_Return_0()
        {
            Assert.AreEqual(0, GetNotificationResult2.ApplicantReferenceCounts.NumberOfReferencesRecieved);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationsRepository_References_Second_LastUpdated_Should_Return_Null()
        {
            Assert.IsNull(GetNotificationResult1.ApplicantReferenceCounts.LastUpdated);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_GetReference_Second_Time_Should_Return_OneItem()
        {
            Assert.AreEqual(1, GetReferenceResult2.Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_GetReference_Second_Time_Should_Return_Null_ReferenceName()
        {
            Assert.IsNull(GetReferenceResult2[0].ReferenceName.FirstName);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_GetReference_Second_Time_Should_Return_ReferenceEmailOne()
        {
            Assert.AreEqual(ReferenceOneEmailAddress, GetReferenceResult2[0].ReferenceEmailAddress);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_GetReference_Second_Time_Should_Return_False_ReferenceLetterRecieved()
        {
            Assert.IsFalse(GetReferenceResult2[0].ReferenceLetterReceived);
        }

        #endregion

        #region AddReferenceSecondTime

        private static void AddReferenceSecondTime()
        {
            CreateReferenceAlreadyInSystem();

            ReferenceTwoGuidLink = Guid.NewGuid();

            var referenceDto = new ApplicantReferenceForEntityFrameworkDto
            {
                GuidLink = ReferenceTwoGuidLink,
                ReferenceEmail = ReferenceTwoEmailAddress,
                RelationshipToReference = Relationship + 2
            };

            _applicantsReferencesRepository.AddReference(referenceDto);

            using (var context = GetRootContext())
            {
                var reference = context.People.First(
                    persons => persons.ContactInformation.EmailAddress == ReferenceTwoEmailAddress);

                var applicant = context.People.First(person => person.Guid == ApplicantsGuid);

                AddApplicantReferencesResult2 = applicant.Applicant.LettersOfRecommendation.ToList();
                AddReferenceResult2 = reference.Reference.LettersOfRecommendation.ToList();

                var forceContactInfo = reference.Reference.Person.ContactInformation;
            }

            AddDateTime2 = DateTime.UtcNow;
        }

        private static void CreateReferenceAlreadyInSystem()
        {
            ReferenceTwoEmailAddress = CreateRandomEmail();
            ReferenceTwoGuid = Guid.NewGuid();

            var reference = new Reference
            {
                Person = new Person
                {
                    Guid = ReferenceTwoGuid,
                    DateCreated = DateTime.UtcNow,
                    ContactInformation = new ContactInformation
                    {
                        EmailAddress = ReferenceTwoEmailAddress,
                        LastUpdated = DateTime.UtcNow
                    },
                    Name = new Name
                    {
                        FirstName = TestHelpersCommonFields.FirstName,
                        LastName = TestHelpersCommonFields.LastName,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            };

            using (var context = GetRootContext())
            {
                context.References.Add(reference);
                context.SaveChanges();
            }
        }

        private static List<LetterOfRecommendation> AddApplicantReferencesResult2 { get; set; }
        private static List<LetterOfRecommendation> AddReferenceResult2 { get; set; }
        private static DateTime AddDateTime2 { get; set; }
        private static Guid ReferenceTwoGuidLink { get; set; }
        private static Guid ReferenceTwoGuid { get; set; }
        private static string ReferenceTwoEmailAddress { get; set; }

        [TestMethod, TestCategory("Integration")]
        public void
            ApplicantsReferencesRepository_AddReference_Second_Time_Should_SetTheProperValues_Reference_Applicant_Same()
        {
            Assert.AreSame(AddApplicantReferencesResult2[1], AddReferenceResult2[0]);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_AddReference_Second_Time_Should_SetTheProperValues_Guid()
        {
            Assert.AreEqual(ReferenceTwoGuidLink, AddReferenceResult2[0].GuidSentToReference);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_AddReference_Second_Time_Should_SetTheProperValues_LastUpdated_Null()
        {
            Assert.IsNull(AddReferenceResult2[0].LastUpdated);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_AddReference_Second_Time_Should_SetTheProperValues_KeyValue()
        {
            Assert.IsNull(AddReferenceResult2[0].LetterOfRecommendationRowKey);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_AddReference_Second_Time_Should_Have_Positive_Id()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(AddReferenceResult2[0].Id);
        }

        [TestMethod, TestCategory("Integration")]
        public void
            ApplicantsReferencesRepository_AddReference_Second_Time_Applicant_Should_Have_Two_LetterOfRecommentdation()
        {
            Assert.AreEqual(2, AddApplicantReferencesResult2.Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void
            ApplicantsReferencesRepository_AddReference_Second_Time_Reference_Should_HaveOne_LetterOfRecommentdation()
        {
            Assert.AreEqual(1, AddReferenceResult2.Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_AddReference_Second_Time_Reference_Should_EmailAddress()
        {
            Assert.AreEqual(ReferenceTwoEmailAddress,
                AddReferenceResult2[0].Reference.Person.ContactInformation.EmailAddress);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_AddReference_Second_Time_Reference_Should_Have_Persons_Guid()
        {
            Assert.AreEqual(ReferenceTwoGuid, AddReferenceResult2[0].Reference.Person.Guid);
        }

        #endregion

        private static void GetReferencesThirdTime()
        {
            LastUpdated = DateTime.UtcNow;

            using (var context = GetRootContext())
            {
                var letterOfRecommendation =
                    context.LetterOfRecommendations.First(letter => letter.GuidSentToReference == ReferenceTwoGuidLink);

                letterOfRecommendation.LastUpdated = LastUpdated;
                letterOfRecommendation.LetterOfRecommendationRowKey = "keyvalue";
                context.SaveChanges();
            }

            GetReferenceResult3 = _applicantsReferencesRepository.GetReferences().ToList();
            GetNotificationResult3 = _notificationRepo.GetApplicantNotifications();
        }

        private static DateTime? LastUpdated { get; set; }
        private static ApplicantNotificationsDto GetNotificationResult3 { get; set; }
        private static List<ReferenceDto> GetReferenceResult3 { get; set; }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationsRepository_References_Third_Send_Should_Return_2()
        {
            Assert.AreEqual(2, GetNotificationResult3.ApplicantReferenceCounts.NumberOfReferenceInvitationsSent);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationsRepository_References_Third_Recieved_Should_Return_1()
        {
            Assert.AreEqual(1, GetNotificationResult3.ApplicantReferenceCounts.NumberOfReferencesRecieved);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationsRepository_References_Third_LastUpdated_Should_Return_Equal_To_What_We_Inputed()
        {
            Assert.AreEqual(LastUpdated.Value.Second, GetNotificationResult3.ApplicantReferenceCounts.LastUpdated.Value.Second);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_GetReference_Third_Time_Should_Return_TwoItems()
        {
            Assert.AreEqual(2, GetReferenceResult3.Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_GetReference_Third_Time_Should_Return_ReferenceName_First()
        {
            Assert.AreEqual(TestHelpersCommonFields.FirstName, GetReferenceResult3[1].ReferenceName.FirstName);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_GetReference_Third_Time_Should_Return_ReferenceName_Last()
        {
            Assert.AreEqual(TestHelpersCommonFields.LastName, GetReferenceResult3[1].ReferenceName.LastName);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_GetReference_Third_Time_Should_Return_ReferenceEmailOne()
        {
            Assert.AreEqual(ReferenceTwoEmailAddress, GetReferenceResult3[1].ReferenceEmailAddress);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsReferencesRepository_GetReference_Third_Time_Should_Return_True_ReferenceLetterRecieved()
        {
            Assert.IsTrue(GetReferenceResult3[1].ReferenceLetterReceived);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationsRepository_Deadline_Date_Should_Be_What_The_Deadline_Utilities_Returns()
        {

            Assert.AreEqual(Deadline, GetNotificationResult1.DeadlineDate);
        }

        #region Utilities

        private static DatabaseRootContext GetRootContext()
        {
            return new DatabaseRootContext(TestHelpersCommonFields.DatabaseName);
        }

        private static string CreateRandomEmail()
        {
            return Guid.NewGuid() + "@email.email";
        }

        [ClassCleanup]
        public static void CleanDb()
        {
            using (var context = new DatabaseRootContext(TestHelpersCommonFields.DatabaseName))
            {
                context.ApplicantPersonalInformations.Where(info => info.Id > 0).Delete();
                context.GraduatingClasses.Where(gradClass => gradClass.Id > 0).Delete();
                context.LetterOfRecommendations.Where(letterOfRecommendation => letterOfRecommendation.Id > 0).Delete();
                context.References.Where(reference => reference.Id > 0).Delete();
            }
        }

        #endregion

    }
}
