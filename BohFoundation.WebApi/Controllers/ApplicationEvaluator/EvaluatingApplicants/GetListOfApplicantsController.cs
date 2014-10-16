using System;
using System.Web.Http;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants
{
    [Route("api/applicationevaluator/evaluatingapplicants/getlistofapplicants"), RequireApplicationEvaluator]
    public class GetListOfApplicantsController : ApiController
    {
        private readonly IGetListOfFinalizedApplicantsRepository _getListOfFinalizedApplicantsRepository;

        public GetListOfApplicantsController(IGetListOfFinalizedApplicantsRepository getListOfFinalizedApplicantsRepository)
        {
            _getListOfFinalizedApplicantsRepository = getListOfFinalizedApplicantsRepository;
        }

        public IHttpActionResult Get()
        {
            AllFinalizedApplicantsForAGraduatingYearDto dto;
            
            try
            {
                dto = _getListOfFinalizedApplicantsRepository.GetAllFinalizedApplicantsByGraduatingYear(DateTime.UtcNow.Year);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(new ServerMessage(dto));
        }
    }
}
