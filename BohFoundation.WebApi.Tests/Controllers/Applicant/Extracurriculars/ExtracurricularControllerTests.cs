using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Extracurricular;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Family;
using BohFoundation.WebApi.Controllers.Applicant.Extracurriculars;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Applicant.Extracurriculars
{
    [TestClass]
    public class ExtracurricularControllerTests
    {
        private IExtracurricularActivitiesRepository _repo;
        private ExtracurricularController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _repo = A.Fake<IExtracurricularActivitiesRepository>();
            _controller = new ExtracurricularController(_repo);
            ExtracurricularDto = new ExtracurricularActivitiesDto();
        }

        private ExtracurricularActivitiesDto ExtracurricularDto { get; set; }

        #region Get

        [TestMethod]
        public void ExtracurricularController_Get_Should_Call_Repo()
        {
            Get();
            A.CallTo(() => _repo.GetExtracurricularActivities()).MustHaveHappened();
        }

        [TestMethod]
        public void ExtracurricularController_Get_Should_Return_InternlServerError_OnException()
        {
            A.CallTo(() => _repo.GetExtracurricularActivities()).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Get());
        }

        [TestMethod]
        public void ExtracurricularController_Get_Should_Return_Ok_WithObject_OnHappyPath()
        {
            A.CallTo(() => _repo.GetExtracurricularActivities()).Returns(ExtracurricularDto);
            var result = Get() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(ExtracurricularDto, result.Content.Data);
        }

        private IHttpActionResult Get()
        {
            return _controller.Get();
        }

        #endregion

        [TestMethod]
        public void ExtracurricularController_Post_Should_Call_Repo()
        {
            Post();
            A.CallTo(() => _repo.UpsertExtracurricularActivities(ExtracurricularDto)).MustHaveHappened();
        }

        [TestMethod]
        public void ExtracurricularController_Post_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _repo.UpsertExtracurricularActivities(ExtracurricularDto)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Post());
        }

        [TestMethod]
        public void ExtracurricularController_Post_Should_Return_Ok_OnHappyPath()
        {
            WebApiCommonAsserts.IsOkResult(Post());
        }

        private IHttpActionResult Post()
        {
            return _controller.Post(ExtracurricularDto);
        }
    }
}
