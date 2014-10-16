using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.PersonalInformation;
using BohFoundation.WebApi.Controllers.Applicant.PersonalInformation;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Applicant.PersonalInformation
{
    [TestClass]
    public class ApplicantPersonalInformationControllerTests
    {
        private ApplicantPersonalInformationController _applicantPersonalInformationController;
        private IPersonalInformationRepository _applicantRepo;
        private ApplicantPersonalInformationDto PersonalInformationDto { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            _applicantRepo = A.Fake<IPersonalInformationRepository>();
            _applicantPersonalInformationController = new ApplicantPersonalInformationController(_applicantRepo);


            PersonalInformationDto = new ApplicantPersonalInformationDto{Birthdate = new DateTime(1984,7,6,5,6,4,5)};
        }

        #region Get

        [TestMethod]
        public void ApplicantPersonInformationController_Get_Should_Call_GetPersonalInformation()
        {
            GetPersonalInformation();
            A.CallTo(() => _applicantRepo.GetPersonalInformation()).MustHaveHappened();
        }

        [TestMethod]
        public void ApplicantPersonInformationController_Get_Should_ReturnInternalServerError_OnException()
        {
            A.CallTo(() => _applicantRepo.GetPersonalInformation()).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(GetPersonalInformation());
        }

        [TestMethod]
        public void ApplicantPersonInformationController_Get_Should_Ok_WithNullPackaged()
        {
            A.CallTo(() => _applicantRepo.GetPersonalInformation()).Returns(null);
            var result = GetPersonalInformation() as OkNegotiatedContentResult<ServerMessage>;
            Assert.IsNull(result.Content.Data);
        }

        [TestMethod]
        public void ApplicantPersonInformationController_Get_Should_Ok_WithPackaged()
        {
            var package = new ApplicantPersonalInformationDto();

            A.CallTo(() => _applicantRepo.GetPersonalInformation()).Returns(package);
            var result = GetPersonalInformation() as OkNegotiatedContentResult<ServerMessage>;
            Assert.IsInstanceOfType(result.Content.Data, typeof (ApplicantPersonalInformationDto));
        }

        private IHttpActionResult GetPersonalInformation()
        {
            return _applicantPersonalInformationController.Get();
        }

        #endregion

        #region Post

        [TestMethod]
        public void ApplicantPersonInformationController_Post_Should_Call_UpsertPersonalInformation()
        {
            PostPersonalInformation();
            A.CallTo(() => _applicantRepo.UpsertPersonalInformation(PersonalInformationDto)).MustHaveHappened();
        }

        [TestMethod]
        public void ApplicantPersonInformationController_Post_Should_Return_OK_OnHappyPather()
        {
            WebApiCommonAsserts.IsOkResult(PostPersonalInformation());
        }

        [TestMethod]
        public void ApplicantPersonInformationController_Post_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _applicantRepo.UpsertPersonalInformation(A<ApplicantPersonalInformationDto>.Ignored))
                .Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(PostPersonalInformation());
        }

        private IHttpActionResult PostPersonalInformation()
        {
            return _applicantPersonalInformationController.Post(PersonalInformationDto);
        }

        #endregion
    }
}
