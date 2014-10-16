using System;
using System.Web.Http;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.RatingSummary;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.ApplicationEvaluator.RatingSummary
{
    [Route("api/applicationevaluator/evaluatingapplicants/ratingSummaries"), RequireApplicationEvaluator]
    public class RatingSummariesController : ApiController
    {
        private readonly IGetTopRatedApplicantsRepository _getTopRatedApplicantsRepository;

        public RatingSummariesController(IGetTopRatedApplicantsRepository getTopRatedApplicantsRepository)
        {
            _getTopRatedApplicantsRepository = getTopRatedApplicantsRepository;
        }

        public IHttpActionResult Get()
        {
            Top5ApplicantsDto dto;

            try
            {
               dto = _getTopRatedApplicantsRepository.GetTop5Applicants(DateTime.UtcNow.Year);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            
            return Ok(new ServerMessage(dto));
        }
    }
}
