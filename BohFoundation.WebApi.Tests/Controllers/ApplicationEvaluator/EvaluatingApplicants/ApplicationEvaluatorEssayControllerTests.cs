using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.AzureStorage.TableStorage.Interfaces.Essay;
using BohFoundation.Domain.Dtos.Applicant.Essay;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate;
using BohFoundation.Domain.Dtos.Common.AzureQueuryObjects;
using BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.ApplicationEvaluator.EvaluatingApplicants
{
    [TestClass]
    public class ApplicationEvaluatorEssayControllerTests
    {
        private IRateApplicationRespository _rateRepo;
        private IAzureEssayRepository _azureEssayRepo;
        private ApplicationEvaluatorEssayController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _rateRepo = A.Fake<IRateApplicationRespository>();
            _azureEssayRepo = A.Fake<IAzureEssayRepository>();

            _controller = new ApplicationEvaluatorEssayController(_azureEssayRepo, _rateRepo);

            AzureDto = new AzureTableStorageEntityKeyDto();
            EssayRatingDto = new EssayRatingDto();
        }

        private EssayRatingDto EssayRatingDto { get; set; }
        private AzureTableStorageEntityKeyDto AzureDto { get; set; }

        #region Get

        [TestMethod]
        public void ApplicationEvaluatorEssayController_Get_Should_Call_Repo_Dto()
        {
            Get();
            A.CallTo(() => _azureEssayRepo.GetEssay(AzureDto)).MustHaveHappened();
        }

        [TestMethod]
        public void ApplicationEvaluatorEssayController_Get_Should_Return_Error_On_Exception()
        {
            A.CallTo(() => _azureEssayRepo.GetEssay(AzureDto)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Get());
        }

        [TestMethod]
        public void ApplicationEvaluatorEssayController_Get_Should_Return_EssayDto()
        {
            var item = new EssayDto();
            A.CallTo(() => _azureEssayRepo.GetEssay(AzureDto)).Returns(item);

            var returnedItem = Get() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(item, returnedItem.Content.Data);
        }

        private IHttpActionResult Get()
        {
            return _controller.Get(AzureDto);
        }

        #endregion

        [TestMethod]
        public void ApplicationEvaluatorEssayController_Post_Should_Call_Repo_With_Dto()
        {
            Post();
            A.CallTo(() => _rateRepo.UpsertEssayRating(EssayRatingDto)).MustHaveHappened();
        }

        [TestMethod]
        public void ApplicationEvaluatorEssayController_Post_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _rateRepo.UpsertEssayRating(EssayRatingDto)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Post());
        }

        [TestMethod]
        public void ApplicationEvaluatorEssayController_Post_Should_Return_Ok_On_HappyPath()
        {
            WebApiCommonAsserts.IsOkResult(Post());
        }

        private IHttpActionResult Post()
        {
            return _controller.Post(EssayRatingDto);
        }

    }
}
