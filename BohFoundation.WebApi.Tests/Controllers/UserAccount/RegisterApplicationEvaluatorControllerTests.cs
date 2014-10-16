using System;
using System.Web.Http;
using BohFoundation.Domain.Dtos;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.Domain.Enums;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;
using BohFoundation.TestHelpers;
using BohFoundation.WebApi.Controllers.UserAccount;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.UserAccount
{
    [TestClass]
    public class RegisterApplicationEvaluatorControllerTests
    {
        private RegisterApplicationEvaluatorController _registerApplicantController;
        private IRegisterUserService _registerApplicant;
        private RegisterInputModel Model { get; set; }
        private IHttpActionResult Result { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            _registerApplicant = A.Fake<IRegisterUserService>();
            _registerApplicantController = new RegisterApplicationEvaluatorController(_registerApplicant);

            Model = new RegisterInputModel();
        }

        [TestMethod]
        public void RegisterApplicantController_Post_Should_Call_RegisterApplicant_CreateAccount()
        {
            PostRegisteration();
            A.CallTo(() => _registerApplicant.CreateAccount(Model, MemberTypesEnum.PendingApplicationEvaluator)).MustHaveHappened();
        }

        [TestMethod]
        public void RegisterApplicantController_Post_Should_Return_HttpActionResult()
        {
            PostRegisteration();
            WebApiCommonAsserts.IsHttpActionResult(Result);
        }

        [TestMethod]
        public void RegisterApplicantController_Post_Should_ReturnInternalServerError_OnException()
        {
            A.CallTo(() => _registerApplicant.CreateAccount(Model, MemberTypesEnum.PendingApplicationEvaluator)).Throws(new Exception());
            PostRegisteration();
            WebApiCommonAsserts.IsInternalServerError(Result);
        }

        [TestMethod]
        public void RegisterApplicantController_Post_Should_Have_A_BadRequest_When_FailureSuccessModelReturn()
        {
            A.CallTo(() => _registerApplicant.CreateAccount(Model, MemberTypesEnum.PendingApplicationEvaluator)).Returns(new SuccessOrFailureDto { Success = false, ExceptionMessage = TestHelpersCommonFields.ExceptionMessage });
            PostRegisteration();
            WebApiCommonAsserts.IsBadResultWithMessage(Result);
        }

        [TestMethod]
        public void RegisterApplicantController_Post_Should_Have_A_BadRequest_WithExceptionMessage_When_FailureSuccessModelReturn()
        {
            A.CallTo(() => _registerApplicant.CreateAccount(Model, MemberTypesEnum.PendingApplicationEvaluator)).Returns(new SuccessOrFailureDto { Success = false, ExceptionMessage = TestHelpersCommonFields.ExceptionMessage });
            PostRegisteration();
            WebApiCommonAsserts.BadResultHasTheCorrectErrorMessage(Result, TestHelpersCommonFields.ExceptionMessage);
        }

        [TestMethod]
        public void RegisterApplicantController_Post_HappyPath_Should_ReturnOKResult()
        {
            A.CallTo(() => _registerApplicant.CreateAccount(Model, MemberTypesEnum.PendingApplicationEvaluator)).Returns(new SuccessOrFailureDto { Success = true });
            PostRegisteration();
            WebApiCommonAsserts.IsOkResult(Result);
        }


        private void PostRegisteration()
        {
            Result = _registerApplicantController.Post(Model);
        }
    }
}
