using System;
using System.Web.Http;
using System.Web.Http.Results;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants;
using BohFoundation.WebApi.Controllers.ApplicationEvaluator.EvaluatingApplicants;
using BohFoundation.WebApi.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.ApplicationEvaluator.EvaluatingApplicants
{
    [TestClass]
    public class GetListOfApplicantsControllerTests
    {
        private IGetListOfFinalizedApplicantsRepository _getListOfFinalizedApplicantsRepo;
        private GetListOfApplicantsController _getListOfApplicantsController;

        [TestInitialize]
        public void Initialize()
        {
            _getListOfFinalizedApplicantsRepo = A.Fake<IGetListOfFinalizedApplicantsRepository>();
            _getListOfApplicantsController = new GetListOfApplicantsController(_getListOfFinalizedApplicantsRepo);
        }

        [TestMethod]
        public void GetListOfApplicantsController_Get_Should_Call_Repo_With_CurrentYear()
        {
            Get();
            
            A.CallTo(
                () => _getListOfFinalizedApplicantsRepo.GetAllFinalizedApplicantsByGraduatingYear(DateTime.Now.Year))
                .MustHaveHappened();
        }

        [TestMethod]
        public void GetListOfApplicantsController_Get_Should_Return_InternalExceptionError_On_Exception()
        {
            A.CallTo(
                () => _getListOfFinalizedApplicantsRepo.GetAllFinalizedApplicantsByGraduatingYear(DateTime.Now.Year))
                .Throws(new Exception());
            WebApiCommonAsserts.IsInternalServerError(Get());
        }

        [TestMethod]
        public void GetListOfApplicantsController_Get_Should_Return_Object_From_Repo()
        {
            var item = new AllFinalizedApplicantsForAGraduatingYearDto();
            
            A.CallTo(
                () => _getListOfFinalizedApplicantsRepo.GetAllFinalizedApplicantsByGraduatingYear(DateTime.Now.Year))
                .Returns(item);

            var returnItem = Get() as OkNegotiatedContentResult<ServerMessage>;
            Assert.AreSame(item, returnItem.Content.Data);
        }

        private IHttpActionResult Get()
        {
            return _getListOfApplicantsController.Get();
        }
    }
}
