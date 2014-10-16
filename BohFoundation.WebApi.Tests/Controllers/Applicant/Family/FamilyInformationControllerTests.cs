using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Family;
using BohFoundation.WebApi.Controllers.Applicant.FamilyInformation;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Applicant.Family
{
    [TestClass]
    public class FamilyInformationControllerTests
    {
        private IFamilyInformationRepository _familyInformationRepo;
        private FamilyInformationController _familyController;

        [TestInitialize]
        public void Initialize()
        {
            _familyInformationRepo = A.Fake<IFamilyInformationRepository>();
            _familyController = new FamilyInformationController(_familyInformationRepo);
            FamilyInformation = new FamilyInformationDto();
        }

        private FamilyInformationDto FamilyInformation { get; set; }

        #region Get

        [TestMethod]
        public void FamilyInformationController_Get_Should_Call_Repo()
        {
            Get();
            A.CallTo(() => _familyInformationRepo.GetFamilyInformation()).MustHaveHappened();
        }

        [TestMethod]
        public void FamilyInformationController_Get_Should_Return_InternlServerError_OnException()
        {
            A.CallTo(() => _familyInformationRepo.GetFamilyInformation()).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Get());
        }

        [TestMethod]
        public void FamilyInformationController_Get_Should_Return_Ok_WithObject_OnHappyPath()
        {
            A.CallTo(() => _familyInformationRepo.GetFamilyInformation()).Returns(FamilyInformation);
            var result = Get() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(FamilyInformation, result.Content.Data);
        }

        private IHttpActionResult Get()
        {
            return _familyController.Get();
        }

        #endregion

        [TestMethod]
        public void FamilyInformationController_Post_Should_Call_Repo()
        {
            Post();
            A.CallTo(() => _familyInformationRepo.UpsertFamilyInformation(FamilyInformation)).MustHaveHappened();
        }

        [TestMethod]
        public void FamilyInformationController_Post_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _familyInformationRepo.UpsertFamilyInformation(FamilyInformation)).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Post());
        }

        [TestMethod]
        public void FamilyInformationController_Post_Should_Return_Ok_OnHappyPath()
        {
            WebApiCommonAsserts.IsOkResult(Post());
        }

        private IHttpActionResult Post()
        {
            return _familyController.Post(FamilyInformation);
        }

    }
}
