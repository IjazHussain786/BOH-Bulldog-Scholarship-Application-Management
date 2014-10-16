using System;
using System.Web.Http;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Notifications;
using BohFoundation.WebApi.Filters;

namespace BohFoundation.WebApi.Controllers.Applicant
{
    [RequireApplicant]
    [Route("api/applicant/notifications")]
    public class ApplicantNotificationController : ApiController
    {
        private readonly IApplicantsNotificationRepository _notificationRepository;

        public ApplicantNotificationController(IApplicantsNotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public IHttpActionResult Get()
        {
            ApplicantNotificationsDto dto;
            try
            {
                dto = _notificationRepository.GetApplicantNotifications();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok(dto);
        }
    }
}
