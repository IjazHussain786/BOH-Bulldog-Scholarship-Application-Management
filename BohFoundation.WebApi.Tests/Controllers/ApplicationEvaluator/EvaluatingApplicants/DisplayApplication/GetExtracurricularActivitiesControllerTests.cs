using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Extracurricular;
using BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication
{
    [TestClass]
    public class GetExtracurricularActivitiesControllerTests
    {
        private IGetCompletedApplicationRepository _getCompletedApplicationRepo;
        private GetExtracurricularActivitiesController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _getCompletedApplicationRepo = A.Fake<IGetCompletedApplicationRepository>();
            _controller = new GetExtracurricularActivitiesController(_getCompletedApplicationRepo);
            ApplicantsGuid = Guid.NewGuid();
        }

        private Guid ApplicantsGuid { get; set; }

        [TestMethod]
        public void GetExtracurricularActivitiesController_Get_Should_Call_Repo()
        {
            Get();
            A.CallTo(() => _getCompletedApplicationRepo.GetExtracurricularActivities(ApplicantsGuid)).MustHaveHappened();
        }

        [TestMethod]
        public void GetExtracurricularActivitiesController_Get_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _getCompletedApplicationRepo.GetExtracurricularActivities(ApplicantsGuid))
                .Throws(new Exception());

            WebApiCommonAsserts.IsInternalServerError(Get());
        }

        [TestMethod]
        public void GetExtracurricularActivitiesController_Get_Should_Return_Whatever_The_Repo_Returns()
        {
            var item = new ExtracurricularActivitiesDto();
            A.CallTo(() => _getCompletedApplicationRepo.GetExtracurricularActivities(ApplicantsGuid)).Returns(item);

            var returnedItem = Get() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(item, returnedItem.Content.Data);
        }

        private IHttpActionResult Get()
        {
            return _controller.Get(ApplicantsGuid);
        }
    }
}
