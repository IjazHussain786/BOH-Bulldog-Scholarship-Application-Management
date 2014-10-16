using System;
using System.Web.Http;
using BohFoundation.Domain.Dtos.Admin;
using BohFoundation.MembershipProvider.UserManagement.Admin.Interfaces;
using BohFoundation.TestHelpers;
using BohFoundation.WebApi.Controllers.Admin.InviteConfirmApplicationEvaluator;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Admin.InviteConfirmApplicationEvaluator
{
    [TestClass]
    public class ConfirmApplicationEvaluatorControllerTests
    {
        private ConfirmApplicationEvaluatorController _confirmApplicationEvaluatorController;
        private IConfirmApplicationEvaluator _confirmApplicationEvaluator;
        private ConfirmApplicationEvaluatorDto Model { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            _confirmApplicationEvaluator = A.Fake<IConfirmApplicationEvaluator>();
            _confirmApplicationEvaluatorController = new ConfirmApplicationEvaluatorController( _confirmApplicationEvaluator);

            Model = new ConfirmApplicationEvaluatorDto{Confirm = true, EmailAddress = TestHelpersCommonFields.Email};
        }

       
        [TestMethod]
        public void AdminNotificationController_Post_Should_Call_ConfirmOrRejectApplicationEvaluator()
        {
            ConfirmApplicationEvaluator();
            A.CallTo(() => _confirmApplicationEvaluator.ConfirmOrRejectApplicationEvaluator(Model)).MustHaveHappened();
        }

        [TestMethod]
        public void AdminNotificationController_Post_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _confirmApplicationEvaluator.ConfirmOrRejectApplicationEvaluator(Model))
                .Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(ConfirmApplicationEvaluator());
        }

        [TestMethod]
        public void AdminNotificationController_Post_Should_Return_OK_HappyPath()
        {
            WebApiCommonAsserts.IsOkResult(ConfirmApplicationEvaluator());
        }

        private IHttpActionResult ConfirmApplicationEvaluator()
        {
            return _confirmApplicationEvaluatorController.Post(Model);
        }


    }
}
