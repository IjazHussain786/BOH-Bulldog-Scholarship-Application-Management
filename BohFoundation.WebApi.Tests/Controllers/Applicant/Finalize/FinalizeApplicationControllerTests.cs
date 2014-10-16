using System;
using System.Web.Http;
using BohFoundation.MiddleTier.ApplicantsOrchestration.Interfaces;
using BohFoundation.WebApi.Controllers.Applicant.Finalize;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Applicant.Finalize
{
    [TestClass]
    public class FinalizeApplicationControllerTests
    {
        private FinalizeApplicationController _controller;

        [TestInitialize]
        public void Initialize()
        {
            FinalizeApplicationOrchestration = A.Fake<IFinalizeApplicationOrchestration>();

            _controller = new FinalizeApplicationController(FinalizeApplicationOrchestration);
        }

        private IFinalizeApplicationOrchestration FinalizeApplicationOrchestration { get; set; }

        [TestMethod]
        public void FinalizeApplicationController_Post_Should_Return_OK_HappyPath()
        {
            WebApiCommonAsserts.IsOkResult(Post());
        }

        [TestMethod]
        public void FinalizeApplicationController_Post_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => FinalizeApplicationOrchestration.FinalizeApplication()).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Post());
        }

        [TestMethod]
        public void FinalizeApplicationController_Post_Should_Call_FinalizeApplication()
        {
            Post();
            A.CallTo(() => FinalizeApplicationOrchestration.FinalizeApplication()).MustHaveHappened();
        }

        private IHttpActionResult Post()
        {
            return _controller.Post();
        }
    }
}
