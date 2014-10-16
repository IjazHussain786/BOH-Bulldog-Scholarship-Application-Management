using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.AzureStorage.BlobStorageStreamProvider;
using BohFoundation.AzureStorage.BlobStorageStreamProvider.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Academic;
using BohFoundation.Domain.Dtos.Common;
using BohFoundation.Utilities.Context.Interfaces;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BohFoundation.WebApi.Controllers.Applicant.Transcript
{
    //toDo not tested!

    [RequireApplicant]
    [Route("api/applicant/transcript")]
    public class TranscriptUploadController : ApiController
    {
        private readonly ITranscriptReferenceRepository _transcriptReferenceRepository;
        private readonly string _containerName;
        private readonly Lazy<Task<CloudBlobContainer>> _blobContainer;

        public TranscriptUploadController(IClaimsInformationGetters claimsInformationGetters, IBlobHelper blobHelper, ITranscriptReferenceRepository transcriptReferenceRepository)
        {
            _transcriptReferenceRepository = transcriptReferenceRepository;

            const string transcriptsName = "transcripts-";

            var usersGraduatingYear = claimsInformationGetters.GetApplicantsGraduatingYear();
            _containerName = transcriptsName + usersGraduatingYear;

            _blobContainer = new Lazy<Task<CloudBlobContainer>>(async () => await blobHelper.GetBlobContainer(_containerName));
        }

        public async Task<IHttpActionResult> Post()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var container = await _blobContainer.Value;

                //Create a AzureBlobStorageMultipartProvider and process the request
                var streamProvider = new AzureBlobStorageMultipartProvider(container, Path.GetTempPath());
                await Request.Content.ReadAsMultipartAsync(streamProvider);
                
                var dto = new TranscriptBlobReferenceDto { BlobContainerName = streamProvider.Container, ReferenceToTranscriptPdf = streamProvider.Reference };
                _transcriptReferenceRepository.UpsertTranscriptReference(dto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();
        }

        public IHttpActionResult Get()
        {
            LastUpdatedDto lastUpdatedDto;
            try
            {
                lastUpdatedDto = _transcriptReferenceRepository.LastUpdatedTranscript();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok(new ServerMessage(lastUpdatedDto));
        }
    }
}