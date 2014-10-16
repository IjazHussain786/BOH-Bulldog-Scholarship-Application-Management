using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.AzureStorage.TableStorage.Interfaces.LettersOfRecommendation;
using BohFoundation.Domain.Dtos.Common.AzureQueuryObjects;
using BohFoundation.Domain.Dtos.Reference.Anonymous;
using BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication
{
    [TestClass]
    public class GetLetterOfRecommendationControllerTests
    {
        private GetLetterOfRecommendationController _controller;
        private IAzureLettersOfRecommendationRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            _repository = A.Fake<IAzureLettersOfRecommendationRepository>();
            _controller = new GetLetterOfRecommendationController(_repository);
            AzureDto = new AzureTableStorageEntityKeyDto();
        }

        private AzureTableStorageEntityKeyDto AzureDto { get; set; }

        [TestMethod]
        public void GetLetterOfRecommendationController_Get_Should_Call_Repo_With_Dto()
        {
            Get();
            A.CallTo(() => _repository.GetLetterOfRecommendation(AzureDto)).MustHaveHappened();
        }

        [TestMethod]
        public void GetLetterOfRecommendationController_Get_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _repository.GetLetterOfRecommendation(AzureDto)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Get());
        }

        [TestMethod]
        public void GetLetterOfRecommendationController_Get_Should_Return_Whatever_Repo_Returns()
        {
            var item = new LetterOfRecommendationDto();
            A.CallTo(() => _repository.GetLetterOfRecommendation(AzureDto)).Returns(item);

            var returnedItem = Get() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(item, returnedItem.Content.Data);
        }

        private IHttpActionResult Get()
        {
            return _controller.Get(AzureDto);
        }
    }
}
