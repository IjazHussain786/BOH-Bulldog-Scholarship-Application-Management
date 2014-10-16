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
    public class ChangePasswordControllerTests
    {
        private ChangePasswordController _changePassword;
        private ChangePasswordInputModelDto _changePasswordModel;
        private IChangePasswordService _changePasswordService;

        [TestInitialize]
        public void Initialize()
        {
            _changePasswordService = A.Fake<IChangePasswordService>();
            _changePassword = new ChangePasswordController(_changePasswordService);
            _changePasswordModel = new ChangePasswordInputModelDto
            {
                NewPassword = TestHelpersCommonFields.Password + "new",
                OldPassword = TestHelpersCommonFields.Password
            };
        }

        [TestMethod]
        public void ChangePasswordController_Posts_InternalServerError()
        {
            A.CallTo(
                () =>
                    _changePasswordService.ChangePassword(A<ChangePasswordInputModelDto>.Ignored)).Throws(new Exception(TestHelpersCommonFields.ExceptionMessage));

            WebApiCommonAsserts.IsInternalServerError(Post());

            CallsHadToHappen(true);
        }

        [TestMethod]
        public void ChangePasswordController_Posts_BadRequest()
        {
            ReturnFromChangePasswordServices(false);

            WebApiCommonAsserts.IsBadResult(Post());

            CallsHadToHappen(true);
        }

        [TestMethod]
        public void ChangePasswordController_Posts_OkResult()
        {
            ReturnFromChangePasswordServices(true);

            WebApiCommonAsserts.IsOkResult(Post());

            CallsHadToHappen(true);
        }

        private IHttpActionResult Post()
        {
            return _changePassword.Post(_changePasswordModel);
        }

        private void ReturnFromChangePasswordServices(bool success)
        {
            A.CallTo(
                () =>
                    _changePasswordService.ChangePassword(A<ChangePasswordInputModelDto>.Ignored))
                        .Returns(new SuccessOrFailureDto { Success = success });
        }

        private void CallsHadToHappen(bool happened)
        {
            if (happened)
            {
                A.CallTo(() => _changePasswordService.ChangePassword(
                    A<ChangePasswordInputModelDto>.That.Matches(
                        x => x.OldPassword == TestHelpersCommonFields.Password && x.NewPassword == TestHelpersCommonFields.Password + "new")))
                    .MustHaveHappened();
            }
            else
            {
                A.CallTo(() => _changePasswordService.ChangePassword(
                    A<ChangePasswordInputModelDto>.That.Matches(
                        x => x.OldPassword == TestHelpersCommonFields.Password && x.NewPassword == TestHelpersCommonFields.Password + "new")))
                    .MustNotHaveHappened();
            }
        }
    }
}
