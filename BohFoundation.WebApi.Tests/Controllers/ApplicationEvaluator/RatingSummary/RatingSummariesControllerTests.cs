using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.RatingSummary;
using BohFoundation.WebApi.Controllers.ApplicationEvaluator.RatingSummary;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.ApplicationEvaluator.RatingSummary
{
    [TestClass]
    public class RatingSummariesControllerTests
    {
        private IGetTopRatedApplicantsRepository _getTopRatedApplicantsRepository;
        private RatingSummariesController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _getTopRatedApplicantsRepository = A.Fake<IGetTopRatedApplicantsRepository>();
            _controller = new RatingSummariesController(_getTopRatedApplicantsRepository);

            Dto = new Top5ApplicantsDto();
        }

        private Top5ApplicantsDto Dto { get; set; }

        [TestMethod]
        public void RatingSummariesController_Get_Should_Call_Repo()
        {
            Get();
            A.CallTo(() => _getTopRatedApplicantsRepository.GetTop5Applicants(DateTime.UtcNow.Year)).MustHaveHappened();
        }

        [TestMethod]
        public void RatingSummariesController_Get_Should_Return_Data_On_Happy_Path()
        {
            A.CallTo(() => _getTopRatedApplicantsRepository.GetTop5Applicants(DateTime.UtcNow.Year)).Returns(Dto);
            var result = Get() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(Dto, result.Content.Data);
        }

        [TestMethod]
        public void RatingSummariesController_Get_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _getTopRatedApplicantsRepository.GetTop5Applicants(DateTime.UtcNow.Year))
                .Throws(new Exception());

            WebApiCommonAsserts.IsInternalServerError(Get());
        }

        private IHttpActionResult Get()
        {
            return _controller.Get();
        }
    }
}
