using System;
using System.Linq;
using BohFoundation.AdminsRepository.DbContext;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.Enums;
using BohFoundation.PersonsRepository.Repositories.Implementations;
using BohFoundation.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.PersonsRepository.Tests.IntegrationTests
{
    [TestClass]
    public class CreatePersonRepositoryIntegrationTests
    {
        private static CreatePersonRepository _personRepository;

        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            SetupTests();
            InsertPendingApplicationEvaluator();
            InsertApplicant();
            InsertApplicationEvaluator();
            InsertAdmin();
        }
        
        private static void SetupTests()
        {
            TestHelpersCommonFields.InitializeFields();
            
            _personRepository = new CreatePersonRepository(TestHelpersCommonFields.DatabaseName);
        }
        
        #region PendingApplicationEvaluator

        private static void InsertPendingApplicationEvaluator()
        {
            GuidPendingApplicantEvalautor = Guid.NewGuid();

            const string pendingEvaluatorPrefix = "pendingEvaluator";
            FirstNamePendingEvaluator = CreateFirstName(pendingEvaluatorPrefix);
            LastNamePendingEvaluator = CreateLastName(pendingEvaluatorPrefix);

            _personRepository.CreatePerson(GuidPendingApplicantEvalautor,
                CreateName(FirstNamePendingEvaluator, LastNamePendingEvaluator), MemberTypesEnum.PendingApplicationEvaluator);

            ResultPendingEvaluator = GetPersonFromUpsert(GuidPendingApplicantEvalautor);
        }

        private static Person ResultPendingEvaluator { get; set; }
        private static Guid GuidPendingApplicantEvalautor { get; set; }
        private static string LastNamePendingEvaluator { get; set; }
        private static string FirstNamePendingEvaluator { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_PendingEvaluator_Person_Should_Equal_Null()
        {
            Assert.IsNull(ResultPendingEvaluator);
        }

        #endregion

        #region Applicant

        private static void InsertApplicant()
        {
            GuidApplicant = Guid.NewGuid();
            const string prefix = "applicant";
            FirstNameApplicant = CreateFirstName(prefix);
            LastNameApplicant = CreateLastName(prefix);

            _personRepository.CreatePerson(GuidApplicant, CreateName(FirstNameApplicant, LastNameApplicant),
                MemberTypesEnum.Applicant);

            ResultApplicant = GetPersonFromUpsert(GuidApplicant);
        }

        private static Person ResultApplicant { get; set; }
        private static string FirstNameApplicant { get; set; }
        private static string LastNameApplicant { get; set; }
        private static Guid GuidApplicant { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_Applicant_Person_Should_Have_Positive_Id()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(ResultApplicant.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_Applicant_Name_Should_Have_Correct_FirstName()
        {
            Assert.AreEqual(ResultApplicant.Name.FirstName, FirstNameApplicant);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_Applicant_Name_Should_Have_Correct_LastName()
        {
            Assert.AreEqual(ResultApplicant.Name.LastName, LastNameApplicant);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_Applicant_Applicant_Should_Have_PositiveId()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(ResultApplicant.Applicant.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_Applicant_ApplicantEvaluator_Should_BeNull()
        {
            Assert.IsNull(ResultApplicant.ApplicationEvaluator);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_Applicant_Admin_Should_BeNull()
        {
            Assert.IsNull(ResultApplicant.Admin);
        }

        #endregion

        #region AppEvaluator

        private static void InsertApplicationEvaluator()
        {
            GuidApplicationEvaluator = Guid.NewGuid();
            const string prefix = "applicantionEvalautor";
            FirstNameAppEvaluator = CreateFirstName(prefix);
            LastNameAppEvalautor = CreateLastName(prefix);

            _personRepository.CreatePerson(GuidApplicationEvaluator,
                CreateName(FirstNameAppEvaluator, LastNameAppEvalautor),
                MemberTypesEnum.ApplicationEvaluator);

            ResultAppEvalautor = GetPersonFromUpsert(GuidApplicationEvaluator);
        }

        private static Guid GuidApplicationEvaluator { get; set; }
        private static Person ResultAppEvalautor { get; set; }
        private static string FirstNameAppEvaluator { get; set; }
        private static string LastNameAppEvalautor { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_AppEvaluator_Person_Should_Have_Positive_Id()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(ResultAppEvalautor.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_AppEvaluator_Name_Should_Have_Correct_FirstName()
        {
            Assert.AreEqual(ResultAppEvalautor.Name.FirstName, FirstNameAppEvaluator);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_AppEvaluator_Name_Should_Have_Correct_LastName()
        {
            Assert.AreEqual(ResultAppEvalautor.Name.LastName, LastNameAppEvalautor);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_AppEvaluator_Applicant_Should_Be_Null()
        {
            Assert.IsNull(ResultAppEvalautor.Applicant);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_AppEvaluator_ApplicantEvaluator_Should_Have_Positive_Id()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(ResultAppEvalautor.ApplicationEvaluator.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_AppEvaluator_Admin_Should_BeNull()
        {
            Assert.IsNull(ResultApplicant.Admin);
        }

        #endregion

        private static void InsertAdmin()
        {
            GuidAdmin = Guid.NewGuid();
            const string prefix = "admin";
            FirstNameAdmin = CreateFirstName(prefix);
            LastNameAdmin = CreateLastName(prefix);

            _personRepository.CreatePerson(GuidAdmin, CreateName(FirstNameAdmin, LastNameAdmin),
                MemberTypesEnum.Admin);

            ResultAdmin = GetPersonFromUpsert(GuidAdmin);
        }

        private static Person ResultAdmin { get; set; }
        private static string LastNameAdmin { get; set; }
        private static string FirstNameAdmin { get; set; }
        private static Guid GuidAdmin { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_Admin_Person_Should_Have_Positive_Id()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(ResultAdmin.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_Admin_Name_Should_Have_Correct_FirstName()
        {
            Assert.AreEqual(ResultAdmin.Name.FirstName, FirstNameAdmin);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_Admin_Name_Should_Have_Correct_LastName()
        {
            Assert.AreEqual(ResultAdmin.Name.LastName, LastNameAdmin);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_Admin_Applicant_Should_Have_Positive_Id()
        {
            Assert.IsNull(ResultAdmin.Applicant);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_Admin_ApplicantEvaluator_Should_Have_Positive_Id()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(ResultAdmin.ApplicationEvaluator.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreatePersonRepository_Admin_Admin_Should_Have_Positive_Id()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(ResultAdmin.Admin.Id);
        }

        private static Person GetPersonFromUpsert(Guid idOfPerson)
        {
            Person person;
            using (var context = new AdminsRepositoryDbContext(TestHelpersCommonFields.DatabaseName))
            {
                person = context.People.FirstOrDefault(persons => persons.Guid == idOfPerson);
                if (person != null)
                {
                    var loadName = person.Name;
                    var loadApplicant = person.Applicant;
                    var loadAdmin = person.Admin;
                    var loadApplicationEvaluator = person.ApplicationEvaluator;
                }

            }
            return person;
        }

        private static string CreateFirstName(string prefix)
        {
            return prefix + TestHelpersCommonFields.FirstName;
        }

        private static string CreateLastName(string prefix)
        {
            return prefix + TestHelpersCommonFields.LastName;
        }

        private static Name CreateName(string first, string last)
        {
            return new Name {FirstName = first, LastName = last};
        }
    }
}
