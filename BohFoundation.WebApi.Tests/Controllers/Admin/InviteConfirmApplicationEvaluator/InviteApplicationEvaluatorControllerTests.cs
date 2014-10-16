using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.Domain.Dtos.Email;
using BohFoundation.MembershipProvider.UserManagement.Admin.Interfaces;
using BohFoundation.Utilities.Context.Interfaces.Context;
using BohFoundation.WebApi.Controllers.Admin.InviteConfirmApplicationEvaluator;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Admin.InviteConfirmApplicationEvaluator
{
    [TestClass]
    public class InviteApplicationEvaluatorControllerTests
    {
        private InviteApplicationEvaluatorController _confirmApplicationEvaluatorController;
        private IInviteApplicationEvaluator _inviteApplicationEvaluator;
        private SendEmailContactDto SendEmailContactDto { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            _inviteApplicationEvaluator = A.Fake<IInviteApplicationEvaluator>();
            _confirmApplicationEvaluatorController = new InviteApplicationEvaluatorController(_inviteApplicationEvaluator);
            
            SendEmailContactDto = new SendEmailContactDto();
        }
       
        [TestMethod]
        public void InviteApplicationEvaluatorController_Post_Should_Call_SendApplicationEvaluatorInvitation()
        {
            InviteApplicationEvaluator();
            A.CallTo(() => _inviteApplicationEvaluator.SendApplicationEvaluatorInvitation(SendEmailContactDto)).MustHaveHappened();
        }

        [TestMethod]
        public void InviteApplicationEvaluatorController_Post_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _inviteApplicationEvaluator.SendApplicationEvaluatorInvitation(SendEmailContactDto)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(InviteApplicationEvaluator());
        }

        [TestMethod]
        public void InviteApplicationEvaluatorController_Post_Should_Return_Ok_WhenHappyPath()
        {
            WebApiCommonAsserts.IsOkResult(InviteApplicationEvaluator());
        }

        private IHttpActionResult InviteApplicationEvaluator()
        {
            return _confirmApplicationEvaluatorController.Post(SendEmailContactDto);
        }
    }
}
