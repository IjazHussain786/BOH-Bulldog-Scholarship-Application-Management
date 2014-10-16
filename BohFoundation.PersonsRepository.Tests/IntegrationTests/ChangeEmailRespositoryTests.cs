using System;
using System.Linq;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.EntityFrameworkBaseClass;
using BohFoundation.PersonsRepository.Repositories.Implementations;
using BohFoundation.TestHelpers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.PersonsRepository.Tests.IntegrationTests
{
    [TestClass]
    public class ChangeEmailRespositoryTests
    {
        private static ChangeEmailRepository _changeEmailRepository;

        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            TestHelpersCommonFields.InitializeFields();
            TestHelpersCommonFakes.InitializeFakes();

            _changeEmailRepository = new ChangeEmailRepository(TestHelpersCommonFields.DatabaseName);
            
            Setup();

            //In case of a person changing their email before they update their contact information, it should gracefully handle nulls.
            NoEmailInMyDb();
            
            AddNewApplicant();
            FirstCheck();
            ChangeEmail();
        }

        #region Setup

        private static void Setup()
        {
            EmailOne = TestHelpersCommonFields.EmailDynamic();
            EmailTwo = TestHelpersCommonFields.EmailDynamic();
            ApplicantGuid = Guid.NewGuid();
            FirstTime = DateTime.UtcNow;

            A.CallTo(() => TestHelpersCommonFakes.ClaimsInformationGetters.GetUsersGuid()).Returns(ApplicantGuid);
        }

        private static DateTime FirstTime { get; set; }
        private static Guid ApplicantGuid { get; set; }
        private static string EmailOne { get; set; }
        private static string EmailTwo { get; set; }

        private static void AddNewApplicant()
        {
            var applicant = new Applicant
            {
                Person = new Person
                {
                    DateCreated = DateTime.UtcNow,
                    Guid = ApplicantGuid,
                    ContactInformation = new ContactInformation
                    {
                        EmailAddress = EmailOne,
                        LastUpdated = FirstTime
                    }
                }
            };

            using (var context = GetRootContext())
            {
                context.Applicants.Add(applicant);
                context.SaveChanges();
            }
        }

        #endregion
        
        #region MakingSureSetupIsRight

        private static void NoEmailInMyDb()
        {
            _changeEmailRepository.ChangeEmailAddress(EmailTwo, ApplicantGuid);
            using (var context = GetRootContext())
            {
                Result0 = context.People.FirstOrDefault(person => person.Guid == ApplicantGuid);
            }
        }

        public static Person Result0 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void ChangeEmailRepository_ChangeEmailAddress_Should_Be_Null_Before_A_Person_Is_Added()
        {
            Assert.IsNull(Result0);
        }

        private static void FirstCheck()
        {
            using (var context = GetRootContext())
            {
                Result1 =
                    context.People.Where(person => person.Guid == ApplicantGuid)
                        .Select(x => new { x.ContactInformation.EmailAddress, x.ContactInformation.LastUpdated})
                        .First();
            }
        }

        public static dynamic Result1 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void ChangeEmailRepository_ChangeEmailAddress_Should_Contain_EmailOne_Before_Change()
        {
            Assert.AreEqual(EmailOne, Result1.EmailAddress);
        }

        [TestCategory("Integration"), TestMethod]
        public void ChangeEmailRepository_ChangeEmailAddress_Should_Contain_TimeOne_Before_Change()
        {
            TestHelpersTimeAsserts.RecentTime(Result1.LastUpdated);
        }

        [TestCategory("Integration"), TestMethod]
        public void ChangeEmailRepository_ChangeEmailAddress_Should_Email_One_And_Two_Should_Be_Different()
        {
            Assert.AreNotEqual(EmailOne, EmailTwo);
        }

        #endregion

        #region ChangeEmail

        private static void ChangeEmail()
        {
            _changeEmailRepository.ChangeEmailAddress(EmailTwo, ApplicantGuid);
            
            using (var context = GetRootContext())
            {
                Result2 =
                    context.People.Where(person => person.Guid == ApplicantGuid)
                        .Select(x => new { x.ContactInformation.EmailAddress, x.ContactInformation.LastUpdated })
                        .First();
            }
        }

        public static dynamic Result2 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void ChangeEmailRepository_ChangeEmailAddress_Should_Contain_EmailTwo_After_Change()
        {
            Assert.AreEqual(EmailTwo, Result2.EmailAddress);
        }

        [TestCategory("Integration"), TestMethod]
        public void ChangeEmailRepository_ChangeEmailAddress_Should_Contain_A_Time_Greated_Than_TimeOne_After_Change()
        {
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(Result2.LastUpdated, FirstTime);
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
