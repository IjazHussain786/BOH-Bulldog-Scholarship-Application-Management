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
    public class GetCollectionOfEssaysAndLettersOfRecommendationsControllerTests
    {
        private IGetCompletedApplicationRepository _getCompletedApplicationRepo;
        private GetCollectionOfEssaysAndLettersOfRecommendationsController _controller;
        private Guid ApplicantsGuid { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            _getCompletedApplicationRepo = A.Fake<IGetCompletedApplicationRepository>();
            _controller = new GetCollectionOfEssaysAndLettersOfRecommendationsController(_getCompletedApplicationRepo);
            ApplicantsGuid = Guid.NewGuid();
        }

        [TestMethod]
        public void GetCollectionOfEssaysAndLettersOfRecommendationsController_Get_Should_Call_Repo_With_Guid()
        {
            Get();
            A.CallTo(() => _getCompletedApplicationRepo.GetCollectionsOfEssaysAndLettersOfRecommendation(ApplicantsGuid))
                .MustHaveHappened();
        }

        [TestMethod]
        public void GetCollectionOfEssaysAndLettersOfRecommendationsController_Get_Should_Return_InternalExceptionError_On_Exception()
        {
            A.CallTo(() => _getCompletedApplicationRepo.GetCollectionsOfEssaysAndLettersOfRecommendation(ApplicantsGuid))
                .Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Get());
        }

        [TestMethod]
        public void GetCollectionOfEssaysAndLettersOfRecommendationsController_Get_Should_Return_Whatever_Is_Returned_From_Repo()
        {
            var item = new CollectionsOfEssaysAndLettersOfRecommendationDto();

            A.CallTo(() => _getCompletedApplicationRepo.GetCollectionsOfEssaysAndLettersOfRecommendation(ApplicantsGuid))
                .Returns(item);

            var returnedItem = Get() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(item, returnedItem.Content.Data);
        }

        private IHttpActionResult Get()
        {
            return _controller.Get(ApplicantsGuid);
        }
    }
}
