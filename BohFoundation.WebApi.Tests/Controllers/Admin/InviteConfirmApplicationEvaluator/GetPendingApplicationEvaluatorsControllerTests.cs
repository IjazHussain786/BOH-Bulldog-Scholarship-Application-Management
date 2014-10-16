using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.MembershipProvider.UserManagement.Admin.Interfaces;
using BohFoundation.WebApi.Controllers.Admin.InviteConfirmApplicationEvaluator;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Admin.InviteConfirmApplicationEvaluator
{
    [TestClass]
    public class GetPendingApplicationEvaluatorsControllerTests
    {
        private GetPendingApplicationEvaluatorsController _getPendingApplicationEvaluatorsController;
        private IConfirmApplicationEvaluator _confirmApplicationEvaluator;

        [TestInitialize]
        public void Initialize()
        {
            _confirmApplicationEvaluator = A.Fake<IConfirmApplicationEvaluator>();
            
            _getPendingApplicationEvaluatorsController = new GetPendingApplicationEvaluatorsController(_confirmApplicationEvaluator);

        }

        [TestMethod]
        public void GetPendingApplicationEvaluatorsController_Get_Should_Call_GetPendingApplicationEvaluators()
        {
            GetPendingApplicationEvaluators();
            A.CallTo(() => _confirmApplicationEvaluator.GetPendingApplicationEvaluators()).MustHaveHappened();
        }

        [TestMethod]
        public void GetPendingApplicationEvaluatorsController_Get_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _confirmApplicationEvaluator.GetPendingApplicationEvaluators()).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(GetPendingApplicationEvaluators());
        }

        [TestMethod]
        public void GetPendingApplicationEvaluatorsController_Get_Should_Return_Ok_WithInformation()
        {
            var personList = new List<PersonDto>();
            A.CallTo(() => _confirmApplicationEvaluator.GetPendingApplicationEvaluators()).Returns(personList);
            var result = GetPendingApplicationEvaluators() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(personList, result.Content.Data);
        }

        private IHttpActionResult GetPendingApplicationEvaluators()
        {
            return _getPendingApplicationEvaluatorsController.Get();
        }
    }
}
