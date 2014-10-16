using System;
using System.Web.Http;
using BohFoundation.AdminsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Admin.EssayTopics;
using BohFoundation.WebApi.Controllers.Admin.EditEssayTopic;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Admin.EditEssayTopic
{
    [TestClass]
    public class ModifyEssayTopicGraduatingYearControllerTests
    {
        private IEditEssayTopicRepository _editEssayTopicRepository;
        private ModifyEssayTopicGraduatingYearController _modifyEssayTopicGraduatingYearController;

        [TestInitialize]
        public void Initialize()
        {
            _editEssayTopicRepository = A.Fake<IEditEssayTopicRepository>();
            _modifyEssayTopicGraduatingYearController =
                new ModifyEssayTopicGraduatingYearController(_editEssayTopicRepository);

            Dto = new EditEssayTopicByGraduatingClassDto();
        }

        private EditEssayTopicByGraduatingClassDto Dto { get; set; }

        [TestMethod]
        public void ModifyEssayTopicGraduatingYearControlle_Post_Should_Call_Add()
        {
            Post();
            A.CallTo(() => _editEssayTopicRepository.AddEssayTopicToGraduatingYear(Dto)).MustHaveHappened();
        }

        [TestMethod]
        public void ModifyEssayTopicGraduatingYearControlle_Post_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _editEssayTopicRepository.AddEssayTopicToGraduatingYear(Dto))
                .Throws(new Exception());

            WebApiCommonAsserts.IsInternalServerError(Post());
        }

        [TestMethod]
        public void ModifyEssayTopicGraduatingYearControlle_Post_Should_Return_OK_HappyPath()
        {
            WebApiCommonAsserts.IsOkResult(Post());
        }

        public IHttpActionResult Post()
        {
            return _modifyEssayTopicGraduatingYearController.Post(Dto);
        }

        [TestMethod]
        public void ModifyEssayTopicGraduatingYearControlle_Delete_Should_Call_Delete()
        {
            Delete();
            A.CallTo(() => _editEssayTopicRepository.DeleteEssayTopicFromGraduatingYear(Dto)).MustHaveHappened();
        }

        [TestMethod]
        public void ModifyEssayTopicGraduatingYearControlle_Delete_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _editEssayTopicRepository.DeleteEssayTopicFromGraduatingYear(Dto))
                .Throws(new Exception());

            WebApiCommonAsserts.IsInternalServerError(Delete());
        }

        [TestMethod]
        public void ModifyEssayTopicGraduatingYearControlle_Delete_Should_Return_OK_HappyPath()
        {
            WebApiCommonAsserts.IsOkResult(Delete());
        }

        public IHttpActionResult Delete()
        {
            return _modifyEssayTopicGraduatingYearController.Delete(Dto);
        }
    }
}
