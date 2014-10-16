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
    public class FinalRatingControllerTests
    {
        private FinalRatingController _controller;
        private IRateApplicationRespository _ratingRepo;

        [TestInitialize]
        public void Initialize()
        {
            _ratingRepo = A.Fake<IRateApplicationRespository>();
            _controller = new FinalRatingController(_ratingRepo);

            RatingDto = new RatingWithApplicantsGuidDto();
        }

        private RatingWithApplicantsGuidDto RatingDto { get; set; }

        [TestMethod]
        public void FinalRatingController_Post_Should_Call_Repo()
        {
            Post();
            A.CallTo(() => _ratingRepo.UpsertFinalOverallRating(RatingDto)).MustHaveHappened();
        }

        [TestMethod]
        public void FinalRatingController_Post_Should_Return_Internal_Exception_OnExcpetion()
        {
            A.CallTo(() => _ratingRepo.UpsertFinalOverallRating(RatingDto)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Post());
        }

        [TestMethod]
        public void FinalRatingController_Post_Should_Return_Ok_OnHappyPath()
        {
            WebApiCommonAsserts.IsOkResult(Post());
        }

        private IHttpActionResult Post()
        {
            return _controller.Post(RatingDto);
        }
    }
}
