using System;
using System.Web.Http;
using BohFoundation.AzureStorage.TableStorage.Interfaces.LettersOfRecommendation;
using BohFoundation.Domain.Dtos.Common.AzureQueuryObjects;
using BohFoundation.Domain.Dtos.Reference.Anonymous;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication
{
    [RoutePrefix("api/applicationevaluator/evaluatingapplicants/displayapplication/letterofrecommendation"), RequireApplicationEvaluator]
    public class GetLetterOfRecommendationController : ApiController
    {
        private readonly IAzureLettersOfRecommendationRepository _lettersOfRecommendationRepository;

        public GetLetterOfRecommendationController(IAzureLettersOfRecommendationRepository lettersOfRecommendationRepository)
        {
            _lettersOfRecommendationRepository = lettersOfRecommendationRepository;
        }

        [Route("partitionkey/{PartitionKey}/rowkey/{RowKey}")]
        public IHttpActionResult Get([FromUri] AzureTableStorageEntityKeyDto azureTableStorageKeyDto)
        {
            LetterOfRecommendationDto letterOfRecommendation;

            try
            {
                letterOfRecommendation =
                    _lettersOfRecommendationRepository.GetLetterOfRecommendation(azureTableStorageKeyDto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(new ServerMessage(letterOfRecommendation));
        }
    }
}
