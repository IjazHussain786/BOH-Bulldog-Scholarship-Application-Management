using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.AdminsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Admin.EssayTopics;
using BohFoundation.WebApi.Controllers.Admin.EditEssayTopic;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Admin.EditEssayTopic
{
    [TestClass]
    public class EssayTopicControllerTests
    {
        private IEditEssayTopicRepository _editEssayTopicRepository;
        private EssayTopicController _essayTopicController;

        [TestInitialize]
        public void Initialize()
        {
            _editEssayTopicRepository = A.Fake<IEditEssayTopicRepository>();
            _essayTopicController = new EssayTopicController(_editEssayTopicRepository);

            CreateAndModifyEssayTopicDto = new CreateAndModifyEssayTopicDto();
        }

        private CreateAndModifyEssayTopicDto CreateAndModifyEssayTopicDto { get; set; }

        #region Post

        [TestMethod]
        public void EssayTopicController_Post_Should_Call_UpsertEssayTopic()
        {
            Post();
            A.CallTo(() => _editEssayTopicRepository.UpsertEssayTopic(CreateAndModifyEssayTopicDto)).MustHaveHappened();
        }

        [TestMethod]
        public void EssayTopicController_Post_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _editEssayTopicRepository.UpsertEssayTopic(CreateAndModifyEssayTopicDto))
                .Throws(new Exception());

            WebApiCommonAsserts.IsInternalServerError(Post());
        }

        [TestMethod]
        public void EssayTopicController_Post_Should_Return_OK_HappyPath()
        {
            WebApiCommonAsserts.IsOkResult(Post());
        }

        private IHttpActionResult Post()
        {
            return _essayTopicController.Post(CreateAndModifyEssayTopicDto);
        }

        #endregion


        [TestMethod]
        public void EssayTopicController_Get_Should_Call_GetEssayTopics()
        {
            Get();
            A.CallTo(() => _editEssayTopicRepository.GetEssayTopics()).MustHaveHappened();
        }

        [TestMethod]
        public void EssayTopicController_Get_Should_Return_Internal_Server_Error_OnExcepetion()
        {
            A.CallTo(() => _editEssayTopicRepository.GetEssayTopics()).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Get());
        }

        [TestMethod]
        public void EssayTopicController_Get_Should_Returns_A_Bunch_Of_Dtos()
        {
            A.CallTo(() => _editEssayTopicRepository.GetEssayTopics())
               .Returns(new Collection<EssayTopicDto>
                {
                    new EssayTopicDto(),
                    new EssayTopicDto(),
                    new EssayTopicDto(),
                    new EssayTopicDto()
                });

            var result = Get() as OkNegotiatedContentResult<ServerMessage>;
            var array = result.Content.Data as ICollection<EssayTopicDto>;

            Assert.AreEqual(4, array.Count);
        }

        public IHttpActionResult Get()
        {
            return _essayTopicController.Get();
        }

    }
}
