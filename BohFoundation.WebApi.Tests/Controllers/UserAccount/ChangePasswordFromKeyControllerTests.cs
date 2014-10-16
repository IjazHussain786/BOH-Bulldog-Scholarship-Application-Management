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
    public class ChangePasswordFromKeyControllerTests
    {
        private ChangePasswordFromKeyController _changePasswordFromKeyController;
        private ResetPasswordThruEmailDto _changePasswordFromKeyRequestModel;
        private ChangePasswordFromResetKeyDto _changePasswordFromKeyModel;
        private IChangePasswordFromEmailedKeyService _changePasswordFromKeyService;
        private const string Key = "Key";

        [TestInitialize]
        public void Initialize()
        {
            _changePasswordFromKeyService = A.Fake<IChangePasswordFromEmailedKeyService>();
            _changePasswordFromKeyController = new ChangePasswordFromKeyController(_changePasswordFromKeyService);
            _changePasswordFromKeyRequestModel = new ResetPasswordThruEmailDto { EmailAddress = TestHelpersCommonFields.Email };
            _changePasswordFromKeyModel = new ChangePasswordFromResetKeyDto
            {
                Key = Key,
                NewPassword = TestHelpersCommonFields.Password
            };
        }

        #region Post

        [TestMethod]
        public void ChangeUserNameController_Posts_InternalServerError()
        {
            A.CallTo(
                () =>
                   _changePasswordFromKeyService.ResetPasswordRequest(A<ResetPasswordThruEmailDto>.Ignored)).Throws(new Exception(TestHelpersCommonFields.ExceptionMessage));

            WebApiCommonAsserts.IsInternalServerError(Post());

            CallsHadToHappenPost();
        }

        [TestMethod]
        public void ChangeUserNameController_Posts_BadRequest()
        {
            ReturnFromResetPassword(false);

            WebApiCommonAsserts.IsBadResultWithMessage(Post());

            CallsHadToHappenPost();
        }

        [TestMethod]
        public void ChangeUserNameController_Posts_OkResult()
        {
            ReturnFromResetPassword(true);

            WebApiCommonAsserts.IsOkResult(Post());

            CallsHadToHappenPost();
        }

        private IHttpActionResult Post()
        {
            return _changePasswordFromKeyController.Post(_changePasswordFromKeyRequestModel);
        }

        private void ReturnFromResetPassword(bool success)
        {
            A.CallTo(
                () =>
                    _changePasswordFromKeyService.ResetPasswordRequest(A<ResetPasswordThruEmailDto>.Ignored)).Returns(new SuccessOrFailureDto { Success = success });
        }

        private void CallsHadToHappenPost()
        {
            A.CallTo(() => _changePasswordFromKeyService.ResetPasswordRequest(
                A<ResetPasswordThruEmailDto>.That.Matches(
                    x => x.EmailAddress == TestHelpersCommonFields.Email)))
                .MustHaveHappened();
        }

        #endregion

        #region ChangePasswordFromResetKey


        [TestMethod]
        public void ChangeUserNameController_ChangePasswordFromResetKey_InternalServerError()
        {
            A.CallTo(
                () =>
                    _changePasswordFromKeyService.ChangePasswordFromResetKey(A<ChangePasswordFromResetKeyDto>.Ignored))
                        .Throws(new Exception(TestHelpersCommonFields.ExceptionMessage));

            WebApiCommonAsserts.IsInternalServerError(ChangePasswordFromResetKey());

            CallsHadToHappenChangePasswordFromResetKey();
        }

        [TestMethod]
        public void ChangeUserNameController_ChangePasswordFromResetKey_BadRequest()
        {
            ReturnFromChangePasswordFromResetKey(false);

            WebApiCommonAsserts.IsBadResult(ChangePasswordFromResetKey());

            CallsHadToHappenChangePasswordFromResetKey();
        }

        [TestMethod]
        public void ChangeUserNameController_ChangePasswordFromResetKey_OkResult()
        {
            ReturnFromChangePasswordFromResetKey(true);

            WebApiCommonAsserts.IsOkResult(ChangePasswordFromResetKey());

            CallsHadToHappenChangePasswordFromResetKey();
        }

        private IHttpActionResult ChangePasswordFromResetKey()
        {
            return _changePasswordFromKeyController.ChangePasswordFromResetKey(_changePasswordFromKeyModel);
        }

        private void ReturnFromChangePasswordFromResetKey(bool success)
        {
            A.CallTo(
                () =>
                   _changePasswordFromKeyService.ChangePasswordFromResetKey(A<ChangePasswordFromResetKeyDto>.Ignored))
                        .Returns(new SuccessOrFailureDto { Success = success });
        }

        private void CallsHadToHappenChangePasswordFromResetKey()
        {
            A.CallTo(() => _changePasswordFromKeyService.ChangePasswordFromResetKey(
                A<ChangePasswordFromResetKeyDto>.That.Matches(
                    x => x.NewPassword == TestHelpersCommonFields.Password && x.Key == Key)))
                .MustHaveHappened();

        }

        #endregion
    }
}
