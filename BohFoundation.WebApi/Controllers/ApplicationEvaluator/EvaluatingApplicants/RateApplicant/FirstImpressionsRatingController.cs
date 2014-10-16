using System;
using System.Web.Http;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate;
using BohFoundation.WebApi.Filters;

namespace BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants.RateApplicant
{
    [RequireApplicationEvaluator]
    public class FirstImpressionsRatingController : ApiController
    {
        private readonly IRateApplicationRespository _ratingRepo;

        public FirstImpressionsRatingController(IRateApplicationRespository ratingRepo)
        {
            _ratingRepo = ratingRepo;
        }

        [Route("api/applicationevaluator/evaluatingapplicants/rateapplicants/firstimpression")]
        public IHttpActionResult Post([FromBody] RatingWithApplicantsGuidDto ratingDto)
        {
            try
            {
                _ratingRepo.UpsertFirstImpression(ratingDto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();
        }
    }
}
