using System;
using System.Collections.Generic;
using System.Linq;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants;
using BohFoundation.Domain.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.Domain.Tests.Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants
{
    [TestClass]
    public class AllApplicantsForAGraduatingYearDtoTests
    {

        [TestInitialize]
        public void Initialize()
        {
            AllApplicantsForAGraduatingYearDto = new AllFinalizedApplicantsForAGraduatingYearDto();
        }

        private AllFinalizedApplicantsForAGraduatingYearDto AllApplicantsForAGraduatingYearDto { get; set; }

        [TestMethod]
        public void AllApplicantsForAGraduatingYearDto_PercentRated_Should_Be_Zero_Default()
        {
            Assert.AreEqual(0.0, AllApplicantsForAGraduatingYearDto.PercentRated);
        }

        [TestMethod]
        public void AllApplicantsForAGraduatingYearDto_NumberOfApplicantsNotYetRated_Should_Be_Zero_Default()
        {
            Assert.AreEqual(0, AllApplicantsForAGraduatingYearDto.NumberOfApplicantsNotYetRated);
        }

        [TestMethod]
        public void AllApplicantsForAGraduatingYearDto_NextRandomApplicantForReview_Should_Be_New_Guid_Default()
        {
            Assert.AreEqual(new Guid(), AllApplicantsForAGraduatingYearDto.NextRandomApplicantForReview);
        }

        [TestMethod]
        public void AllApplicantsForAGraduatingYearDto_PercentRated_Should_Be_100()
        {
            AddApplicantSummarys(2, 2);
            Assert.AreEqual(1.0, AllApplicantsForAGraduatingYearDto.PercentRated);
        }

        [TestMethod]
        public void AllApplicantsForAGraduatingYearDto_NumberOfApplicantsNotYetRated_Should_Be_0()
        {
            AddApplicantSummarys(2, 2);
            Assert.AreEqual(0, AllApplicantsForAGraduatingYearDto.NumberOfApplicantsNotYetRated);
        }

        [TestMethod]
        public void AllApplicantsForAGraduatingYearDto_PercentRated_Should_Be_50()
        {
            AddApplicantSummarys(4, 2);
            Assert.AreEqual(0.5, AllApplicantsForAGraduatingYearDto.PercentRated);
        }

        [TestMethod]
        public void AllApplicantsForAGraduatingYearDto_NumberOfApplicantsNotYetRated_Should_Be_2()
        {
            AddApplicantSummarys(4, 2);
            Assert.AreEqual(2, AllApplicantsForAGraduatingYearDto.NumberOfApplicantsNotYetRated);
        }

        [TestMethod]
        public void AllApplicantsForAGraduatingYearDto_PercentRated_Should_Be_0()
        {
            AddApplicantSummarys(4, 0);
            Assert.AreEqual(0.0, AllApplicantsForAGraduatingYearDto.PercentRated);
        }

        [TestMethod]
        public void AllApplicantsForAGraduatingYearDto_NumberOfApplicantsNotYetRated_Should_Be_4()
        {
            AddApplicantSummarys(4, 0);
            Assert.AreEqual(4, AllApplicantsForAGraduatingYearDto.NumberOfApplicantsNotYetRated);
        }

        [TestMethod]
        public void AllApplicantsForAGraduatingYearDto_PercentRated_Should_Be_33()
        {
            AddApplicantSummarys(3, 1);
            Assert.AreEqual(0.33, AllApplicantsForAGraduatingYearDto.PercentRated);
        }

        [TestMethod]
        public void AllApplicantsForAGraduatingYearDto_NumberOfApplicantsNotYetRated_Should_Be_2_For_3_1()
        {
            AddApplicantSummarys(3, 1);
            Assert.AreEqual(2, AllApplicantsForAGraduatingYearDto.NumberOfApplicantsNotYetRated);
        }

        [TestMethod]
        public void AllApplicantsForAGraduatingYearDto_NextRandomApplicantForReview_Should_Be_The_First_One()
        {
            AddApplicantSummarys(1,0);
            Assert.AreEqual(AllApplicantsForAGraduatingYearDto.ApplicantSummaries.First().ApplicantGuid, AllApplicantsForAGraduatingYearDto.NextRandomApplicantForReview);
        }

        [TestMethod]
        public void AllApplicantsForAGraduatingYearDto_NextRandomApplicantForReview_Should_The_Only_One_Not_Rated()
        {
            AddApplicantSummarys(5, 4);

            var arrayOfApplicantGuidsWithOutRating =
                AllApplicantsForAGraduatingYearDto.ApplicantSummaries.Where(
                    applicantSummary => applicantSummary.YourRating == null)
                    .Select(applicant => applicant.ApplicantGuid).ToList();

            Assert.IsTrue(arrayOfApplicantGuidsWithOutRating.Contains(AllApplicantsForAGraduatingYearDto.NextRandomApplicantForReview));
        }

        [TestMethod]
        public void AllApplicantsForAGraduatingYearDto_NextRandomApplicantForReview_Should_One_Of_The_Items_Not_Rated()
        {
            AddApplicantSummarys(30, 15);

            var arrayOfApplicantGuidsWithOutRating =
                AllApplicantsForAGraduatingYearDto.ApplicantSummaries.Where(
                    applicantSummary => applicantSummary.YourRating == null)
                    .Select(applicant => applicant.ApplicantGuid).ToList();

            Assert.IsTrue(arrayOfApplicantGuidsWithOutRating.Contains(AllApplicantsForAGraduatingYearDto.NextRandomApplicantForReview));
        }

        [TestMethod]
        public void AllApplicantsForAGraduatingYearDto_NextRandomApplicantForReview_Should_Be_New_Guid_When_There_Are_Non_Left()
        {
            AddApplicantSummarys(15, 15);
            Assert.AreEqual(new Guid(), AllApplicantsForAGraduatingYearDto.NextRandomApplicantForReview);
        }

        private void AddApplicantSummarys(int totalSummaries, int ratings)
        {
            AllApplicantsForAGraduatingYearDto.ApplicantSummaries = new List<ApplicantSummaryDto>();

            var ratingsAlreadyDone = 0;
            for (var i = 0; i < totalSummaries; i++)
            {
                if (ratings > ratingsAlreadyDone)
                {
                    AllApplicantsForAGraduatingYearDto.ApplicantSummaries.Add(new ApplicantSummaryDto{YourRating = RatingEnum.AMinus, ApplicantGuid = Guid.NewGuid()});
                    ratingsAlreadyDone++;
                }
                else
                {
                    AllApplicantsForAGraduatingYearDto.ApplicantSummaries.Add(new ApplicantSummaryDto{ApplicantGuid = Guid.NewGuid()});
                }
            }
        }
    }
}
