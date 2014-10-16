using System;
using System.Collections.Generic;
using System.Linq;

namespace BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants
{
    public class AllFinalizedApplicantsForAGraduatingYearDto
    {
        public int GraduatingYear { get; set; }
        public List<ApplicantSummaryDto> ApplicantSummaries { get; set; }
        
        public double PercentRated
        {
            get
            {
                if (ApplicantSummaries == null) return 0;
                var percentDone = ApplicantSummaries.Count(x => x.YourRating != null)/
                                  (double) (ApplicantSummaries.Count);
                return
                    Math.Round(percentDone, 2);
            }
        }

        public int NumberOfApplicantsNotYetRated
        {
            get
            {
                if (ApplicantSummaries == null) return 0;
                return ApplicantSummaries.Count - ApplicantSummaries.Count(applicantSummaries => applicantSummaries.YourRating != null);
            }
        }

        public Guid NextRandomApplicantForReview
        {
            get { return RandomApplicantForReview(); }
        }

        private Guid RandomApplicantForReview()
        {
            if(ApplicantSummaries == null) return new Guid();

            var applicantsGuidsThatArentRated =
                ApplicantSummaries.Where(applicantSummary => applicantSummary.YourRating == null)
                    .Select(applicantSummary => applicantSummary.ApplicantGuid)
                    .ToArray();

            if (applicantsGuidsThatArentRated.Length == 0) return new Guid();

            var randomIndex = new Random().Next(1, applicantsGuidsThatArentRated.Length);
            
            return applicantsGuidsThatArentRated[randomIndex - 1];
        }
    }
}
