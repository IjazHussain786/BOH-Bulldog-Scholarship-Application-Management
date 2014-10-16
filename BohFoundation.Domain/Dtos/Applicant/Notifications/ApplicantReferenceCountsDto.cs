using System;

namespace BohFoundation.Domain.Dtos.Applicant.Notifications
{
    public class ApplicantReferenceCountsDto
    {
        public int NumberOfReferenceInvitationsSent { get; set; }
        public int NumberOfReferencesRecieved { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
