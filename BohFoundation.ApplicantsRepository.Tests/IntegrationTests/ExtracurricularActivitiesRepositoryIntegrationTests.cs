using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BohFoundation.ApplicantsRepository.Repositories.Implementations;
using BohFoundation.Domain.Dtos.Applicant.Extracurricular;
using BohFoundation.Domain.Dtos.Applicant.Notifications;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.EntityFrameworkBaseClass;
using BohFoundation.TestHelpers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.ApplicantsRepository.Tests.IntegrationTests
{
    [TestClass]
    public class ExtracurricularActivitiesRepositoryIntegrationTests
    {
        private static ApplicantsesNotificationRepository _notificationsRepo;
        private static ExtracurricularActivitiesRepository _extracurricularActivitiesRepository;

        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            Setup();

            GetExtracurricularActivities1();
            UpsertExtracurricularActivities1();
            GetExtracurricularActivities2();
            UpsertExtracurricularActivities2();
            GetExtracurricularActivities3();
            UpsertExtracurricularActivities3();
            GetExtracurricularActivities4();
        }

        #region Setup

        private static void Setup()
        {
            TestHelpersCommonFields.InitializeFields();
            TestHelpersCommonFakes.InitializeFakes();

            AddNewApplicant();

            A.CallTo(() => TestHelpersCommonFakes.ClaimsInformationGetters.GetUsersGuid()).Returns(ApplicantGuid);

            _extracurricularActivitiesRepository =
                new ExtracurricularActivitiesRepository(TestHelpersCommonFields.DatabaseName,
                    TestHelpersCommonFakes.ClaimsInformationGetters);
            _notificationsRepo = new ApplicantsesNotificationRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters, TestHelpersCommonFakes.DeadlineUtilities);
        }

        private static void AddNewApplicant()
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
        }

        private static Guid ApplicantGuid { get; set; }

        #endregion

        #region Get1

        private static void GetExtracurricularActivities1()
        {
            GetResult1 = _extracurricularActivitiesRepository.GetExtracurricularActivities();
            GetNotifications1 = _notificationsRepo.GetApplicantNotifications();
        }

        private static ApplicantNotificationsDto GetNotifications1 { get; set; }
        private static ExtracurricularActivitiesDto GetResult1 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get1_Should_Return_Null()
        {
            Assert.IsNull(GetResult1);
        }

        [TestCategory("Integration"), TestMethod]
        public void NotificationRepository_Get1_Should_Return_Null()
        {
            Assert.IsNull(GetNotifications1.LastUpdatedExtracurriculars);
        }

        #endregion

        private static void UpsertExtracurricularActivities1()
        {
            ActivityName1 = "name";
            ActivityName2 = "name2";
            ActivitySummary1 = "summary";
            ActivitySummary2 = "asdfjk";

            var dto = new ExtracurricularActivitiesDto
            {
                Activities = new List<ActivityDto> { 
                    new ActivityDto { Name = ActivityName1, ShortSummaryOfWhatIsInvolved = ActivitySummary1}, 
                    new ActivityDto { Name = ActivityName2, ShortSummaryOfWhatIsInvolved = ActivitySummary2}
                },
                HasNonPaidActivities = true
            };

            _extracurricularActivitiesRepository.UpsertExtracurricularActivities(dto);
        }

        private static string ActivitySummary2 { get; set; }
        private static string ActivitySummary1 { get; set; }
        private static string ActivityName2 { get; set; }
        private static string ActivityName1 { get; set; }

        private static void GetExtracurricularActivities2()
        {
            GetResult2 = _extracurricularActivitiesRepository.GetExtracurricularActivities();
            GetNotifications2 = _notificationsRepo.GetApplicantNotifications();
        }

        private static ApplicantNotificationsDto GetNotifications2 { get; set; }
        private static ExtracurricularActivitiesDto GetResult2 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void NotificationRepository_Extracurriculars_Get2_Should_Return_SameAsGetResult2()
        {
            Assert.AreEqual(GetResult2.LastUpdated, GetNotifications2.LastUpdatedExtracurriculars);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get2_Should_Return_ExtracurricularActivitiesDto()
        {
            Assert.IsInstanceOfType(GetResult2, typeof(ExtracurricularActivitiesDto));
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get2_Should_Return_2_Activities()
        {
            Assert.AreEqual(2, GetResult2.Activities.Count);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get2_Should_Return_True_HasNonPaidActivities()
        {
            Assert.IsTrue(GetResult2.HasNonPaidActivities);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get2_Should_Return_Recent_LastUpdated()
        {
            TestHelpersTimeAsserts.RecentTime(GetResult2.LastUpdated);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get2_Should_Return_Activity1_With_Right_Summary()
        {
            Assert.AreEqual(ActivitySummary1, GetResult2.Activities.First(activity => activity.Name == ActivityName1).ShortSummaryOfWhatIsInvolved);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get2_Should_Return_Activity2_With_Right_Summary()
        {
            Assert.AreEqual(ActivitySummary2, GetResult2.Activities.First(activity => activity.Name == ActivityName2).ShortSummaryOfWhatIsInvolved);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get2_Should_Return_False_PaidWork()
        {
            Assert.IsFalse(GetResult2.PaidWork);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get2_Should_Return_0_Jobs()
        {
            Assert.AreEqual(0, GetResult2.Jobs.Count);
        }

        private static void UpsertExtracurricularActivities2()
        {
            JobPosition1 = "asdfjkkj";
            JobPosition2 = "20031";

            Employer1 = "3992";
            Employer2 = "99903021123adsfasdf";

            JobSummary1 = "sdfajkjkn238989asdf";
            JobSummary2 = "asdfjknjk23099asdfn32asdf";

            var dto = new ExtracurricularActivitiesDto
            {
                HasNonPaidActivities = false,
                PaidWork = true,
                Jobs = new Collection<JobDto>
                {
                    new JobDto
                    {
                        Employer = Employer1,
                        Position = JobPosition1,
                        ShortSummaryOfWorkResponsibilities = JobSummary1
                    },
                    new JobDto
                    {
                        Employer = Employer2,
                        Position = JobPosition2,
                        ShortSummaryOfWorkResponsibilities = JobSummary2
                    }
                }
            };

            _extracurricularActivitiesRepository.UpsertExtracurricularActivities(dto);
        }

        private static string JobSummary2 { get; set; }
        private static string JobSummary1 { get; set; }
        private static string Employer2 { get; set; }
        private static string Employer1 { get; set; }
        private static string JobPosition2 { get; set; }
        private static string JobPosition1 { get; set; }

        private static void GetExtracurricularActivities3()
        {
            GetResult3 = _extracurricularActivitiesRepository.GetExtracurricularActivities();
            GetNotifications3 = _notificationsRepo.GetApplicantNotifications();
        }

        private static ApplicantNotificationsDto GetNotifications3 { get; set; }
        private static ExtracurricularActivitiesDto GetResult3 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void NotificationRepository_Extracurriculars_Get3_Should_Return_SameAsGetResult3()
        {
            Assert.AreEqual(GetResult3.LastUpdated, GetNotifications3.LastUpdatedExtracurriculars);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get3_Should_Return_ExtracurricularActivitiesDto()
        {
            Assert.IsInstanceOfType(GetResult3, typeof(ExtracurricularActivitiesDto));
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get3_Should_Return_0_Activities()
        {
            Assert.AreEqual(0, GetResult3.Activities.Count);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get3_Should_Return_False_HasNonPaidActivities()
        {
            Assert.IsFalse(GetResult3.HasNonPaidActivities);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get3_Should_Return_LastUpdated_Greater_Than_2()
        {
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(GetResult3.LastUpdated ,GetResult2.LastUpdated);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get3_Should_Return_Job1_With_Right_Summary()
        {
            Assert.AreEqual(JobSummary1, GetResult3.Jobs.First(job => job.Employer == Employer1).ShortSummaryOfWorkResponsibilities);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get3_Should_Return_Job1_With_Right_Position()
        {
            Assert.AreEqual(JobPosition1, GetResult3.Jobs.First(job => job.Employer == Employer1).Position);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get3_Should_Return_Job2_With_Right_Summary()
        {
            Assert.AreEqual(JobSummary2, GetResult3.Jobs.First(job => job.Employer == Employer2).ShortSummaryOfWorkResponsibilities);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get3_Should_Return_Job2_With_Right_Position()
        {
            Assert.AreEqual(JobPosition2, GetResult3.Jobs.First(job => job.Employer == Employer2).Position);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get3_Should_Return_True_PaidWork()
        {
            Assert.IsTrue(GetResult3.PaidWork);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get3_Should_Return_2_Jobs()
        {
            Assert.AreEqual(2, GetResult3.Jobs.Count);
        }

        private static void UpsertExtracurricularActivities3()
        {
            JobPosition3 = "poai";
            JobSummary3 = "564564asd8932";
            Employer3 = "12003asdf";

            ActivityName3 = "asdfjk";
            ActivitySummary3 = "asdjkj09asdf3";

            var dto = new ExtracurricularActivitiesDto
            {
                PaidWork = true,
                HasNonPaidActivities = true,
                Jobs = new Collection<JobDto>
                {
                    new JobDto
                    {
                        Employer = Employer3,
                        Position = JobPosition3,
                        ShortSummaryOfWorkResponsibilities = JobSummary3
                    }
                },
                Activities = new Collection<ActivityDto>
                {
                    new ActivityDto
                    {
                        Name = ActivityName3,
                        ShortSummaryOfWhatIsInvolved = ActivitySummary3
                    }
                }
            };
    
            _extracurricularActivitiesRepository.UpsertExtracurricularActivities(dto);
        }

        private static string ActivitySummary3 { get; set; }
        private static string ActivityName3 { get; set; }
        private static string Employer3 { get; set; }
        private static string JobSummary3 { get; set; }
        private static string JobPosition3 { get; set; }

        private static void GetExtracurricularActivities4()
        {
            GetResult4 = _extracurricularActivitiesRepository.GetExtracurricularActivities();
        }

        private static ExtracurricularActivitiesDto GetResult4 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get4_Should_Return_ExtracurricularActivitiesDto()
        {
            Assert.IsInstanceOfType(GetResult4, typeof(ExtracurricularActivitiesDto));
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get4_Should_Return_1_Activities()
        {
            Assert.AreEqual(1, GetResult4.Activities.Count);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get4_Should_Return_Activity3_With_Right_Summary()
        {
            Assert.AreEqual(ActivitySummary3, GetResult4.Activities.First(activity => activity.Name == ActivityName3).ShortSummaryOfWhatIsInvolved);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get4_Should_Return_True_HasNonPaidActivities()
        {
            Assert.IsTrue(GetResult4.HasNonPaidActivities);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get4_Should_Return_LastUpdated_Greater_Than_3()
        {
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(GetResult4.LastUpdated, GetResult3.LastUpdated);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get4_Should_Return_Job3_With_Right_Summary()
        {
            Assert.AreEqual(JobSummary3, GetResult4.Jobs.First(job => job.Employer == Employer3).ShortSummaryOfWorkResponsibilities);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get4_Should_Return_Job3_With_Right_Position()
        {
            Assert.AreEqual(JobPosition3, GetResult4.Jobs.First(job => job.Employer == Employer3).Position);
        }
        
        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get4_Should_Return_True_PaidWork()
        {
            Assert.IsTrue(GetResult4.PaidWork);
        }

        [TestCategory("Integration"), TestMethod]
        public void ExtracurricularActivitiesRepository_Get4_Should_Return_1_Jobs()
        {
            Assert.AreEqual(1, GetResult4.Jobs.Count);
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
