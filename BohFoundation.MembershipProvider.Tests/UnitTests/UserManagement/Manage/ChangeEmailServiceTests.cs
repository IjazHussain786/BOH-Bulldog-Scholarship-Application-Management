using System;
using BohFoundation.Domain.Dtos;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.MembershipProvider.Tests.UnitTests.CommonStaticItemsForTests;
using BohFoundation.MembershipProvider.UserManagement.Manage.Implementation;
using BohFoundation.PersonsRepository.Repositories.Interfaces;
using BohFoundation.TestHelpers;
using BrockAllen.MembershipReboot.Relational;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.MembershipProvider.Tests.UnitTests.UserManagement.Manage
{
    [TestClass]
    public class ChangeEmailServiceTests
    {
        private ChangeEmailService _changeEmailService;
        private ChangeEmailInputModelDto _changeEmailInputModelDto;
        private VerificationKeyDto _verificationKey;
        private IChangeEmailRepository _changeEmailRepository;
        private const string Key = "key";

        [TestInitialize]
        public void Initialize()
        {
            MembershipProviderCommonFakes.CreateNewInstances();
            TestHelpersCommonFields.InitializeFields();

            _changeEmailRepository = A.Fake<IChangeEmailRepository>();

            _verificationKey = new VerificationKeyDto();

            _changeEmailService = new ChangeEmailService(MembershipProviderCommonFakes.UserAccountService, MembershipProviderCommonFakes.ClaimsInformationGetters, _changeEmailRepository);

            _changeEmailInputModelDto = new ChangeEmailInputModelDto { NewEmail = TestHelpersCommonFields.Email };

            A.CallTo(() => MembershipProviderCommonFakes.ClaimsInformationGetters.GetUsersGuid()).Returns(TestHelpersCommonFields.GuidOne);

        }

        #region ChangeEmail

        [TestMethod]
        public void ChangeEmail_AllWorks()
        {
            CallChangeEmailAndVerify(true);
        }

        [TestMethod]
        public void ChangeEmail_Error()
        {
            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.ChangeEmailRequest(TestHelpersCommonFields.GuidOne, TestHelpersCommonFields.Email))
                .Throws(new Exception(TestHelpersCommonFields.ExceptionMessage));

            CallChangeEmailAndVerify(false);
        }


        public void CallChangeEmailAndVerify(bool success)
        {
            var result = ChangeEmail();
            MembershipProviderCommonAsserts.DidMethodCreateRightSuccess(success, result);
            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.ChangeEmailRequest(TestHelpersCommonFields.GuidOne, TestHelpersCommonFields.Email)).MustHaveHappened();
        }

        public SuccessOrFailureDto ChangeEmail()
        {
            return _changeEmailService.ChangeEmail(_changeEmailInputModelDto);
        }

        #endregion

        #region ConfirmTests

        [TestMethod]
        public void ChangeEmailService_Confirm_Success()
        {
            _verificationKey.Password = TestHelpersCommonFields.Password;
            _verificationKey.Cancel = false;

            CallVerifyAndThenAsserts(true);
        }

        [TestMethod]
        public void ChangeEmailService_Confirm_Success_Should_Call_Change_Email_In_BOH_Db()
        {
            _verificationKey.Password = TestHelpersCommonFields.Password;
            _verificationKey.Cancel = false;

            var account = MembershipProviderCommonFakes.RelationalUserAccount;

            A.CallTo(
                () =>
                    MembershipProviderCommonFakes.UserAccountService.VerifyEmailFromKey(A<string>.Ignored,
                        A<string>.Ignored, out account)).AssignsOutAndRefParameters(account);

            CallVerify();

            A.CallTo(() => _changeEmailRepository.ChangeEmailAddress("", new Guid())).MustHaveHappened();
        }

        #endregion

        #region CancelTests

        [TestMethod]
        public void ChangeEmailService_Cancel_Error()
        {
            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.CancelVerification(A<string>.Ignored))
                .Throws(new Exception(TestHelpersCommonFields.ExceptionMessage));

            _verificationKey.Cancel = true;

            var result = CallVerify();
            MembershipProviderCommonAsserts.AssertAFalseSuccess(result);
            MembershipProviderCommonAsserts.AssertAFailureExceptionMessage(result);
        }

        [TestMethod]
        public void ChangeEmailService_Cancel_Success()
        {
            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.CancelVerification(A<string>.Ignored))
                .DoesNothing();

            _verificationKey.Cancel = true;

            CallVerifyAndThenAsserts(true);
        }

        private void CallVerifyAndThenAsserts(bool success)
        {
            var account = MembershipProviderCommonFakes.RelationalUserAccount;

            A.CallTo(
                () =>
                    MembershipProviderCommonFakes.UserAccountService.VerifyEmailFromKey(A<string>.Ignored,
                        A<string>.Ignored, out account)).AssignsOutAndRefParameters(account);

            var result = CallVerify();
            MembershipProviderCommonAsserts.DidMethodCreateRightSuccess(success, result);
        }

        private SuccessOrFailureDto CallVerify()
        {
            return _changeEmailService.VerifyEmailAddress(_verificationKey);
        }

        #endregion

    }
}
