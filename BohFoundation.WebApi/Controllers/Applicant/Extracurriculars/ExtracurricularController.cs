using System;
using System.Web.Http;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Extracurricular;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.Applicant.Extracurriculars
{
    [Route("api/applicant/extracurriculars"), RequireApplicant]
    public class ExtracurricularController : ApiController
    {
        private readonly IExtracurricularActivitiesRepository _extracurricularActivitiesRepository;

        public ExtracurricularController(IExtracurricularActivitiesRepository extracurricularActivitiesRepository)
        {
            _extracurricularActivitiesRepository = extracurricularActivitiesRepository;
        }

        public IHttpActionResult Get()
        {
            ExtracurricularActivitiesDto dto;

            try
            {
                dto = _extracurricularActivitiesRepository.GetExtracurricularActivities();
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(new ServerMessage(dto));
        }

        public IHttpActionResult Post([FromBody] ExtracurricularActivitiesDto dto)
        {
            try
            {
                _extracurricularActivitiesRepository.UpsertExtracurricularActivities(dto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();
        }
    }
}
