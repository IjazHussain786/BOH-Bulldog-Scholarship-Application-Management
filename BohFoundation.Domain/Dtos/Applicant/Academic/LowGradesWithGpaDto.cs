using System.Collections.Generic;

namespace BohFoundation.Domain.Dtos.Applicant.Academic
{
    public class LowGradesWithGpaDto
    {
        public ICollection<LowGradeDto> LowGrades { get; set; }
        public double Gpa { get; set; }
    }
}
