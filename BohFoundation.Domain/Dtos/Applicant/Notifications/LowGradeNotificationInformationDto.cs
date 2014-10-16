using System;

namespace BohFoundation.Domain.Dtos.Applicant.Notifications
{
    public class LowGradeNotificationInformationDto
    {
        public double? Gpa { get; set; }
        public int NumberOfLowGradesInformationSaved { get; set; }
        public DateTime? LastUpdatedLowGrade { get; set; }
    }
}
