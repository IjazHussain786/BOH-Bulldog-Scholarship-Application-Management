using System;
using BohFoundation.Domain.Enums;

namespace BohFoundation.Domain.Dtos.Applicant.Academic
{
    public class LowGradeDto
    {
        public string Grade { get; set; }
        public string ClassTitle { get; set; }
        public bool WasAp { get; set; }
        public YearOfHighSchoolEnum YearOfHighSchool { get; set; }
        public string Explanation { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
