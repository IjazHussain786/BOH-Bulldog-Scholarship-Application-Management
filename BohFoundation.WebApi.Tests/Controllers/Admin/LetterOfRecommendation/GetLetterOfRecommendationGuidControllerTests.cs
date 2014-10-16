using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.AdminsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Admin.References;
using BohFoundation.WebApi.Controllers.Admin.LetterOfRecommendation;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Admin.LetterOfRecommendation
{
    [TestClass]
    public class GetLetterOfRecommendationGuidControllerTests
    {
        private IGetLetterOfRecommendationGuidRepository _getLetterOFRecommendationGuidRepo;
        private GetLetterOfRecommendationGuidController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _getLetterOFRecommendationGuidRepo = A.Fake<IGetLetterOfRecommendationGuidRepository>();
            _controller = new GetLetterOfRecommendationGuidController(_getLetterOFRecommendationGuidRepo);

            ApplicantEmail = "appemail";
            ReferenceEmail = "referemail";
        }

        private string ReferenceEmail { get; set; }
        private string ApplicantEmail { get; set; }

        [TestMethod]
        public void GetLetterOfRecommendationGuidController_Post_Should_Call_Repo()
        {
            Get();
            A.CallTo(() => _getLetterOFRecommendationGuidRepo.GetLetterOfRecommendationGuid(A<GetLetterOfRecommendationGuidDto>.That.Matches(
                LetterOfRecommendation => LetterOfRecommendation.ApplicantsEmailAddress == ApplicantEmail && 
                    LetterOfRecommendation.ReferencesEmailAddress == ReferenceEmail))).MustHaveHappened();
        }

        [TestMethod]
        public void GetLetterOfRecommendationGuidController_Post_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _getLetterOFRecommendationGuidRepo.GetLetterOfRecommendationGuid(A<GetLetterOfRecommendationGuidDto>.Ignored)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Get());
        }

        [TestMethod]
        public void GetLetterOfRecommendationGuidController_Post_Should_Return_GuidSentToReferenceDto()
        {
            var returnDto = new GuidSentToReferenceDto();
            A.CallTo(() => _getLetterOFRecommendationGuidRepo.GetLetterOfRecommendationGuid(A<GetLetterOfRecommendationGuidDto>.Ignored)).Returns(returnDto);

            var result = Get() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(returnDto, result.Content.Data);
        }

        private IHttpActionResult Get()
        {
            return _controller.Post(new GetLetterOfRecommendationGuidDto{ApplicantsEmailAddress = ApplicantEmail, ReferencesEmailAddress = ReferenceEmail});
        }
    }
}
