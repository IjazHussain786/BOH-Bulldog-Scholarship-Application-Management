using System;
using BohFoundation.Domain.Enums;

namespace BohFoundation.Domain.EntityFrameworkModels.Applicants.Academic
{
    public class LowGrade
    {
        public int Id { get; set; }
        public string Grade { get; set; }
        public string ClassTitle { get; set; }
        public bool WasAp { get; set; }
        public YearOfHighSchoolEnum YearOfHighSchool { get; set; }
        public string Explanation { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual AcademicInformation AcademicInformation { get; set; }
    }
}