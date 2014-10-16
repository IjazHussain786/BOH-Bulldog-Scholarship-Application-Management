using System;
using BohFoundation.Domain.Dtos;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.Enums;
using BohFoundation.MembershipProvider.Tests.UnitTests.CommonStaticItemsForTests;
using BohFoundation.MembershipProvider.UserManagement.Manage.Implementation;
using BohFoundation.TestHelpers;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.MembershipProvider.Tests.UnitTests.UserManagement.Manage
{
    [TestClass]
    public class RegisterUserServiceTests
    {
        private RegisterUserService _registerApplicant;
        
        private SuccessOrFailureDto Result { get; set; }
        private RegisterInputModel RegisterApplicationInputModel { get; set; }
        private RelationalUserAccount RelationalUserAccount { get; set; }
        private UserClaimCollection UserClaimsCollection { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            MembershipProviderCommonFakes.CreateNewInstances();

            RelationalUserAccount = A.Fake<RelationalUserAccount>();
            UserClaimsCollection = new UserClaimCollection();

            A.CallTo(
                () =>
                    MembershipProviderCommonFakes.UserAccountService.CreateAccount(null, TestHelpersCommonFields.Password,
                        TestHelpersCommonFields.Email, null, null)).Returns(RelationalUserAccount);

            A.CallTo(
                () =>
                     MembershipProviderCommonFakes.CreateUserClaimCollection.CreateClaimsCollection(A<RegisterInputModel>.Ignored, MemberTypesEnum.Applicant))
                        .Returns(UserClaimsCollection);
            
            RegisterApplicationInputModel = new RegisterInputModel{EmailAddress = TestHelpersCommonFields.Email, FirstName = TestHelpersCommonFields.FirstName, LastName = TestHelpersCommonFields.LastName, GraduatingYear = TestHelpersCommonFields.GraduatingYear, Password = TestHelpersCommonFields.Password};
     
            _registerApplicant = new RegisterUserService(MembershipProviderCommonFakes.UserAccountService, MembershipProviderCommonFakes.CreateUserClaimCollection, MembershipProviderCommonFakes.PersonsRepository);
        }

        [TestMethod]
        public void RegisterUserService_CreateAccount_Should_ReturnASuccessOrFailureDto()
        {
            CallRegisterApplicant();
            Assert.IsInstanceOfType(Result, typeof(SuccessOrFailureDto));
        }

        [TestMethod]
        public void RegisterUserService_CreateAccount_Should_HaveCalled_UserAccountService_With_Password_Email()
        {
            CallRegisterApplicant();
            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.CreateAccount(null, TestHelpersCommonFields.Password, TestHelpersCommonFields.Email, null, null)).MustHaveHappened();
        }

        #region CreateAccountThrowsException

        [TestMethod]
        public void RegisterUserService_CreateAccount_Should_ReturnAFailureDto_When_CreateAccountFails()
        {
            CreateAccountThrowsException();
            CallRegisterApplicant();
            Assert.IsFalse(Result.Success);
        }

        [TestMethod]
        public void RegisterUserService_CreateAccount_Should_ReturnAExceptionMessage_When_CreateAccountFails()
        {
            CreateAccountThrowsException();
            CallRegisterApplicant();
            Assert.AreEqual(TestHelpersCommonFields.ExceptionMessage, Result.ExceptionMessage);
        }

        private void CreateAccountThrowsException()
        {
            A.CallTo(
                () =>
                    MembershipProviderCommonFakes.UserAccountService.CreateAccount(null, TestHelpersCommonFields.Password,
                        TestHelpersCommonFields.Email, null, null))
                .Throws(new Exception(TestHelpersCommonFields.ExceptionMessage));
        }

        #endregion

        #region AddingClaimsThrowsException

        [TestMethod]
        public void RegisterUserService_CreateAccount_Should_ReturnAFailureDto_When_AddingClaimsFails()
        {
            AddClaimsThrowsException();
            CallRegisterApplicant();
            Assert.IsFalse(Result.Success);
        }

        [TestMethod]
        public void RegisterUserService_CreateAccount_Should_ReturnAExceptionMessage_When_AddingClaimsFails()
        {
            AddClaimsThrowsException();
            CallRegisterApplicant();
            Assert.AreEqual(TestHelpersCommonFields.ExceptionMessage + 2, Result.ExceptionMessage);
        }

        private void AddClaimsThrowsException()
        {
            A.CallTo(
                () =>
                    MembershipProviderCommonFakes.UserAccountService.AddClaims(A<Guid>.Ignored, A<UserClaimCollection>.Ignored))
                .Throws(new Exception(TestHelpersCommonFields.ExceptionMessage + 2));
        }

        #endregion

        [TestMethod]
        public void RegisterUserService_CreateAccount_Should_HaveCalled_AddClaims_With_AllClaimsINeed()
        {
            CallRegisterApplicant();
            
            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.AddClaims(A<Guid>.Ignored, UserClaimsCollection)).MustHaveHappened();
        }

        [TestMethod]
        public void RegisterUserService_CreateAccount_Should_HaveCalled_CreatePerson_WithNameAndGuid()
        {
            CallRegisterApplicant();

            A.CallTo(
                () =>
                    MembershipProviderCommonFakes.PersonsRepository.CreatePerson(A<Guid>.Ignored,
                        A<Name>.That.Matches(
                            x =>
                                x.FirstName == TestHelpersCommonFields.FirstName &&
                                x.LastName == TestHelpersCommonFields.LastName), MemberTypesEnum.Applicant)).MustHaveHappened();
        }

        [TestMethod]
        public void RegisterUserService_CreateAccount_Should_HaveCalled_CreateUserClaimCollection_With_Model()
        {
            CallRegisterApplicant();

            A.CallTo(
                () =>
                    MembershipProviderCommonFakes.CreateUserClaimCollection.CreateClaimsCollection(
                        RegisterApplicationInputModel, MemberTypesEnum.Applicant)).MustHaveHappened();
        }

        [TestMethod]
        public void RegisterUserService_CreateAccount_HappyPath_Should_ReturnASuccessDto()
        {
            CallRegisterApplicant();
            Assert.IsTrue(Result.Success);
        }

        private void CallRegisterApplicant()
        {
            Result = _registerApplicant.CreateAccount(RegisterApplicationInputModel, MemberTypesEnum.Applicant);
        }
    }
}
