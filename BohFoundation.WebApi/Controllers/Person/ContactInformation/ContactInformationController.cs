using System;
using System.Web.Http;
using BohFoundation.Domain.Dtos.Person;
using BohFoundation.PersonsRepository.Repositories.Interfaces;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.Person.ContactInformation
{
    [RequireApplicant]
    [Route("api/person/contactinformation")]
    public class ContactInformationController : ApiController
    {
        private readonly IContactInformationRepository _contactInformationRepository;

        public ContactInformationController(IContactInformationRepository contactInformationRepository)
        {
            _contactInformationRepository = contactInformationRepository;
        }

        public IHttpActionResult Get()
        {
            ContactInformationDto dto;
            try
            {
                dto = _contactInformationRepository.GetContactInformation();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok(new ServerMessage(dto));
        }

        public IHttpActionResult Post([FromBody] ContactInformationDto contactInformation)
        {
            try
            {
                _contactInformationRepository.UpsertContactInformation(contactInformation);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }
    }
}
