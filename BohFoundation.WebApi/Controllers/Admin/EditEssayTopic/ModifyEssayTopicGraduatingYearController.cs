using System;
using System.Web.Http;
using BohFoundation.AdminsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Admin.EssayTopics;
using BohFoundation.WebApi.Filters;

namespace BohFoundation.WebApi.Controllers.Admin.EditEssayTopic
{

    [RoutePrefix("api/admin/essaytopic/graduatingyear"), RequireAdmin]
    public class ModifyEssayTopicGraduatingYearController : ApiController
    {
        private readonly IEditEssayTopicRepository _editEssayTopicRepository;

        public ModifyEssayTopicGraduatingYearController(IEditEssayTopicRepository editEssayTopicRepository)
        {
            _editEssayTopicRepository = editEssayTopicRepository;
        }

        [Route("")]
        public IHttpActionResult Post([FromBody] EditEssayTopicByGraduatingClassDto dto)
        {
            try
            {
                _editEssayTopicRepository.AddEssayTopicToGraduatingYear(dto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }

        [Route("{EssayId}/{ClassYear}")]
        public IHttpActionResult Delete([FromUri] EditEssayTopicByGraduatingClassDto dto)
        {
            try
            {
                _editEssayTopicRepository.DeleteEssayTopicFromGraduatingYear(dto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }
    }
}
