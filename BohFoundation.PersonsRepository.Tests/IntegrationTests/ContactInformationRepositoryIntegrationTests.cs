using System.Linq;
using BohFoundation.ApplicantsRepository.Repositories.Implementations;
using BohFoundation.Domain.Dtos.Applicant.Notifications;
using BohFoundation.Domain.Dtos.Person;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.Enums;
using BohFoundation.PersonsRepository.DbContext;
using BohFoundation.PersonsRepository.Repositories.Implementations;
using BohFoundation.TestHelpers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.PersonsRepository.Tests.IntegrationTests
{
    [TestClass]
    public class ContactInformationRepositoryIntegrationTests
    {
        private static ContactInformationRepository _contactInformationRepo;
        private static ApplicantsesNotificationRepository _applicantsesNotificationRepo;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestHelpersCommonFields.InitializeFields();
            TestHelpersCommonFakes.InitializeFakes();

            CreateRepos();
            CreatePerson();
            
            FirstGetContactInformation();
            CreateContactInformation();
            UpdateContactInformation();
            SecondGetContactInformation();
        }

        #region Setup

        private static void CreateRepos()
        {
            A.CallTo(() => TestHelpersCommonFakes.ClaimsInformationGetters.GetUsersGuid())
                .Returns(TestHelpersCommonFields.GuidOne);

            _contactInformationRepo = new ContactInformationRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters);
            _applicantsesNotificationRepo = new ApplicantsesNotificationRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters, TestHelpersCommonFakes.DeadlineUtilities);
        }

        private static void CreatePerson()
        {
            var name = new Name
            {
                FirstName = TestHelpersCommonFields.FirstName,
                LastName = TestHelpersCommonFields.LastName
            };
            var personsRepo = new CreatePersonRepository(TestHelpersCommonFields.DatabaseName);

            personsRepo.CreatePerson(TestHelpersCommonFields.GuidOne, name, MemberTypesEnum.Applicant);
        }

        #endregion

        #region FirstGetRequest

        private static void FirstGetContactInformation()
        {
            FirstGetContactInformationDto = _contactInformationRepo.GetContactInformation();
            FirstNotificationDto = _applicantsesNotificationRepo.GetApplicantNotifications();
        }

        private static ApplicantNotificationsDto FirstNotificationDto { get; set; }
        private static ContactInformationDto FirstGetContactInformationDto { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_Get_First_ShouldReturnNull()
        {
            Assert.IsNull(FirstGetContactInformationDto);
        }

        #endregion

        #region FirstUpsert

        private const string StreetAddressOneOne = "streetaddressone";
        private const string CityOne = "columbia";
        private const string StateOne = "MO";
        private const int ZipCodeOne = 65203;

        private const long PhoneNumberOne = 5739995202;
        private const TimeOfDayEnum BestTimeOfDayOne = TimeOfDayEnum.Evening;
        private static string _email;

        private static void CreateContactInformation()
        {
            _email = TestHelpersCommonFields.EmailDynamic();

            var addressOne = new AddressDto
            {
                City = CityOne,
                State = StateOne,
                StreetAddress1 = StreetAddressOneOne,
                ZipCode = ZipCodeOne
            };

            var contactInfoOne = new ContactInformationDto
            {
                Address = addressOne,
                EmailAddress = _email,
                PhoneInformation = new PhoneInformationDto
                {
                    PhoneNumber = PhoneNumberOne,
                    BestTimeToContactByPhone = BestTimeOfDayOne
                }
            };

            _contactInformationRepo.UpsertContactInformation(contactInfoOne);

            using (var context = new PersonsRepositoryDbContext(TestHelpersCommonFields.DatabaseName))
            {
                var result = context.People.First(person => person.Guid == TestHelpersCommonFields.GuidOne);
                FirstContactInformationUpsert = result.ContactInformation;
                //FORCES ADDRESS TO POPULATE
                var address = FirstContactInformationUpsert.Address;
                var phone = FirstContactInformationUpsert.PhoneInformation;
            }
        }

        private static ContactInformation FirstContactInformationUpsert { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_FirstInsert_Should_Add_City()
        {
            Assert.AreEqual(CityOne, FirstContactInformationUpsert.Address.City);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_FirstInsert_Should_Add_State()
        {
            Assert.AreEqual(StateOne, FirstContactInformationUpsert.Address.State);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_FirstInsert_Should_Add_Zipcode()
        {
            Assert.AreEqual(ZipCodeOne, FirstContactInformationUpsert.Address.ZipCode);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_FirstInsert_Should_Add_StreetAddress()
        {
            Assert.AreEqual(StreetAddressOneOne, FirstContactInformationUpsert.Address.StreetAddress1);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_FirstInsert_Should_Add_StreetAddressTwo()
        {
            Assert.IsNull(FirstContactInformationUpsert.Address.StreetAddress2);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_FirstInsert_Should_Add_Email()
        {
            Assert.AreEqual(_email, FirstContactInformationUpsert.EmailAddress);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_FirstInsert_Should_Add_PhoneNumber()
        {
            Assert.AreEqual(PhoneNumberOne, FirstContactInformationUpsert.PhoneInformation.PhoneNumber);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_FirstInsert_Should_Add_BestTimeOfDay()
        {
            Assert.AreEqual(BestTimeOfDayOne, FirstContactInformationUpsert.PhoneInformation.BestTimeToContactByPhone);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_FirstInsert_Should_Add_PhoneInformation_LastUpdated()
        {
            Assert.AreEqual(FirstContactInformationUpsert.LastUpdated, FirstContactInformationUpsert.PhoneInformation.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_FirstInsert_Should_Add_Id_ContactInfo()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(FirstContactInformationUpsert.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void
            ContactInformationRepository_UpsertContactInformation_FirstInsert_Should_Add_LastUpdatedTime_ContactInfor()
        {
            TestHelpersTimeAsserts.RecentTime(FirstContactInformationUpsert.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_FirstInsert_Should_Add_Id_Address()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(FirstContactInformationUpsert.Address.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_FirstInsert_Should_Add_LastUpdatedTime_Address
            ()
        {
            TestHelpersTimeAsserts.RecentTime(FirstContactInformationUpsert.Address.LastUpdated);
        }

        #endregion

        #region SecondUpsert

        private const string StreetAddressOneTwo = "streetaddressOnetwo";
        private const string StreetAddressTwoTwo = "streetaddressTwotwo";
        private const string CityTwo = "Little Falls";
        private const string StateTwo = "MN";
        private const int ZipCodeTwo = 65209;

        private const long PhoneNumberTwo = 5738524561;
        private const TimeOfDayEnum BestTimeOfDayTwo = TimeOfDayEnum.Noon;
        private static string _email2;

        private static void UpdateContactInformation()
        {
            _email2 = "2" + _email;

            var addressDto = new AddressDto
            {
                City = CityTwo,
                State = StateTwo,
                StreetAddress1 = StreetAddressOneTwo,
                StreetAddress2 = StreetAddressTwoTwo,
                ZipCode = ZipCodeTwo
            };

            var contactInfoTwo = new ContactInformationDto
            {
                EmailAddress = _email2,
                Address = addressDto,
                PhoneInformation = new PhoneInformationDto
                {
                    BestTimeToContactByPhone = BestTimeOfDayTwo,
                    PhoneNumber = PhoneNumberTwo
                }
            };

            _contactInformationRepo.UpsertContactInformation(contactInfoTwo);

            using (var context = new PersonsRepositoryDbContext(TestHelpersCommonFields.DatabaseName))
            {
                var result = context.People.First(person => person.Guid == TestHelpersCommonFields.GuidOne);
                SecondContactInformationUpsert = result.ContactInformation;
                //FORCES ADDRESS TO POPULATE
                var address = SecondContactInformationUpsert.Address;
                var phone = SecondContactInformationUpsert.PhoneInformation;
            }
        }

        private static ContactInformation SecondContactInformationUpsert { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_SecondInsert_Should_Add_City()
        {
            Assert.AreEqual(CityTwo, SecondContactInformationUpsert.Address.City);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_SecondInsert_Should_Add_State()
        {
            Assert.AreEqual(StateTwo, SecondContactInformationUpsert.Address.State);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_SecondInsert_Should_Add_Zipcode()
        {
            Assert.AreEqual(ZipCodeTwo, SecondContactInformationUpsert.Address.ZipCode);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_SecondInsert_Should_Add_StreetAddress()
        {
            Assert.AreEqual(StreetAddressOneTwo, SecondContactInformationUpsert.Address.StreetAddress1);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_SecondInsert_Should_Add_StreetAddressTwo()
        {
            Assert.AreEqual(StreetAddressTwoTwo, SecondContactInformationUpsert.Address.StreetAddress2);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_SecondInsert_Should_Add_Email()
        {
            Assert.AreEqual(_email2, SecondContactInformationUpsert.EmailAddress);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_SecondInsert_Should_Add_PhoneNumber()
        {
            Assert.AreEqual(PhoneNumberTwo, SecondContactInformationUpsert.PhoneInformation.PhoneNumber);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_SecondInsert_Should_Add_BestTime()
        {
            Assert.AreEqual(BestTimeOfDayTwo, SecondContactInformationUpsert.PhoneInformation.BestTimeToContactByPhone);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_SecondInsert_Should_Add_PhoneNumber_LastUpdated()
        {
            Assert.AreEqual(SecondContactInformationUpsert.LastUpdated, SecondContactInformationUpsert.PhoneInformation.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void
            ContactInformationRepository_UpsertContactInformation_SecondInsert_Should_Equal_Id_ContactInfo_FromFirst()
        {
            Assert.AreEqual(FirstContactInformationUpsert.Id, SecondContactInformationUpsert.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void
            ContactInformationRepository_UpsertContactInformation_FirstInsert_Should_BeGreaterThan_FirstLastUpdated_ContactInfo
            ()
        {
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(SecondContactInformationUpsert.LastUpdated,
                FirstContactInformationUpsert.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_UpsertContactInformation_FirstInsert_Should_Have_Same_Id_Address()
        {
            Assert.AreEqual(FirstContactInformationUpsert.Address.Id, SecondContactInformationUpsert.Address.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void
            ContactInformationRepository_UpsertContactInformation_FirstInsert_Should_BeGreaterThan_FirstLastUpdated_Address
            ()
        {
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(SecondContactInformationUpsert.Address.LastUpdated,
                FirstContactInformationUpsert.Address.LastUpdated);
        }

        #endregion

        #region SecondGet

        private static void SecondGetContactInformation()
        {
            SecondGetContactInformationDto = _contactInformationRepo.GetContactInformation();
            SecondApplicantsNotificationRepoCall = _applicantsesNotificationRepo.GetApplicantNotifications();
        }

        private static ApplicantNotificationsDto SecondApplicantsNotificationRepoCall { get; set; }
        private static ContactInformationDto SecondGetContactInformationDto { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_GetContactInformation_SecondInsert_Should_Have_City()
        {
            Assert.AreEqual(CityTwo, SecondGetContactInformationDto.Address.City);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_GetContactInformation_SecondInsert_Should_Have_State()
        {
            Assert.AreEqual(StateTwo, SecondGetContactInformationDto.Address.State);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_GetContactInformation_SecondInsert_Should_Have_Zipcode()
        {
            Assert.AreEqual(ZipCodeTwo, SecondGetContactInformationDto.Address.ZipCode);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_GetContactInformation_SecondInsert_Should_Have_StreetAddress()
        {
            Assert.AreEqual(StreetAddressOneTwo, SecondGetContactInformationDto.Address.StreetAddress1);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_GetContactInformation_SecondInsert_Should_Have_StreetAddressTwo()
        {
            Assert.AreEqual(StreetAddressTwoTwo, SecondGetContactInformationDto.Address.StreetAddress2);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_GetContactInformation_SecondInsert_Should_Have_Email()
        {
            Assert.AreEqual(_email2, SecondGetContactInformationDto.EmailAddress);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_GetContactInformation_SecondInsert_Should_Have_PhoneNumber()
        {
            Assert.AreEqual(PhoneNumberTwo, SecondGetContactInformationDto.PhoneInformation.PhoneNumber);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_GetContactInformation_SecondInsert_Should_Have_BestTimeToContact()
        {
            Assert.AreEqual(BestTimeOfDayTwo, SecondGetContactInformationDto.PhoneInformation.BestTimeToContactByPhone);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ContactInformationRepository_GetContactInformation_SecondInsert_Should_Have_LastUpdated_PhoneInformation()
        {
            Assert.AreEqual(SecondContactInformationUpsert.LastUpdated, SecondGetContactInformationDto.PhoneInformation.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void
            ContactInformationRepository_GetContactInformation_SecondInsert_Should_Have_Same_LastUpdatedFromUpsert_Contact
            ()
        {
            Assert.AreEqual(SecondContactInformationUpsert.LastUpdated, SecondGetContactInformationDto.LastUpdated);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void
            ContactInformationRepository_GetContactInformation_SecondInsert_Should_Have_Same_LastUpdatedFromUpsert_Address
            ()
        {
            Assert.AreEqual(SecondContactInformationUpsert.Address.LastUpdated,
                SecondGetContactInformationDto.Address.LastUpdated);
        }

        #endregion

        #region ApplicantsNotificationRepository

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepository_GetApplicantNotifications_FirstTry_Should_Return_A_Null_Object_ContactInformation()
        {
            Assert.IsNull(FirstNotificationDto.LastUpdatedContactInformation);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepository_GetApplicantNotifications_SecondTry_Should_Return_A_Null_Object_PersonalInformation()
        {
            Assert.IsNull(SecondApplicantsNotificationRepoCall.LastUpdatedPersonalInformation);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ApplicantsNotificationRepository_GetApplicantNotifications_SecondTry_Should_Return_TheSameDate_AsLastUpsert_ContactInformation()
        {
            Assert.AreEqual(SecondGetContactInformationDto.LastUpdated, SecondApplicantsNotificationRepoCall.LastUpdatedContactInformation);
        }


        #endregion

    }
}