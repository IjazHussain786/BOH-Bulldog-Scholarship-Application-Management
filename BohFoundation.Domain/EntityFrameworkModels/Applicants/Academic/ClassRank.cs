using System;

namespace BohFoundation.Domain.EntityFrameworkModels.Applicants.Academic
{
    public class ClassRank
    {
        public int Id { get; set; }
        public int ClassNumericalRank { get; set; }
        public int GraduatingClassSize { get; set; }

        public DateTime LastUpdated { get; set; }

        public virtual AcademicInformation AcademicInformation { get; set; }
    }
}
