using System;
using System.Web.Http;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication
{
    [RoutePrefix("api/applicationevaluator/evaluatingapplicants/displayapplication/academicinformation"), RequireApplicationEvaluator]
    public class GetAcademicInformationController : ApiController
    {
        private readonly IGetCompletedApplicationRepository _getCompletedApplicationRepo;

        public GetAcademicInformationController(IGetCompletedApplicationRepository getCompletedApplicationRepo)
        {
            _getCompletedApplicationRepo = getCompletedApplicationRepo;
        }

        [Route("{applicantsGuid}")]
        public IHttpActionResult Get([FromUri]Guid applicantsGuid)
        {
            CompletedAcademicInformationDto dto;

            try
            {
                dto = _getCompletedApplicationRepo.GetAcademicInformation(applicantsGuid);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(new ServerMessage(dto));
        }
    }
}
