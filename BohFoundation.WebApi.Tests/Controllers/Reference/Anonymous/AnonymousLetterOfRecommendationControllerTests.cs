using System;
using System.Web.Http;
using BohFoundation.Domain.Dtos.Reference.Anonymous;
using BohFoundation.MiddleTier.ReferencesOrchestration.Interfaces;
using BohFoundation.WebApi.Controllers.Reference.Anonymous;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Reference.Anonymous
{
    [TestClass]
    public class AnonymousLetterOfRecommendationControllerTests
    {
        private IAnonymousLetterOfRecommendationOrchestration _letterOrRecommendationRepo;
        private AnonymousLetterOfRecommendationController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _letterOrRecommendationRepo = A.Fake<IAnonymousLetterOfRecommendationOrchestration>();
            _controller = new AnonymousLetterOfRecommendationController(_letterOrRecommendationRepo);

            Dto = new LetterOfRecommendationDto();
        }

        private LetterOfRecommendationDto Dto { get; set; }

        [TestMethod]
        public void AnonymousLetterOfRecommendationController_Post_Should_Call_AddLetterOfRecommendation()
        {
            Post();
            A.CallTo(() => _letterOrRecommendationRepo.AddLetterOfRecommendation(Dto)).MustHaveHappened();
        }

        [TestMethod]
        public void AnonymousLetterOfRecommendationController_Post_Should_HappyPath_Ok()
        {
            WebApiCommonAsserts.IsOkResult(Post());
        }

        [TestMethod]
        public void AnonymousLetterOfRecommendationController_Post_Should_Return_InternalServerError_When_Exception_Is_Thrown()
        {
            A.CallTo(() => _letterOrRecommendationRepo.AddLetterOfRecommendation(Dto)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Post());
        }

        private IHttpActionResult Post()
        {
            return _controller.Post(Dto);
        }
    }
}
