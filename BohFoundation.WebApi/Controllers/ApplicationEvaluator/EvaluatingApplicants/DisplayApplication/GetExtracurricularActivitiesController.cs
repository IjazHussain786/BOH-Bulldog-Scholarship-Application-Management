using System;
using System.Web.Http;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Extracurricular;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication
{
    [RoutePrefix("api/applicationevaluator/evaluatingapplicants/displayapplication/extracurricularactivities"), RequireApplicationEvaluator]
    public class GetExtracurricularActivitiesController : ApiController
    {
        private readonly IGetCompletedApplicationRepository _getCompletedApplicationRepo;

        public GetExtracurricularActivitiesController(IGetCompletedApplicationRepository getCompletedApplicationRepo)
        {
            _getCompletedApplicationRepo = getCompletedApplicationRepo;
        }

        [Route("{applicantsGuid}")]
        public IHttpActionResult Get([FromUri] Guid applicantsGuid)
        {
            ExtracurricularActivitiesDto dto;

            try
            {
                dto = _getCompletedApplicationRepo.GetExtracurricularActivities(applicantsGuid);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(new ServerMessage(dto));
        }
    }
}
