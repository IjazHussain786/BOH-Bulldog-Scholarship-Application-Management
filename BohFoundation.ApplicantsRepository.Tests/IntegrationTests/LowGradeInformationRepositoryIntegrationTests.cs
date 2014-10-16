using System;
using System.Collections.Generic;
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
    public class LowGradeInformationRepositoryIntegrationTests
    {
        private static LowGradeInformationRepository _lowGradeRepo;
        private static ApplicantsesNotificationRepository _applicantsesNotificationRepo;
        private static Guid Guid { get; set; }

        private const double Gpa = 3.1;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            TestHelpersCommonFields.InitializeFields();
            TestHelpersCommonFakes.InitializeFakes();
            Guid = TestHelpersCommonFields.GuidOne;

            CreateLowGradesRepo();
            AddPersonAndCreateAcademicInformation();
            FirstGetLowGradesInformation();
            FirstUpsert();
            SecondUpsert();
            SecondGetLowGradesInformation();
        }

        #region Setup

        private static void CreateLowGradesRepo()
        {
            A.CallTo(() => TestHelpersCommonFakes.ClaimsInformationGetters.GetUsersGuid())
                .Returns(Guid);

            _lowGradeRepo = new LowGradeInformationRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters);
            _applicantsesNotificationRepo = new ApplicantsesNotificationRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters, TestHelpersCommonFakes.DeadlineUtilities);
        }

        private static void AddPersonAndCreateAcademicInformation()
        {
            var name = new Name
            {
                FirstName = TestHelpersCommonFields.FirstName,
                LastName = TestHelpersCommonFields.LastName
            };
            var personsRepo = new CreatePersonRepository(TestHelpersCommonFields.DatabaseName);

            personsRepo.CreatePerson(Guid, name, MemberTypesEnum.Applicant);

            using (var context = new ApplicantRepositoryDbContext(TestHelpersCommonFields.DatabaseName))
            {
                var newPerson = context.People.First(person => person.Guid == Guid);
                newPerson.Applicant.AcademicInformation = new AcademicInformation{Gpa = Gpa};
                context.SaveChanges();
            }
        }

        #endregion

        #region FirstGet

        private static void FirstGetLowGradesInformation()
        {
            FirstGetLowGradesResult = _lowGradeRepo.GetLowGradeInformation();
            FirstNotificationResult = _applicantsesNotificationRepo.GetApplicantNotifications();
        }

        private static LowGradesWithGpaDto FirstGetLowGradesResult { get; set; }
        private static ApplicantNotificationsDto FirstNotificationResult { get; set; }


        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_FirstGet_Should_Return_ZeroItems()
        {
            Assert.AreEqual(0, FirstGetLowGradesResult.LowGrades.Count);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_FirstGet_Should_Return_Gpa()
        {
            Assert.AreEqual(Gpa, FirstGetLowGradesResult.Gpa);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_FirstGet_Should_Return_Null_Gpa()
        {
            Assert.AreEqual(Gpa, FirstNotificationResult.LowGradeNotificationInformation.Gpa);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_FirstGet_Should_Return_Null_LastUpdated()
        {
            Assert.IsNull(FirstNotificationResult.LowGradeNotificationInformation.LastUpdatedLowGrade);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_FirstGet_Should_Return_Zero_NumberOfLowGrades()
        {
            Assert.AreEqual(0, FirstNotificationResult.LowGradeNotificationInformation.NumberOfLowGradesInformationSaved);
        }


        #endregion

        #region FirstUpsert

        private static void FirstUpsert()
        {
            var collectionOfLowGrades = CreateListOfLowGradeDtos(2);
            _lowGradeRepo.UpsertLowGradeInformation(collectionOfLowGrades);

            using (var context = new ApplicantRepositoryDbContext(TestHelpersCommonFields.DatabaseName))
            {
                var person = context.People.First(persons => persons.Guid == Guid);
                var lowGrades = person.Applicant.AcademicInformation.LowGrades;
                FirstUpsertResult = lowGrades;
            }
            if (FirstUpsertResult != null)
            {
                FirstUpsertResultZeroItem = FirstUpsertResult.FirstOrDefault(item => item.ClassTitle == Class + 0);
            }
        }

        private static LowGrade FirstUpsertResultZeroItem { get; set; }
        private static ICollection<LowGrade> FirstUpsertResult { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_FirstUpsert_Should_Have_One_Item()
        {
            Assert.AreEqual(2, FirstUpsertResult.Count);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_FirstUpsert_Should_Have_Positive_Id()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(FirstUpsertResultZeroItem.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_FirstUpsert_Should_Have_Correct_Grade()
        {
            Assert.AreEqual(Grade + 0, FirstUpsertResultZeroItem.Grade);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_FirstUpsert_Should_Have_Correct_Class()
        {
            Assert.AreEqual(Class + 0, FirstUpsertResultZeroItem.ClassTitle);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_FirstUpsert_Should_Have_Correct_WasAp()
        {
            Assert.IsTrue(FirstUpsertResultZeroItem.WasAp);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_FirstUpsert_Should_Have_Correct_YearOfHs()
        {
            Assert.AreEqual(YearOfHighSchoolEnum.Junior, FirstUpsertResultZeroItem.YearOfHighSchool);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_FirstUpsert_Should_Have_Correct_Explain()
        {
            Assert.AreEqual(Explaination + 0, FirstUpsertResultZeroItem.Explanation);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_FirstUpsert_Should_Have_Recent_LastUpdated()
        {
            TestHelpersTimeAsserts.RecentTime(FirstUpsertResultZeroItem.LastUpdated);
        }

        #endregion

        #region SecondUpsert

        private static void SecondUpsert()
        {
            var collectionOfLowGrades = CreateListOfLowGradeDtos(3);
            _lowGradeRepo.UpsertLowGradeInformation(collectionOfLowGrades);

            using (var context = new ApplicantRepositoryDbContext(TestHelpersCommonFields.DatabaseName))
            {
                var person = context.People.First(persons => persons.Guid == Guid);
                var lowGrades = person.Applicant.AcademicInformation.LowGrades;
                SecondUpsertResult = lowGrades;
            }
            if (FirstUpsertResult != null)
            {
                SecondUpsertResultZeroItem = SecondUpsertResult.FirstOrDefault(item => item.ClassTitle == Class + 0);
                SecondUpsertResultOneItem = SecondUpsertResult.FirstOrDefault(item => item.ClassTitle == Class + 1);
                SecondUpsertResultTwoItem = SecondUpsertResult.FirstOrDefault(item => item.ClassTitle == Class + 2);
            }
        }

        private static LowGrade SecondUpsertResultTwoItem { get; set; }
        private static LowGrade SecondUpsertResultOneItem { get; set; }
        private static LowGrade SecondUpsertResultZeroItem { get; set; }
        private static ICollection<LowGrade> SecondUpsertResult { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_One_Item()
        {
            Assert.AreEqual(3, SecondUpsertResult.Count);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Positive_Id_Zero()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(SecondUpsertResultZeroItem.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Different_Id_From_First()
        {
            Assert.AreNotEqual(FirstUpsertResultZeroItem.Id, SecondUpsertResultZeroItem.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Correct_Grade_Zero()
        {
            Assert.AreEqual(Grade + 0, SecondUpsertResultZeroItem.Grade);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Correct_Class_Zero()
        {
            Assert.AreEqual(Class + 0, SecondUpsertResultZeroItem.ClassTitle);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Correct_WasAp_Zero()
        {
            Assert.IsTrue(SecondUpsertResultZeroItem.WasAp);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Correct_YearOfHs_Zero()
        {
            Assert.AreEqual(YearOfHighSchoolEnum.Junior, SecondUpsertResultZeroItem.YearOfHighSchool);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Correct_Explain_Zero()
        {
            Assert.AreEqual(Explaination + 0, SecondUpsertResultZeroItem.Explanation);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Different_LastUpdated_Zero()
        {
            Assert.AreNotEqual(FirstUpsertResultZeroItem.LastUpdated, SecondUpsertResultZeroItem.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Correct_Grade_One()
        {
            Assert.AreEqual(Grade + 1, SecondUpsertResultOneItem.Grade);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Correct_Class_One()
        {
            Assert.AreEqual(Class + 1, SecondUpsertResultOneItem.ClassTitle);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Correct_WasAp_One()
        {
            Assert.IsFalse(SecondUpsertResultOneItem.WasAp);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Correct_YearOfHs_One()
        {
            Assert.AreEqual(YearOfHighSchoolEnum.Sophomore, SecondUpsertResultOneItem.YearOfHighSchool);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Correct_Explain_One()
        {
            Assert.AreEqual(Explaination + 1, SecondUpsertResultOneItem.Explanation);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Correct_Grade_Two()
        {
            Assert.AreEqual(Grade + 2, SecondUpsertResultTwoItem.Grade);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Correct_Class_Two()
        {
            Assert.AreEqual(Class + 2, SecondUpsertResultTwoItem.ClassTitle);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Correct_WasAp_Two()
        {
            Assert.IsTrue(SecondUpsertResultTwoItem.WasAp);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Correct_YearOfHs_Two()
        {
            Assert.AreEqual(YearOfHighSchoolEnum.Junior, SecondUpsertResultTwoItem.YearOfHighSchool);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Correct_Explain_Two()
        {
            Assert.AreEqual(Explaination + 2, SecondUpsertResultTwoItem.Explanation);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Same_LastUpdated_One_Zero()
        {
            Assert.AreEqual(SecondUpsertResultOneItem.LastUpdated, SecondUpsertResultZeroItem.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondUpsert_Should_Have_Same_LastUpdated_One_Two()
        {
            Assert.AreEqual(SecondUpsertResultOneItem.LastUpdated, SecondUpsertResultTwoItem.LastUpdated);
        }

        #endregion

        #region SecondGet

        private static void SecondGetLowGradesInformation()
        {
            SecondGetLowGradesResult = _lowGradeRepo.GetLowGradeInformation();
            SecondNotificationResult = _applicantsesNotificationRepo.GetApplicantNotifications();
        }
        private static LowGradesWithGpaDto SecondGetLowGradesResult { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondGet_Should_Return_Three_Items()
        {
            Assert.AreEqual(3, SecondGetLowGradesResult.LowGrades.Count);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondGet_Should_Return_Gpa()
        {
            Assert.AreEqual(Gpa, SecondGetLowGradesResult.Gpa);
        }

        [TestCategory("Integration")] 
        [TestMethod] 
        public void LowGradeInformationRepository_SecondGet_Should_Return_CorrectClasses()
        {
            var array = SecondGetLowGradesResult.LowGrades.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(Class+i, array[i].ClassTitle);
            }
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondGet_Should_Return_Correct_Explanations()
        {
            var array = SecondGetLowGradesResult.LowGrades.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(Explaination + i, array[i].Explanation);
            }
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void LowGradeInformationRepository_SecondGet_Should_Return_Correct_Grade()
        {
            var array = SecondGetLowGradesResult.LowGrades.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(Grade + i, array[i].Grade);
            }
        }

        #endregion
        
        #region SecondNotifications

        private static ApplicantNotificationsDto SecondNotificationResult { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_SecondGet_Should_Return_Same_As_Second_Upsert_Gpa()
        {
            Assert.AreEqual(Gpa, SecondNotificationResult.LowGradeNotificationInformation.Gpa);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_SecondGet_Should_Return_Same_As_SecondUpsert_LastUpdated()
        {
            Assert.AreEqual(SecondUpsertResultZeroItem.LastUpdated,
                SecondNotificationResult.LowGradeNotificationInformation.LastUpdatedLowGrade);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepo_SecondGet_Should_Return_3_NumberOfLowGrades()
        {
            Assert.AreEqual(3, SecondNotificationResult.LowGradeNotificationInformation.NumberOfLowGradesInformationSaved);
        }

        #endregion


        #region CreateListOfLowGrades

        private const string Grade = "Grade";
        private const string Class = "Class";
        private const string Explaination = "explanation.... asdfnjkhsafd230safhjjkn asdfjnjklnjkl23f09asdfjnkl askjfnjksdjklfj2389asdfkjn kjjkasdjfsafdnlkj";


        private static ICollection<LowGradeDto> CreateListOfLowGradeDtos(int howMany)
        {
            var list = new List<LowGradeDto>();

            for (int i = 0; i < howMany; i++)
            {
                list.Add(new LowGradeDto
                {
                    ClassTitle = Class + i,
                    Explanation = Explaination + i,
                    Grade = Grade + i,
                    WasAp = ClassWasAp(i),
                    YearOfHighSchool = WhichYear(i)
                });
            }
            return list;
        }

        private static bool ClassWasAp(int index)
        {
            return index % 2 == 0;
        }

        private static YearOfHighSchoolEnum WhichYear(int index)
        {
            return index % 2 == 0 ? YearOfHighSchoolEnum.Junior : YearOfHighSchoolEnum.Sophomore;
        }

        #endregion

    }
}
