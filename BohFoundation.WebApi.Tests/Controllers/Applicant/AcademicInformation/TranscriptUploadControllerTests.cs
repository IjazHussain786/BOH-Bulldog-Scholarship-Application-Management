using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.AzureStorage.BlobStorageStreamProvider.Interfaces;
using BohFoundation.Domain.Dtos.Common;
using BohFoundation.Utilities.Context.Interfaces;
using BohFoundation.WebApi.Controllers.Applicant.Transcript;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Applicant.AcademicInformation
{
    [TestClass]
    public class TranscriptUploadControllerTests
    {
        private ITranscriptReferenceRepository _transcriptRepo;
        private IBlobHelper _blobHelper;
        private IClaimsInformationGetters _claimsInforamtionGetters;
        private TranscriptUploadController _transcriptReferenceController;

        [TestInitialize]
        public void Initialize()
        {
            _claimsInforamtionGetters = A.Fake<IClaimsInformationGetters>();
            _blobHelper = A.Fake<IBlobHelper>();
            _transcriptRepo = A.Fake<ITranscriptReferenceRepository>();

            _transcriptReferenceController = new TranscriptUploadController(_claimsInforamtionGetters, _blobHelper,
                _transcriptRepo);
        }

        [TestMethod]
        public void TranscriptUploadController_Get_Should_Call_Repo()
        {
            Get();
            A.CallTo(() => _transcriptRepo.LastUpdatedTranscript()).MustHaveHappened();
        }

        [TestMethod]
        public void TranscriptUploadController_Get_Should_Return_Internal_Exception_On_Exception()
        {
            A.CallTo(() => _transcriptRepo.LastUpdatedTranscript()).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Get());
        }

        [TestMethod]
        public void TranscriptUploadController_Get_Should_Return_Whatever_Repo_Returns()
        {
            var newLastUpdatedDto = new LastUpdatedDto();
            A.CallTo(() => _transcriptRepo.LastUpdatedTranscript()).Returns(newLastUpdatedDto);
            var result = Get() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(newLastUpdatedDto, result.Content.Data);
        }

        private IHttpActionResult Get()
        {
            return _transcriptReferenceController.Get();
        }
    }
}
