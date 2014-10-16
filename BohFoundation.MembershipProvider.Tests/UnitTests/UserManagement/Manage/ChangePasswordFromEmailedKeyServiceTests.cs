using System;
using BohFoundation.Domain.Dtos;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.MembershipProvider.Tests.UnitTests.CommonStaticItemsForTests;
using BohFoundation.MembershipProvider.UserManagement.Manage.Implementation;
using BohFoundation.TestHelpers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.MembershipProvider.Tests.UnitTests.UserManagement.Manage
{
    [TestClass]
    public class ChangePasswordFromEmailedKeyServiceTests
    {
        private ChangePasswordFromEmailKeyService _changePassword;
        private const string Key = "key";
        private ChangePasswordFromResetKeyDto _changePasswordFromKeyDto;

        [TestInitialize]
        public void Initialize()
        {
            MembershipProviderCommonFakes.CreateNewInstances();

            _changePassword = new ChangePasswordFromEmailKeyService(MembershipProviderCommonFakes.UserAccountService);

            _changePasswordFromKeyDto = new ChangePasswordFromResetKeyDto
            {
                Key = Key,
                NewPassword = TestHelpersCommonFields.Password
            };
        }

        #region ResetPassword

        [TestMethod]
        public void ResetPassword_HappyPath()
        {
            ResetPasswordWithAsserts(true);

            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.ResetPassword(TestHelpersCommonFields.Email)).MustHaveHappened();
        }

        [TestMethod]
        public void ResetPassword_Error_ReturnsFailureSuccess()
        {
            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.ResetPassword(TestHelpersCommonFields.Email))
                .Throws(new Exception(TestHelpersCommonFields.ExceptionMessage));

            ResetPasswordWithAsserts(false);
        }

        private void ResetPasswordWithAsserts(bool success)
        {
            var result = _changePassword.ResetPasswordRequest(new ResetPasswordThruEmailDto { EmailAddress = TestHelpersCommonFields.Email });
            MembershipProviderCommonAsserts.DidMethodCreateRightSuccess(success, result);
        }

        #endregion

        #region ChangePasswordFromResetKey

        [TestMethod]
        public void ChangePasswordFromResetKey_HappyPath()
        {
            FakeChangePasswordFromResetKeyReturns(true);

            ChangePasswordFromResetKeyWithAsserts(true);
        }

        [TestMethod]
        public void ChangePasswordFromResetKey_ThereWasAnError()
        {
            FakeChangePasswordFromResetKeyReturns(false);

            var result = ChangePasswordFromResetKey();
            MembershipProviderCommonAsserts.AssertAFalseSuccess(result);
        }

        [TestMethod]
        public void ChangePasswordFromResetKey_ThrowsExcpetion()
        {
            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.ChangePasswordFromResetKey(Key, TestHelpersCommonFields.Password))
                .Throws(new Exception(TestHelpersCommonFields.ExceptionMessage));

            ChangePasswordFromResetKeyWithAsserts(false);
        }

        private void ChangePasswordFromResetKeyWithAsserts(bool success)
        {
            var result = ChangePasswordFromResetKey();

            MembershipProviderCommonAsserts.DidMethodCreateRightSuccess(success, result);
        }

        private SuccessOrFailureDto ChangePasswordFromResetKey()
        {
            return _changePassword.ChangePasswordFromResetKey(_changePasswordFromKeyDto);
        }

        private void FakeChangePasswordFromResetKeyReturns(bool success)
        {
            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.ChangePasswordFromResetKey(Key, TestHelpersCommonFields.Password))
                .Returns(success);
        }

        #endregion

    }
}
