using System;
using System.Collections.Generic;
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
    public class LowGradeInformationControllerTests
    {

        private ILowGradeInformationRepository _lowGradeInformationRepository;
        private LowGradeInformationController _lowGradeInformationController;
        private List<LowGradeDto> LowGrades { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            _lowGradeInformationRepository = A.Fake<ILowGradeInformationRepository>();
            _lowGradeInformationController = new LowGradeInformationController(_lowGradeInformationRepository);

            LowGradesWithGpaDto = new LowGradesWithGpaDto();
            LowGrades = new List<LowGradeDto>();

        }

        public LowGradesWithGpaDto LowGradesWithGpaDto { get; set; }

        #region Get

        [TestMethod]
        public void LowGradeInformationController_Get_Exception_Should_Return_InternalServerError()
        {
            A.CallTo(() => _lowGradeInformationRepository.GetLowGradeInformation()).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(_lowGradeInformationController.Get());
        }

        [TestMethod]
        public void LowGradeInformationController_Get_HappyPath_Should_Return_LowGrades()
        {
            A.CallTo(() => _lowGradeInformationRepository.GetLowGradeInformation()).Returns(LowGradesWithGpaDto);
            var result = _lowGradeInformationController.Get() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(LowGradesWithGpaDto, result.Content.Data);
        }

        #endregion

        #region Post

        [TestMethod]
        public void LowGradeInformationController_Post_Exception_Should_Return_InternalServerError()
        {
            A.CallTo(() => _lowGradeInformationRepository.UpsertLowGradeInformation(LowGrades)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(PostLowGrades());
        }

        [TestMethod]
        public void LowGradeInformationController_Post_HappyPath_Should_ReturnOk()
        {
            A.CallTo(() => _lowGradeInformationRepository.UpsertLowGradeInformation(LowGrades))
                .DoesNothing();

            WebApiCommonAsserts.IsOkResult(PostLowGrades());
        }

        [TestMethod]
        public void LowGradeInformationController_Post_HappyPath_Should_Call_Upsert_WithObject_Saving()
        {
            PostLowGrades();

            A.CallTo(() => _lowGradeInformationRepository.UpsertLowGradeInformation(LowGrades)).MustHaveHappened();
        }

        private IHttpActionResult PostLowGrades()
        {
            return _lowGradeInformationController.Post(LowGrades);
        }

        #endregion

    }
}
