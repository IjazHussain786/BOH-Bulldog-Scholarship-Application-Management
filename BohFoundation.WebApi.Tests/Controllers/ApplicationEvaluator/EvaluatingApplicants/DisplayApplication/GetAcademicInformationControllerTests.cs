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
    public class GetAcademicInformationControllerTests
    {
        private IGetCompletedApplicationRepository _getCompletedApplicationRepo;
        private GetAcademicInformationController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _getCompletedApplicationRepo = A.Fake<IGetCompletedApplicationRepository>();
            _controller = new GetAcademicInformationController(_getCompletedApplicationRepo);
            ApplicantsGuid = Guid.NewGuid();
        }

        private Guid ApplicantsGuid { get; set; }

        [TestMethod]
        public void GetAcademicInformationController_Get_Should_Call_Repo_With_ApplicantsGuid()
        {
            Get();
            A.CallTo(() => _getCompletedApplicationRepo.GetAcademicInformation(ApplicantsGuid)).MustHaveHappened();
        }

        [TestMethod]
        public void GetAcademicInformationController_Get_Should_Return_InternalExceptionError_On_Exception()
        {
            A.CallTo(() => _getCompletedApplicationRepo.GetAcademicInformation(ApplicantsGuid)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Get());
        }

        [TestMethod]
        public void GetAcademicInformationController_Get_Should_Return_Whatever_Is_Returned_From_Repo()
        {
            var item = new CompletedAcademicInformationDto();
            A.CallTo(() => _getCompletedApplicationRepo.GetAcademicInformation(ApplicantsGuid)).Returns(item);

            var returnedItem = Get() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(item, returnedItem.Content.Data);
        }

        private IHttpActionResult Get()
        {
            return _controller.Get(ApplicantsGuid);
        }
    }
}
