using System;
using System.Web.Http;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Academic;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.Applicant.AcademicInformation
{
    [RequireApplicant]
    [Route("api/applicant/academicinformation")]
    public class AcademicInformationController : ApiController
    {
        private readonly IAcademicInformationRepository _academicInformationRepository;
        
        public AcademicInformationController(IAcademicInformationRepository academicInformationRepository)
        {
            _academicInformationRepository = academicInformationRepository;
        }

        public IHttpActionResult Get()
        {
            AcademicInformationDto dto;
            try
            {
                dto = _academicInformationRepository.GetAcademicInformation();
                if (dto != null)
                {
                    if (dto.Gpa == 0)
                    {
                        dto = null;
                    } 
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok(new ServerMessage(dto));
        }

        public IHttpActionResult Post([FromBody] AcademicInformationDto academicInformationDto)
        {
            try
            {
                _academicInformationRepository.UpsertAcademicInformation(academicInformationDto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }
    }
}
