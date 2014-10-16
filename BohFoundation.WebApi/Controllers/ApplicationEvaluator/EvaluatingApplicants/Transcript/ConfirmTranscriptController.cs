using System;
using System.Web.Http;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants;
using BohFoundation.WebApi.Filters;

namespace BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants.Transcript
{
    [RequireApplicationEvaluator, Route("api/applicationevaluator/evaluatingapplicants/transcript/confirm")]
    public class ConfirmTranscriptController : ApiController
    {
        private readonly IConfirmTranscriptRepository _confirmTranscriptRepository;

        public ConfirmTranscriptController(IConfirmTranscriptRepository confirmTranscriptRepository)
        {
            _confirmTranscriptRepository = confirmTranscriptRepository;
        }

        public IHttpActionResult Post(ConfirmTranscriptDto dto)
        {
            try
            {
                _confirmTranscriptRepository.ConfirmTranscript(dto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }
    }
}
