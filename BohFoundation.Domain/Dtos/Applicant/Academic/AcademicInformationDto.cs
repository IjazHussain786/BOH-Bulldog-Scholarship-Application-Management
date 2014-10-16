using System;

namespace BohFoundation.Domain.Dtos.Applicant.Academic
{
    public class AcademicInformationDto
    {
        public double Gpa { get; set; }
        public string CareerChoice { get; set; }
        public string ProbableNextSchool { get; set; }
        public DateTime? LastUpdated { get; set; }

        public ClassRankDto ClassRank { get; set; }
    }
}
