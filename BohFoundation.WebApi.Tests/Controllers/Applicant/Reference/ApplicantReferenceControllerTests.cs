using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.Domain.Dtos.Applicant.References;
using BohFoundation.MiddleTier.ApplicantsOrchestration.Interfaces;
using BohFoundation.WebApi.Controllers.Applicant.References;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Applicant.Reference
{
    [TestClass]
    public class ApplicantReferenceControllerTests
    {
        private IApplicantReferenceOrchestration _applicantReference;
        private ApplicantReferenceController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _applicantReference = A.Fake<IApplicantReferenceOrchestration>();
            _controller = new ApplicantReferenceController(_applicantReference);

            References = new List<ReferenceDto>();

            ApplicantReferenceDto = new ApplicantReferenceInputDto();

            A.CallTo(() => _applicantReference.GetReferences()).Returns(References);
        }

        private ApplicantReferenceInputDto ApplicantReferenceDto { get; set; }
        private List<ReferenceDto> References { get; set; }

        #region Get

        [TestMethod]
        public void ApplicantReferenceController_Get_Should_Return_Whatever_ApplicantReferenceComposition_returns()
        {
            var result = Get() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(References, result.Content.Data);
        }

        [TestMethod]
        public void ApplicantReferenceController_Get_Should_Return_OnException_InternalServerError()
        {
            A.CallTo(() => _applicantReference.GetReferences()).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Get());
        }

        [TestMethod]
        public void ApplicantReferenceController_Get_Should_Call_ApplicantReferenceComposition()
        {
            Get();
            A.CallTo(() => _applicantReference.GetReferences()).MustHaveHappened(Repeated.Exactly.Once);
        }

        private IHttpActionResult Get()
        {
            return _controller.Get();
        }

        #endregion

        #region Post

        [TestMethod]
        public void ApplicantReferenceController_Post_Should_Call_ApplicantReferenceComposition()
        {
            Post();
            A.CallTo(() => _applicantReference.AddReference(ApplicantReferenceDto))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void ApplicantReferenceController_Post_Error_Should_return_InternalError()
        {
            A.CallTo(() => _applicantReference.AddReference(ApplicantReferenceDto)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Post());
        }

        [TestMethod]
        public void ApplicantReferenceController_Post_HappyPath_Should_return_Ok()
        {
            WebApiCommonAsserts.IsOkResult(Post());
        }

        private IHttpActionResult Post()
        {
            return _controller.Post(ApplicantReferenceDto);
        }

        #endregion

    }
}
