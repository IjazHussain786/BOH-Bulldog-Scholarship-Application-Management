using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Implementations;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate;
using BohFoundation.Domain.Dtos.Common;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Academic;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Extracurricular;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Family;
using BohFoundation.Domain.EntityFrameworkModels.ApplicationEvaluators;
using BohFoundation.Domain.EntityFrameworkModels.Common;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.EntityFrameworkModels.References;
using BohFoundation.Domain.Enums;
using BohFoundation.EntityFrameworkBaseClass;
using BohFoundation.TestHelpers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.ApplicationEvaluatorsRepository.Tests.IntegrationTests
{
    [TestClass]
    public class GetApplicationAndRateApplicationIntegrationTests
    {
        private static DateTime _now;
        private static GetCompletedApplicationRepository _getRepo;
        private static RateApplicationRespository _upsertRepo;
        private static ConfirmTranscriptRepository _confirmTranscriptRepo;

        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            TestHelpersCommonFields.InitializeFields();
            TestHelpersCommonFakes.InitializeFakes();

            _now = DateTime.UtcNow;

            CreateApplicationEvaluatorApplicantAndReferences();
            InstantiateRepos();
            FirstGets();
            FirstInsertOfRatings();
            UpsertOfRatings();
        }
        
        #region Setup

        private static void CreateApplicationEvaluatorApplicantAndReferences()
        {
            ApplicationEvaluator = new ApplicationEvaluator
            {
                Person = new Person
                {
                    Guid = Guid.NewGuid(),
                    DateCreated = _now
                }
            };

            GraduatingClass = new GraduatingClass
            {
                GraduatingYear = new Random().Next()
            };

            #region EssayStuff

            var essayTopic1TitleStartsWithB = new EssayTopic
            {
                EssayPrompt = CreateRandomString(),
                RevisionDateTime = _now,
                TitleOfEssay = "B" + Guid.NewGuid(),
                ForWhatGraduatingYears = new Collection<GraduatingClass> {GraduatingClass}
            };

            var essayTopic2TitlesStartsWithZ = new EssayTopic
            {
                EssayPrompt = CreateRandomString(),
                RevisionDateTime = _now,
                TitleOfEssay = "Z" + Guid.NewGuid(),
                ForWhatGraduatingYears = new Collection<GraduatingClass> {GraduatingClass}
            };

            var essayTopic3TitlesStartsWithA = new EssayTopic
            {
                EssayPrompt = CreateRandomString(),
                RevisionDateTime = _now,
                TitleOfEssay = "A" + Guid.NewGuid(),
                ForWhatGraduatingYears = new Collection<GraduatingClass> {GraduatingClass}
            };

            var essay1 = new Essay
            {
                CharacterLength = 1,
                RowKey = CreateRandomString(),
                RevisionDateTime = _now,
                EssayTopic = essayTopic1TitleStartsWithB,
                PartitionKey = CreateRandomString()
            };

            var essay2 = new Essay
            {
                CharacterLength = 2,
                RowKey = CreateRandomString(),
                RevisionDateTime = _now,
                EssayTopic = essayTopic2TitlesStartsWithZ,
                PartitionKey = CreateRandomString()
            };

            var essay3 = new Essay
            {
                CharacterLength = 3,
                RowKey = CreateRandomString(),
                RevisionDateTime = _now,
                EssayTopic = essayTopic3TitlesStartsWithA,
                PartitionKey = CreateRandomString()
            };

            #endregion

            #region ReferenceStuff

            var reference1LastNameStartsWithA = new Reference
            {
                Occupation = CreateRandomString(),
                Person = new Person
                {
                    DateCreated = _now,
                    Guid = Guid.NewGuid(),
                    ContactInformation = new ContactInformation
                    {
                        PhoneInformation = new PhoneInformation
                        {
                            BestTimeToContactByPhone = TimeOfDayEnum.Evening,
                            LastUpdated = _now,
                            PhoneNumber = 5664582315
                        },
                        LastUpdated = _now,
                        EmailAddress = CreateRandomString()
                    },
                    Name = new Name
                    {
                        FirstName = CreateRandomString(),
                        LastName = "Z" + Guid.NewGuid(),
                        LastUpdated = _now
                    }
                }
            };

            var reference2LastNameStartsWithZ = new Reference
            {
                Occupation = CreateRandomString(),
                Person = new Person
                {
                    DateCreated = _now,
                    Guid = Guid.NewGuid(),
                    ContactInformation = new ContactInformation
                    {
                        PhoneInformation = new PhoneInformation
                        {
                            BestTimeToContactByPhone = TimeOfDayEnum.Anytime,
                            LastUpdated = _now,
                            PhoneNumber = 129399140123
                        },
                        LastUpdated = _now,
                        EmailAddress = CreateRandomString()
                    },
                    Name = new Name
                    {
                        FirstName = CreateRandomString(),
                        LastName = "A" + Guid.NewGuid(),
                        LastUpdated = _now
                    }
                }
            };

            var letterOfRecommendation1 = new LetterOfRecommendation
            {
                GuidSentToReference = Guid.NewGuid(),
                LetterOfRecommendationPartitionKey = CreateRandomString(),
                LetterOfRecommendationRowKey = CreateRandomString(),
                LastUpdated = _now,
                Reference = reference1LastNameStartsWithA,
                ReferenceRelationshipToApplicant = CreateRandomString()
            };

            var letterOfRecommendation2 = new LetterOfRecommendation
            {
                GuidSentToReference = Guid.NewGuid(),
                LetterOfRecommendationPartitionKey = CreateRandomString(),
                LetterOfRecommendationRowKey = CreateRandomString(),
                LastUpdated = _now,
                Reference = reference2LastNameStartsWithZ,
                ReferenceRelationshipToApplicant = CreateRandomString()
            };

            #endregion

            #region ExtracurricularStuff

            var job1StartsWithB = new Job
            {
                Employer = "B" + Guid.NewGuid(),
                Position = CreateRandomString(),
                ShortSummaryOfWorkResponsibilities = CreateRandomString()
            };

            var job2StartsWithZ = new Job
            {
                Employer = "Z" + Guid.NewGuid(),
                Position = CreateRandomString(),
                ShortSummaryOfWorkResponsibilities = CreateRandomString()
            };

            var job3StartsWithA = new Job
            {
                Employer = "A" + Guid.NewGuid(),
                Position = CreateRandomString(),
                ShortSummaryOfWorkResponsibilities = CreateRandomString()
            };

            var activity1StartsWithX = new Activity
            {
                Name = "x" + Guid.NewGuid(),
                ShortSummaryOfWhatIsInvolved = CreateRandomString()
            };

            var activity2StartsWithB = new Activity
            {
                Name = "B" + Guid.NewGuid(),
                ShortSummaryOfWhatIsInvolved = CreateRandomString()
            };

            var activity3StartsWithZa = new Activity
            {
                Name = "ZA" + Guid.NewGuid(),
                ShortSummaryOfWhatIsInvolved = CreateRandomString()
            };

            var extracurriculars = new ExtracurricularActivities
            {
                Activities = new Collection<Activity>
                {
                    activity1StartsWithX,
                    activity2StartsWithB,
                    activity3StartsWithZa
                },
                HasNonPaidActivities = true,
                Jobs = new Collection<Job>
                {
                    job1StartsWithB,
                    job2StartsWithZ,
                    job3StartsWithA
                },
                LastUpdated = _now,
                PaidWork = true
            };

            #endregion

            #region AcademicInformation

            var classRank = new ClassRank
            {
                LastUpdated = _now,
                ClassNumericalRank = 2,
                GraduatingClassSize = 4
            };

            var transcriptBlobReference = new TranscriptBlobReference
            {
                BlobContainerName = CreateRandomString(),
                ReferenceToTranscriptPdf = CreateRandomString(),
                LastUpdated = _now
            };


            var lowGrade1StartsWithZ = new LowGrade
            {
                ClassTitle = "Z" + Guid.NewGuid(),
                Grade = CreateRandomString(),
                WasAp = true,
                YearOfHighSchool = YearOfHighSchoolEnum.Freshman,
                Explanation = CreateRandomString(),
                LastUpdated = _now
            };

            var lowGrade2StartsWithB = new LowGrade
            {
                ClassTitle = "B" + Guid.NewGuid(),
                Grade = CreateRandomString(),
                WasAp = false,
                YearOfHighSchool = YearOfHighSchoolEnum.Senior,
                Explanation = CreateRandomString(),
                LastUpdated = _now
            };

            var lowGrade3StartsWithC = new LowGrade
            {
                ClassTitle = "C" + Guid.NewGuid(),
                Grade = CreateRandomString(),
                WasAp = false,
                YearOfHighSchool = YearOfHighSchoolEnum.Junior,
                Explanation = CreateRandomString(),
                LastUpdated = _now
            };

            var lowGrade4StartsWithA = new LowGrade
            {
                ClassTitle = "A" + Guid.NewGuid(),
                Grade = CreateRandomString(),
                WasAp = true,
                YearOfHighSchool = YearOfHighSchoolEnum.Sophomore,
                Explanation = CreateRandomString(),
                LastUpdated = _now
            };
            

            AcademicInformation = new AcademicInformation
            {
                Gpa = 2312,
                CareerChoice = CreateRandomString(),
                ProbableNextSchool = CreateRandomString(),
                ClassRank = classRank,
                LowGrades = new Collection<LowGrade>
                {
                    lowGrade1StartsWithZ,
                    lowGrade2StartsWithB,
                    lowGrade3StartsWithC,
                    lowGrade4StartsWithA
                },
                Transcript = transcriptBlobReference,
                LastUpdated = _now
            };

            #endregion

            #region PersonalInformation

            var name = new Name
            {
                FirstName = CreateRandomString(),
                LastUpdated = _now,
                LastName = CreateRandomString()
            };

            var address = new Address
            {
                City = CreateRandomString(),
                StreetAddress1 = CreateRandomString(),
                StreetAddress2 = CreateRandomString(),
                State = CreateRandomString(),
                ZipCode = 1023,
                LastUpdated = _now
            };

            var contactInformation = new ContactInformation
            {
                EmailAddress = CreateRandomString(),
                LastUpdated = _now,
                PhoneInformation = new PhoneInformation
                {
                    LastUpdated = _now,
                    BestTimeToContactByPhone = TimeOfDayEnum.Evening,
                    PhoneNumber = 2134898123489
                },
                Address = address
            };

            var familyInformation = new FamilyInformation
            {
                HighestAttainedDegreeInHome = EducationalDegreesEnum.Associates,
                LastUpdated = _now,
                NumberOfPeopleInHousehold = 123,
                YearlyHouseholdIncomeRange = IncomeRangeEnum.Mt30000Lt40000
            };

            #endregion

            Applicant2 = new Applicant
            {
                Person = new Person
                {
                    DateCreated = _now,
                    Guid = Guid.NewGuid()
                },
                Essays = new Collection<Essay>
                {
                    new Essay
                    {
                        CharacterLength = 2,
                        EssayTopic = essayTopic3TitlesStartsWithA,
                        PartitionKey = CreateRandomString(),
                        RevisionDateTime = _now,
                        RowKey = CreateRandomString()
                    }
                }
            };

            Applicant = new Applicant
            {
                Person = new Person
                {
                    DateCreated = _now,
                    Guid = Guid.NewGuid(),
                    Name = name,
                    ContactInformation = contactInformation
                },
                Essays = new Collection<Essay>
                {
                    essay1,
                    essay2,
                    essay3
                },
                ApplicantPersonalInformation = new ApplicantPersonalInformation
                {
                    GraduatingClass = GraduatingClass,
                    Birthdate = new DateTime(1902, 7, 20),
                    LastUpdated = _now
                },
                LettersOfRecommendation = new Collection<LetterOfRecommendation>
                {
                    letterOfRecommendation1,
                    letterOfRecommendation2
                },
                ExtracurricularActivities = extracurriculars,
                AcademicInformation = AcademicInformation,
                Metadata = new ApplicantMetadata
                {
                    ApplicationFinalized = true
                },
                FamilyInformation = familyInformation
            };

            using (var context = GetRootContext())
            {
                context.ApplicationEvaluators.Add(ApplicationEvaluator);
                context.Applicants.Add(Applicant);
                context.Applicants.Add(Applicant2);

                try { context.SaveChanges(); }
                catch (Exception) { }
                

                essay1.EssayTopic.Id =
                    context.EssayTopics.First(topic => topic.TitleOfEssay == essayTopic1TitleStartsWithB.TitleOfEssay)
                        .Id;

                essay2.EssayTopic.Id =
                    context.EssayTopics.First(topic => topic.TitleOfEssay == essayTopic2TitlesStartsWithZ.TitleOfEssay)
                        .Id;

                essay3.EssayTopic.Id =
                    context.EssayTopics.First(topic => topic.TitleOfEssay == essayTopic3TitlesStartsWithA.TitleOfEssay)
                        .Id;
            }

            #region OrderObjectsInOrderTheyShouldComeBackToUser

            EssayArrayInCorrectOrder = new Collection<Essay>
            {
                essay3,
                essay1,
                essay2
            };

            LettersOfRecommendationInRightOrder = new Collection<LetterOfRecommendation>
            {
                letterOfRecommendation2,
                letterOfRecommendation1
            };

            JobsIfRightOrder = new Collection<Job>
            {
                job3StartsWithA,
                job1StartsWithB,
                job2StartsWithZ
            };

            ActivitiesInRightOrder = new Collection<Activity>
            {
                activity2StartsWithB,
                activity1StartsWithX,
                activity3StartsWithZa
            };

            LowGradesInRightOrder = new Collection<LowGrade>
            {
                lowGrade4StartsWithA,
                lowGrade2StartsWithB,
                lowGrade3StartsWithC,
                lowGrade1StartsWithZ
            };

            #endregion
        }

        private static Applicant Applicant2 { get; set; }

        private static Collection<LowGrade> LowGradesInRightOrder { get; set; }
        private static AcademicInformation AcademicInformation { get; set; }

        private static Collection<Activity> ActivitiesInRightOrder { get; set; }
        private static Collection<Job> JobsIfRightOrder { get; set; }
        
        private static Collection<LetterOfRecommendation> LettersOfRecommendationInRightOrder { get; set; }

        private static Applicant Applicant { get; set; }
        private static ApplicationEvaluator ApplicationEvaluator { get; set; }
        private static GraduatingClass GraduatingClass { get; set; }

        private static void InstantiateRepos()
        {
            A.CallTo(() => TestHelpersCommonFakes.ClaimsInformationGetters.GetUsersGuid())
                .Returns(ApplicationEvaluator.Person.Guid);

            _getRepo = new GetCompletedApplicationRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters);
        }

        #endregion

        private static void FirstGets()
        {
            ApplicantGuid = Applicant.Person.Guid;
            Applicant2Guid = Applicant2.Person.Guid;
            FirstGetResult = GetAllForPiecies();
            FirstGetResultForApplicant2 = _getRepo.GetCollectionsOfEssaysAndLettersOfRecommendation(Applicant2Guid);

        }

        private static CollectionsOfEssaysAndLettersOfRecommendationDto FirstGetResultForApplicant2 { get; set; }

        private static Guid Applicant2Guid { get; set; }
        private static Guid ApplicantGuid { get; set; }

        private static CompletedApplicationDto FirstGetResult { get; set; }

        #region InitialCheck_Essays_And_GeneralRatings

        private static Collection<Essay> EssayArrayInCorrectOrder { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Should_Have_Null_YourFirstImpressionRating()
        {
            Assert.IsNull(FirstGetResult.ApplicantsGeneralInformation.YourFirstImpressionRating);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Should_Have_Null_YourFinalOverallRating()
        {
            Assert.IsNull(FirstGetResult.ApplicantsGeneralInformation.YourFinalOverallRating);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Should_Have_Correct_Guid()
        {
            Assert.AreEqual(Applicant.Person.Guid, FirstGetResult.ApplicantsGeneralInformation.ApplicantGuid);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Should_Have_3_Essays()
        {
            Assert.AreEqual(3, FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.Count);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Should_Be_In_AlphbeticalOrder_By_Title()
        {
            foreach (var essaySummary in FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(EssayArrayInCorrectOrder[essaySummary.i].EssayTopic.TitleOfEssay, essaySummary.value.TitleOfEssay);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Should_Have_Correct_Number_Of_Essays_Applicant2()
        {
            Assert.AreEqual(1, FirstGetResultForApplicant2.EssaySummaries.Count);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Should_Have_Correct_EssayTopicId_Applicant2()
        {
            Assert.AreEqual(EssayArrayInCorrectOrder[0].EssayTopic.Id, FirstGetResultForApplicant2.EssaySummaries.First().EssayTopicId);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Should_Have_Correct_EssayTopicIds()
        {
            //http://stackoverflow.com/a/11437562/2740086
            foreach (var essaySummary in FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.Select((value,i) => new {i, value}))
            {
                Assert.AreEqual(EssayArrayInCorrectOrder[essaySummary.i].EssayTopic.Id, essaySummary.value.EssayTopicId);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Should_Have_Correct_EssayPrompts()
        {
            foreach (var essaySummary in FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(EssayArrayInCorrectOrder[essaySummary.i].EssayTopic.EssayPrompt, essaySummary.value.EssayPrompt);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Should_Have_Correct_EssayCharactedLengths()
        {
            foreach (var essaySummary in FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(EssayArrayInCorrectOrder[essaySummary.i].CharacterLength, essaySummary.value.EssayCharacterLength);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Should_Have_All_Null_Ratings()
        {
            foreach (var essaySummary in FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.Select((value, i) => new { i, value }))
            {
                Assert.IsNull(essaySummary.value.YourRating);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Should_Have_Right_PartitionKeys()
        {
            foreach (var essaySummary in FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(EssayArrayInCorrectOrder[essaySummary.i].PartitionKey, essaySummary.value.EssayKeyValues.PartitionKey);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Should_Have_Right_RowKeys()
        {
            foreach (var essaySummary in FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(EssayArrayInCorrectOrder[essaySummary.i].RowKey, essaySummary.value.EssayKeyValues.RowKey);
            }
        }

        #endregion

        #region LettersOfRecommendation

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_LetterOfRecommendation_Should_Be_In_AlphbeticalOrder_By_ReferenceLastName_Right_ReferenceRelationShip
            ()
        {
            foreach (
                var lorSummary in FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.LetterOfRecommendationSummaries.Select((value, i) => new {i, value}))
            {
                Assert.AreEqual(LettersOfRecommendationInRightOrder[lorSummary.i].ReferenceRelationshipToApplicant,
                    lorSummary.value.ReferenceRelationshipToApplicant);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_LetterOfRecommendation_Should_Be_In_AlphbeticalOrder_By_ReferenceLastName_Right_EmailAddress
            ()
        {
            foreach (
                var lorSummary in FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.LetterOfRecommendationSummaries.Select((value, i) => new {i, value}))
            {
                Assert.AreEqual(
                    LettersOfRecommendationInRightOrder[lorSummary.i].Reference.Person.ContactInformation.EmailAddress,
                    lorSummary.value.ReferencePersonalInformation.EmailAddress);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_LetterOfRecommendation_Should_Right_Occupation()
        {
            foreach (
                var lorSummary in FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.LetterOfRecommendationSummaries.Select((value, i) => new {i, value}))
            {
                Assert.AreEqual(LettersOfRecommendationInRightOrder[lorSummary.i].Reference.Occupation,
                    lorSummary.value.ReferencePersonalInformation.Occupation);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_LetterOfRecommendation_Should_Right_Name()
        {
            foreach (
                var lorSummary in FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.LetterOfRecommendationSummaries.Select((value, i) => new {i, value}))
            {
                Assert.AreEqual(LettersOfRecommendationInRightOrder[lorSummary.i].Reference.Person.Name.FirstName,
                    lorSummary.value.ReferencePersonalInformation.Name.FirstName);
                Assert.AreEqual(LettersOfRecommendationInRightOrder[lorSummary.i].Reference.Person.Name.LastName,
                    lorSummary.value.ReferencePersonalInformation.Name.LastName);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_LetterOfRecommendation_Should_Right_PhoneInformation()
        {
            foreach (
                var lorSummary in FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.LetterOfRecommendationSummaries.Select((value, i) => new {i, value}))
            {
                Assert.AreEqual(
                    LettersOfRecommendationInRightOrder[lorSummary.i].Reference.Person.ContactInformation
                        .PhoneInformation.BestTimeToContactByPhone,
                    lorSummary.value.ReferencePersonalInformation.PhoneInformationDto.BestTimeToContactByPhone);

                Assert.AreEqual(
                    LettersOfRecommendationInRightOrder[lorSummary.i].Reference.Person.ContactInformation
                        .PhoneInformation.PhoneNumber,
                    lorSummary.value.ReferencePersonalInformation.PhoneInformationDto.PhoneNumber);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_LetterOfRecommendation_Should_Right_KeyValues()
        {
            foreach (
                var lorSummary in FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.LetterOfRecommendationSummaries.Select((value, i) => new {i, value}))
            {
                Assert.AreEqual(LettersOfRecommendationInRightOrder[lorSummary.i].LetterOfRecommendationPartitionKey,
                    lorSummary.value.LetterOfRecommendationKeyValues.PartitionKey);

                Assert.AreEqual(LettersOfRecommendationInRightOrder[lorSummary.i].LetterOfRecommendationRowKey,
                    lorSummary.value.LetterOfRecommendationKeyValues.RowKey);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_LetterOfRecommendation_Should_There_Should_Be_Two()
        {
            Assert.AreEqual(2, FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.LetterOfRecommendationSummaries.Count);
        }

        #endregion

        #region Extracurriculars

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Extracurriculars_Should_Have_True_PaidWork()
        {
            Assert.IsTrue(FirstGetResult.ExtracurricularActivities.PaidWork);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Extracurriculars_Should_Have_True_HasNonPaidActivities()
        {
            Assert.IsTrue(FirstGetResult.ExtracurricularActivities.HasNonPaidActivities);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Extracurriculars_Should_Have_Correct_Jobs()
        {
            foreach (var jobs in FirstGetResult.ExtracurricularActivities.Jobs.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(JobsIfRightOrder[jobs.i].Employer, jobs.value.Employer);
                Assert.AreEqual(JobsIfRightOrder[jobs.i].Position, jobs.value.Position);
                Assert.AreEqual(JobsIfRightOrder[jobs.i].ShortSummaryOfWorkResponsibilities, jobs.value.ShortSummaryOfWorkResponsibilities);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstTime_Extracurriculars_Should_Have_Correct_Activities()
        {
            foreach (var activity in FirstGetResult.ExtracurricularActivities.Activities.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(ActivitiesInRightOrder[activity.i].Name, activity.value.Name);
                Assert.AreEqual(ActivitiesInRightOrder[activity.i].ShortSummaryOfWhatIsInvolved, activity.value.ShortSummaryOfWhatIsInvolved);
            }
        }

        #endregion

        #region AcademicInformation

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_AcademicInformation_Should_Have_False_TranscriptMatchesDatabaseValues
            ()
        {
            Assert.IsFalse(FirstGetResult.AcademicInformation.TranscriptMatchesDatabaseValues);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_AcademicInformation_Should_Have_False_TranscriptDoesNotMatchDatabaseValues
            ()
        {
            Assert.IsFalse(FirstGetResult.AcademicInformation.TranscriptDoesNotMatchDatabaseValues);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_AcademicInformation_Should_Have_Correct_ClassNumbericalRank
            ()
        {
            Assert.AreEqual(AcademicInformation.ClassRank.ClassNumericalRank, FirstGetResult.AcademicInformation.BasicAcademicInformation.ClassRank.ClassNumericalRank);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_AcademicInformation_Should_Have_Correct_GraduatingClassSize
            ()
        {
            Assert.AreEqual(AcademicInformation.ClassRank.GraduatingClassSize, FirstGetResult.AcademicInformation.BasicAcademicInformation.ClassRank.GraduatingClassSize); 
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_AcademicInformation_Should_Have_Correct_Gpa
            ()
        {
            Assert.AreEqual(AcademicInformation.Gpa, FirstGetResult.AcademicInformation.BasicAcademicInformation.Gpa);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_AcademicInformation_Should_Have_Correct_CareerChoice
            ()
        {
            Assert.AreEqual(AcademicInformation.CareerChoice, FirstGetResult.AcademicInformation.BasicAcademicInformation.CareerChoice);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_AcademicInformation_Should_Have_Correct_NextSchool
            ()
        {
            Assert.AreEqual(AcademicInformation.ProbableNextSchool, FirstGetResult.AcademicInformation.BasicAcademicInformation.ProbableNextSchool);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_AcademicInformation_Should_Have_Correct_LowGrades_In_Correct_Order
            ()
        {
            foreach (var lowGrade in FirstGetResult.AcademicInformation.LowGrades.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(LowGradesInRightOrder[lowGrade.i].Grade,            lowGrade.value.Grade);
                Assert.AreEqual(LowGradesInRightOrder[lowGrade.i].ClassTitle,       lowGrade.value.ClassTitle);
                Assert.AreEqual(LowGradesInRightOrder[lowGrade.i].WasAp,            lowGrade.value.WasAp);
                Assert.AreEqual(LowGradesInRightOrder[lowGrade.i].YearOfHighSchool, lowGrade.value.YearOfHighSchool);
                Assert.AreEqual(LowGradesInRightOrder[lowGrade.i].Explanation,      lowGrade.value.Explanation); 
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_AcademicInformation_Should_Have_4_LowGrades
            ()
        {
            Assert.AreEqual(4, FirstGetResult.AcademicInformation.LowGrades.Count);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_AcademicInformation_Should_Have_ReferenceToTrascriptPdf
            ()
        {
            Assert.AreEqual(AcademicInformation.Transcript.ReferenceToTranscriptPdf, FirstGetResult.AcademicInformation.TranscriptBlobReference.ReferenceToTranscriptPdf);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_AcademicInformation_Should_Have_BlobContainerName
            ()
        {
            Assert.AreEqual(AcademicInformation.Transcript.BlobContainerName, FirstGetResult.AcademicInformation.TranscriptBlobReference.BlobContainerName);
        }

        #endregion

        #region GeneralInfo

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_GeneralInformation_Should_Have_Correct_FirstName
            ()
        {
            Assert.AreEqual(Applicant.Person.Name.FirstName, FirstGetResult.ApplicantsGeneralInformation.Name.FirstName);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_GeneralInformation_Should_Have_Correct_LastName
            ()
        {
            Assert.AreEqual(Applicant.Person.Name.LastName, FirstGetResult.ApplicantsGeneralInformation.Name.LastName);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_GeneralInformation_Should_Have_Correct_EmailAddress
            ()
        {
            Assert.AreEqual(Applicant.Person.ContactInformation.EmailAddress, FirstGetResult.ApplicantsGeneralInformation.ContactInformation.EmailAddress);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_GeneralInformation_Should_Have_Correct_PhoneNumber
            ()
        {
            Assert.AreEqual(Applicant.Person.ContactInformation.PhoneInformation.PhoneNumber, FirstGetResult.ApplicantsGeneralInformation.ContactInformation.PhoneInformation.PhoneNumber);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_GeneralInformation_Should_Have_Correct_BestTimeToContact
            ()
        {
            Assert.AreEqual(Applicant.Person.ContactInformation.PhoneInformation.BestTimeToContactByPhone, FirstGetResult.ApplicantsGeneralInformation.ContactInformation.PhoneInformation.BestTimeToContactByPhone);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_GeneralInformation_Should_Have_Correct_StreetAddress1
            ()
        {
            Assert.AreEqual(Applicant.Person.ContactInformation.Address.StreetAddress1, FirstGetResult.ApplicantsGeneralInformation.ContactInformation.Address.StreetAddress1);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_GeneralInformation_Should_Have_Correct_StreetAddress2
            ()
        {
            Assert.AreEqual(Applicant.Person.ContactInformation.Address.StreetAddress2, FirstGetResult.ApplicantsGeneralInformation.ContactInformation.Address.StreetAddress2);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_GeneralInformation_Should_Have_Correct_City
            ()
        {
            Assert.AreEqual(Applicant.Person.ContactInformation.Address.City, FirstGetResult.ApplicantsGeneralInformation.ContactInformation.Address.City);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_GeneralInformation_Should_Have_Correct_State
            ()
        {
            Assert.AreEqual(Applicant.Person.ContactInformation.Address.State, FirstGetResult.ApplicantsGeneralInformation.ContactInformation.Address.State);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_GeneralInformation_Should_Have_Correct_ZipCode
            ()
        {
            Assert.AreEqual(Applicant.Person.ContactInformation.Address.ZipCode, FirstGetResult.ApplicantsGeneralInformation.ContactInformation.Address.ZipCode);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_GeneralInformation_Should_Have_Correct_NumberOfPeopleInHouseHold
            ()
        {
            Assert.AreEqual(Applicant.FamilyInformation.NumberOfPeopleInHousehold, FirstGetResult.ApplicantsGeneralInformation.FamilyInformation.NumberOfPeopleInHousehold);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_GeneralInformation_Should_Have_Correct_YearlyHouseholdIncomeRange
            ()
        {
            Assert.AreEqual(Applicant.FamilyInformation.YearlyHouseholdIncomeRange, FirstGetResult.ApplicantsGeneralInformation.FamilyInformation.YearlyHouseholdIncomeRange);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_GeneralInformation_Should_Have_Correct_HighestAttainedDegreeInHome
            ()
        {
            Assert.AreEqual(Applicant.FamilyInformation.HighestAttainedDegreeInHome, FirstGetResult.ApplicantsGeneralInformation.FamilyInformation.HighestAttainedDegreeInHome);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_GeneralInformation_Should_Have_Correct_GraduatingYear
            ()
        {
            Assert.AreEqual(Applicant.ApplicantPersonalInformation.GraduatingClass.GraduatingYear, FirstGetResult.ApplicantsGeneralInformation.PersonalInformation.GraduatingYear);
        }

        [TestCategory("Integration"), TestMethod]
        public void
            GetCompletedApplicationRepository_FirstTime_GeneralInformation_Should_Have_Correct_Birthdate
            ()
        {
            Assert.AreEqual(Applicant.ApplicantPersonalInformation.Birthdate, FirstGetResult.ApplicantsGeneralInformation.PersonalInformation.Birthdate);
        }

        #endregion

        #region FirstInsertOfRatings

        private static void FirstInsertOfRatings()
        {
            _confirmTranscriptRepo = new ConfirmTranscriptRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters);

            _upsertRepo = new RateApplicationRespository(TestHelpersCommonFields.DatabaseName, TestHelpersCommonFakes.ClaimsInformationGetters);

            var explainArray = new List<string>();

            for (var i = 0; i < 10; i++)
            {
                explainArray.Add("reason" + i);
            }

            ExplainArray = explainArray.ToArray();

            RandomlyTimeRatings();
            AddEssayRatingToApplicant2();

            _confirmTranscriptRepo.ConfirmTranscript(new ConfirmTranscriptDto{ApplicantsGuid = Applicant.Person.Guid, InformationMatchesTranscriptPdf = false});

            SecondGetResult = GetAllForPiecies();
            Applicant2GetEssayResult1 = _getRepo.GetCollectionsOfEssaysAndLettersOfRecommendation(Applicant2Guid);
        }

        private static void RandomlyTimeRatings()
        {
            var whichIsFirst = new Random().Next(1, 3);

            var firstImpression1 = new RatingWithApplicantsGuidDto
                    {
                        ApplicantsGuid = FirstGetResult.ApplicantsGeneralInformation.ApplicantGuid,
                        Rating = new GenericRatingDto
                        {
                            Explanation = ExplainArray[0],
                            RatingEnum = RatingEnum.CMinus
                        }
                    };

            var overall1 = new RatingWithApplicantsGuidDto
                    {
                        ApplicantsGuid = FirstGetResult.ApplicantsGeneralInformation.ApplicantGuid,
                        Rating = new GenericRatingDto
                        {
                            Explanation = ExplainArray[1],
                            RatingEnum = RatingEnum.F
                        }
                    };

            var essayRatingOne = new EssayRatingDto
            {
                ApplicantsGuid = FirstGetResult.ApplicantsGeneralInformation.ApplicantGuid,
                EssayTopicId = FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.First(x => x.TitleOfEssay == EssayArrayInCorrectOrder[1].EssayTopic.TitleOfEssay).EssayTopicId,
                Rating = new GenericRatingDto
                {
                    Explanation = ExplainArray[2],
                    RatingEnum = RatingEnum.C
                }
            };

            switch (whichIsFirst)
            {
                case 1:
                    _upsertRepo.UpsertFirstImpression(firstImpression1);
                    _getRepo.GetGeneralInformation(ApplicantGuid); // To make sure that no exception is thrown when Final Overall is not in Db. In response to bug.
                    _upsertRepo.UpsertFinalOverallRating(overall1);
                    _upsertRepo.UpsertEssayRating(essayRatingOne);
                    break;
                case 2:
                    _upsertRepo.UpsertFinalOverallRating(overall1);
                    _getRepo.GetGeneralInformation(ApplicantGuid); // To make sure that no exception is thrown when First Impression is not in Db. In response to bug. 
                    _upsertRepo.UpsertFirstImpression(firstImpression1);
                    _upsertRepo.UpsertEssayRating(essayRatingOne);
                    break;
                default:
                    _upsertRepo.UpsertEssayRating(essayRatingOne);
                    _upsertRepo.UpsertFinalOverallRating(overall1);
                    _upsertRepo.UpsertFirstImpression(firstImpression1);
                    break;
            }
        }

        private static void AddEssayRatingToApplicant2()
        {
            var essayRating = new EssayRatingDto
            {
                ApplicantsGuid = Applicant2Guid,
                EssayTopicId = FirstGetResultForApplicant2.EssaySummaries.First().EssayTopicId,
                Rating = new GenericRatingDto
                {
                    Explanation = ExplainArray[0],
                    RatingEnum = RatingEnum.B
                }
            };

            _upsertRepo.UpsertEssayRating(essayRating);
        }

        private static string[] ExplainArray { get; set; }
        private static CompletedApplicationDto SecondGetResult { get; set; }
        private static CollectionsOfEssaysAndLettersOfRecommendationDto Applicant2GetEssayResult1 { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstInsert_Should_Have_B_Essay_Applicant2()
        {
            Assert.AreEqual(RatingEnum.B, Applicant2GetEssayResult1.EssaySummaries.First().YourRating.RatingEnum);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstInsert_Should_Have_Explain0_Essay_Applicant2()
        {
            Assert.AreEqual(ExplainArray[0], Applicant2GetEssayResult1.EssaySummaries.First().YourRating.Explanation);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstInsert_Should_Have_1_Essay_InArray_Applicant2()
        {
            Assert.AreEqual(1, Applicant2GetEssayResult1.EssaySummaries.Count);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstInsert_Should_Have_CMinus_FirstImpression()
        {
            Assert.AreEqual(RatingEnum.CMinus, SecondGetResult.ApplicantsGeneralInformation.YourFirstImpressionRating.RatingEnum);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstInsert_Should_Have_Explain0_FirstImpression()
        {
            Assert.AreEqual(ExplainArray[0], SecondGetResult.ApplicantsGeneralInformation.YourFirstImpressionRating.Explanation);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstInsert_Should_Have_F_Overall()
        {
            Assert.AreEqual(RatingEnum.F, SecondGetResult.ApplicantsGeneralInformation.YourFinalOverallRating.RatingEnum);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstInsert_Should_Have_Explain1_Overall()
        {
            Assert.AreEqual(ExplainArray[1], SecondGetResult.ApplicantsGeneralInformation.YourFinalOverallRating.Explanation);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstInsert_Should_Have_C_EssayArrayInCorrectOrder1()
        {
            Assert.AreEqual(RatingEnum.C, SecondGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.First(x => x.TitleOfEssay == EssayArrayInCorrectOrder[1].EssayTopic.TitleOfEssay).YourRating.RatingEnum);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstInsert_Should_Have_Explain2_EssayArrayInCorrectOrder1()
        {
            Assert.AreEqual(ExplainArray[2], SecondGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.First(x => x.TitleOfEssay == EssayArrayInCorrectOrder[1].EssayTopic.TitleOfEssay).YourRating.Explanation);
        }
                    
        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstInsert_Should_Have_Nulls_In_Other_Two_Ratings()
        {
            Assert.IsNull(SecondGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.First(x => x.TitleOfEssay == EssayArrayInCorrectOrder[0].EssayTopic.TitleOfEssay).YourRating);
            Assert.IsNull(SecondGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.First(x => x.TitleOfEssay == EssayArrayInCorrectOrder[2].EssayTopic.TitleOfEssay).YourRating);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstInsert_Should_True_TranscirptDoesNotMatchDatabaseValues()
        {
            Assert.IsTrue(SecondGetResult.AcademicInformation.TranscriptDoesNotMatchDatabaseValues);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_FirstInsert_Should_False_TranscirptMatchesDatabaseValues()
        {
            Assert.IsFalse(SecondGetResult.AcademicInformation.TranscriptMatchesDatabaseValues);
        }

        #endregion

        #region UpsertRatings

        private static void UpsertOfRatings()
        {
            var firstImpression2 = new RatingWithApplicantsGuidDto
            {
                ApplicantsGuid = FirstGetResult.ApplicantsGeneralInformation.ApplicantGuid,
                Rating = new GenericRatingDto
                {
                    Explanation = ExplainArray[3],
                    RatingEnum = RatingEnum.CPlus
                }
            };

            var overall2 = new RatingWithApplicantsGuidDto
            {
                ApplicantsGuid = FirstGetResult.ApplicantsGeneralInformation.ApplicantGuid,
                Rating = new GenericRatingDto
                {
                    Explanation = ExplainArray[4],
                    RatingEnum = RatingEnum.BMinus
                }
            };

            var essayRating2 = new EssayRatingDto
            {
                ApplicantsGuid = FirstGetResult.ApplicantsGeneralInformation.ApplicantGuid,
                EssayTopicId =
                    FirstGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.First(
                        x => x.TitleOfEssay == EssayArrayInCorrectOrder[1].EssayTopic.TitleOfEssay).EssayTopicId,
                Rating = new GenericRatingDto
                {
                    Explanation = ExplainArray[5],
                    RatingEnum = RatingEnum.B
                }
            };

            _upsertRepo.UpsertEssayRating(essayRating2);
            _upsertRepo.UpsertFinalOverallRating(overall2);
            _upsertRepo.UpsertFirstImpression(firstImpression2);
            _confirmTranscriptRepo.ConfirmTranscript(new ConfirmTranscriptDto{ApplicantsGuid = Applicant.Person.Guid, InformationMatchesTranscriptPdf = true});

            ThirdGetResult = GetAllForPiecies();
        }

        private static CompletedApplicationDto ThirdGetResult { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_SecondUpsert_Should_Have_CPlus_FirstImpression()
        {
            Assert.AreEqual(RatingEnum.CPlus, ThirdGetResult.ApplicantsGeneralInformation.YourFirstImpressionRating.RatingEnum);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_SecondUpsert_Should_Have_Explain3_FirstImpression()
        {
            Assert.AreEqual(ExplainArray[3], ThirdGetResult.ApplicantsGeneralInformation.YourFirstImpressionRating.Explanation);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_SecondUpsert_Should_Have_BMinus_Overall()
        {
            Assert.AreEqual(RatingEnum.BMinus, ThirdGetResult.ApplicantsGeneralInformation.YourFinalOverallRating.RatingEnum);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_SecondUpsert_Should_Have_Explain4_Overall()
        {
            Assert.AreEqual(ExplainArray[4], ThirdGetResult.ApplicantsGeneralInformation.YourFinalOverallRating.Explanation);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_SecondUpsert_Should_Have_B_EssayArrayInCorrectOrder1()
        {
            Assert.AreEqual(RatingEnum.B,
                ThirdGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.First(
                    x => x.TitleOfEssay == EssayArrayInCorrectOrder[1].EssayTopic.TitleOfEssay).YourRating.RatingEnum);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_SecondUpsert_Should_Have_Explain5_EssayArrayInCorrectOrder1()
        {
            Assert.AreEqual(ExplainArray[5],
                ThirdGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.First(
                    x => x.TitleOfEssay == EssayArrayInCorrectOrder[1].EssayTopic.TitleOfEssay).YourRating.Explanation);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_SecondUpsert_Should_Have_Nulls_In_Other_Two_Ratings()
        {
            Assert.IsNull(
                ThirdGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.First(
                    x => x.TitleOfEssay == EssayArrayInCorrectOrder[0].EssayTopic.TitleOfEssay).YourRating);
            Assert.IsNull(
                ThirdGetResult.CollectionsOfEssaysAndLettersOfRecommendation.EssaySummaries.First(
                    x => x.TitleOfEssay == EssayArrayInCorrectOrder[2].EssayTopic.TitleOfEssay).YourRating);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_SecondUpsert_Should_True_TranscirptMatchesDatabaseValues()
        {
            Assert.IsFalse(ThirdGetResult.AcademicInformation.TranscriptDoesNotMatchDatabaseValues);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetCompletedApplicationRepository_SecondUpsert_Should_False_TranscirptDoesNotMatchDatabaseValues()
        {
            Assert.IsTrue(ThirdGetResult.AcademicInformation.TranscriptMatchesDatabaseValues);
        }

        #endregion
        
        #region Utilities

        private static CompletedApplicationDto GetAllForPiecies()
        {
            return new CompletedApplicationDto
            {
                ApplicantsGeneralInformation = _getRepo.GetGeneralInformation(ApplicantGuid),
                ExtracurricularActivities = _getRepo.GetExtracurricularActivities(ApplicantGuid),
                AcademicInformation = _getRepo.GetAcademicInformation(ApplicantGuid),
                CollectionsOfEssaysAndLettersOfRecommendation = _getRepo.GetCollectionsOfEssaysAndLettersOfRecommendation(ApplicantGuid)
            };
        }

        private static DatabaseRootContext GetRootContext()
        {
            return new DatabaseRootContext(TestHelpersCommonFields.DatabaseName);
        }

        public static string CreateRandomString()
        {
            return TestHelpersCommonFields.GetRandomString();
        }

        [ClassCleanup]
        public static void CleanDb()
        {

        }

        #endregion
    }
}
