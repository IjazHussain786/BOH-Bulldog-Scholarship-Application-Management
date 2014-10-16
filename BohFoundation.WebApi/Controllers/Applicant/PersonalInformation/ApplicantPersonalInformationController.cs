using System;
using System.Web.Http;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.PersonalInformation;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.Applicant.PersonalInformation
{
    [RequireApplicant]
    [Route("api/applicant/personalinformation")]
    public class ApplicantPersonalInformationController : ApiController
    {
        private readonly IPersonalInformationRepository _applicantRepo;

        public ApplicantPersonalInformationController(IPersonalInformationRepository applicantRepo)
        {
            _applicantRepo = applicantRepo;
        }
        
        public IHttpActionResult Get()
        {
            ApplicantPersonalInformationDto dto;
            try
            {
                dto = _applicantRepo.GetPersonalInformation();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok(new ServerMessage(dto));
        }

        public IHttpActionResult Post([FromBody] ApplicantPersonalInformationDto personalInformation)
        {
            try
            {
                _applicantRepo.UpsertPersonalInformation(personalInformation);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }
    }
}
