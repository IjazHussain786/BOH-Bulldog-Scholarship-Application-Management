using System;
using System.Web.Http;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate;
using BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants.RateApplicant;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.ApplicationEvaluator.EvaluatingApplicants.RateApplicant
{
    [TestClass]
    public class FirstImpressionsRatingControllerTests
    {
        private IRateApplicationRespository _ratingRepo;
        private FirstImpressionsRatingController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _ratingRepo = A.Fake<IRateApplicationRespository>();
            _controller = new FirstImpressionsRatingController(_ratingRepo);

            RatingDto = new RatingWithApplicantsGuidDto();
        }

        private RatingWithApplicantsGuidDto RatingDto { get; set; }

        [TestMethod]
        public void FirstImpressionsRatingController_Post_Should_Call_Repo_With_Dto()
        {
            Post();
            A.CallTo(() => _ratingRepo.UpsertFirstImpression(RatingDto)).MustHaveHappened();
        }

        [TestMethod]
        public void FirstImpressionsRatingController_Post_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _ratingRepo.UpsertFirstImpression(RatingDto)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Post());
        }

        [TestMethod]
        public void FirstImpressionsRatingController_Post_Should_Return_Ok_On_HappyPath()
        {
            WebApiCommonAsserts.IsOkResult(Post());
        }

        private IHttpActionResult Post()
        {
            return _controller.Post(RatingDto);
        }
    }
}
