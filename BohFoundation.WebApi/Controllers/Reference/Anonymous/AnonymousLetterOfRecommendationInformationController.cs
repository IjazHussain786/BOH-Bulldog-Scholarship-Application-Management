using System;
using System.Web.Http;
using BohFoundation.Domain.Dtos.Reference.Anonymous;
using BohFoundation.ReferencesRepository.Repositories.Interfaces;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.Reference.Anonymous
{
    [AllowAnonymous, RoutePrefix("api/references/anon/letterofrecommendationinformation")]
    public class AnonymousLetterOfRecommendationInformationController : ApiController
    {
        private readonly IAnonymousLetterOfRecommendationRepository _letterOfRecommendationRepository;

        public AnonymousLetterOfRecommendationInformationController(IAnonymousLetterOfRecommendationRepository letterOfRecommendationRepository)
        {
            _letterOfRecommendationRepository = letterOfRecommendationRepository;
        }

        [Route("{guid}")]
        public IHttpActionResult Get([FromUri] Guid guid)
        {
            InformationForReferenceFormDto formInforamtion;
            try
            {
                formInforamtion = _letterOfRecommendationRepository.GetInformationForReferenceForm(new GuidForLetterOfRecommendationDto{GuidSentToReference = guid});
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(new ServerMessage(formInforamtion));
        }

        [Route("")]
        public IHttpActionResult Post([FromBody] ReferencePersonalInformationDto dto)
        {
            try
            {
                _letterOfRecommendationRepository.UpsertReferencesPersonalInformation(dto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }
    }
}
