using System;
using System.Web.Http;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Family;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.Applicant.FamilyInformation
{
    [Route("api/applicant/familyinformation"), RequireApplicant]
    public class FamilyInformationController : ApiController
    {
        private readonly IFamilyInformationRepository _familyInformationRepo;

        public FamilyInformationController(IFamilyInformationRepository familyInformationRepo)
        {
            _familyInformationRepo = familyInformationRepo;
        }

        public IHttpActionResult Get()
        {
            FamilyInformationDto dto;
            try
            {
                dto = _familyInformationRepo.GetFamilyInformation();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            
            return Ok(new ServerMessage(dto));
        }

        public IHttpActionResult Post([FromBody] FamilyInformationDto dto)
        {
            try
            {
                _familyInformationRepo.UpsertFamilyInformation(dto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }
    }
}
