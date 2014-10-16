using System;
using System.Collections.Generic;
using System.Linq;
using BohFoundation.Domain.Dtos.Person;
using BohFoundation.Domain.Dtos.Reference.Anonymous;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.EntityFrameworkModels.References;
using BohFoundation.Domain.Enums;
using BohFoundation.EntityFrameworkBaseClass;
using BohFoundation.ReferencesRepository.Repositories.Implementations;
using BohFoundation.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.ReferencesRepository.Tests.IntegrationTests
{
    [TestClass]
    public class ReferencesRepositoryTests
    {
        private static AnonymousLetterOfRecommendationRepository _referencesRepositories;

        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            TestHelpersCommonFields.InitializeFields();

            CreateRepo();
            CreateLetterOfRecommendationAndApplicants();

            FirstGetInformationForReferenceFormApplicant1();
            UpsertReferencesPersonalInformation1();
            UpsertLetterOfRecommendationKeyValue1();
            UpsertLetterOfRecommendationKeyValue2();
            SecondGetInformationForReferenceFormApplicant1();
            FirstGetInformationForReferenceFormApplicant2();
            UpsertReferencesPersonalInformation2UsingSecondLetterOfRec();
        }

        #region Setup

        private static void CreateRepo()
        {
            _referencesRepositories = new AnonymousLetterOfRecommendationRepository(TestHelpersCommonFields.DatabaseName);
        }

        private static void CreateLetterOfRecommendationAndApplicants()
        {
            GuidKeyToLetterOfRecommendation1 = Guid.NewGuid();
            EmailOfReference = TestHelpersCommonFields.EmailDynamic();
            FirstNameOfApplicant1 = "FirstNameOfApplicant";
            LastNameOfApplicant1 = "LastNameOfApplicant";
            ApplicantRelationshipToReference1 = "Relationship2";
            GuidKeyToLetterOfRecommendation2 = Guid.NewGuid();
            FirstNameOfApplicant2 = "FistName";
            LastNameOfApplicant2 = "LastName";
            ApplicantRelationshipToReference2 = "Relationship1";

            var reference = new Reference
            {
                Person = new Person
                {
                    ContactInformation =
                        new ContactInformation {EmailAddress = EmailOfReference, LastUpdated = DateTime.UtcNow},
                    Guid = Guid.NewGuid(),
                    DateCreated = DateTime.UtcNow
                }
            };

            var applicant1 = new Person
            {
                Guid = Guid.NewGuid(),
                DateCreated = DateTime.UtcNow,
                Name = new Name
                {
                    FirstName = FirstNameOfApplicant1,
                    LastName = LastNameOfApplicant1,
                    LastUpdated = DateTime.UtcNow
                },
                Applicant = new Applicant
                {
                    LettersOfRecommendation = new List<LetterOfRecommendation>
                    {
                        new LetterOfRecommendation
                        {
                            GuidSentToReference = GuidKeyToLetterOfRecommendation1,
                            Reference = reference,
                            ReferenceRelationshipToApplicant = ApplicantRelationshipToReference1
                        }
                    }
                }
            };

            var applicant2 = new Person
            {
                Guid = Guid.NewGuid(),
                DateCreated = DateTime.UtcNow,
                Name = new Name
                {
                    FirstName = FirstNameOfApplicant2,
                    LastName = LastNameOfApplicant2,
                    LastUpdated = DateTime.UtcNow
                },
                Applicant = new Applicant
                {
                    LettersOfRecommendation = new List<LetterOfRecommendation>
                    {
                        new LetterOfRecommendation
                        {
                            GuidSentToReference = GuidKeyToLetterOfRecommendation2,
                            Reference = reference,
                            ReferenceRelationshipToApplicant = ApplicantRelationshipToReference2
                        }
                    }
                }
            };

            using (var context = GetRootContext())
            {
                context.People.Add(applicant1);
                context.People.Add(applicant2);
                context.SaveChanges();
            }
        }

        private static string ApplicantRelationshipToReference2 { get; set; }
        private static string LastNameOfApplicant2 { get; set; }
        private static string FirstNameOfApplicant2 { get; set; }
        private static Guid GuidKeyToLetterOfRecommendation2 { get; set; }
        private static string ApplicantRelationshipToReference1 { get; set; }
        private static string LastNameOfApplicant1 { get; set; }
        private static Guid GuidKeyToLetterOfRecommendation1 { get; set; }
        private static string EmailOfReference { get; set; }
        private static string FirstNameOfApplicant1 { get; set; }

        #endregion

        #region FirstGetInformationForReferenceFormApplicant1

        private static void FirstGetInformationForReferenceFormApplicant1()
        {
            ResultOfFirstGetForApplicant1 =
                _referencesRepositories.GetInformationForReferenceForm(new GuidForLetterOfRecommendationDto
                {
                    GuidSentToReference = GuidKeyToLetterOfRecommendation1
                });
        }

        private static InformationForReferenceFormDto ResultOfFirstGetForApplicant1 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_FirstGetInformationForReferenceFormApplicant1_Should_Return_Applicants_FirstName()
        {
            Assert.AreEqual(FirstNameOfApplicant1, ResultOfFirstGetForApplicant1.ApplicantsName.FirstName);
        }

        [TestCategory("Integration"), TestMethod]
        public void ReferencesRepository_FirstGetInformationForReferenceFormApplicant1_Should_Return_Applicants_LastName
            ()
        {
            Assert.AreEqual(LastNameOfApplicant1, ResultOfFirstGetForApplicant1.ApplicantsName.LastName);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_FirstGetInformationForReferenceFormApplicant1_Should_Return_Applicants_Relationship()
        {
            Assert.AreEqual(ApplicantRelationshipToReference1, ResultOfFirstGetForApplicant1.ApplicantsRelationshipToYou);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_FirstGetInformationForReferenceFormApplicant1_Should_Return_Null_References_FirstName()
        {
            Assert.IsNull(ResultOfFirstGetForApplicant1.ReferencesName.FirstName);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_FirstGetInformationForReferenceFormApplicant1_Should_Return_Null_References_LastName()
        {
            Assert.IsNull(ResultOfFirstGetForApplicant1.ReferencesName.LastName);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_FirstGetInformationForReferenceFormApplicant1_Should_Return_Null_LastUpdatedPhoneNumber
            ()
        {
            Assert.IsNull(ResultOfFirstGetForApplicant1.PhoneNumberLastUpdated);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_FirstGetInformationForReferenceFormApplicant1_Should_Return_False_LetterOfRecommendationAlreadyRecieved
            ()
        {
            Assert.IsFalse(ResultOfFirstGetForApplicant1.LetterOfRecommendationAlreadyRecieved);
        }

        #endregion

        #region UpsertReferencesPersonalInformation1

        private static void UpsertReferencesPersonalInformation1()
        {
            ReferenceFirstName = "ReferenceFirstName";
            ReferenceLastName = "ReferenceLastName";
            ReferenceOccupation = "Occupation";
            ReferencePhoneNumber = 5735551235;
            ReferenceBestTimeOfDay = TimeOfDayEnum.Evening;

            var dto = new ReferencePersonalInformationDto
            {
                Name = new NameDto
                {
                    FirstName = ReferenceFirstName,
                    LastName = ReferenceLastName
                },
                Occupation = ReferenceOccupation,
                PhoneInformationDto = new PhoneInformationDto
                {
                    BestTimeToContactByPhone = ReferenceBestTimeOfDay,
                    PhoneNumber = ReferencePhoneNumber
                },
                GuidSentToReference = GuidKeyToLetterOfRecommendation1
            };

            _referencesRepositories.UpsertReferencesPersonalInformation(dto);

            using (var context = GetRootContext())
            {
                ResultOfFirstPersonalInformationUpsert = context.LetterOfRecommendations.Where(
                    letter => letter.GuidSentToReference == GuidKeyToLetterOfRecommendation1)
                    .Select(letter => new
                    {
                        letter.Reference.Person.Name,
                        letter.Reference.Person.ContactInformation.PhoneInformation,
                        letter.Reference
                    }).First();
            }
        }

        private static dynamic ResultOfFirstPersonalInformationUpsert { get; set; }
        private static TimeOfDayEnum ReferenceBestTimeOfDay { get; set; }
        private static long ReferencePhoneNumber { get; set; }
        private static string ReferenceOccupation { get; set; }
        private static string ReferenceLastName { get; set; }
        private static string ReferenceFirstName { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void ReferencesRepository_UpsertReferencesPersonalInformation1_Name_Should_Have_Positive_Id()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(ResultOfFirstPersonalInformationUpsert.Name.Id);
        }

        [TestCategory("Integration"), TestMethod]
        public void ReferencesRepository_UpsertReferencesPersonalInformation1_Name_Should_Have_Recent_LastUpdated()
        {
            TestHelpersTimeAsserts.RecentTime(ResultOfFirstPersonalInformationUpsert.Name.LastUpdated);
        }

        [TestCategory("Integration"), TestMethod]
        public void ReferencesRepository_UpsertReferencesPersonalInformation1_Name_Should_Have_Right_FirstName()
        {
            Assert.AreEqual(ReferenceFirstName, ResultOfFirstPersonalInformationUpsert.Name.FirstName);
        }

        [TestCategory("Integration"), TestMethod]
        public void ReferencesRepository_UpsertReferencesPersonalInformation1_Name_Should_Have_Right_LastName()
        {
            Assert.AreEqual(ReferenceLastName, ResultOfFirstPersonalInformationUpsert.Name.LastName);
        }

        [TestCategory("Integration"), TestMethod]
        public void ReferencesRepository_UpsertReferencesPersonalInformation1_PhoneInformation_Should_Have_Positive_Id()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(ResultOfFirstPersonalInformationUpsert.PhoneInformation.Id);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_UpsertReferencesPersonalInformation1_PhoneInformation_Should_Have_Recent_LastUpdated()
        {
            TestHelpersTimeAsserts.RecentTime(ResultOfFirstPersonalInformationUpsert.PhoneInformation.LastUpdated);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_UpsertReferencesPersonalInformation1_PhoneInformation_Should_Have_Correct_PhoneNumber()
        {
            Assert.AreEqual(ReferencePhoneNumber, ResultOfFirstPersonalInformationUpsert.PhoneInformation.PhoneNumber);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_UpsertReferencesPersonalInformation1_PhoneInformation_Should_Have_Correct_BestTimeToContactByPhone
            ()
        {
            Assert.AreEqual(ReferenceBestTimeOfDay,
                ResultOfFirstPersonalInformationUpsert.PhoneInformation.BestTimeToContactByPhone);
        }

        [TestCategory("Integration"), TestMethod]
        public void ReferencesRepository_UpsertReferencesPersonalInformation1_Reference_Should_Have_Positive_Id()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(ResultOfFirstPersonalInformationUpsert.Reference.Id);
        }

        [TestCategory("Integration"), TestMethod]
        public void ReferencesRepository_UpsertReferencesPersonalInformation1_Reference_Should_Have_Correct_Occupation()
        {
            Assert.AreEqual(ReferenceOccupation, ResultOfFirstPersonalInformationUpsert.Reference.Occupation);
        }

        [TestCategory("Integration"), TestMethod]
        public void ReferencesRepository_UpsertReferencesPersonalInformation1_Reference_Should_Null_Key()
        {
            Assert.IsFalse(ResultOfFirstPersonalInformationUpsert.Reference.HasMembershipRebootAccount);
        }

        #endregion

        #region UpsertLetterOfRecommendationKeyValue1

        private static void UpsertLetterOfRecommendationKeyValue1()
        {
            KeyPostfix = "_" + DateTime.Now.Year;
            RowKeyValueOfReference1 = GuidKeyToLetterOfRecommendation1 + KeyPostfix;
            PartitionKeyValueOfReference1 = "reference" + KeyPostfix;

            var dto = new LetterOfRecommendationKeyValueForEntityFrameworkAndAzureDto
            {
                RowKey = RowKeyValueOfReference1,
                LetterOfRecommendationGuid = GuidKeyToLetterOfRecommendation1,
                PartitionKey = PartitionKeyValueOfReference1
            };

            _referencesRepositories.UpsertLetterOfRecommendationKeyValues(dto);

            using (var context = GetRootContext())
            {
                ResultOfUpsertKeyValue1 =
                    context.LetterOfRecommendations.First(
                        letter => letter.GuidSentToReference == dto.LetterOfRecommendationGuid);
            }
        }

        private static string PartitionKeyValueOfReference1 { get; set; }
        private static LetterOfRecommendation ResultOfUpsertKeyValue1 { get; set; }
        private static string RowKeyValueOfReference1 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void ReferencesRepository_UpsertLetterOfRecommendationKeyValue1_Should_Have_Positive_Id()
        {
            TestHelpersCommonAsserts.IsGreaterThanZero(ResultOfUpsertKeyValue1.Id);
        }

        [TestCategory("Integration"), TestMethod]
        public void ReferencesRepository_UpsertLetterOfRecommendationKeyValue1_Should_Have_Recent_LastUpdated()
        {
            TestHelpersTimeAsserts.RecentTime(ResultOfUpsertKeyValue1.LastUpdated.Value);
        }

        [TestCategory("Integration"), TestMethod]
        public void ReferencesRepository_UpsertLetterOfRecommendationKeyValue1_Should_Have_Correct_RowKeyValue()
        {
            Assert.AreEqual(RowKeyValueOfReference1, ResultOfUpsertKeyValue1.LetterOfRecommendationRowKey);
        }

        [TestCategory("Integration"), TestMethod]
        public void ReferencesRepository_UpsertLetterOfRecommendationKeyValue1_Should_Have_Correct_PartitionKeyValue()
        {
            Assert.AreEqual(PartitionKeyValueOfReference1, ResultOfUpsertKeyValue1.LetterOfRecommendationPartitionKey);
        }

        #endregion
        
        #region UpsertLetterOfRecommendationKeyValue2

        private static void UpsertLetterOfRecommendationKeyValue2()
        {
            KeyValueOfReference2 = "asdjkfjknlasdfj";
            PartitionKeyValueOfReference2 = "asdkjjksadf";

            var dto = new LetterOfRecommendationKeyValueForEntityFrameworkAndAzureDto
            {
                RowKey = KeyValueOfReference2,
                LetterOfRecommendationGuid = GuidKeyToLetterOfRecommendation1,
                PartitionKey = PartitionKeyValueOfReference2
            };

            _referencesRepositories.UpsertLetterOfRecommendationKeyValues(dto);

            using (var context = GetRootContext())
            {
                ResultOfUpsertKeyValue2 =
                    context.LetterOfRecommendations.First(
                        letter => letter.GuidSentToReference == dto.LetterOfRecommendationGuid);
            }
        }

        private static string PartitionKeyValueOfReference2 { get; set; }
        private static LetterOfRecommendation ResultOfUpsertKeyValue2 { get; set; }
        private static string KeyValueOfReference2 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void ReferencesRepository_UpsertLetterOfRecommendationKeyValue2_Should_Have_Same_Id_As_First_Inserts()
        {
            Assert.AreEqual(ResultOfUpsertKeyValue1.Id, ResultOfUpsertKeyValue2.Id);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_UpsertLetterOfRecommendationKeyValue2_Should_Have_Larger_LastUpdated_Than_First_Upsert()
        {
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(ResultOfUpsertKeyValue2.LastUpdated.Value,
                ResultOfUpsertKeyValue1.LastUpdated.Value);
        }

        [TestCategory("Integration"), TestMethod]
        public void ReferencesRepository_UpsertLetterOfRecommendationKeyValue2_Should_Have_Correct_RowKeyValue()
        {
            Assert.AreEqual(KeyValueOfReference2, ResultOfUpsertKeyValue2.LetterOfRecommendationRowKey);
        }

        [TestCategory("Integration"), TestMethod]
        public void ReferencesRepository_UpsertLetterOfRecommendationKeyValue2_Should_Have_Correct_PartitionKeyValue()
        {
            Assert.AreEqual(PartitionKeyValueOfReference2, ResultOfUpsertKeyValue2.LetterOfRecommendationPartitionKey);
        }

        #endregion

        #region SecondGetInformationForReferenceFormApplicant1

        private static void SecondGetInformationForReferenceFormApplicant1()
        {
            ResultOfSecondGetForApplicant1 =
                _referencesRepositories.GetInformationForReferenceForm(new GuidForLetterOfRecommendationDto
                {
                    GuidSentToReference = GuidKeyToLetterOfRecommendation1
                });
        }

        private static InformationForReferenceFormDto ResultOfSecondGetForApplicant1 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_SecondGetInformationForReferenceFormApplicant1_Should_Return_Applicants_FirstName()
        {
            Assert.AreEqual(FirstNameOfApplicant1, ResultOfSecondGetForApplicant1.ApplicantsName.FirstName);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_SecondGetInformationForReferenceFormApplicant1_Should_Return_Applicants_LastName
            ()
        {
            Assert.AreEqual(LastNameOfApplicant1, ResultOfSecondGetForApplicant1.ApplicantsName.LastName);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_SecondGetInformationForReferenceFormApplicant1_Should_Return_Applicants_Relationship()
        {
            Assert.AreEqual(ApplicantRelationshipToReference1,
                ResultOfSecondGetForApplicant1.ApplicantsRelationshipToYou);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_SecondGetInformationForReferenceFormApplicant1_Should_Return_References_FirstName()
        {
            Assert.AreEqual(ReferenceFirstName, ResultOfSecondGetForApplicant1.ReferencesName.FirstName);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_SecondGetInformationForReferenceFormApplicant1_Should_Return_References_LastName()
        {
            Assert.AreEqual(ReferenceLastName, ResultOfSecondGetForApplicant1.ReferencesName.LastName);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_SecondGetInformationForReferenceFormApplicant1_Should_Return_LastUpdatedPhoneNumber_EqualToOther_FirstUpsert
            ()
        {
            Assert.AreEqual(ResultOfFirstPersonalInformationUpsert.PhoneInformation.LastUpdated,
                ResultOfSecondGetForApplicant1.PhoneNumberLastUpdated);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_SecondGetInformationForReferenceFormApplicant1_Should_Return_True_LetterOfRecommendationAlreadyRecieved
            ()
        {
            Assert.IsTrue(ResultOfSecondGetForApplicant1.LetterOfRecommendationAlreadyRecieved);
        }

        #endregion

        #region FirstGetInformationForReferenceFormApplicant2

        private static void FirstGetInformationForReferenceFormApplicant2()
        {
            ResultOfFirstGetForApplicant2 =
                _referencesRepositories.GetInformationForReferenceForm(new GuidForLetterOfRecommendationDto
                {
                    GuidSentToReference = GuidKeyToLetterOfRecommendation2
                });
        }

        private static InformationForReferenceFormDto ResultOfFirstGetForApplicant2 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_FirstGetInformationForReferenceFormApplicant2_Should_Return_Applicants_FirstName()
        {
            Assert.AreEqual(FirstNameOfApplicant2, ResultOfFirstGetForApplicant2.ApplicantsName.FirstName);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_FirstGetInformationForReferenceFormApplicant2_Should_Return_Applicants_LastName
            ()
        {
            Assert.AreEqual(LastNameOfApplicant2, ResultOfFirstGetForApplicant2.ApplicantsName.LastName);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_FirstGetInformationForReferenceFormApplicant2_Should_Return_Applicants_Relationship()
        {
            Assert.AreEqual(ApplicantRelationshipToReference2,
                ResultOfFirstGetForApplicant2.ApplicantsRelationshipToYou);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_FirstGetInformationForReferenceFormApplicant2_Should_Return_References_FirstName()
        {
            Assert.AreEqual(ReferenceFirstName, ResultOfFirstGetForApplicant2.ReferencesName.FirstName);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_FirstGetInformationForReferenceFormApplicant2_Should_Return_References_LastName()
        {
            Assert.AreEqual(ReferenceLastName, ResultOfFirstGetForApplicant2.ReferencesName.LastName);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_FirstGetInformationForReferenceFormApplicant2_Should_Return_LastUpdatedPhoneNumber_EqualToOther_FirstUpsert
            ()
        {
            Assert.AreEqual(ResultOfFirstPersonalInformationUpsert.PhoneInformation.LastUpdated,
                ResultOfFirstGetForApplicant2.PhoneNumberLastUpdated);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_FirstGetInformationForReferenceFormApplicant2_Should_Return_False_LetterOfRecommendationAlreadyRecieved
            ()
        {
            Assert.IsFalse(ResultOfFirstGetForApplicant2.LetterOfRecommendationAlreadyRecieved);
        }

        #endregion

        #region UpsertReferencesPersonalInformation2UsingSecondLetterOfRec

        private static void UpsertReferencesPersonalInformation2UsingSecondLetterOfRec()
        {
            ReferenceOccupation2 = "jknjnkasdjnklsfdjsjnkdnjk";
            ReferenceFirstName2 = "Fiasjkjkl";
            ReferenceLastName2 = "Lkjknas219090";
            ReferenceBestTimeOfDay2 = TimeOfDayEnum.Anytime;
            ReferencePhoneNumber2 = 9999999999;

            _referencesRepositories.UpsertReferencesPersonalInformation(new ReferencePersonalInformationDto
            {
                GuidSentToReference = GuidKeyToLetterOfRecommendation2,
                Name = new NameDto
                {
                    FirstName = ReferenceFirstName2,
                    LastName = ReferenceLastName2
                },
                Occupation = ReferenceOccupation2,
                PhoneInformationDto = new PhoneInformationDto
                {
                    BestTimeToContactByPhone = ReferenceBestTimeOfDay2,
                    PhoneNumber = ReferencePhoneNumber2
                }
            });

            using (var context = GetRootContext())
            {
                ResultOfSecondUpsertReferencePersonalInformation = context.LetterOfRecommendations.Where(
                    letter => letter.GuidSentToReference == GuidKeyToLetterOfRecommendation1)
                    .Select(letter => new
                    {
                        letter.Reference.Person.Name,
                        letter.Reference.Person.ContactInformation.PhoneInformation,
                        letter.Reference
                    }).First();
            }
        }

        private static dynamic ResultOfSecondUpsertReferencePersonalInformation { get; set; }
        private static long ReferencePhoneNumber2 { get; set; }
        private static TimeOfDayEnum ReferenceBestTimeOfDay2 { get; set; }
        private static string ReferenceLastName2 { get; set; }
        private static string ReferenceFirstName2 { get; set; }
        private static string ReferenceOccupation2 { get; set; }
        private static string KeyPostfix { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_UpsertReferencesPersonalInformation2UsingSecondLetterOfRec_Name_Should_Have_Positive_Same_Id
            ()
        {
            Assert.AreEqual(ResultOfFirstPersonalInformationUpsert.Name.Id,
                ResultOfSecondUpsertReferencePersonalInformation.Name.Id);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_UpsertReferencesPersonalInformation2UsingSecondLetterOfRec_Name_Should_Have_More_Recent_LastUpdated
            ()
        {
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(
                ResultOfSecondUpsertReferencePersonalInformation.Name.LastUpdated,
                ResultOfFirstPersonalInformationUpsert.Name.LastUpdated);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_UpsertReferencesPersonalInformation2UsingSecondLetterOfRec_Name_Should_Have_Right_FirstName
            ()
        {
            Assert.AreEqual(ReferenceFirstName2, ResultOfSecondUpsertReferencePersonalInformation.Name.FirstName);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_UpsertReferencesPersonalInformation2UsingSecondLetterOfRec_Name_Should_Have_Right_LastName
            ()
        {
            Assert.AreEqual(ReferenceLastName2, ResultOfSecondUpsertReferencePersonalInformation.Name.LastName);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_UpsertReferencesPersonalInformation2UsingSecondLetterOfRec_PhoneInformation_Should_Have_Same_Id
            ()
        {
            Assert.AreEqual(ResultOfFirstPersonalInformationUpsert.PhoneInformation.Id,
                ResultOfSecondUpsertReferencePersonalInformation.PhoneInformation.Id);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_UpsertReferencesPersonalInformation2UsingSecondLetterOfRec_PhoneInformation_Should_Have_More_Recent_LastUpdated
            ()
        {
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(
                ResultOfSecondUpsertReferencePersonalInformation.PhoneInformation.LastUpdated,
                ResultOfFirstPersonalInformationUpsert.PhoneInformation.LastUpdated);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_UpsertReferencesPersonalInformation2UsingSecondLetterOfRec_PhoneInformation_Should_Have_Correct_PhoneNumber
            ()
        {
            Assert.AreEqual(ReferencePhoneNumber2,
                ResultOfSecondUpsertReferencePersonalInformation.PhoneInformation.PhoneNumber);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_UpsertReferencesPersonalInformation2UsingSecondLetterOfRec_PhoneInformation_Should_Have_Correct_BestTimeToContactByPhone
            ()
        {
            Assert.AreEqual(ReferenceBestTimeOfDay2,
                ResultOfSecondUpsertReferencePersonalInformation.PhoneInformation.BestTimeToContactByPhone);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_UpsertReferencesPersonalInformation2UsingSecondLetterOfRec_Reference_Should_Have_Same_Id
            ()
        {
            Assert.AreEqual(ResultOfFirstPersonalInformationUpsert.Reference.Id,
                ResultOfSecondUpsertReferencePersonalInformation.Reference.Id);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_UpsertReferencesPersonalInformation2UsingSecondLetterOfRec_Reference_Should_Have_Correct_Occupation
            ()
        {
            Assert.AreEqual(ReferenceOccupation2, ResultOfSecondUpsertReferencePersonalInformation.Reference.Occupation);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            ReferencesRepository_UpsertReferencesPersonalInformation2UsingSecondLetterOfRec_Reference_Should_Null_Key()
        {
            Assert.IsFalse(ResultOfSecondUpsertReferencePersonalInformation.Reference.HasMembershipRebootAccount);
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
