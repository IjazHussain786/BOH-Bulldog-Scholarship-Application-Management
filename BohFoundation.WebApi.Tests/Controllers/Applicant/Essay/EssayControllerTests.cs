using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Essay;
using BohFoundation.WebApi.Controllers.Applicant.Essay;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Applicant.Essay
{
    [TestClass]
    public class EssayControllerTests
    {
        private IApplicantsEssayRepository _applicantsEssayRepo;
        private EssayController _essayController;

        [TestInitialize]
        public void Initialize()
        {
            _applicantsEssayRepo = A.Fake<IApplicantsEssayRepository>();
            _essayController = new EssayController(_applicantsEssayRepo);

            Id = 1290903;
            EssayDto = new EssayDto();
        }

        private EssayDto EssayDto { get; set; }
        private int Id { get; set; }

        #region Get

        [TestMethod]
        public void EssayController_Get_Should_Call_GetEssay()
        {
            Get();
            A.CallTo(() => _applicantsEssayRepo.GetEssay(Id)).MustHaveHappened();
        }

        [TestMethod]
        public void EssayController_Get_Should_Return_InternalError_OnException()
        {
            A.CallTo(() => _applicantsEssayRepo.GetEssay(Id)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Get());
        }

        [TestMethod]
        public void EssayController_Get_Should_Return_Ok_With_Data_OnGood()
        {
            var essayDto = new EssayDto();
            A.CallTo(() => _applicantsEssayRepo.GetEssay(Id)).Returns(essayDto);
            var result = Get() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(essayDto, result.Content.Data);
        }

        private IHttpActionResult Get()
        {
            return _essayController.Get(Id);
        }

        #endregion

        [TestMethod]
        public void EssayController_Post_Should_Call_UpsertEssay()
        {
            Post();
            A.CallTo(() => _applicantsEssayRepo.UpsertEssay(EssayDto)).MustHaveHappened();
        }

        [TestMethod]
        public void EssayController_Post_Should_EverythingGoesWell_Ok()
        {
            WebApiCommonAsserts.IsOkResult(Post());
        }

        [TestMethod]
        public void EssayController_Post_Should_Exception_InternalServerError()
        {
            A.CallTo(() => _applicantsEssayRepo.UpsertEssay(EssayDto)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Post());
        }

        private IHttpActionResult Post()
        {
            return _essayController.Post(EssayDto);
        }
    }
}
