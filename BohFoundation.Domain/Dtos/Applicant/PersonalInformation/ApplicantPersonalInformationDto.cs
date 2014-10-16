using System;

namespace BohFoundation.Domain.Dtos.Applicant.PersonalInformation
{
    public class ApplicantPersonalInformationDto
    {
        public int GraduatingYear { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
