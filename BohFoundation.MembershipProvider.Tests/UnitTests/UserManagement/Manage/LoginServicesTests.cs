using System;
using System.Collections.Generic;
using System.Security.Claims;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.MembershipProvider.Tests.UnitTests.CommonStaticItemsForTests;
using BohFoundation.MembershipProvider.UserManagement.Helpers.Interfaces;
using BohFoundation.MembershipProvider.UserManagement.Manage.Implementation;
using BohFoundation.TestHelpers;
using BrockAllen.MembershipReboot.Relational;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.MembershipProvider.Tests.UnitTests.UserManagement.Manage
{
    [TestClass]
    public class LoginServicesTests
    {
        private LoginInputDto _loginInput;
        private LoginServices _loginServices;
        private ICreateClaimsListFromAccount _createClaims;
        
        [TestInitialize]
        public void Initialize()
        {

            _createClaims = A.Fake<ICreateClaimsListFromAccount>();
            MembershipProviderCommonFakes.CreateNewInstances();

            _loginServices = new LoginServices(MembershipProviderCommonFakes.UserAccountService, _createClaims);

            _loginInput = new LoginInputDto
            {
                Password = TestHelpersCommonFields.Password,
                EmailAddress = TestHelpersCommonFields.Email
            };

            A.CallTo(() => _createClaims.CreateClaimsList(A<RelationalUserAccount>.Ignored))
                .Returns(new List<Claim>());
        }

        [TestMethod]
        public void LoginServices_Login_Exception()
        {
            RelationalUserAccount account;
            A.CallTo(
                () =>
                    MembershipProviderCommonFakes.UserAccountService.AuthenticateWithUsernameOrEmail(A<string>.Ignored, A<string>.Ignored,
                        out account)).Throws(new Exception(TestHelpersCommonFields.ExceptionMessage));

            LoginAndThenAsserts(false);
        }

        [TestMethod]
        public void LoginServices_Login_BadPassword()
        {
            RelationalUserAccount account;
            A.CallTo(
                () =>
                    MembershipProviderCommonFakes.UserAccountService.AuthenticateWithUsernameOrEmail(A<string>.Ignored, A<string>.Ignored,
                        out account)).Returns(false);

            var result = Login();
            MembershipProviderCommonAsserts.AssertAFalseSuccess(result);
        }

        [TestMethod]
        public void LoginServices_Login_AllGood()
        {
            RelationalUserAccount account;

            A.CallTo(
                () =>
                    MembershipProviderCommonFakes.UserAccountService.AuthenticateWithUsernameOrEmail(A<string>.Ignored, A<string>.Ignored,
                        out account)).Returns(true).AssignsOutAndRefParameters(new RelationalUserAccount());

            var result = Login();
            MembershipProviderCommonAsserts.AssertATrueSuccess(result);
            Assert.IsInstanceOfType(result.Claims, typeof(List<Claim>));
        }

        private void LoginAndThenAsserts(bool success)
        {
            var result = Login();
            MembershipProviderCommonAsserts.DidMethodCreateRightSuccess(success, result);
        }

        private SuccessOrFailureDtoWithClaims Login()
        {
            return _loginServices.LogIn(_loginInput);
        }
    }
}
