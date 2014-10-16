using System;
using System.Collections.Generic;

namespace BohFoundation.Domain.EntityFrameworkModels.Applicants.Academic
{
    public class AcademicInformation
    {
        
        public int Id { get; set; }
        public double Gpa { get; set; }
        public string CareerChoice { get; set; }
        public string ProbableNextSchool { get; set; }
        public DateTime? LastUpdated { get; set; }
        
        public virtual ICollection<LowGrade> LowGrades { get; set; }

        public virtual ClassRank ClassRank { get; set; }
        public virtual Applicant Applicant { get; set; }
        public virtual TranscriptBlobReference Transcript { get; set; }
    }
}