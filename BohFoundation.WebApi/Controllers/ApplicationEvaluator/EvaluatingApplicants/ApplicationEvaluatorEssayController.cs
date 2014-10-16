using System;
using System.Web.Http;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.AzureStorage.TableStorage.Interfaces.Essay;
using BohFoundation.Domain.Dtos.Applicant.Essay;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate;
using BohFoundation.Domain.Dtos.Common.AzureQueuryObjects;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants
{
    [RoutePrefix("api/applicationevaluator/evaluatingapplicants/essay"), RequireApplicationEvaluator]
    public class ApplicationEvaluatorEssayController : ApiController
    {
        private readonly IAzureEssayRepository _azureEssayRepository;
        private readonly IRateApplicationRespository _rateApplicationRespository;

        public ApplicationEvaluatorEssayController(IAzureEssayRepository azureEssayRepository, IRateApplicationRespository rateApplicationRespository)
        {
            _azureEssayRepository = azureEssayRepository;
            _rateApplicationRespository = rateApplicationRespository;
        }

        [Route("partitionkey/{PartitionKey}/rowkey/{RowKey}")]
        public IHttpActionResult Get([FromUri] AzureTableStorageEntityKeyDto azureTableKeyDto)
        {
            EssayDto essayDto;

            try
            {
                essayDto = _azureEssayRepository.GetEssay(azureTableKeyDto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(new ServerMessage(essayDto));
        }

        [Route("rating")]
        public IHttpActionResult Post([FromBody] EssayRatingDto essayRatingDto)
        {
            try
            {
                _rateApplicationRespository.UpsertEssayRating(essayRatingDto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }
    }
}
