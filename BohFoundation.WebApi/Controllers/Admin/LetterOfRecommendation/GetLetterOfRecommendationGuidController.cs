using System;
using System.Web.Http;
using BohFoundation.AdminsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Admin.References;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.Admin.LetterOfRecommendation
{
    [Route("api/admin/letterofrecommendation/getguid"), RequireAdmin]
    public class GetLetterOfRecommendationGuidController : ApiController
    {
        private readonly IGetLetterOfRecommendationGuidRepository _recommendationGuidRepo;

        public GetLetterOfRecommendationGuidController(IGetLetterOfRecommendationGuidRepository recommendationGuidRepo)
        {
            _recommendationGuidRepo = recommendationGuidRepo;
        }

        //I'm making this into a post. Hacky, but I can't seem to get emails to work with webapi. 
        public IHttpActionResult Post([FromBody] GetLetterOfRecommendationGuidDto dto)
        {
            GuidSentToReferenceDto returnDto;
            try
            {
                returnDto = _recommendationGuidRepo.GetLetterOfRecommendationGuid(dto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            
            return Ok(new ServerMessage(returnDto));
        }
    }
}
