using System;
using System.Collections.Generic;

namespace BohFoundation.Domain.Dtos.Applicant.Notifications
{
    public class ApplicantNotificationsDto
    {
        public DateTime? LastUpdatedPersonalInformation { get; set; }
        public DateTime? LastUpdatedContactInformation { get; set; }
        public DateTime? LastUpdatedTranscriptUpload { get; set; }
        public DateTime? LastUpdatedAcademicInformation { get; set; }
        public DateTime? LastUpdatedFamilyInformation { get; set; }
        public DateTime? LastUpdatedExtracurriculars { get; set; }
        public LowGradeNotificationInformationDto LowGradeNotificationInformation { get; set; }
        public ICollection<EssayNotificationsDto> EssayNotifications { get; set; }
        public ApplicantReferenceCountsDto ApplicantReferenceCounts { get; set; }
        public DateTime DeadlineDate { get; set; }
    }
}
