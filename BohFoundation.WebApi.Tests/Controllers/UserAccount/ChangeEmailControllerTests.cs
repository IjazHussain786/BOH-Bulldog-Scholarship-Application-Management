using System;
using System.Web.Http;
using BohFoundation.Domain.Dtos;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;
using BohFoundation.TestHelpers;
using BohFoundation.WebApi.Controllers.UserAccount;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests.Controllers.UserAccount
{
    [TestClass]
    public class ChangeEmailControllerTests
    {

        private ChangeEmailController _changeEmail;
        private ChangeEmailInputModelDto _changeEmailModel;
        private IChangeEmailService _changeEmailService;
        private VerificationKeyDto _verifyEmailModel;
        private const string Key = "key";

        [TestInitialize]
        public void Initialize()
        {
            _changeEmailService = A.Fake<IChangeEmailService>();

            _changeEmail = new ChangeEmailController(_changeEmailService);

            _changeEmailModel = new ChangeEmailInputModelDto { NewEmail = TestHelpersCommonFields.Email };

            _verifyEmailModel = new VerificationKeyDto
            {
                VerificationKey = Key,
                Password = TestHelpersCommonFields.Password
            };

        }

        #region Post

        [TestMethod]
        public void ChangeEmailController_Posts_InternalServerError()
        {
            A.CallTo(
                () =>
                    _changeEmailService.ChangeEmail(A<ChangeEmailInputModelDto>.Ignored)).Throws(new Exception(TestHelpersCommonFields.ExceptionMessage));

            WebApiCommonAsserts.IsInternalServerError(Post());

            CallsHadToHappenChangeEmail(true);
        }

        [TestMethod]
        public void ChangeEmailController_Posts_BadRequest()
        {
            ReturnFromChangeEmailService(false);

            WebApiCommonAsserts.IsBadResult(Post());

            CallsHadToHappenChangeEmail(true);
        }

        [TestMethod]
        public void ChangeEmailController_Posts_OkResult()
        {
            ReturnFromChangeEmailService(true);

            WebApiCommonAsserts.IsOkResult(Post());

            CallsHadToHappenChangeEmail(true);
        }


        private IHttpActionResult Post()
        {
            return _changeEmail.Post(_changeEmailModel);
        }

        private void ReturnFromChangeEmailService(bool success)
        {
            A.CallTo(
                () =>
                    _changeEmailService.ChangeEmail(A<ChangeEmailInputModelDto>.Ignored)).Returns(new SuccessOrFailureDto{Success = success});
        }

        private void CallsHadToHappenChangeEmail(bool happened)
        {
            if (happened)
            {
                A.CallTo(() => _changeEmailService.ChangeEmail(
                    A<ChangeEmailInputModelDto>.That.Matches(
                        x => x.NewEmail == TestHelpersCommonFields.Email)))
                    .MustHaveHappened();
            }
            else
            {
                A.CallTo(() => _changeEmailService.ChangeEmail(
                    A<ChangeEmailInputModelDto>.That.Matches(
                        x => x.NewEmail == TestHelpersCommonFields.Email)))
                    .MustNotHaveHappened();
            }
        }

        #endregion


        #region ConfirmEmail

        [TestMethod]
        public void ChangeEmailController_ConfirmEmail_ReturnsAnErrorRedirect()
        {
            SetUpTestForSuccessOrFailureCalls(false);

            var result = _changeEmail.ConfirmEmail(_verifyEmailModel);

            WebApiCommonAsserts.BadResultHasTheCorrectErrorMessage(result, TestHelpersCommonFields.ExceptionMessage);

            CallsHadToHappen(false);
        }

        [TestMethod]
        public void ChangeEmailController_ConfirmEmail_ReturnsAnOkRedirect()
        {
            SetUpTestForSuccessOrFailureCalls(true);

            var result = _changeEmail.ConfirmEmail(_verifyEmailModel);

            WebApiCommonAsserts.IsOkResult(result);

            CallsHadToHappen(false);
        }

        [TestMethod]
        public void ChangeEmailController_ConfirmEmail_Exception()
        {
            CallToQueueSenderThrowsException(false);

            var result = _changeEmail.ConfirmEmail(_verifyEmailModel);

            WebApiCommonAsserts.IsInternalServerError(result);
        }

        #endregion

        #region CancelRegistration

        [TestMethod]
        public void RegisterController_CancelRegistration_ReturnsAnErrorRedirect()
        {
            SetUpTestForSuccessOrFailureCalls(false);

            var result = _changeEmail.CancelConfirmation(Key);
            WebApiCommonAsserts.IsBadResult(result);

            CallsHadToHappen(true);
        }

        [TestMethod]
        public void RegisterController_CancelRegistration_ReturnsAnOkRedirect()
        {
            SetUpTestForSuccessOrFailureCalls(true);

            var result = _changeEmail.CancelConfirmation(Key);
            WebApiCommonAsserts.IsOkResult(result);

            CallsHadToHappen(true);
        }

        [TestMethod]
        public void RegisterController_CancelRegistration_Exception()
        {
            CallToQueueSenderThrowsException(true);

            var result = _changeEmail.CancelConfirmation(Key);

            WebApiCommonAsserts.IsInternalServerError(result);
        }

        #endregion

        #region HelperMethodsForTheVerificationMethods



        private void SetUpTestForSuccessOrFailureCalls(bool success)
        {
            var successOrFailureDto = new SuccessOrFailureDto { Success = success };
            if (!success)
            {
                successOrFailureDto = new SuccessOrFailureDto
                {
                    Success = false,
                    ExceptionMessage = TestHelpersCommonFields.ExceptionMessage
                };
            }

            A.CallTo(
                () =>
                    _changeEmailService.VerifyEmailAddress(A<VerificationKeyDto>.Ignored)).Returns(successOrFailureDto);
        }

        private void CallToQueueSenderThrowsException(bool cancel)
        {
            A.CallTo(
                () =>
                    _changeEmailService.VerifyEmailAddress(
                        A<VerificationKeyDto>.That.Matches(
                            x => x.Cancel == cancel && x.VerificationKey == Key)))
                .Throws(new Exception());
        }

        private void CallsHadToHappen(bool cancel)
        {
            if (cancel)
            {
                A.CallTo(() => _changeEmailService.VerifyEmailAddress(
                    A<VerificationKeyDto>.That.Matches(
                        x => x.Cancel == cancel && x.VerificationKey == Key)))
                    .MustHaveHappened();
            }
            else
            {
                A.CallTo(() => _changeEmailService.VerifyEmailAddress(
                    A<VerificationKeyDto>.That.Matches(
                        x => x.Cancel == cancel && x.VerificationKey == Key && x.Password == TestHelpersCommonFields.Password)))
                    .MustHaveHappened();
            }
        }


        #endregion
    }
}
