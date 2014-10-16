using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication;
using BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication
{
    [TestClass]
    public class GetGeneralInformationControllerTests
    {
        private IGetCompletedApplicationRepository _getCompletedApplicationRepo;
        private GetGeneralInformationController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _getCompletedApplicationRepo = A.Fake<IGetCompletedApplicationRepository>();
            _controller = new GetGeneralInformationController(_getCompletedApplicationRepo);
            ApplicantsGuid = Guid.NewGuid();
        }

        private Guid ApplicantsGuid { get; set; }

        [TestMethod]
        public void GetGeneralInformationController_Get_Should_Call_With_ApplicantsGuid()
        {
            Get();
            A.CallTo(() => _getCompletedApplicationRepo.GetGeneralInformation(ApplicantsGuid)).MustHaveHappened();
        }

        [TestMethod]
        public void GetGeneralInformationController_Get_Should_Return_With_InternalExceptionError_When_Excpetion()
        {
            A.CallTo(() => _getCompletedApplicationRepo.GetGeneralInformation(ApplicantsGuid)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Get());
        }

        [TestMethod]
        public void GetGeneralInformationController_Get_Should_Return_With_Whatever_The_Repo_Returns()
        {
            var item = new ApplicantsGeneralInformationDto();

            A.CallTo(() => _getCompletedApplicationRepo.GetGeneralInformation(ApplicantsGuid)).Returns(item);

            var returnedItem = Get() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(item, returnedItem.Content.Data);
        }

        private IHttpActionResult Get()
        {
            return _controller.Get(ApplicantsGuid);
        }
    }
}
