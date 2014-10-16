using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Implementations;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Academic;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Family;
using BohFoundation.Domain.EntityFrameworkModels.ApplicationEvaluators;
using BohFoundation.Domain.EntityFrameworkModels.Common;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.Enums;
using BohFoundation.EntityFrameworkBaseClass;
using BohFoundation.TestHelpers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.ApplicationEvaluatorsRepository.Tests.IntegrationTests
{
    [TestClass]
    public class GetListOfFinalizedApplicantsRepositoryTests
    {
        private static GetListOfFinalizedApplicantsRepository _repo;

        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            TestHelpersCommonFields.InitializeFields();
            TestHelpersCommonFakes.InitializeFakes();

            Setup();
            GetAllFinalizedApplicantsByGraduatingYear1();
            GetAllFinalizedApplicantsByGraduatingYear2();
        }

        #region Setup

        private static Random RandomNumbers { get; set; }

        private static void Setup()
        {
            RandomNumbers = new Random();
            CreateApplicants();
            CreateApplicationEvaluator();
            InstantiateRepo();
        }

        private static void CreateApplicants()
        {
            Year1 = RandomNumbers.Next();
            Year2 = RandomNumbers.Next();

            var graduatingClass1 = new GraduatingClass {GraduatingYear = Year1};
            var graduatingClass2 = new GraduatingClass {GraduatingYear = Year2};

            ApplicantYear1Finalized1AndRated = CreateApplicant(graduatingClass1, true);
            ApplicantYear1Finalized2 = CreateApplicant(graduatingClass1, true, false, "a", 3.5, IncomeRangeEnum.Lt20000);
            ApplicantYear1FinalizedNotCompleted1 = CreateApplicant(graduatingClass1, true, true);
            ApplicantYear1NotFinalized1 = CreateApplicant(graduatingClass1, false);

            ApplicantYear2Finalized1 = CreateApplicant(graduatingClass2, true);
        }

        private static Applicant ApplicantYear2Finalized1 { get; set; }
        private static Applicant ApplicantYear1NotFinalized1 { get; set; }
        private static Applicant ApplicantYear1FinalizedNotCompleted1 { get; set; }
        private static Applicant ApplicantYear1Finalized2 { get; set; }
        private static Applicant ApplicantYear1Finalized1AndRated { get; set; }
        private static int Year1 { get; set; }
        private static int Year2 { get; set; }

        private static void CreateApplicationEvaluator()
        {
            ApplicationEvaluatorsGuid = Guid.NewGuid();
            A.CallTo(() => TestHelpersCommonFakes.ClaimsInformationGetters.GetUsersGuid())
                .Returns(ApplicationEvaluatorsGuid);

            ApplicationEvaluator = new ApplicationEvaluator
            {
                Person = new Person
                {
                    Guid = ApplicationEvaluatorsGuid,
                    DateCreated = DateTime.UtcNow
                },
                ApplicantRating = new Collection<ApplicantRating>
                {
                    new ApplicantRating
                    {
                        Applicant = ApplicantYear1Finalized1AndRated,
                        OverallRating = new GenericRating
                        {
                            RatingEnum = RatingEnum.CPlus
                        }
                    }
                }
            };

            using (var context = GetRootContext())
            {
                var applicantList = new List<Applicant>
                {
                    ApplicantYear1Finalized1AndRated,
                    ApplicantYear1Finalized2,
                    ApplicantYear1FinalizedNotCompleted1,
                    ApplicantYear1NotFinalized1,
                    ApplicantYear2Finalized1
                };
                context.Applicants.AddRange(applicantList);
                context.ApplicationEvaluators.Add(ApplicationEvaluator);
                context.SaveChanges();
            }

        }

        private static ApplicationEvaluator ApplicationEvaluator { get; set; }
        private static Guid ApplicationEvaluatorsGuid { get; set; }


        private static void InstantiateRepo()
        {
            _repo = new GetListOfFinalizedApplicantsRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters);
        }

        #endregion

        #region Year1

        private static void GetAllFinalizedApplicantsByGraduatingYear1()
        {
            ResultFromYear1 = _repo.GetAllFinalizedApplicantsByGraduatingYear(Year1);
        }

        private static AllFinalizedApplicantsForAGraduatingYearDto ResultFromYear1 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year1_Should_Be_Right_Year()
        {
            Assert.AreEqual(Year1, ResultFromYear1.GraduatingYear);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year1_Should_Have_Half_Rated()
        {
            Assert.AreEqual(.5, ResultFromYear1.PercentRated);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year1_Should_2_Summaries()
        {
            Assert.AreEqual(2, ResultFromYear1.ApplicantSummaries.Count);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year1_Should_Be_Alphbetized_By_LastName()
        {
            var array = ResultFromYear1.ApplicantSummaries.ToArray();

            Assert.AreEqual(ApplicantYear1Finalized2.Person.Name.LastName, array[0].ApplicantName.LastName);
            Assert.AreEqual(ApplicantYear1Finalized1AndRated.Person.Name.LastName, array[1].ApplicantName.LastName);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year1_Applicant1_Should_HaveRightName()
        {
            var person =
                ResultFromYear1.ApplicantSummaries.First(
                    x => x.ApplicantGuid == ApplicantYear1Finalized1AndRated.Person.Guid);

            Assert.AreEqual(ApplicantYear1Finalized1AndRated.Person.Name.FirstName, person.ApplicantName.FirstName);
            Assert.AreEqual(ApplicantYear1Finalized1AndRated.Person.Name.LastName, person.ApplicantName.LastName);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year1_Applicant1_Should_Have_A_CPlus_Rating()
        {
            var person =
                ResultFromYear1.ApplicantSummaries.First(
                    x => x.ApplicantGuid == ApplicantYear1Finalized1AndRated.Person.Guid);

            Assert.AreEqual(RatingEnum.CPlus, person.YourRating);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year1_Applicant2_Should_HaveRightName()
        {
            var person =
                ResultFromYear1.ApplicantSummaries.First(
                    x => x.ApplicantGuid == ApplicantYear1Finalized2.Person.Guid);

            Assert.AreEqual(ApplicantYear1Finalized2.Person.Name.FirstName, person.ApplicantName.FirstName);
            Assert.AreEqual(ApplicantYear1Finalized2.Person.Name.LastName, person.ApplicantName.LastName);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year1_Applicant2_Should_Right_Gpa()
        {
            var person =
                ResultFromYear1.ApplicantSummaries.First(
                    x => x.ApplicantGuid == ApplicantYear1Finalized2.Person.Guid);

            Assert.AreEqual(ApplicantYear1Finalized2.AcademicInformation.Gpa, person.Gpa);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year1_Applicant1_Should_Right_Gpa()
        {
            var person =
                ResultFromYear1.ApplicantSummaries.First(
                    x => x.ApplicantGuid == ApplicantYear1Finalized1AndRated.Person.Guid);

            Assert.AreEqual(ApplicantYear1Finalized1AndRated.AcademicInformation.Gpa, person.Gpa);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year1_Applicant1_Should_Right_Income()
        {
            var person =
                ResultFromYear1.ApplicantSummaries.First(
                    x => x.ApplicantGuid == ApplicantYear1Finalized1AndRated.Person.Guid);

            Assert.AreEqual(ApplicantYear1Finalized1AndRated.FamilyInformation.YearlyHouseholdIncomeRange,
                person.IncomeRange);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year1_Applicant2_Should_Right_Income()
        {
            var person =
                ResultFromYear1.ApplicantSummaries.First(
                    x => x.ApplicantGuid == ApplicantYear1Finalized2.Person.Guid);

            Assert.AreEqual(ApplicantYear1Finalized2.FamilyInformation.YearlyHouseholdIncomeRange, person.IncomeRange);
        }

        #endregion

        #region Year2

        private static void GetAllFinalizedApplicantsByGraduatingYear2()
        {
            ResultFromYear2 = _repo.GetAllFinalizedApplicantsByGraduatingYear(Year2);
        }

        private static AllFinalizedApplicantsForAGraduatingYearDto ResultFromYear2 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year2_Should_Be_Right_Year()
        {
            Assert.AreEqual(Year2, ResultFromYear2.GraduatingYear);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year2_Should_Have_Half_Rated()
        {
            Assert.AreEqual(0, ResultFromYear2.PercentRated);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year2_Should_1_Summaries()
        {
            Assert.AreEqual(1, ResultFromYear2.ApplicantSummaries.Count);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year2_Applicant1_Should_HaveRightName()
        {
            var person =
                ResultFromYear2.ApplicantSummaries.First(
                    x => x.ApplicantGuid == ApplicantYear2Finalized1.Person.Guid);

            Assert.AreEqual(ApplicantYear2Finalized1.Person.Name.FirstName, person.ApplicantName.FirstName);
            Assert.AreEqual(ApplicantYear2Finalized1.Person.Name.LastName, person.ApplicantName.LastName);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year2_Applicant1_Should_Right_Gpa()
        {
            var person =
                ResultFromYear2.ApplicantSummaries.First(
                    x => x.ApplicantGuid == ApplicantYear2Finalized1.Person.Guid);

            Assert.AreEqual(ApplicantYear2Finalized1.AcademicInformation.Gpa, person.Gpa);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year2_Applicant1_Should_Right_Income()
        {
            var person =
                ResultFromYear2.ApplicantSummaries.First(
                    x => x.ApplicantGuid == ApplicantYear2Finalized1.Person.Guid);

            Assert.AreEqual(ApplicantYear2Finalized1.FamilyInformation.YearlyHouseholdIncomeRange, person.IncomeRange);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetListOfFinalizedApplicantsRepository_Year2_Applicant1_Should_Right_YourRating_Null()
        {
            var person =
                ResultFromYear2.ApplicantSummaries.First(
                    x => x.ApplicantGuid == ApplicantYear2Finalized1.Person.Guid);

            Assert.IsNull(person.YourRating);
        }

        #endregion

        #region Utilities

        private static DatabaseRootContext GetRootContext()
        {
            return new DatabaseRootContext(TestHelpersCommonFields.DatabaseName);
        }

        private static Applicant CreateApplicant(GraduatingClass graduatingClass, bool finalized, bool notFinished = false, string lastNameStartsWith = "z", double gpa = 2.2, IncomeRangeEnum incomeRange = IncomeRangeEnum.Mt90000Lt100000)
        {
            var newGuid = Guid.NewGuid();
            var now = DateTime.UtcNow;

            return new Applicant
            {
                ApplicantPersonalInformation = new ApplicantPersonalInformation
                {
                    GraduatingClass = graduatingClass,
                    Birthdate = now,
                    LastUpdated = now
                },
                Person = new Person
                {
                    DateCreated = now,
                    Guid = newGuid,
                    Name = new Name
                    {
                        LastUpdated = now,
                        FirstName = newGuid.ToString().Substring(0,4),
                        LastName = lastNameStartsWith + newGuid.ToString().Substring(5,6)
                    }
                },
                Metadata = new ApplicantMetadata
                {
                    ApplicationFinalized = finalized,
                    NotCompletedButFinalizedApplication = notFinished
                },
                AcademicInformation = new AcademicInformation
                {
                    LastUpdated = now,
                    Gpa = gpa
                },
                FamilyInformation = new FamilyInformation
                {
                    LastUpdated = now,
                    YearlyHouseholdIncomeRange = incomeRange
                }
            };
        }

        [ClassCleanup]
        public static void CleanDb()
        {

        }

        #endregion
    }
}
