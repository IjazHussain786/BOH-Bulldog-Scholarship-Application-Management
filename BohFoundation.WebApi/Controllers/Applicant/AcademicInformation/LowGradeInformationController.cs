using System;
using System.Collections.Generic;
using System.Web.Http;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Academic;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.Applicant.AcademicInformation
{
    [RequireApplicant]
    [Route("api/applicant/lowgrades")]
    public class LowGradeInformationController : ApiController
    {
        private readonly ILowGradeInformationRepository _lowGradeInformationRepository;

        public LowGradeInformationController(ILowGradeInformationRepository lowGradeInformationRepository)
        {
            _lowGradeInformationRepository = lowGradeInformationRepository;
        }

        public IHttpActionResult Get()
        {
            LowGradesWithGpaDto lowGradeWithGpaDto;
            try
            {
                lowGradeWithGpaDto = _lowGradeInformationRepository.GetLowGradeInformation();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok(new ServerMessage(lowGradeWithGpaDto));
        }

        public IHttpActionResult Post([FromBody] ICollection<LowGradeDto> lowGrades)
        {
            try
            {
                _lowGradeInformationRepository.UpsertLowGradeInformation(lowGrades);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }
    }
}
