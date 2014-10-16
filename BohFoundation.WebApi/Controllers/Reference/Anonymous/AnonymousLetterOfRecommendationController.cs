using System;
using System.Web.Http;
using BohFoundation.Domain.Dtos.Reference.Anonymous;
using BohFoundation.MiddleTier.ReferencesOrchestration.Interfaces;

namespace BohFoundation.WebApi.Controllers.Reference.Anonymous
{
    [AllowAnonymous, Route("api/references/anon/letterofrecommendation")]
    public class AnonymousLetterOfRecommendationController : ApiController
    {
        private readonly IAnonymousLetterOfRecommendationOrchestration _letterOrRecommendationRepo;

        public AnonymousLetterOfRecommendationController(IAnonymousLetterOfRecommendationOrchestration letterOrRecommendationRepo)
        {
            _letterOrRecommendationRepo = letterOrRecommendationRepo;
        }

        public IHttpActionResult Post([FromBody] LetterOfRecommendationDto letterOfRecommendationDto)
        {
            try
            {
                _letterOrRecommendationRepo.AddLetterOfRecommendation(letterOfRecommendationDto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }
    }
}
