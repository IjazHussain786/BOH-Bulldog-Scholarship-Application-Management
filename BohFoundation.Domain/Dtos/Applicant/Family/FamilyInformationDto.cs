using System;
using BohFoundation.Domain.Enums;

namespace BohFoundation.Domain.Dtos.Applicant.Family
{
    public class FamilyInformationDto
    {
        public int NumberOfPeopleInHousehold { get; set; }
        public IncomeRangeEnum YearlyHouseholdIncomeRange { get; set; }
        public EducationalDegreesEnum HighestAttainedDegreeInHome { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
