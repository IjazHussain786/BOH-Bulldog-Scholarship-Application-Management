using System;
using System.Web.Http;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication
{
    [RoutePrefix("api/applicationevaluator/evaluatingapplicants/displayapplication/collectionessaysandlettersofrecommendation"), RequireApplicationEvaluator]
    public class GetCollectionOfEssaysAndLettersOfRecommendationsController : ApiController
    {
        private readonly IGetCompletedApplicationRepository _getCompletedApplicationRepository;

        public GetCollectionOfEssaysAndLettersOfRecommendationsController(IGetCompletedApplicationRepository getCompletedApplicationRepository)
        {
            _getCompletedApplicationRepository = getCompletedApplicationRepository;
        }

        [Route("{applicantsGuid}")]
        public IHttpActionResult Get([FromUri] Guid applicantsGuid)
        {
            CollectionsOfEssaysAndLettersOfRecommendationDto dto;
         
            try
            {
                dto = _getCompletedApplicationRepository.GetCollectionsOfEssaysAndLettersOfRecommendation(applicantsGuid);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            
            return Ok(new ServerMessage(dto));
        }
    }
}
