using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Implementations;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.RatingSummary;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.ApplicationEvaluators;
using BohFoundation.Domain.EntityFrameworkModels.Common;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.Enums;
using BohFoundation.EntityFrameworkBaseClass;
using BohFoundation.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.ApplicationEvaluatorsRepository.Tests.IntegrationTests
{
    [TestClass]
    public class GetTopRatedApplicantsRepositoryTests
    {
        private static DateTime _now;
        private static GetTopRatedApplicantsRepository _repo;

        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            TestHelpersCommonFields.InitializeFields();
            TestHelpersCommonFakes.InitializeFakes();

            _now = DateTime.UtcNow;

            CreateListOfApplicationEvaluators();
            CreateListOfApplicantsWithRatings();
            GetTop5Applicants();
        }

        #region Setup

        private static void CreateListOfApplicationEvaluators()
        {
            var arrayOfFirstNameLetters = new[] {"a", "z", "m", "x", "d"};

            ApplicationEvaluatorsInRandomOrder = new Collection<ApplicationEvaluator>();

            foreach (var arrayOfFirstNameLetter in arrayOfFirstNameLetters)
            {
                ApplicationEvaluatorsInRandomOrder.Add(CreateApplicationEvaluator(arrayOfFirstNameLetter));
            }

            ApplicationEvaluatorsInRightOrder =
                ApplicationEvaluatorsInRandomOrder.OrderBy(
                    applicantEvaluator => applicantEvaluator.Person.Name.FirstName).ToList();
        }

        private static Collection<ApplicationEvaluator> ApplicationEvaluatorsInRandomOrder { get; set; }
        private static List<ApplicationEvaluator> ApplicationEvaluatorsInRightOrder { get; set; }

        private static void CreateListOfApplicantsWithRatings()
        {
            ApplicantsClass = new GraduatingClass
            {
                GraduatingYear = new Random().Next()
            };

            AddApplicantRated6();
            AddApplicantRated1Only1Rating();
            AddApplicantRated3Only2Rating();
            AddApplicantRated4Only3Rating();
            AddApplicantRated2();
            AddApplicantRated5();

            using (var context = GetRootContext())
            {
                context.Applicants.AddRange(ApplicantsWithRatings);
                context.SaveChanges();
            }

            ApplicantsInCorrectOrder = new Collection<Applicant>
            {
                ApplicantRated1Only1Rating,
                ApplicantRated2,
                ApplicantRated3Only2Rating,
                ApplicantRated4Only3Rating,
                ApplicantRated5
            };
        }

        private static Collection<Applicant> ApplicantsInCorrectOrder { get; set; }

        //Average C-
        private static void AddApplicantRated5()
        {
            ApplicantRated5 = CreateApplicant();

            var applicantRatings = new Collection<GenericRating>
            {
                CreateGenericRating(RatingEnum.CMinus, false),
                CreateGenericRating(RatingEnum.CMinus),
                CreateGenericRating(RatingEnum.CMinus),
                CreateGenericRating(RatingEnum.CMinus),
                CreateGenericRating(RatingEnum.C), // See if it rounds down
            };

            AddRatings(ApplicantRated5, applicantRatings);
            ApplicantsWithRatings.Add(ApplicantRated5);
        }

        //Average A
        private static void AddApplicantRated2()
        {
            ApplicantRated2 = CreateApplicant();

            var applicantRatings = new Collection<GenericRating>
            {
                CreateGenericRating(RatingEnum.APlus, false),
                CreateGenericRating(RatingEnum.APlus),
                CreateGenericRating(RatingEnum.A),
                CreateGenericRating(RatingEnum.AMinus),
                CreateGenericRating(RatingEnum.AMinus)
            };

            AddRatings(ApplicantRated2, applicantRatings);
            ApplicantsWithRatings.Add(ApplicantRated2);
        }

        //Average C
        private static void AddApplicantRated4Only3Rating()
        {
            ApplicantRated4Only3Rating = CreateApplicant();

            var applicantRatings = new Collection<GenericRating>
            {
                CreateGenericRating(RatingEnum.B, false),
                CreateGenericRating(RatingEnum.F),
                CreateGenericRating(RatingEnum.C)
            };

            AddRatings(ApplicantRated4Only3Rating, applicantRatings);
            ApplicantsWithRatings.Add(ApplicantRated4Only3Rating);
        }

        //Average B+
        private static void AddApplicantRated3Only2Rating()
        {
            ApplicantRated3Only2Rating = CreateApplicant();

            var applicantRatings = new Collection<GenericRating>
            {
                CreateGenericRating(RatingEnum.AMinus),
                CreateGenericRating(RatingEnum.B, false)
            };

            AddRatings(ApplicantRated3Only2Rating, applicantRatings);
            ApplicantsWithRatings.Add(ApplicantRated3Only2Rating);
        }


        //Average APlus
        private static void AddApplicantRated1Only1Rating()
        {
            ApplicantRated1Only1Rating = CreateApplicant();

            var applicant1Ratings = new Collection<GenericRating>
            {
                CreateGenericRating(RatingEnum.APlus)
            };

            AddRatings(ApplicantRated1Only1Rating, applicant1Ratings);
            
            //Creating A Null Overall Rating
            ApplicantRated1Only1Rating.ApplicantRatings.Add(new ApplicantRating
            {
                ApplicationEvaluator = ApplicationEvaluatorsInRandomOrder[1],
                FirstImpressionRating = new GenericRating
                {
                    RatingEnum = RatingEnum.A,
                    Explanation = "Making the overall null."
                }
            });

            ApplicantsWithRatings.Add(ApplicantRated1Only1Rating);
        }

        //Average F
        private static void AddApplicantRated6()
        {
            ApplicantRated6 = CreateApplicant();

            var applicant6Ratings = new[]
            {
                CreateGenericRating(RatingEnum.F, false),
                CreateGenericRating(RatingEnum.F, false),
                CreateGenericRating(RatingEnum.F, false),
                CreateGenericRating(RatingEnum.F, false),
                CreateGenericRating(RatingEnum.F, false),
            };

            AddRatings(ApplicantRated6, applicant6Ratings);

            ApplicantsWithRatings = new Collection<Applicant>
            {
                ApplicantRated6
            };
        }

        private static void AddRatings(Applicant applicant, IEnumerable<GenericRating> applicantRatings)
        {
            applicant.ApplicantRatings = new Collection<ApplicantRating>();

            foreach (var applicantRating in applicantRatings.Select((value, i) => new {i, value}))
            {
                applicant.ApplicantRatings.Add(
                    new ApplicantRating
                    {
                        ApplicationEvaluator = ApplicationEvaluatorsInRandomOrder[applicantRating.i],
                        OverallRating = applicantRating.value
                    });
            }
        }

        private static GraduatingClass ApplicantsClass { get; set; }

        private static Applicant ApplicantRated5 { get; set; }
        private static Applicant ApplicantRated2 { get; set; }
        private static Applicant ApplicantRated4Only3Rating { get; set; }
        private static Applicant ApplicantRated3Only2Rating { get; set; }
        private static Applicant ApplicantRated1Only1Rating { get; set; }
        private static Applicant ApplicantRated6 { get; set; }

        private static Collection<Applicant> ApplicantsWithRatings { get; set; }

        #endregion

        private static void GetTop5Applicants()
        {
            _repo = new GetTopRatedApplicantsRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters);

            Result = _repo.GetTop5Applicants(ApplicantsClass.GraduatingYear);

            TopApplicantsArray = Result.TopApplicants.ToArray();
        }

        private static TopApplicantRatingSummaryDto[] TopApplicantsArray { get; set; }
        private static Top5ApplicantsDto Result { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Should()
        {
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Should_Return_5_Applicants()
        {
            Assert.AreEqual(5, Result.TopApplicants.Count);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Should_Return_5_Applicants_InRightOrder_Guid()
        {
            foreach (
            var summary in Result.TopApplicants.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(ApplicantsInCorrectOrder[summary.i].Person.Guid, summary.value.ApplicantsGuid);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Should_Return_5_Applicants_InRightOrder_AverageRating()
        {
            var averageRatingsInRightOrder = new Collection<RatingEnum>
            {
                RatingEnum.APlus,
                RatingEnum.A,
                RatingEnum.BPlus,
                RatingEnum.C,
                RatingEnum.CMinus
            };
            
            foreach (
            var summary in Result.TopApplicants.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(averageRatingsInRightOrder[summary.i], summary.value.AverageRating);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Should_Return_5_Applicants_InRightOrder_ApplicantsName()
        {
            foreach (
            var summary in Result.TopApplicants.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(ApplicantsInCorrectOrder[summary.i].Person.Name.FirstName, summary.value.ApplicantsName.FirstName);
                Assert.AreEqual(ApplicantsInCorrectOrder[summary.i].Person.Name.LastName, summary.value.ApplicantsName.LastName);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Applicant1_Should_Have_1_Ratings()
        {
            Assert.AreEqual(1, Result.TopApplicants.First().ApplicantEvaluatorsRatings.Count);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Applicant2_Should_Have_5_Ratings()
        {
            Assert.AreEqual(5, Result.TopApplicants.First(x => x.ApplicantsGuid == ApplicantRated2.Person.Guid).ApplicantEvaluatorsRatings.Count);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Applicant3_Should_Have_2_Ratings()
        {
            Assert.AreEqual(2, Result.TopApplicants.First(x => x.ApplicantsGuid == ApplicantRated3Only2Rating.Person.Guid).ApplicantEvaluatorsRatings.Count);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Applicant4_Should_Have_3_Ratings()
        {
            Assert.AreEqual(3, Result.TopApplicants.First(x => x.ApplicantsGuid == ApplicantRated4Only3Rating.Person.Guid).ApplicantEvaluatorsRatings.Count);
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Applicant5_Should_Have_5_Ratings()
        {
            Assert.AreEqual(5, Result.TopApplicants.First(x => x.ApplicantsGuid == ApplicantRated5.Person.Guid).ApplicantEvaluatorsRatings.Count);
        }


        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Applicant1_Should_Have_Ratings_InCorrectOrder_With_Correct_Names()
        {
            foreach (
            var component in TopApplicantsArray[0].ApplicantEvaluatorsRatings.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(ApplicationEvaluatorsInRightOrder[component.i].Person.Name.FirstName, component.value.ApplicantEvaluatorsName.FirstName);
                Assert.AreEqual(ApplicationEvaluatorsInRightOrder[component.i].Person.Name.LastName, component.value.ApplicantEvaluatorsName.LastName);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Applicant2_Should_Have_Ratings_InCorrectOrder_With_Correct_Names()
        {
            foreach (
            var component in TopApplicantsArray[1].ApplicantEvaluatorsRatings.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(ApplicationEvaluatorsInRightOrder[component.i].Person.Name.FirstName, component.value.ApplicantEvaluatorsName.FirstName);
                Assert.AreEqual(ApplicationEvaluatorsInRightOrder[component.i].Person.Name.LastName, component.value.ApplicantEvaluatorsName.LastName);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Applicant3_Should_Have_Ratings_InCorrectOrder_With_Correct_Names()
        {
            var applicantEvaluatorsInRightOrder = ApplicationEvaluatorsInRandomOrder
                .Take(2)
                .OrderBy(applicantEvaluators => applicantEvaluators.Person.Name.FirstName)
                .ToArray();
            
            foreach (
            var component in TopApplicantsArray[2].ApplicantEvaluatorsRatings.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(applicantEvaluatorsInRightOrder[component.i].Person.Name.FirstName, component.value.ApplicantEvaluatorsName.FirstName);
                Assert.AreEqual(applicantEvaluatorsInRightOrder[component.i].Person.Name.LastName, component.value.ApplicantEvaluatorsName.LastName);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Applicant4_Should_Have_Ratings_InCorrectOrder_With_Correct_Names()
        {
            var applicantEvaluatorsInRightOrder = ApplicationEvaluatorsInRandomOrder
                .Take(3)
                .OrderBy(applicantEvaluators => applicantEvaluators.Person.Name.FirstName)
                .ToArray();

            foreach (
            var component in TopApplicantsArray[3].ApplicantEvaluatorsRatings.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(applicantEvaluatorsInRightOrder[component.i].Person.Name.FirstName, component.value.ApplicantEvaluatorsName.FirstName);
                Assert.AreEqual(applicantEvaluatorsInRightOrder[component.i].Person.Name.LastName, component.value.ApplicantEvaluatorsName.LastName);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Applicant5_Should_Have_Ratings_InCorrectOrder_With_Correct_Names()
        {
            foreach (
            var component in TopApplicantsArray[4].ApplicantEvaluatorsRatings.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(ApplicationEvaluatorsInRightOrder[component.i].Person.Name.FirstName, component.value.ApplicantEvaluatorsName.FirstName);
                Assert.AreEqual(ApplicationEvaluatorsInRightOrder[component.i].Person.Name.LastName, component.value.ApplicantEvaluatorsName.LastName);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Applicant1_Should_Have_Ratings_With_Correct_RatingEnum_And_Explanation()
        {
            var ratingResultDto = TopApplicantsArray[0].ApplicantEvaluatorsRatings.ToArray();
            var correctlyOrderedRatings =
                ApplicantRated1Only1Rating.ApplicantRatings.OrderBy(
                    applicantRating => applicantRating.ApplicationEvaluator.Person.Name.FirstName).ToArray();

            
            foreach (
            var rating in ratingResultDto.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(correctlyOrderedRatings[rating.i].OverallRating.RatingEnum, rating.value.OverallRating.RatingEnum);
                Assert.AreEqual(correctlyOrderedRatings[rating.i].OverallRating.Explanation, rating.value.OverallRating.Explanation);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Applicant2_Should_Have_Ratings_With_Correct_RatingEnum_And_Explanation()
        {
            var ratingResultDto = TopApplicantsArray[1].ApplicantEvaluatorsRatings.ToArray();
            var correctlyOrderedRatings =
                ApplicantRated2.ApplicantRatings.OrderBy(
                    applicantRating => applicantRating.ApplicationEvaluator.Person.Name.FirstName).ToArray();


            foreach (
            var rating in ratingResultDto.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(correctlyOrderedRatings[rating.i].OverallRating.RatingEnum, rating.value.OverallRating.RatingEnum);
                Assert.AreEqual(correctlyOrderedRatings[rating.i].OverallRating.Explanation, rating.value.OverallRating.Explanation);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Applicant3_Should_Have_Ratings_With_Correct_RatingEnum_And_Explanation()
        {
            var ratingResultDto = TopApplicantsArray[2].ApplicantEvaluatorsRatings.ToArray();
            var correctlyOrderedRatings =
                ApplicantRated3Only2Rating.ApplicantRatings.OrderBy(
                    applicantRating => applicantRating.ApplicationEvaluator.Person.Name.FirstName).ToArray();


            foreach (
            var rating in ratingResultDto.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(correctlyOrderedRatings[rating.i].OverallRating.RatingEnum, rating.value.OverallRating.RatingEnum);
                Assert.AreEqual(correctlyOrderedRatings[rating.i].OverallRating.Explanation, rating.value.OverallRating.Explanation);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Applicant4_Should_Have_Ratings_With_Correct_RatingEnum_And_Explanation()
        {
            var ratingResultDto = TopApplicantsArray[3].ApplicantEvaluatorsRatings.ToArray();
            var correctlyOrderedRatings =
                ApplicantRated4Only3Rating.ApplicantRatings.OrderBy(
                    applicantRating => applicantRating.ApplicationEvaluator.Person.Name.FirstName).ToArray();


            foreach (
            var rating in ratingResultDto.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(correctlyOrderedRatings[rating.i].OverallRating.RatingEnum, rating.value.OverallRating.RatingEnum);
                Assert.AreEqual(correctlyOrderedRatings[rating.i].OverallRating.Explanation, rating.value.OverallRating.Explanation);
            }
        }

        [TestCategory("Integration"), TestMethod]
        public void GetTopRatedApplicantsRepository_GetTop5Applicants_Applicant5_Should_Have_Ratings_With_Correct_RatingEnum_And_Explanation()
        {
            var ratingResultDto = TopApplicantsArray[4].ApplicantEvaluatorsRatings.ToArray();
            var correctlyOrderedRatings =
                ApplicantRated5.ApplicantRatings.OrderBy(
                    applicantRating => applicantRating.ApplicationEvaluator.Person.Name.FirstName).ToArray();


            foreach (
            var rating in ratingResultDto.Select((value, i) => new { i, value }))
            {
                Assert.AreEqual(correctlyOrderedRatings[rating.i].OverallRating.RatingEnum, rating.value.OverallRating.RatingEnum);
                Assert.AreEqual(correctlyOrderedRatings[rating.i].OverallRating.Explanation, rating.value.OverallRating.Explanation);
            }
        }

        
        #region Utilities

        private static GenericRating CreateGenericRating(RatingEnum ratingEnum, bool hasExplanation = true)
        {
            if (hasExplanation)
            {
                return new GenericRating
                {
                    Explanation = TestHelpersCommonFields.GetRandomString(),
                    RatingEnum = ratingEnum
                };
            }
            else
            {
                return new GenericRating{RatingEnum = ratingEnum};
            }
        }

        private static Applicant CreateApplicant()
        {
            return new Applicant
            {
                Person = new Person
                {
                    DateCreated    = _now,
                    Name = new Name
                    {
                        FirstName = TestHelpersCommonFields.GetRandomString(),
                        LastName = TestHelpersCommonFields.GetRandomString(),
                        LastUpdated = _now
                    },
                    Guid = Guid.NewGuid()
                },
                ApplicantPersonalInformation = new ApplicantPersonalInformation
                {
                    Birthdate = _now,
                    GraduatingClass = ApplicantsClass,
                    LastUpdated = _now
                }
            };
        }

        private static ApplicationEvaluator CreateApplicationEvaluator(string firstLetterOfFirstName)
        {
            return new ApplicationEvaluator
            {
                Person = new Person
                {
                    DateCreated = _now,
                    Guid = Guid.NewGuid(),
                    Name  = new Name
                    {
                        FirstName = firstLetterOfFirstName + TestHelpersCommonFields.GetRandomString(),
                        LastName = TestHelpersCommonFields.GetRandomString(),
                        LastUpdated = _now
                    }
                }
            };
        }

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
