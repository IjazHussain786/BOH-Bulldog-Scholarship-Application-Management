using System;
using System.Web.Http;
using System.Web.UI;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants.Transcript;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants;

namespace BohFoundation.WebApi.Tests.Controllers.ApplicationEvaluator.EvaluatingApplicants.Transcript
{
    [TestClass]
    public class ConfirmTranscriptControllerTests
    {
        private ConfirmTranscriptController _controller;
        private IConfirmTranscriptRepository _repo;

        [TestInitialize]
        public void Initialize()
        {
            _repo = A.Fake<IConfirmTranscriptRepository>();
            _controller = new ConfirmTranscriptController(_repo);

            ConfirmTranscriptDto = new ConfirmTranscriptDto();
        }

        private ConfirmTranscriptDto ConfirmTranscriptDto { get; set; }

        [TestMethod]
        public void ConfirmTranscriptController_Should_Call_Repo_With_Dto()
        {
            Post();
            A.CallTo(() => _repo.ConfirmTranscript(ConfirmTranscriptDto)).MustHaveHappened();
        }

        [TestMethod]
        public void ConfirmTranscriptController_Should_Throw_InternalServerError_OnException()
        {
            A.CallTo(() => _repo.ConfirmTranscript(ConfirmTranscriptDto)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Post());
        }

        [TestMethod]
        public void ConfirmTranscriptController_HappyPath_Should_Returns_Ok()
        {
            WebApiCommonAsserts.IsOkResult(Post());
        }

        private IHttpActionResult Post()
        {
            return _controller.Post(ConfirmTranscriptDto);
        }
    }
}
