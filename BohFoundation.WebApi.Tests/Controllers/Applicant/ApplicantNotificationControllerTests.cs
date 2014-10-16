using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant;
using BohFoundation.Domain.Dtos.Applicant.Notifications;
using BohFoundation.WebApi.Controllers.Applicant;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Applicant
{
    [TestClass]
    public class ApplicantNotificationControllerTests
    {
        private IApplicantsNotificationRepository _applicantsNotificationRepository;
        private ApplicantNotificationController _applicantNotificationController;
        
        [TestInitialize]
        public void Initialize()
        {
            _applicantsNotificationRepository = A.Fake<IApplicantsNotificationRepository>();
            _applicantNotificationController = new ApplicantNotificationController(_applicantsNotificationRepository);
        }

        [TestMethod]
        public void ApplicantNotificationController_Get_Should_Call_NotificationRepo()
        {
            GetNotifications();
            A.CallTo(() => _applicantsNotificationRepository.GetApplicantNotifications()).MustHaveHappened();
        }

        [TestMethod]
        public void ApplicantNotificationController_Get_Exception_ReturnsInternalExceptionResponse()
        {
            A.CallTo(() => _applicantsNotificationRepository.GetApplicantNotifications()).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(GetNotifications());
        }

        [TestMethod]
        public void ApplicantNotificationController_Get_HappyPath_ReturnsObject()
        {
            var now = DateTime.UtcNow;
            var dto = new ApplicantNotificationsDto{LastUpdatedPersonalInformation = now};
            A.CallTo(() => _applicantsNotificationRepository.GetApplicantNotifications()).Returns(dto);
            var result = GetNotifications();
            var resultContent = (result as OkNegotiatedContentResult<ApplicantNotificationsDto>).Content;
            Assert.AreEqual(now, resultContent.LastUpdatedPersonalInformation);
        }

        private IHttpActionResult GetNotifications()
        {
            return _applicantNotificationController.Get();
        }
    }
}
