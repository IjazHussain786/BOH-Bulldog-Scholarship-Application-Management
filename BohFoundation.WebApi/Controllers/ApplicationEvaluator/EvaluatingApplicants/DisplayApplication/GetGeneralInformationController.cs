using System;
using System.Web.Http;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication
{
    [RoutePrefix("api/applicationevaluator/evaluatingapplicants/displayapplication/generalinformation"), RequireApplicationEvaluator]
    public class GetGeneralInformationController : ApiController
    {
        private readonly IGetCompletedApplicationRepository _getCompletedApplicationRepo;

        public GetGeneralInformationController(IGetCompletedApplicationRepository getCompletedApplicationRepo)
        {
            _getCompletedApplicationRepo = getCompletedApplicationRepo;
        }

        [Route("{applicantsGuid}")]
        public IHttpActionResult Get([FromUri] Guid applicantsGuid)
        {
            ApplicantsGeneralInformationDto dto;

            try
            {
                dto = _getCompletedApplicationRepo.GetGeneralInformation(applicantsGuid);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(new ServerMessage(dto));
        }
    }
}
