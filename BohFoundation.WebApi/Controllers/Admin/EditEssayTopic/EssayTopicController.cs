using System;
using System.Collections.Generic;
using System.Web.Http;
using BohFoundation.AdminsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Admin.EssayTopics;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.Admin.EditEssayTopic
{
    [RequireAdmin]
    [Route("api/admin/essaytopic")]
    public class EssayTopicController : ApiController
    {
        private readonly IEditEssayTopicRepository _editEssayTopicRepository;

        public EssayTopicController(IEditEssayTopicRepository editEssayTopicRepository)
        {
            _editEssayTopicRepository = editEssayTopicRepository;
        }

        public IHttpActionResult Post([FromBody]CreateAndModifyEssayTopicDto dto)
        {
            try
            {
                _editEssayTopicRepository.UpsertEssayTopic(dto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }

        public IHttpActionResult Get()
        {
            ICollection<EssayTopicDto> essayTopicDtos;
            try
            {
                essayTopicDtos = _editEssayTopicRepository.GetEssayTopics();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok(new ServerMessage(essayTopicDtos));
        }
    }
}
