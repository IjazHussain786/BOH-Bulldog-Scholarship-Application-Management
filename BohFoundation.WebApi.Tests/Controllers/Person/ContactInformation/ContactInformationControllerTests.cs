using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.Domain.Dtos.Person;
using BohFoundation.PersonsRepository.Repositories.Interfaces;
using BohFoundation.WebApi.Controllers.Person.ContactInformation;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.Person.ContactInformation
{
    [TestClass]
    public class ContactInformationControllerTests
    {
        private IContactInformationRepository _contactInformationRepository;
        private ContactInformationController _contactInformationController;
        private ContactInformationDto ContactInformationDto { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            _contactInformationRepository = A.Fake<IContactInformationRepository>();
            _contactInformationController = new ContactInformationController(_contactInformationRepository);

            ContactInformationDto = new ContactInformationDto {Address = new AddressDto()};
        }

        #region Post

        [TestMethod]
        public void ContactInformationController_Post_Should_Call_UpsertContactInformation()
        {
            PostContactInformation();
            A.CallTo(() => _contactInformationRepository.UpsertContactInformation(ContactInformationDto))
                .MustHaveHappened();
        }

        [TestMethod]
        public void ContactInformationController_Post_Should_Return_OK_OnHappyPather()
        {
            WebApiCommonAsserts.IsOkResult(PostContactInformation());
        }

        [TestMethod]
        public void ContactInformationController_Post_Should_Return_InternalServerError_OnException()
        {
            A.CallTo(() => _contactInformationRepository.UpsertContactInformation(A<ContactInformationDto>.Ignored))
                .Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(PostContactInformation());
        }

        private IHttpActionResult PostContactInformation()
        {
            return _contactInformationController.Post(ContactInformationDto);
        }

        #endregion

        [TestMethod]
        public void ContactInformationController_Get_Should_Call_GetContactInformation()
        {
            GetContactInformation();
            A.CallTo(() => _contactInformationRepository.GetContactInformation()).MustHaveHappened();
        }

        [TestMethod]
        public void ContactInformationController_Get_Should_ReturnInternalServerError_OnException()
        {
            A.CallTo(() => _contactInformationRepository.GetContactInformation()).Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(GetContactInformation());
        }

        [TestMethod]
        public void ContactInformationController_Get_Should_Ok_WithNullPackaged()
        {
            A.CallTo(() => _contactInformationRepository.GetContactInformation()).Returns(null);
            var result = GetContactInformation() as OkNegotiatedContentResult<ServerMessage>;
            Assert.IsNull(result.Content.Data);
        }

        [TestMethod]
        public void ContactInformationController_Get_Should_Ok_WithPackaged()
        {
            var package = new ContactInformationDto();

            A.CallTo(() => _contactInformationRepository.GetContactInformation()).Returns(package);
            var result = GetContactInformation() as OkNegotiatedContentResult<ServerMessage>;
            Assert.IsInstanceOfType(result.Content.Data, typeof(ContactInformationDto));
        }

        private IHttpActionResult GetContactInformation()
        {
            return _contactInformationController.Get();
        }

    }
}
