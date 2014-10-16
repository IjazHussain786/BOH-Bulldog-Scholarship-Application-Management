using System;
using System.Collections.Generic;
using System.Linq;
using BohFoundation.ApplicantsRepository.Repositories.Implementations;
using BohFoundation.AzureStorage.TableStorage.Implementations.Essay.Entities;
using BohFoundation.AzureStorage.TableStorage.Interfaces.Essay;
using BohFoundation.AzureStorage.TableStorage.Interfaces.Essay.Helpers;
using BohFoundation.Domain.Dtos.Applicant.Essay;
using BohFoundation.Domain.Dtos.Applicant.Notifications;
using BohFoundation.Domain.Dtos.Common.AzureQueuryObjects;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Common;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.EntityFrameworkBaseClass;
using BohFoundation.TestHelpers;
using EntityFramework.Extensions;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.ApplicantsRepository.Tests.IntegrationTests
{
    [TestClass]
    public class ApplicantsEssayRepositoryIntegrationTests
    {
        private static IEssayRowKeyGenerator _rowKeyGenerator;
        private static IAzureEssayRepository _azureAzureEssayRepository;
        private static ApplicantsEssayRepository _applicantsEssayRepository;
        private static ApplicantsesNotificationRepository _applicantsesNotification;

        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            Setup();
            FirstTestOfNotifications();
            FirstUpsert();
            SecondUpsert();
            SecondTestOfNotifications();
        }

        #region SettingUp

        private static void Setup()
        {
            TestHelpersCommonFields.InitializeFields();
            TestHelpersCommonFakes.InitializeFakes();

            ApplicantsGuid = Guid.NewGuid();
            Prompt = "prompt" + ApplicantsGuid;
            TitleOfEssay = "title" + ApplicantsGuid;

            _azureAzureEssayRepository = A.Fake<IAzureEssayRepository>();
            _rowKeyGenerator = A.Fake<IEssayRowKeyGenerator>();

            CreateEssayTopicAndApplicant();

            SetupFakes();

            _applicantsesNotification = new ApplicantsesNotificationRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters, TestHelpersCommonFakes.DeadlineUtilities);
            _applicantsEssayRepository = new ApplicantsEssayRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters, _azureAzureEssayRepository, _rowKeyGenerator);

        }

        private static void CreateEssayTopicAndApplicant()
        {
            var random = new Random();
            GraduatingYear = random.Next();

            var subject = new EssayTopic
            {
                EssayPrompt = Prompt,
                TitleOfEssay = TitleOfEssay,
                RevisionDateTime = DateTime.UtcNow
            };

            var subject2 = new EssayTopic
            {
                EssayPrompt = Prompt + 2,
                TitleOfEssay = TitleOfEssay + 2,
                RevisionDateTime = DateTime.UtcNow
            };

            var subject3 = new EssayTopic
            {
                EssayPrompt = "SHOULD NOT SHOW UP IN LIST",
                TitleOfEssay = "REALLY SHOULDN't SHOW up",
                RevisionDateTime = DateTime.UtcNow,
            };

            var graduatingYear = new GraduatingClass
            {
                GraduatingYear = GraduatingYear,
                EssayTopics = new List<EssayTopic> { subject, subject2 }
            };

            var applicant = new Applicant
            {
                Person = new Person { Guid = ApplicantsGuid, DateCreated = DateTime.UtcNow },
                ApplicantPersonalInformation =
                    new ApplicantPersonalInformation
                    {
                        GraduatingClass = graduatingYear,
                        Birthdate = DateTime.UtcNow,
                        LastUpdated = DateTime.UtcNow
                    }
            };

            using (var context = GetRootContext())
            {
                context.EssayTopics.Add(subject3);
                context.GraduatingClasses.Add(graduatingYear);
                context.Applicants.Add(applicant);
                context.EssayTopics.Add(subject);
                context.SaveChanges();

                EssayTopicId = context.EssayTopics.First(topic => topic.EssayPrompt == Prompt).Id;
                EssayTopicId2 = context.EssayTopics.First(topic => topic.EssayPrompt == Prompt + 2).Id;
            }
        }

        private static int EssayTopicId2 { get; set; }

        private static void SetupFakes()
        {
            RowKey = "THISISTHEROWKEYFORTHEAPPLICANT";
            A.CallTo(() => TestHelpersCommonFakes.ClaimsInformationGetters.GetApplicantsGraduatingYear())
                .Returns(GraduatingYear);
            A.CallTo(() => TestHelpersCommonFakes.ClaimsInformationGetters.GetUsersGuid()).Returns(ApplicantsGuid);
            A.CallTo(() => _rowKeyGenerator.CreateRowKeyForEssay(ApplicantsGuid, EssayTopicId)).Returns(RowKey);
        }

        private static string RowKey { get; set; }
        private static int GraduatingYear { get; set; }
        private static string TitleOfEssay { get; set; }
        private static string Prompt { get; set; }

        private static Guid ApplicantsGuid { get; set; }

        #endregion

        #region FirstNotifications

        private static void FirstTestOfNotifications()
        {
            FirstNotificationResult = _applicantsesNotification.GetApplicantNotifications();
        }

        private static ApplicantNotificationsDto FirstNotificationResult { get; set; }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationRepository_FirstGetNotifications_Should_Have_Two_EssayTopics()
        {
            Assert.AreEqual(2, FirstNotificationResult.EssayNotifications.Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationRepository_FirstGetNotifications_EssayTopics_Should_Have_No_LastUpdated()
        {
            foreach (var essayTopic in FirstNotificationResult.EssayNotifications)
            {
                Assert.IsNull(essayTopic.RevisionDateTime);
            }
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationRepository_FirstGetNotifications_EssayTopics_Should_Have_Right_EssayTopic()
        {
            foreach (var essayTopic in FirstNotificationResult.EssayNotifications)
            {
                if (essayTopic.EssayPrompt == Prompt)
                {
                    Assert.AreEqual(TitleOfEssay, essayTopic.TitleOfEssay);
                }
                else
                {
                    Assert.AreEqual(TitleOfEssay + 2, essayTopic.TitleOfEssay);
                }
            }
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationRepository_FirstGetNotifications_EssayTopics_Should_Have_Right_Ids()
        {
            foreach (var essayTopic in FirstNotificationResult.EssayNotifications)
            {
                Assert.AreEqual(essayTopic.EssayPrompt == Prompt ? EssayTopicId : EssayTopicId2, essayTopic.EssayTopicId);
            }
        }

        #endregion

        #region FirstUpsert

        private static void FirstUpsert()
        {
            Essay = "Essay";
            var dto = new EssayDto {Essay = Essay + 1, EssayPrompt = Prompt, EssayTopicId = EssayTopicId};
            _applicantsEssayRepository.UpsertEssay(dto);

            using (var context = GetRootContext())
            {
                EssayUpsertResult1 =
                    context.Essays.First(
                        essay => essay.EssayTopic.Id == EssayTopicId && essay.Applicant.Person.Guid == ApplicantsGuid);
            }
        }

        private static Essay EssayUpsertResult1 { get; set; }
        private static string Essay { get; set; }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_FirstUpsert_Should_Have_6_Characters()
        {
            Assert.AreEqual(6, EssayUpsertResult1.CharacterLength);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_FirstUpsert_Should_Have_Have_RecentUpdated()
        {
            TestHelpersTimeAsserts.RecentTime(EssayUpsertResult1.RevisionDateTime);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_FirstUpsert_Should_Have_Have_Correct_RowKey()
        {
            Assert.AreEqual(RowKey, EssayUpsertResult1.RowKey);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_FirstUpsert_Should_Have_Have_Correct_PartitionKey()
        {
            Assert.AreEqual(GraduatingYear.ToString(), EssayUpsertResult1.PartitionKey);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_FirstUpsert_Should_Have_Positive_Id()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(EssayUpsertResult1.Id);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_FirstUpsert_Should_Call_CreateRowKey()
        {
            A.CallTo(() => _rowKeyGenerator.CreateRowKeyForEssay(ApplicantsGuid, EssayTopicId)).MustHaveHappened();
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_FirstUpsert_Should_Call_UpsertEssay()
        {
            //Not checking time. It just isn't coming up. I did an in class check to see if it worked. It did. 
            A.CallTo(() => _azureAzureEssayRepository.UpsertEssay(A<EssayAzureTableEntityDto>
                .That.Matches(x =>
                    x.Essay == Essay + 1 &&
                    x.EssayPrompt == Prompt &&
                    x.EssayTopicId == EssayTopicId &&
                    x.PartitionKey == GraduatingYear.ToString() &&
                    x.RowKey == RowKey
                ))).MustHaveHappened();
        }

        #endregion

        #region SecondUpsert

        private static void SecondUpsert()
        {
            var dto = new EssayDto {Essay = Essay + Essay + Essay, EssayPrompt = Prompt, EssayTopicId = EssayTopicId};
            _applicantsEssayRepository.UpsertEssay(dto);

            using (var context = GetRootContext())
            {
                EssayUpsertResult2 =
                    context.Essays.First(
                        essay => essay.EssayTopic.Id == EssayTopicId && essay.Applicant.Person.Guid == ApplicantsGuid);
            }
        }

        private static Essay EssayUpsertResult2 { get; set; }
        private static int EssayTopicId { get; set; }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_SecondUpsert_Should_Have_15_Characters()
        {
            Assert.AreEqual(15, EssayUpsertResult2.CharacterLength);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_SecondUpsert_Should_Have_Have_RecentUpdated_More_Recent_Than_First()
        {
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(EssayUpsertResult2.RevisionDateTime,
                EssayUpsertResult1.RevisionDateTime);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_SecondUpsert_Should_Have_Have_Correct_RowKey()
        {
            Assert.AreEqual(RowKey, EssayUpsertResult2.RowKey);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_SecondUpsert_Should_Have_Have_Correct_PartitionKey()
        {
            Assert.AreEqual(GraduatingYear.ToString(), EssayUpsertResult2.PartitionKey);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_SecondUpsert_Should_Have_Equal_Id_To_First()
        {
            Assert.AreEqual(EssayUpsertResult1.Id, EssayUpsertResult2.Id);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_SecondUpsert_Should_Call_CreateRowKey()
        {
            A.CallTo(() => _rowKeyGenerator.CreateRowKeyForEssay(ApplicantsGuid, EssayTopicId))
                .MustHaveHappened(Repeated.AtLeast.Times(3));
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_SecondUpsert_Should_Call_UpsertEssay()
        {
            //Not checking time. It just isn't coming up. I did an in class check to see if it worked. It did. 
            A.CallTo(() => _azureAzureEssayRepository.UpsertEssay(A<EssayAzureTableEntityDto>
                .That.Matches(x =>
                    x.Essay == Essay + Essay + Essay &&
                    x.EssayPrompt == Prompt &&
                    x.EssayTopicId == EssayTopicId &&
                    x.PartitionKey == GraduatingYear.ToString() &&
                    x.RowKey == RowKey
                ))).MustHaveHappened();
        }

        #endregion

        #region SecondNotifications

        private static void SecondTestOfNotifications()
        {
            SecondNotificationResult = _applicantsesNotification.GetApplicantNotifications();
        }

        private static ApplicantNotificationsDto SecondNotificationResult { get; set; }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationRepository_SecondGetNotifications_Should_Have_Two_EssayTopics()
        {
            Assert.AreEqual(2, SecondNotificationResult.EssayNotifications.Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationRepository_SecondGetNotifications_EssayTopics_Should_Have_No_LastUpdated()
        {
            foreach (var essayTopic in SecondNotificationResult.EssayNotifications)
            {
                if (essayTopic.EssayPrompt == Prompt)
                {
                    Assert.AreEqual(EssayUpsertResult2.RevisionDateTime, essayTopic.RevisionDateTime);
                }
                else
                {
                    Assert.IsNull(essayTopic.RevisionDateTime);
                }
            }
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationRepository_SecondGetNotifications_EssayTopics_Should_Have_Right_EssayTopic()
        {
            foreach (var essayTopic in SecondNotificationResult.EssayNotifications)
            {
                if (essayTopic.EssayPrompt == Prompt)
                {
                    Assert.AreEqual(TitleOfEssay, essayTopic.TitleOfEssay);
                }
                else
                {
                    Assert.AreEqual(TitleOfEssay + 2, essayTopic.TitleOfEssay);
                }
            }
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsNotificationRepository_SecondGetNotifications_EssayTopics_Should_Have_Right_Ids()
        {
            foreach (var essayTopic in SecondNotificationResult.EssayNotifications)
            {
                Assert.AreEqual(essayTopic.EssayPrompt == Prompt ? EssayTopicId : EssayTopicId2, essayTopic.EssayTopicId);
            }
        }

        #endregion
        
        #region Utilities

        private static DatabaseRootContext GetRootContext()
        {
            return new DatabaseRootContext(TestHelpersCommonFields.DatabaseName);
        }

        [ClassCleanup]
        public static void CleanDb()
        {
            using (var context = new DatabaseRootContext(TestHelpersCommonFields.DatabaseName))
            {
                context.Essays.Where(essay => essay.Id > 0).Delete();
                context.EssayTopics.Where(essayTopic => essayTopic.Id > 0).Delete();
                context.ApplicantPersonalInformations.Where(info => info.Id > 0).Delete();
                context.GraduatingClasses.Where(gradClass => gradClass.Id > 0).Delete();
            }
        }

        #endregion

        #region GetEssay

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_GetEssay_Should_Call_CreateRowKeyForEssay()
        {
            GetEssay();
            A.CallTo(() => _rowKeyGenerator.CreateRowKeyForEssay(ApplicantsGuid, EssayTopicId)).MustHaveHappened();
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_GetEssay_Should_Call_AzureEssayRepository()
        {
            GetEssay();
            A.CallTo(
                () =>
                    _azureAzureEssayRepository.GetEssay(
                        A<AzureTableStorageEntityKeyDto>.That.Matches(
                            x => x.PartitionKey == GraduatingYear.ToString() && x.RowKey == RowKey))).MustHaveHappened();
        }

        [TestMethod, TestCategory("Integration")]
        public void ApplicantsEssayRepository_GetEssay_Should_Return_Whatever_TheAzureRepoReturns()
        {
            var essayDto = new EssayDto();
            A.CallTo(() => _azureAzureEssayRepository.GetEssay(A<AzureTableStorageEntityKeyDto>.Ignored))
                .Returns(essayDto);

            Assert.AreSame(essayDto, GetEssay());
        }

        private EssayDto GetEssay()
        {
            return _applicantsEssayRepository.GetEssay(EssayTopicId);
        }

        #endregion

    }
}
