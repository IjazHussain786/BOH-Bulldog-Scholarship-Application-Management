using System;
using System.Web.Http;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate;
using BohFoundation.WebApi.Filters;

namespace BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants.RateApplicant
{
    [RequireApplicationEvaluator]
    public class FinalRatingController : ApiController
    {
        private readonly IRateApplicationRespository _rateApplicationRespository;

        public FinalRatingController(IRateApplicationRespository rateApplicationRespository)
        {
            _rateApplicationRespository = rateApplicationRespository;
        }

        [Route("api/applicationevaluator/evaluatingapplicants/rateapplicants/final")]
        public IHttpActionResult Post([FromBody] RatingWithApplicantsGuidDto ratingDto)
        {
            try
            {
                _rateApplicationRespository.UpsertFinalOverallRating(ratingDto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();
        }
    }
}
