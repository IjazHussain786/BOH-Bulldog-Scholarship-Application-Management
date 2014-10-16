using BohFoundation.Domain.Dtos.Applicant.Notifications;

namespace BohFoundation.ApplicantsRepository.Repositories.Interfaces
{
    public interface IApplicantsNotificationRepository
    {
       ApplicantNotificationsDto GetApplicantNotifications();
    }
}
