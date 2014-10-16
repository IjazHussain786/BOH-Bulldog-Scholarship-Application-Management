using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BohFoundation.AzureStorage.BlobStorageSharedAccessSignature.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Academic;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants.Transcript
{
    //todo: not tested

    [RoutePrefix("api/applicationevaluator/evaluatingapplicants/transcript/geturi"), RequireApplicationEvaluator]
    public class GetTranscriptUriController : ApiController
    {
        private readonly ICreateSharedAccessSignatureForBlobItem _createSharedAccessSignature;

        public GetTranscriptUriController(ICreateSharedAccessSignatureForBlobItem createSharedAccessSignature)
        {
            _createSharedAccessSignature = createSharedAccessSignature;
        }

        [Route("blobcontainername/{BlobContainerName}/referencetotranscriptwithoutfileextension/{ReferenceToTranscriptPdf}/extension/{extension}")]
        public async Task<IHttpActionResult> Get([FromUri]TranscriptBlobReferenceDto transcriptBlobReferenceDto, string extension)
        {
            string uri;

            var newReferenceToTranscript =
                new StringBuilder().Append(transcriptBlobReferenceDto.ReferenceToTranscriptPdf)
                    .Append(".")
                    .Append(extension).ToString();

            transcriptBlobReferenceDto.ReferenceToTranscriptPdf = newReferenceToTranscript;

            try
            {
                uri = await _createSharedAccessSignature.GetApplicantsTranscriptUri(transcriptBlobReferenceDto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(new ServerMessage(uri));
        }
    }
}
