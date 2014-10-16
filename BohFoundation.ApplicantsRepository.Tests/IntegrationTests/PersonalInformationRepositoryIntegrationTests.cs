using System;
using System.Linq;
using BohFoundation.ApplicantsRepository.DbContext;
using BohFoundation.ApplicantsRepository.Repositories.Implementations;
using BohFoundation.Domain.Dtos.Applicant.Notifications;
using BohFoundation.Domain.Dtos.Applicant.PersonalInformation;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.Enums;
using BohFoundation.PersonsRepository.Repositories.Implementations;
using BohFoundation.TestHelpers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.ApplicantsRepository.Tests.IntegrationTests
{
    [TestClass]
    public class PersonalInformationRepositoryIntegrationTests
    {
        private static PersonalInformationRepository _applicantsRepository;
        private static ApplicantsesNotificationRepository _applicantsesNotificationRepo;

        private static Name Name { get; set; }
        private static string FirstName { get { return TestHelpersCommonFields.FirstName; } }
        private static string LastName { get { return TestHelpersCommonFields.LastName; } }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestHelpersCommonFields.InitializeFields();

            CreateApplicantsRepo();

            AddPerson();
            GetPersonalInformationNoPersonalInformation();
            CreatePersonalInformation();
            UpdatePersonalInformation();
            GetPersonalInformationSomePersonalInformation();
        }

        private static void CreateApplicantsRepo()
        {
            TestHelpersCommonFakes.InitializeFakes();
            Guid = TestHelpersCommonFields.GuidOne;

            A.CallTo(() => TestHelpersCommonFakes.ClaimsInformationGetters.GetUsersGuid())
                .Returns(Guid);
            

            _applicantsRepository = new PersonalInformationRepository(TestHelpersCommonFields.DatabaseName, TestHelpersCommonFakes.ClaimsInformationGetters);
            _applicantsesNotificationRepo = new ApplicantsesNotificationRepository(TestHelpersCommonFields.DatabaseName, TestHelpersCommonFakes.ClaimsInformationGetters, TestHelpersCommonFakes.DeadlineUtilities);

        }

        private static Guid Guid { get; set; }

        #region AddPerson

        public static void AddPerson()
        {
            Name = new Name {FirstName = FirstName, LastName = LastName};

            var personsRepo = new CreatePersonRepository(TestHelpersCommonFields.DatabaseName);

            personsRepo.CreatePerson(Guid, Name, MemberTypesEnum.Applicant);

            using (var context = new ApplicantRepositoryDbContext(TestHelpersCommonFields.DatabaseName))
            {
                PersonFromServer = context.People.FirstOrDefault(x => x.Guid == Guid);
                if (PersonFromServer != null)
                {
                    NameFromServer = PersonFromServer.Name;
                }

                ApplicantFromServer =
                    context.Applicants.FirstOrDefault(x => x.Person.Guid == Guid);
            }
        }

        private static Applicant ApplicantFromServer { get; set; }
        private static Name NameFromServer { get; set; }
        private static Person PersonFromServer { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void PersonsRepository_CreatePerson_Should_CreateAPerson_Checks_NotNull()
        {
            Assert.IsNotNull(PersonFromServer);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void PersonsRepository_CreatePerson_Should_SetCreatedDate_Within_1Seconds()
        {
            TestHelpersTimeAsserts.RecentTime(PersonFromServer.DateCreated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void PersonsRepository_CreatePerson_Should_SetGuid_ToGuid_PassedIn()
        {
            Assert.AreEqual(Guid, PersonFromServer.Guid);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void PersonsRepository_CreatePerson_Should_Set_Id_ToAPositiveNumber()
        {
            Assert.IsTrue(PersonFromServer.Id > 0);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void PersonsRepository_CreatePerson_Should_Create_A_Person()
        {
            Assert.IsInstanceOfType(PersonFromServer, typeof (Person));
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void PersonsRepository_CreatePerson_Should_CreateAName_Checks_NotNull()
        {
            Assert.IsNotNull(NameFromServer);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void PersonsRepository_CreatePerson_Should_Create_A_Name()
        {
            Assert.IsInstanceOfType(NameFromServer, typeof (Name));
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void PersonsRepository_CreatePerson_Should_Have_Right_FirstName()
        {
            Assert.AreEqual(FirstName, NameFromServer.FirstName);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void PersonsRepository_CreatePerson_Should_Have_Right_LastName()
        {
            Assert.AreEqual(LastName, NameFromServer.LastName);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void PersonsRepository_CreatePerson_Should_Set_Name_Id_ToAPositiveNumber()
        {
            Assert.IsTrue(NameFromServer.Id > 0);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void PersonsRepository_CreatePerson_Should_Name_LastUpdated_Within_1Seconds()
        {
            TestHelpersTimeAsserts.RecentTime(NameFromServer.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void PersonsRepository_CreatePerson_Applicant_Should_Set_Id_ToAPositiveNumber()
        {
            Assert.IsTrue(ApplicantFromServer.Id > 0);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void PersonsRepository_CreatePerson_Should_Create_An_Applicant()
        {
            Assert.IsInstanceOfType(ApplicantFromServer, typeof (Applicant));
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void PersonsRepository_CreatePerson_Applicant_Should_Not_Be_Null()
        {
            Assert.IsNotNull(ApplicantFromServer);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void PersonsRepository_CreatePerson_Applicant_Id_Should_Be_Person_Id()
        {
            Assert.AreEqual(ApplicantFromServer.Id, PersonFromServer.Id);
        }

        #endregion

        #region FirstGetPersonalInformation

        private static void GetPersonalInformationNoPersonalInformation()
        {
            FirstGetPersonalInformation = _applicantsRepository.GetPersonalInformation();
            FirstNotificationDto = _applicantsesNotificationRepo.GetApplicantNotifications();
        }

        private static ApplicantNotificationsDto FirstNotificationDto { get; set; }
        private static ApplicantPersonalInformationDto FirstGetPersonalInformation { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_GetPersonalInformation_FirstTry_Should_Return_A_Null_Object()
        {
            Assert.IsNull(FirstGetPersonalInformation);
        }

        #endregion

        #region FirstCreatePersonInformation

        private static void CreatePersonalInformation()
        {
            BirthdayOne = new DateTime(1984, 7, 6, 12, 3, 4);
            BirthdayOneWithoutHourInformation = new DateTime(1984, 7, 6);
            GraduatingYear = 2009;
            var personalInformationDto = new ApplicantPersonalInformationDto
            {
                Birthdate = BirthdayOne,
                GraduatingYear = GraduatingYear
            };
            _applicantsRepository.UpsertPersonalInformation(personalInformationDto);

            using (var context = new ApplicantRepositoryDbContext(TestHelpersCommonFields.DatabaseName))
            {
                var firstOrDefault = context.People.FirstOrDefault(x => x.Guid == Guid);
                if (firstOrDefault != null)
                    FirstPersonalInformationFromServer =
                        firstOrDefault
                            .Applicant.ApplicantPersonalInformation;

                var loadOtherObject = FirstPersonalInformationFromServer.GraduatingClass;
            }
        }
        

        private static ApplicantPersonalInformation FirstPersonalInformationFromServer { get; set; }
        private static int GraduatingYear { get; set; }
        private static DateTime BirthdayOne { get; set; }
        private static DateTime BirthdayOneWithoutHourInformation { get; set; }
        
        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_UpsertPersonalInformation_FirstInsert_Should_Add_The_Birthdate_WithoutHours_Info()
        {
            Assert.AreEqual(BirthdayOneWithoutHourInformation, FirstPersonalInformationFromServer.Birthdate);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_UpsertPersonalInformation_FirstInsert_Should_Add_The_GraduatingYear()
        {
            Assert.AreEqual(GraduatingYear, FirstPersonalInformationFromServer.GraduatingClass.GraduatingYear);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_UpsertPersonalInformation_FirstInsert_Should_Have_A_Non_Null_Item_From_Server()
        {
            Assert.IsNotNull(FirstPersonalInformationFromServer);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_UpsertPersonalInformation_FirstInsert_Should_Set_LastUpdated()
        {
            TestHelpersTimeAsserts.RecentTime(FirstPersonalInformationFromServer.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_UpsertPersonalInformation_FirstInsert_Should_Set_Id_To_Something_Positive()
        {
            Assert.IsTrue(FirstPersonalInformationFromServer.Id > 0);
        }

        #endregion

        #region UpdatePersonInformation

        private static void UpdatePersonalInformation()
        {
            BirthdayTwo = new DateTime(1974, 9, 19);

            var personalInformationDto = new ApplicantPersonalInformationDto
            {
                Birthdate = BirthdayTwo,
                GraduatingYear = GraduatingYear
            };

            _applicantsRepository.UpsertPersonalInformation(personalInformationDto);

            using (var context = new ApplicantRepositoryDbContext(TestHelpersCommonFields.DatabaseName))
            {
                var firstOrDefault = context.People.FirstOrDefault(x => x.Guid == Guid);
                if (firstOrDefault != null)
                    SecondPersonalInformationFromServer =
                        firstOrDefault
                            .Applicant.ApplicantPersonalInformation;

                var loadOtherObject = SecondPersonalInformationFromServer.GraduatingClass;
            }
        }

        private static ApplicantPersonalInformation SecondPersonalInformationFromServer { get; set; }
        private static DateTime BirthdayTwo { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_UpsertPersonalInformation_SecondInsert_Should_Add_The_Birthdate()
        {
            Assert.AreEqual(BirthdayTwo, SecondPersonalInformationFromServer.Birthdate);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_UpsertPersonalInformation_SecondInsert_Should_Add_The_GraduatingYear()
        {
            Assert.AreEqual(GraduatingYear, SecondPersonalInformationFromServer.GraduatingClass.GraduatingYear);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_UpsertPersonalInformation_SecondInsert_Should_Have_A_Non_Null_Item_From_Server()
        {
            Assert.IsNotNull(SecondPersonalInformationFromServer);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_UpsertPersonalInformation_SecondInsert_Should_Set_LastUpdated()
        {
            TestHelpersTimeAsserts.RecentTime(SecondPersonalInformationFromServer.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_UpsertPersonalInformation_SecondInsert_Should_Set_Id_To_Something_Positive()
        {
            Assert.IsTrue(SecondPersonalInformationFromServer.Id > 0);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_UpsertPersonalInformation_SecondInsert_Should_Have_A_Bigger_TimeStamp_Than_First
            ()
        {
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(SecondPersonalInformationFromServer.LastUpdated, FirstPersonalInformationFromServer.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_UpsertPersonalInformation_SecondInsert_Should_Have_Same_Ids()
        {
            Assert.AreEqual(SecondPersonalInformationFromServer.Id, FirstPersonalInformationFromServer.Id);
        }
    
    #endregion

        #region GetPersonalInformationTwo

        private static void GetPersonalInformationSomePersonalInformation()
        {
            SecondGetPersonalInformation = _applicantsRepository.GetPersonalInformation();
            SecondApplicantsNotificationRepoCall = _applicantsesNotificationRepo.GetApplicantNotifications();
        }

        private static ApplicantNotificationsDto SecondApplicantsNotificationRepoCall { get; set; }
        private static ApplicantPersonalInformationDto SecondGetPersonalInformation { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_GetPersonalInformation_SecondTry_Should_Return_A_ApplicantPersonalInformationDto
            ()
        {
            Assert.IsInstanceOfType(SecondGetPersonalInformation, typeof (ApplicantPersonalInformationDto));
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_GetPersonalInformation_SecondTry_Should_Have_TheSame_LastUpdated_AsUpdate()
        {
            Assert.AreEqual(SecondPersonalInformationFromServer.LastUpdated, SecondGetPersonalInformation.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_GetPersonalInformation_SecondTry_Should_Have_TheSame_Birthdate_AsSecond()
        {
            Assert.AreEqual(BirthdayTwo, SecondGetPersonalInformation.Birthdate);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantRepository_GetPersonalInformation_SecondTry_Should_Have_TheSame_GraduatingYear()
        {
            Assert.AreEqual(GraduatingYear, SecondGetPersonalInformation.GraduatingYear);
        }

        #endregion

        #region ApplicantsNotificationRepository

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepository_GetApplicantNotifications_FirstTry_Should_Return_A_Null_Object()
        {
            Assert.IsNull(FirstNotificationDto.LastUpdatedPersonalInformation);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepository_GetApplicantNotifications_SecondTry_Should_Return_TheSameDate_AsLastUpsert()
        {
            Assert.AreEqual(SecondGetPersonalInformation.LastUpdated, SecondApplicantsNotificationRepoCall.LastUpdatedPersonalInformation);
        }

        #endregion

    }
}
