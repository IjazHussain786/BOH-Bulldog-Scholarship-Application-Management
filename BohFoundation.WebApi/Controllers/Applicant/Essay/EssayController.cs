using System;
using System.Web.Http;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Essay;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.Applicant.Essay
{
    [RoutePrefix("api/applicant/essay"), RequireApplicant]
    public class EssayController : ApiController
    {
        private readonly IApplicantsEssayRepository _applicantsEssayRepository;

        public EssayController(IApplicantsEssayRepository applicantsEssayRepository)
        {
            _applicantsEssayRepository = applicantsEssayRepository;
        }
        
        [Route("")]
        public IHttpActionResult Post([FromBody] EssayDto essayDto)
        {
            try
            {
                _applicantsEssayRepository.UpsertEssay(essayDto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }

        [Route("{essayTopicId}")]
        public IHttpActionResult Get([FromUri] int essayTopicId)
        {
            EssayDto essayDto;
            try
            {
                essayDto = _applicantsEssayRepository.GetEssay(essayTopicId);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(new ServerMessage(essayDto));
        }
    }
}
