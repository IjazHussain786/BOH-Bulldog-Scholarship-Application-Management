using System;
using BohFoundation.Domain.Enums;

namespace BohFoundation.Domain.EntityFrameworkModels.Applicants.Family
{
    public class FamilyInformation
    {
        public int Id { get; set; }
        public int NumberOfPeopleInHousehold { get; set; }
        public IncomeRangeEnum YearlyHouseholdIncomeRange { get; set; }
        public EducationalDegreesEnum HighestAttainedDegreeInHome { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual Applicant Applicant { get; set; }
        public virtual W2 W2 { get; set; }
    }
}