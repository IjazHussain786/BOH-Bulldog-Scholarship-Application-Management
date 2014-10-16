using System;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.MembershipProvider.Tests.UnitTests.CommonStaticItemsForTests;
using BohFoundation.MembershipProvider.UserManagement.Manage.Implementation;
using BohFoundation.TestHelpers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.MembershipProvider.Tests.UnitTests.UserManagement.Manage
{
    [TestClass]
    public class ChangePasswordServicesTests
    {
        private ChangePasswordService _changePasswordServices;
        private ChangePasswordInputModelDto _changePasswordInputModelDto;
        private Guid _guid;

        [TestInitialize]
        public void Initialize()
        {
            MembershipProviderCommonFakes.CreateNewInstances();
            TestHelpersCommonFields.InitializeFields();

            _changePasswordServices = new ChangePasswordService(MembershipProviderCommonFakes.UserAccountService, MembershipProviderCommonFakes.ClaimsInformationGetters);

            _changePasswordInputModelDto = new ChangePasswordInputModelDto
            {
                NewPassword = TestHelpersCommonFields.Password + "new",
                OldPassword = TestHelpersCommonFields.Password
            };

            _guid = TestHelpersCommonFields.GuidOne;

            A.CallTo(() => MembershipProviderCommonFakes.ClaimsInformationGetters.GetUsersGuid()).Returns(_guid);

        }

        [TestMethod]
        public void ChangePassword_Works_CallsRightThings()
        {
            ChangePassword(true);
            //A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.ChangePassword(_guid, _changePasswordInputModelDto.OldPassword, _changePasswordInputModelDto.NewPassword)).MustHaveHappened();
        }

        [TestMethod]
        public void ChangePassword_Exception_ReturnsFalseSuccess()
        {
            A.CallTo(
                () =>
                    MembershipProviderCommonFakes.UserAccountService.ChangePassword(_guid, _changePasswordInputModelDto.OldPassword,
                        _changePasswordInputModelDto.NewPassword)).Throws(new Exception(TestHelpersCommonFields.ExceptionMessage));

            ChangePassword(false);

            //A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.ChangePassword(_guid, _changePasswordInputModelDto.OldPassword, _changePasswordInputModelDto.NewPassword)).MustHaveHappened();
        }

        private void ChangePassword(bool success)
        {
            var result = _changePasswordServices.ChangePassword(_changePasswordInputModelDto);
            MembershipProviderCommonAsserts.DidMethodCreateRightSuccess(success, result);
        }
    }
}
