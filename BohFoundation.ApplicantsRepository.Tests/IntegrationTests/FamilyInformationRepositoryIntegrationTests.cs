using System;
using System.Linq;
using BohFoundation.ApplicantsRepository.Repositories.Implementations;
using BohFoundation.Domain.Dtos.Applicant.Family;
using BohFoundation.Domain.Dtos.Applicant.Notifications;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Family;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.Enums;
using BohFoundation.EntityFrameworkBaseClass;
using BohFoundation.TestHelpers;
using BohFoundation.Utilities.Context.Interfaces;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.ApplicantsRepository.Tests.IntegrationTests
{
    [TestClass]
    public class FamilyInformationRepositoryIntegrationTests
    {
        private static FamilyInformationRepository _familyInformationRepo;
        private static ApplicantsesNotificationRepository _notificationsRepo;
        private static IDeadlineUtilities _deadlineNotifications;

        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {

            Setup();
            GetFamilyInformation1();
            UpsertFamilyInformation1();
            GetFamilyInformation2();
            UpsertFamilyInformation2();
            GetFamilyInformation3();
        }

        #region Setup

        private static void Setup()
        {
            TestHelpersCommonFields.InitializeFields();
            TestHelpersCommonFakes.InitializeFakes();

            AddNewApplicant();

            A.CallTo(() => TestHelpersCommonFakes.ClaimsInformationGetters.GetUsersGuid()).Returns(ApplicantGuid);

            _familyInformationRepo = new FamilyInformationRepository(TestHelpersCommonFields.DatabaseName,
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

        #region GetFamilyInformation1

        private static void GetFamilyInformation1()
        {
            GetFamilyResult1 = _familyInformationRepo.GetFamilyInformation();
            FamilyInformationNotification1 = _notificationsRepo.GetApplicantNotifications().LastUpdatedFamilyInformation;
        }

        private static DateTime? FamilyInformationNotification1 { get; set; }
        private static FamilyInformationDto GetFamilyResult1 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void FamilyInformationRepository_GetFamilyInformation_GetFamilyInformation1_Returns_Null()
        {
            Assert.IsNull(GetFamilyResult1);
        }

        [TestCategory("Integration"), TestMethod]
        public void FamilyInformationRepository_Notifications_FamilyInformationNotification1_Returns_Null()
        {
            Assert.IsNull(FamilyInformationNotification1);
        }

        #endregion

        #region UpsertFamilyInformation1

        private static void UpsertFamilyInformation1()
        {
            HighestDegreeAttained1 = EducationalDegreesEnum.Bachelors;
            NumberOfPeople1 = 3;
            HouseholdIncome = IncomeRangeEnum.Mt40000Lt50000;


            var familyInformationDto = new FamilyInformationDto
            {
                HighestAttainedDegreeInHome = HighestDegreeAttained1,
                NumberOfPeopleInHousehold = NumberOfPeople1,
                YearlyHouseholdIncomeRange = HouseholdIncome
            };

            _familyInformationRepo.UpsertFamilyInformation(familyInformationDto);

            using (var context = GetRootContext())
            {
                UpsertInformationResult1 =
                    context.People.First(person => person.Guid == ApplicantGuid).Applicant.FamilyInformation;
            }
        }

        private static FamilyInformation UpsertInformationResult1 { get; set; }
        private static IncomeRangeEnum HouseholdIncome { get; set; }
        private static int NumberOfPeople1 { get; set; }
        private static EducationalDegreesEnum HighestDegreeAttained1 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void FamilyInformationRepository_UpsertFamilyInformation_UpsertFamilyInformation1_Returns_Positive_Id()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(UpsertInformationResult1.Id);
        }

        [TestCategory("Integration"), TestMethod]
        public void FamilyInformationRepository_UpsertFamilyInformation_UpsertFamilyInformation1_Returns_Recent_LastUpdated()
        {
            TestHelpersTimeAsserts.RecentTime(UpsertInformationResult1.LastUpdated);
        }

        #endregion

        #region GetFamilyInformation2

        private static void GetFamilyInformation2()
        {
            GetFamilyResult2 = _familyInformationRepo.GetFamilyInformation();
            FamilyInformationNotification2 = _notificationsRepo.GetApplicantNotifications().LastUpdatedFamilyInformation;
        }

        private static DateTime? FamilyInformationNotification2 { get; set; }
        private static FamilyInformationDto GetFamilyResult2 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void FamilyInformationRepository_Notifications_FamilyInformationNotification2_Returns_Same_As_GetFamilyResult2()
        {
            Assert.AreEqual(GetFamilyResult2.LastUpdated, FamilyInformationNotification2);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            FamilyInformationRepository_GetFamilyInformation_GetFamilyInformation2_Returns_Correct_NumberOfPeopleInHousehold
            ()
        {
            Assert.AreEqual(NumberOfPeople1, GetFamilyResult2.NumberOfPeopleInHousehold);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            FamilyInformationRepository_GetFamilyInformation_GetFamilyInformation2_Returns_Correct_YearlyHouseholdIncomeRange
            ()
        {
            Assert.AreEqual(HouseholdIncome, GetFamilyResult2.YearlyHouseholdIncomeRange);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            FamilyInformationRepository_GetFamilyInformation_GetFamilyInformation2_Returns_Correct_HighestAttainedDegreeInHome
            ()
        {
            Assert.AreEqual(HighestDegreeAttained1, GetFamilyResult2.HighestAttainedDegreeInHome);
        }

        [TestCategory("Integration"), TestMethod]
        public void FamilyInformationRepository_GetFamilyInformation_GetFamilyInformation2_Returns_Correct_LastUpdated()
        {
            Assert.AreEqual(UpsertInformationResult1.LastUpdated, GetFamilyResult2.LastUpdated);
        }

        #endregion

        #region UpsertFamilyInformation2

        private static void UpsertFamilyInformation2()
        {
            HighestDegreeAttained2 = EducationalDegreesEnum.Doctorate;
            HouseholdIncome2 = IncomeRangeEnum.Mt90000Lt100000;
            NumberOfPeople2 = 19909;

            var dto = new FamilyInformationDto
            {
                HighestAttainedDegreeInHome = HighestDegreeAttained2,
                NumberOfPeopleInHousehold = NumberOfPeople2,
                YearlyHouseholdIncomeRange = HouseholdIncome2
            };

            _familyInformationRepo.UpsertFamilyInformation(dto);

            using (var context = GetRootContext())
            {
                UpsertInformationResult2 =
                    context.People.First(person => person.Guid == ApplicantGuid).Applicant.FamilyInformation;
            }
        }

        private static FamilyInformation UpsertInformationResult2 { get; set; }
        private static int NumberOfPeople2 { get; set; }
        private static IncomeRangeEnum HouseholdIncome2 { get; set; }
        private static EducationalDegreesEnum HighestDegreeAttained2 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void FamilyInformationRepository_UpsertFamilyInformation_UpsertFamilyInformation2_Returns_Same_Id()
        {
            Assert.AreEqual(UpsertInformationResult1.Id, UpsertInformationResult2.Id);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            FamilyInformationRepository_UpsertFamilyInformation_UpsertFamilyInformation2_Returns_Greater_LastUpdated()
        {
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(UpsertInformationResult2.LastUpdated,
                UpsertInformationResult1.LastUpdated);
        }

        #endregion
        
        #region GetFamilyInformation3

        private static void GetFamilyInformation3()
        {
            GetFamilyResult3 = _familyInformationRepo.GetFamilyInformation();
            FamilyInformationNotification3 = _notificationsRepo.GetApplicantNotifications().LastUpdatedFamilyInformation;
        }

        private static DateTime? FamilyInformationNotification3 { get; set; }
        private static FamilyInformationDto GetFamilyResult3 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void FamilyInformationRepository_Notifications_FamilyInformationNotification3_Returns_Same_As_GetFamilyResult3()
        {
            Assert.AreEqual(GetFamilyResult3.LastUpdated, FamilyInformationNotification3);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            FamilyInformationRepository_GetFamilyInformation_GetFamilyInformation3_Returns_Correct_NumberOfPeopleInHousehold
            ()
        {
            Assert.AreEqual(NumberOfPeople2, GetFamilyResult3.NumberOfPeopleInHousehold);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            FamilyInformationRepository_GetFamilyInformation_GetFamilyInformation3_Returns_Correct_YearlyHouseholdIncomeRange
            ()
        {
            Assert.AreEqual(HouseholdIncome2, GetFamilyResult3.YearlyHouseholdIncomeRange);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            FamilyInformationRepository_GetFamilyInformation_GetFamilyInformation3_Returns_Correct_HighestAttainedDegreeInHome
            ()
        {
            Assert.AreEqual(HighestDegreeAttained2, GetFamilyResult3.HighestAttainedDegreeInHome);
        }

        [TestCategory("Integration"), TestMethod]
        public void FamilyInformationRepository_GetFamilyInformation_GetFamilyInformation3_Returns_Correct_LastUpdated()
        {
            Assert.AreEqual(UpsertInformationResult2.LastUpdated, GetFamilyResult3.LastUpdated);
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

        }

        #endregion
    }
}
