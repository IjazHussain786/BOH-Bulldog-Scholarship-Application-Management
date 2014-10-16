using System;
using System.Linq;
using BohFoundation.ApplicantsRepository.DbContext;
using BohFoundation.ApplicantsRepository.Repositories.Implementations;
using BohFoundation.Domain.Dtos.Applicant.Academic;
using BohFoundation.Domain.Dtos.Applicant.Notifications;
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
    public class AcademicInformationRepositoryIntegrationTests
    {
        private static ApplicantsesNotificationRepository _applicantsesNotificationRepo;
        private static AcademicInformationRepository _academicInformationRepository;
        private static Guid Guid { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            TestHelpersCommonFields.InitializeFields();
            TestHelpersCommonFakes.InitializeFakes();
            Guid = TestHelpersCommonFields.GuidOne;

            CreateTranscriptReferenceRepository();
            AddPerson();
            FirstGetAcademicInformation();
            UpsertWithNoClassRank();
            FirstUpsert();
            SecondUpsert();
            SecondGetAcademicInformation();
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

            _academicInformationRepository = new AcademicInformationRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters);
            _applicantsesNotificationRepo = new ApplicantsesNotificationRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters, TestHelpersCommonFakes.DeadlineUtilities);
        }

        #endregion

        #region FirstGet

        private static void FirstGetAcademicInformation()
        {
            FirstGetAcademicInformationResult = _academicInformationRepository.GetAcademicInformation();
            FirstNotificationResult = _applicantsesNotificationRepo.GetApplicantNotifications();
        }

        private static AcademicInformationDto FirstGetAcademicInformationResult { get; set; }
        private static ApplicantNotificationsDto FirstNotificationResult { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void AcademicInformationRepository_FirstGet_Should_Return_Null()
        {
            Assert.IsNull(FirstGetAcademicInformationResult);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_FirstGet_Should_Return_Null()
        {
            Assert.IsNull(FirstNotificationResult.LastUpdatedAcademicInformation);
        }


        #endregion

        private static void UpsertWithNoClassRank()
        {
            Gpa0 = 3.9;
            CareerChoice0 = "Programmer";
            ProbableNextSchool0 = "Miami";

            var academicInformation = new AcademicInformation
            {
                Gpa = Gpa0,
                CareerChoice = CareerChoice0,
                ProbableNextSchool = ProbableNextSchool0,
                LastUpdated = DateTime.UtcNow
            };

            using (var context = new ApplicantRepositoryDbContext(TestHelpersCommonFields.DatabaseName))
            {
                var person = context.People.First(persons => persons.Guid == Guid);
                person.Applicant.AcademicInformation = academicInformation;
                context.SaveChanges();
                
                NoClassRankResult = person.Applicant.AcademicInformation;
                var populate = NoClassRankResult.ClassRank;
            }

        }


        private static AcademicInformation NoClassRankResult { get; set; }
        private static string ProbableNextSchool0 { get; set; }
        private static string CareerChoice0 { get; set; }
        private static double Gpa0 { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_NoRank_Should_Return_LastUpdated_AcademicInformation()
        {
            TestHelpersTimeAsserts.RecentTime(NoClassRankResult.LastUpdated.Value);
        }

        #region FirstUpsert

        private static void FirstUpsert()
        {
            Gpa1 = 4.0;
            CareerChoice1 = "Farmer";
            ProbableNextSchool1 = "NYI";
            ClassNumericalRank1 = 13;
            GraduatingClassSize1 = 500;

            var classRank = new ClassRankDto
            {
                ClassNumericalRank = ClassNumericalRank1,
                GraduatingClassSize = GraduatingClassSize1
            };

            var academicInformation = new AcademicInformationDto
            {
                CareerChoice = CareerChoice1,
                ClassRank = classRank,
                Gpa = Gpa1,
                ProbableNextSchool = ProbableNextSchool1
            };

            _academicInformationRepository.UpsertAcademicInformation(academicInformation);

            using (var context = new ApplicantRepositoryDbContext(TestHelpersCommonFields.DatabaseName))
            {
                var person = context.People.First(persons => persons.Guid == Guid);
                FirstUpsertResult = person.Applicant.AcademicInformation;
                var forceLoad = FirstUpsertResult.ClassRank;
            }


            NotificationForHighGpaResult = _applicantsesNotificationRepo.GetApplicantNotifications();
        }

        private static AcademicInformation FirstUpsertResult { get; set; }
        private static ApplicantNotificationsDto NotificationForHighGpaResult { get; set; }

        private static int GraduatingClassSize1 { get; set; }
        private static int ClassNumericalRank1 { get; set; }
        private static string ProbableNextSchool1 { get; set; }
        private static string CareerChoice1 { get; set; }
        private static double Gpa1 { get; set; }


        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_FirstUpsert_Should_Return_LastUpdated_ClassRank()
        {
            TestHelpersTimeAsserts.RecentTime(FirstUpsertResult.ClassRank.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_FirstUpsert_Should_Return_LastUpdated_AcademicInformation()
        {
            Assert.AreEqual(FirstUpsertResult.LastUpdated, FirstUpsertResult.ClassRank.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_FirstUpsert_Should_Return_Positive_Id_ClassRank()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(FirstUpsertResult.ClassRank.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_FirstUpsert_Should_Have_Same_Ids_ClassRank_AcademicInformation()
        {
            Assert.AreEqual(FirstUpsertResult.Id, FirstUpsertResult.ClassRank.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_FirstUpsert_Should_Return_Correct_Gpa()
        {
            Assert.AreEqual(Gpa1, FirstUpsertResult.Gpa);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_FirstUpsert_Should_Return_Correct_CareerChoice()
        {
            Assert.AreEqual(CareerChoice1, FirstUpsertResult.CareerChoice);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_FirstUpsert_Should_Return_Correct_ProbableNextSchool()
        {
            Assert.AreEqual(ProbableNextSchool1, FirstUpsertResult.ProbableNextSchool);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_FirstUpsert_Should_Return_Correct_ClassNumericalRank()
        {
            Assert.AreEqual(ClassNumericalRank1, FirstUpsertResult.ClassRank.ClassNumericalRank);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_FirstUpsert_Should_Return_Correct_GraduatingClassSize()
        {
            Assert.AreEqual(GraduatingClassSize1, FirstUpsertResult.ClassRank.GraduatingClassSize);
        }

        #endregion

        #region SecondUpsert

        private static void SecondUpsert()
        {
            Gpa2 = 3.49;
            CareerChoice2 = "Accountant";
            ProbableNextSchool2 = "BYU";
            ClassNumericalRank2 = 203;
            GraduatingClassSize2 = 400;

            var classRank = new ClassRankDto
            {
                ClassNumericalRank = ClassNumericalRank2,
                GraduatingClassSize = GraduatingClassSize2
            };

            var academicInformation = new AcademicInformationDto
            {
                CareerChoice = CareerChoice2,
                ClassRank = classRank,
                Gpa = Gpa2,
                ProbableNextSchool = ProbableNextSchool2
            };

            _academicInformationRepository.UpsertAcademicInformation(academicInformation);

            using (var context = new ApplicantRepositoryDbContext(TestHelpersCommonFields.DatabaseName))
            {
                var person = context.People.First(persons => persons.Guid == Guid);
                SecondUpsertResult = person.Applicant.AcademicInformation;
                var forceLoad = SecondUpsertResult.ClassRank;
            }
        }


        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_SecondUpsert_Should_Return_LastUpdated_ClassRank_GreaterThanFirst()
        {
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(SecondUpsertResult.ClassRank.LastUpdated,
                FirstUpsertResult.ClassRank.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void
            ApplicantsNotificationRepo_SecondUpsert_Should_Return_LastUpdated_AcademicInformation_GreaterThanFirst()
        {
            var secondUpsertResultLastUpdated = SecondUpsertResult.LastUpdated.GetValueOrDefault();
            var firstUpsertResultLastUpdated = FirstUpsertResult.LastUpdated.GetValueOrDefault();
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(secondUpsertResultLastUpdated, firstUpsertResultLastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_SecondUpsert_Should_Return_Same_Id_AsFirst_ClassRank()
        {
            Assert.AreEqual(FirstUpsertResult.ClassRank.Id, SecondUpsertResult.ClassRank.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_SecondUpsert_Should_Have_Same_Id_AsFirst_ClassRank_AcademicInformation()
        {
            Assert.AreEqual(FirstUpsertResult.Id, SecondUpsertResult.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_SecondUpsert_Should_Return_Correct_Gpa()
        {
            Assert.AreEqual(Gpa2, SecondUpsertResult.Gpa);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_SecondUpsert_Should_Return_Correct_CareerChoice()
        {
            Assert.AreEqual(CareerChoice2, SecondUpsertResult.CareerChoice);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_SecondUpsert_Should_Return_Correct_ProbableNextSchool()
        {
            Assert.AreEqual(ProbableNextSchool2, SecondUpsertResult.ProbableNextSchool);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_SecondUpsert_Should_Return_Correct_ClassNumericalRank()
        {
            Assert.AreEqual(ClassNumericalRank2, SecondUpsertResult.ClassRank.ClassNumericalRank);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_SecondUpsert_Should_Return_Correct_GraduatingClassSize()
        {
            Assert.AreEqual(GraduatingClassSize2, SecondUpsertResult.ClassRank.GraduatingClassSize);
        }

        private static AcademicInformation SecondUpsertResult { get; set; }

        private static int GraduatingClassSize2 { get; set; }
        private static int ClassNumericalRank2 { get; set; }
        private static string ProbableNextSchool2 { get; set; }
        private static string CareerChoice2 { get; set; }
        private static double Gpa2 { get; set; }

        #endregion
        
        private static void SecondGetAcademicInformation()
        {
            SecondGetAcademicInformationResult = _academicInformationRepository.GetAcademicInformation();
            SecondNotificationLowGpaResult = _applicantsesNotificationRepo.GetApplicantNotifications();
        }

        private static AcademicInformationDto SecondGetAcademicInformationResult { get; set; }
        private static ApplicantNotificationsDto SecondNotificationLowGpaResult { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void AcademicInformationRepository_SecondGet_Should_Return_Second_Gpa()
        {
            Assert.AreEqual(Gpa2, SecondGetAcademicInformationResult.Gpa);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void AcademicInformationRepository_SecondGet_Should_Return_Second_CareerChoice()
        {
            Assert.AreEqual(CareerChoice2, SecondGetAcademicInformationResult.CareerChoice);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void AcademicInformationRepository_SecondGet_Should_Return_Second_ProbableNextSchool()
        {
            Assert.AreEqual(ProbableNextSchool2, SecondGetAcademicInformationResult.ProbableNextSchool);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void AcademicInformationRepository_SecondGet_Should_Return_Second_LastUpdated_AcademicInformation()
        {
            Assert.AreEqual(SecondUpsertResult.LastUpdated, SecondGetAcademicInformationResult.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void AcademicInformationRepository_SecondGet_Should_Return_Second_LastUpdated_ClassRank()
        {
            Assert.AreEqual(SecondUpsertResult.ClassRank.LastUpdated, SecondGetAcademicInformationResult.ClassRank.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void AcademicInformationRepository_SecondGet_Should_Return_Second_ClassNumericalRank()
        {
            Assert.AreEqual(ClassNumericalRank2, SecondGetAcademicInformationResult.ClassRank.ClassNumericalRank);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void AcademicInformationRepository_SecondGet_Should_Return_Second_GraduatingClassSize()
        {
            Assert.AreEqual(GraduatingClassSize2, SecondGetAcademicInformationResult.ClassRank.GraduatingClassSize); 
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_SecondGet_Should_Return_LastUpdatedFromMainObject()
        {
            Assert.AreEqual(SecondUpsertResult.LastUpdated, SecondNotificationLowGpaResult.LastUpdatedAcademicInformation);
        }

    }
}
