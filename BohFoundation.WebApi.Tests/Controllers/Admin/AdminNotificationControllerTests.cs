using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.Domain.Dtos;
using BohFoundation.Domain.Dtos.Admin;
using BohFoundation.MembershipProvider.UserManagement.Admin.Interfaces;
using BohFoundation.WebApi.Controllers.Admin;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Admin
{
    [TestClass]
    public class AdminNotificationControllerTests
    {
        private AdminNotificationController _adminNotificationController;
        private IConfirmApplicationEvaluator _confirmApplicationEvaluator;

        [TestInitialize]
        public void Initialize()
        {
            _confirmApplicationEvaluator = A.Fake<IConfirmApplicationEvaluator>();
            _adminNotificationController = new AdminNotificationController(_confirmApplicationEvaluator);
        }

        [TestMethod]
        public void AdminNotificationController_Post_Should_Call_CountOfPendingApplicationEvaluators()
        {
            GetNotifications();
            A.CallTo(() => _confirmApplicationEvaluator.CountOfPendingApplicationEvaluators()).MustHaveHappened();
        }
        
        [TestMethod]
        public void AdminNotificationController_Post_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _confirmApplicationEvaluator.CountOfPendingApplicationEvaluators()).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(GetNotifications());
        }
        
        [TestMethod]
        public void AdminNotificationController_Post_Should_Return_AdminNotificationDto_WithRightData()
        {
            var count = 91;
            A.CallTo(() => _confirmApplicationEvaluator.CountOfPendingApplicationEvaluators()).Returns(count);
            var result = GetNotifications() as OkNegotiatedContentResult<AdminNotificationCountsDto>;
            Assert.AreEqual(count, result.Content.PendingApplicationEvaluators);
            Assert.AreEqual(count, result.Content.TotalCount);
        }

        private IHttpActionResult GetNotifications()
        {
            return _adminNotificationController.Get();
        }
    }
}
