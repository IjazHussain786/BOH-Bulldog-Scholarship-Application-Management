using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Academic;
using BohFoundation.WebApi.Controllers.Applicant.AcademicInformation;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Applicant.AcademicInformation
{
    [TestClass]
    public class AcademicInformationRepositoryTests
    {
        private IAcademicInformationRepository _academicInformationRepository;
        private AcademicInformationController _academicInformationController;
        private AcademicInformationDto AcademicInformation { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            _academicInformationRepository = A.Fake<IAcademicInformationRepository>();
            _academicInformationController = new AcademicInformationController(_academicInformationRepository);

            AcademicInformation = new AcademicInformationDto();
        }

        #region Get

        [TestMethod]
        public void ApplicantPersonInformationController_Get_Should_Call_GetPersonalInformation()
        {
            GetAcademicInformation();
            A.CallTo(() => _academicInformationRepository.GetAcademicInformation()).MustHaveHappened();
        }

        [TestMethod]
        public void ApplicantPersonInformationController_Get_Should_ReturnInternalServerError_OnException()
        {
            A.CallTo(() => _academicInformationRepository.GetAcademicInformation()).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(GetAcademicInformation());
        }

        [TestMethod]
        public void ApplicantPersonInformationController_Get_Should_Ok_WithNullPackaged()
        {
            var academicInformationDto = new AcademicInformationDto
            {
                Gpa = 0.0,
                LastUpdated = null,
                ClassRank = null,
                ProbableNextSchool = null
            };

            A.CallTo(() => _academicInformationRepository.GetAcademicInformation()).Returns(academicInformationDto);
            var result = GetAcademicInformation() as OkNegotiatedContentResult<ServerMessage>;
            Assert.IsNull(result.Content.Data);
        }

        [TestMethod]
        public void ApplicantPersonInformationController_Get_Should_Ok_WithPackaged()
        {
            var package = new AcademicInformationDto{Gpa = 3.2};

            A.CallTo(() => _academicInformationRepository.GetAcademicInformation()).Returns(package);
            var result = GetAcademicInformation() as OkNegotiatedContentResult<ServerMessage>;
            Assert.IsInstanceOfType(result.Content.Data, typeof(AcademicInformationDto));
        }

        private IHttpActionResult GetAcademicInformation()
        {
            return _academicInformationController.Get();
        }

        #endregion

        #region Post

        [TestMethod]
        public void ApplicantPersonInformationController_Post_Should_Call_UpsertPersonalInformation()
        {
            PostAcademicInformation();
            A.CallTo(() => _academicInformationRepository.UpsertAcademicInformation(AcademicInformation)).MustHaveHappened();
        }

        [TestMethod]
        public void ApplicantPersonInformationController_Post_Should_Return_OK_OnHappyPather()
        {
            WebApiCommonAsserts.IsOkResult(PostAcademicInformation());
        }

        [TestMethod]
        public void ApplicantPersonInformationController_Post_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _academicInformationRepository.UpsertAcademicInformation(A<AcademicInformationDto>.Ignored))
                .Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(PostAcademicInformation());
        }

        private IHttpActionResult PostAcademicInformation()
        {
            return _academicInformationController.Post(AcademicInformation);
        }

        #endregion
    }
}
