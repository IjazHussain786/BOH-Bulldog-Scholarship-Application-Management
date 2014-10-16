using System;
using BohFoundation.Domain.Dtos.Person;
using BohFoundation.Domain.Enums;

namespace BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants
{
    public class ApplicantSummaryDto
    {
        public NameDto ApplicantName { get; set; }
        public Guid ApplicantGuid { get; set; }
        public double Gpa { get; set; }
        public IncomeRangeEnum IncomeRange { get; set; }
        public RatingEnum? YourRating { get; set; }
    }
}
