using System;
using System.Collections.Generic;
using System.Security.Claims;
using BohFoundation.Domain.Claims;
using BohFoundation.Domain.Dtos.Admin;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.Enums;
using BohFoundation.MembershipProvider.Repositories.Interfaces;
using BohFoundation.MembershipProvider.Tests.UnitTests.CommonStaticItemsForTests;
using BohFoundation.MembershipProvider.UserManagement.Admin.Implementation;
using BohFoundation.TestHelpers;
using BohFoundation.Utilities.Context.Interfaces;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.MembershipProvider.Tests.UnitTests.UserManagement.Admin
{
    [TestClass]
    public class ConfirmApplicationEvaluatorTests
    {
        private ConfirmApplicationEvaluator _confirmApplicationEvaluator;
        private IClaimsInformationGetters _claimsInformationGetters;
        private IMembershipRebootCustomQueries _membershipRebootCustomQueuries;

        [TestInitialize]
        public void Initialize()
        {
            MembershipProviderCommonFakes.CreateNewInstances();
            TestHelpersCommonFields.InitializeFields();

            _claimsInformationGetters = A.Fake<IClaimsInformationGetters>();
            _membershipRebootCustomQueuries = A.Fake<IMembershipRebootCustomQueries>();

            _confirmApplicationEvaluator = new ConfirmApplicationEvaluator(_claimsInformationGetters, _membershipRebootCustomQueuries, MembershipProviderCommonFakes.UserAccountService, MembershipProviderCommonFakes.CreateClaimsListFromAccount, MembershipProviderCommonFakes.PersonsRepository);

            A.CallTo(() => _claimsInformationGetters.IsAdmin()).Returns(true);
        }

        #region GetPendingApplicationEvaluators

        [TestMethod]
        public void ConfirmApplicationEvaluator_GetPendingApplicationEvaluators_Should_AlwaysSeeIfTheUserInAnAdmin()
        {
            CallGetPendingApplicationEvaluators();
            A.CallTo(() => _claimsInformationGetters.IsAdmin()).MustHaveHappened();
        }

        [TestMethod, ExpectedException(typeof (UnauthorizedAccessException))]
        public void ConfirmApplicationEvaluator_GetPendingApplicationEvaluators_NotAdmin_ThrowException()
        {
            A.CallTo(() => _claimsInformationGetters.IsAdmin()).Returns(false);
            CallGetPendingApplicationEvaluators();
        }

        [TestMethod]
        public void
            ConfirmApplicationEvaluator_GetPendingApplicationEvaluators_Should_CallMembershipRebootCustomQueuries_GetPendingApplicationEvaluators()
        {
            CallGetPendingApplicationEvaluators();
            A.CallTo(()=>_membershipRebootCustomQueuries.GetPendingApplicationEvaluators()).MustHaveHappened();
        }

        [TestMethod]
        public void
            ConfirmApplicationEvaluator_GetPendingApplicationEvaluators_Should_Return_WhatGetPendingApplicationEvaluatorsReturns()
        {
            var returnList = new List<PersonDto>();
            A.CallTo(() => _membershipRebootCustomQueuries.GetPendingApplicationEvaluators()).Returns(returnList);
            Assert.AreSame(returnList, CallGetPendingApplicationEvaluators());
        }

        private List<PersonDto> CallGetPendingApplicationEvaluators()
        {
            return _confirmApplicationEvaluator.GetPendingApplicationEvaluators();
        }

        #endregion

        #region CountOfPendingApplicationEvaluators

        [TestMethod]
        public void ConfirmApplicationEvaluator_CountOfPendingApplicationEvaluators_Should_AlwaysSeeIfTheUserInAnAdmin()
        {
            CountOfPendingApplicationEvaluators();
            A.CallTo(() => _claimsInformationGetters.IsAdmin()).MustHaveHappened();
        }

        [TestMethod, ExpectedException(typeof (UnauthorizedAccessException))]
        public void ConfirmApplicationEvaluator_CountOfPendingApplicationEvaluators_NotAdmin_ThrowException()
        {
            A.CallTo(() => _claimsInformationGetters.IsAdmin()).Returns(false);
            CountOfPendingApplicationEvaluators();
        }
        
        [TestMethod]
        public void ConfirmApplicationEvaluator_CountOfPendingApplicationEvaluators_Should_CallCountPendingApplicationEvaluators()
        {
            CountOfPendingApplicationEvaluators();
            A.CallTo(() => _membershipRebootCustomQueuries.CountPendingApplicationEvaluators()).MustHaveHappened();
        }

        [TestMethod]
        public void ConfirmApplicationEvaluator_CountOfPendingApplicationEvaluators_Should_ReturnWhatCountPendingApplicationEvaluatorsReturns()
        {
            const int amount = 92190;
            A.CallTo(() => _membershipRebootCustomQueuries.CountPendingApplicationEvaluators()).Returns(amount);
            Assert.AreEqual(amount, CountOfPendingApplicationEvaluators());    
        }

        private int CountOfPendingApplicationEvaluators()
        {
            return _confirmApplicationEvaluator.CountOfPendingApplicationEvaluators();
        }

        #endregion

        #region ConfirmOrRejectApplicationEvaluator
        
        [TestMethod]
        public void ConfirmApplicationEvaluator_ConfirmOrRejectApplicationEvaluator_Should_AlwaysSeeIfTheUserInAnAdmin()
        {
            ConfirmOrRejectApplicationEvaluator(true);
            A.CallTo(() => _claimsInformationGetters.IsAdmin()).MustHaveHappened();
        }

        [TestMethod, ExpectedException(typeof (UnauthorizedAccessException))]
        public void ConfirmApplicationEvaluator_ConfirmOrRejectApplicationEvaluator_NotAdmin_ThrowException()
        {
            A.CallTo(() => _claimsInformationGetters.IsAdmin()).Returns(false);
            ConfirmOrRejectApplicationEvaluator(true);
        }

        [TestMethod]
        public void ConfirmApplicationEvaluator_ConfirmOrRejectApplicationEvaluator_Should_GetMemberFromEmail()
        {
            ConfirmOrRejectApplicationEvaluator(true);
            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.GetByEmail(TestHelpersCommonFields.Email)).MustHaveHappened();
        }

        [TestMethod]
        public void ConfirmApplicationEvaluator_ConfirmOrRejectApplicationEvaluator_IfConfirmIsTrue_Should_RemovePendingClaim()
        {
            ConfirmOrRejectApplicationEvaluator(true);
            A.CallTo(
                () =>
                    _membershipRebootCustomQueuries.RemovePendingApplicationEvaluatorClaim(0)).MustHaveHappened();
        }

        [TestMethod]
        public void ConfirmApplicationEvaluator_ConfirmOrRejectApplicationEvaluator_IfConfirmIsTrue_Should_AddIsEvaluator()
        {
            ConfirmOrRejectApplicationEvaluator(true);
            A.CallTo(
                () =>
                    MembershipProviderCommonFakes.UserAccountService.AddClaim(new Guid(),
                        ClaimsNames.ApplicationEvaluator, true.ToString())).MustHaveHappened();
        }

        [TestMethod]
        public void ConfirmApplicationEvaluator_ConfirmOrRejectApplicationEvaluator_IfConfirmIsFalse_Should_DeleteAccount()
        {
            ConfirmOrRejectApplicationEvaluator(false);
            A.CallTo(
                () =>
                    MembershipProviderCommonFakes.UserAccountService.DeleteAccount(new Guid())).MustHaveHappened();
        }

        [TestMethod]
        public void ConfirmApplicationEvaluator_ConfirmOrRejectApplicationEvaluator_IfConfirmIsTrue_IsAdmin_Should_AddAdminClaim()
        {
            ConfirmOrRejectApplicationEvaluator(true, true);
            A.CallTo(
                () =>
                    MembershipProviderCommonFakes.UserAccountService.AddClaim(new Guid(),
                        ClaimsNames.Admin, true.ToString())).MustHaveHappened();
        }

        [TestMethod]
        public void ConfirmApplicationEvaluator_ConfirmOrRejectApplicationEvaluator_IfConfirmIsTrue_IsNotAdmin_Should_Not_AddAdminClaim()
        {
            ConfirmOrRejectApplicationEvaluator(true);
            A.CallTo(
                () =>
                    MembershipProviderCommonFakes.UserAccountService.AddClaim(new Guid(),
                        ClaimsNames.Admin, true.ToString())).MustNotHaveHappened();
        }

        [TestMethod]
        public void ConfirmApplicationEvaluator_ConfirmOrRejectApplicationEvaluator_IfConfirmIsTrue_IsNotAdmin_Should_Call_PersonsRepo_WithApplicationEvaluator()
        {
            ConfirmOrRejectApplicationEvaluator(true);
            A.CallTo(()=>MembershipProviderCommonFakes.PersonsRepository.CreatePerson(new Guid(), A<Name>.That.Matches(name=>name.FirstName == TestHelpersCommonFields.FirstName && name.LastName == TestHelpersCommonFields.LastName), MemberTypesEnum.ApplicationEvaluator)).MustHaveHappened();
        }

        [TestMethod]
        public void ConfirmApplicationEvaluator_ConfirmOrRejectApplicationEvaluator_IfConfirmIsTrue_IsNotAdmin_Should_Not_Call_PersonsRepo_WithAdmin()
        {
            ConfirmOrRejectApplicationEvaluator(true);
            A.CallTo(() => MembershipProviderCommonFakes.PersonsRepository.CreatePerson(new Guid(), A<Name>.That.Matches(name => name.FirstName == TestHelpersCommonFields.FirstName && name.LastName == TestHelpersCommonFields.LastName), MemberTypesEnum.Admin)).MustNotHaveHappened();
        }

        [TestMethod]
        public void ConfirmApplicationEvaluator_ConfirmOrRejectApplicationEvaluator_IfConfirmIsTrue_IsAdmin_ShouldNot_Call_PersonsRepo_WithApplicationEvaluator()
        {
            ConfirmOrRejectApplicationEvaluator(true, true);
            A.CallTo(() => MembershipProviderCommonFakes.PersonsRepository.CreatePerson(new Guid(), A<Name>.That.Matches(name => name.FirstName == TestHelpersCommonFields.FirstName && name.LastName == TestHelpersCommonFields.LastName), MemberTypesEnum.ApplicationEvaluator)).MustNotHaveHappened();
        }

        [TestMethod]
        public void ConfirmApplicationEvaluator_ConfirmOrRejectApplicationEvaluator_IfConfirmIsTrue_IsAdmin_Should_Call_PersonsRepo_WithAdmin()
        {
            ConfirmOrRejectApplicationEvaluator(true, true);
            A.CallTo(() => MembershipProviderCommonFakes.PersonsRepository.CreatePerson(new Guid(), A<Name>.That.Matches(name => name.FirstName == TestHelpersCommonFields.FirstName && name.LastName == TestHelpersCommonFields.LastName), MemberTypesEnum.Admin)).MustHaveHappened();

        }

        private void ConfirmOrRejectApplicationEvaluator(bool confirm, bool isAdmin)
        {
            var dto = new ConfirmApplicationEvaluatorDto
            {
                Confirm = confirm,
                EmailAddress = TestHelpersCommonFields.Email,
                CreateAdmin = isAdmin
            };

            CallConfirmOrReject(dto);
        }

        private void ConfirmOrRejectApplicationEvaluator(bool confirm)
        {
            var dto = new ConfirmApplicationEvaluatorDto
            {
                Confirm = confirm,
                EmailAddress = TestHelpersCommonFields.Email
            };

            CallConfirmOrReject(dto);
        }

        private void CallConfirmOrReject(ConfirmApplicationEvaluatorDto dto)
        {
            AddClaims();

            A.CallTo(() => MembershipProviderCommonFakes.UserAccountService.GetByEmail(A<string>.Ignored))
                .Returns(MembershipProviderCommonFakes.RelationalUserAccount);
            
            _confirmApplicationEvaluator.ConfirmOrRejectApplicationEvaluator(dto);
        }

        private void AddClaims()
        {
            var firstNameClaim = new Claim(ClaimsNames.FirstName, TestHelpersCommonFields.FirstName);
            var lastNameClaim = new Claim(ClaimsNames.LastName, TestHelpersCommonFields.LastName);
            var listOfClaims = new List<Claim> {firstNameClaim, lastNameClaim};
            A.CallTo(
                () =>
                    MembershipProviderCommonFakes.CreateClaimsListFromAccount.CreateClaimsList(
                        MembershipProviderCommonFakes.RelationalUserAccount)).Returns(listOfClaims);
        }

        #endregion

    }
}
