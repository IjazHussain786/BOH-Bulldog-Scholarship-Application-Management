using System;
using BohFoundation.Domain.EntityFrameworkModels.Common;

namespace BohFoundation.Domain.EntityFrameworkModels.Applicants
{
    public class ApplicantPersonalInformation
    {
        public int Id { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual Applicant Applicant { get; set; }
        public virtual GraduatingClass GraduatingClass { get; set; }
    }
}