using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.Domain.Dtos.Reference.Anonymous;
using BohFoundation.ReferencesRepository.Repositories.Interfaces;
using BohFoundation.WebApi.Controllers.Reference.Anonymous;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Reference.Anonymous
{
    [TestClass]
    public class AnonymousLetterOfRecommendationInformationControllerTests
    {
        private IAnonymousLetterOfRecommendationRepository _letterOfRecommendationRepository;
        private AnonymousLetterOfRecommendationInformationController _controller;
        private GuidForLetterOfRecommendationDto GuidDto { get; set; }
        private ReferencePersonalInformationDto ReferencePersonalInformationDto { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            _letterOfRecommendationRepository = A.Fake<IAnonymousLetterOfRecommendationRepository>();
            _controller = new AnonymousLetterOfRecommendationInformationController(_letterOfRecommendationRepository);

            Guid = Guid.NewGuid();
            GuidDto = new GuidForLetterOfRecommendationDto();
            ReferencePersonalInformationDto = new ReferencePersonalInformationDto();
        }

        private Guid Guid { get; set; }

        #region Get

        [TestMethod]
        public void AnonymousLetterOfRecommendationInformationController_Get_Should_Call_GetInformationForForm()
        {
            Get();
            A.CallTo(() => _letterOfRecommendationRepository.GetInformationForReferenceForm(A<GuidForLetterOfRecommendationDto>.That.Matches(guid => guid.GuidSentToReference == Guid))).MustHaveHappened();
        }

        [TestMethod]
        public void AnonymousLetterOfRecommendationInformationController_Get_Should_HappyPath_Should_Return_Object()
        {
            var objectToReturn = new InformationForReferenceFormDto();
            A.CallTo(() => _letterOfRecommendationRepository.GetInformationForReferenceForm(A<GuidForLetterOfRecommendationDto>.That.Matches(guid => guid.GuidSentToReference == Guid)))
                .Returns(objectToReturn);
            var result = Get() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(objectToReturn, result.Content.Data);
        }

        [TestMethod]
        public void
            AnonymousLetterOfRecommendationInformationController_Get_Should_Return_InternalServerError_On_Exception()
        {
            A.CallTo(() => _letterOfRecommendationRepository.GetInformationForReferenceForm(A<GuidForLetterOfRecommendationDto>.Ignored))
                .Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Get());
        }

        private IHttpActionResult Get()
        {
            return _controller.Get(Guid);
        }

        #endregion

        [TestMethod]
        public void AnonymousLetterOfRecommendationInformationController_Post_Should_Call_Upsert()
        {
            Post();
            A.CallTo(() => _letterOfRecommendationRepository.UpsertReferencesPersonalInformation(ReferencePersonalInformationDto)).MustHaveHappened();
        }

        [TestMethod]
        public void AnonymousLetterOfRecommendationInformationController_Post_Should_HappyPath_Should_Return_Ok()
        {
            WebApiCommonAsserts.IsOkResult(Post());
        }

        [TestMethod]
        public void
            AnonymousLetterOfRecommendationInformationController_Post_Should_Return_InternalServerError_On_Exception()
        {
            A.CallTo(() => _letterOfRecommendationRepository.UpsertReferencesPersonalInformation(ReferencePersonalInformationDto))
                .Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Post());
        }

        private IHttpActionResult Post()
        {
            return _controller.Post(ReferencePersonalInformationDto);
        }

    }
}
