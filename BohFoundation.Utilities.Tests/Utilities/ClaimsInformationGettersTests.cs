using System;
using BohFoundation.TestHelpers;
using BohFoundation.Utilities.Context.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.Utilities.Tests.Utilities
{
    [TestClass]
    public class ClaimsInformationGettersTests
    {
        private ClaimsInformationGetters _claimsInformationGetters;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestHelpersCommonFields.InitializeFields();
        }

        [TestInitialize]
        public void Initialize()
        {
            _claimsInformationGetters = new ClaimsInformationGetters();
            TestHelpersIdentity.PopulateIdentityWithNoPermissions(TestHelpersCommonFields.GuidOne, TestHelpersCommonFields.Email);
        }


        [TestMethod]
        public void ClaimsInformationGetters_GetUsersGraduatingYear_Should_ReturnTheGraduatingYearOnPrinciple()
        {
            Assert.AreEqual(TestHelpersCommonFields.GraduatingYear, _claimsInformationGetters.GetApplicantsGraduatingYear());
        }

        [TestMethod]
        public void ClaimsInformationGetters_GetUsersGuid_Should_ReturnTheGuidOnPrinciple()
        {
            Assert.AreEqual(TestHelpersCommonFields.GuidOne, _claimsInformationGetters.GetUsersGuid());
        }

        [TestMethod]
        public void ClaimsInformationGetters_GetUsersGuid_Should_Return_Null_For_AnonymousUsers()
        {
            TestHelpersIdentity.PopulateIdentityWithStandardAnon();
            Assert.AreEqual(new Guid(), _claimsInformationGetters.GetUsersGuid());
        }

        [TestMethod]
        public void ClaimsInformationGetters_GetUsersEmail_Should_ReturnTheEmailOnPrinciple()
        {
            Assert.AreEqual(TestHelpersCommonFields.Email, _claimsInformationGetters.GetUsersEmail());
        }

        [TestMethod]
        public void ClaimsInformationGetters_GetUsersFirstName_Should_ReturnTheFirstNameOnPrinciple()
        {
            Assert.AreEqual(TestHelpersCommonFields.FirstName, _claimsInformationGetters.GetUsersFirstName());
        }

        [TestMethod]
        public void ClaimsInformationGetters_GetUsersLastName_Should_ReturnTheLastNameOnPrinciple()
        {
            Assert.AreEqual(TestHelpersCommonFields.LastName, _claimsInformationGetters.GetUsersLastName());
        }

        [TestMethod]
        public void ClaimsInformationGetters_GetUsersFullName_Should_ReturnTheFullNameOnPrinciple()
        {
            Assert.AreEqual(TestHelpersCommonFields.FirstName + " " + TestHelpersCommonFields.LastName, _claimsInformationGetters.GetUsersFullName());
        }
        

        [TestMethod]
        public void ClaimsInformationGetters_IsAdmin_Should_Return_False_WhenUser_IsNotAnAdmin()
        {
            Assert.IsFalse(_claimsInformationGetters.IsAdmin());
        }

        [TestMethod]
        public void ClaimsInformationGetters_IsAdmin_Should_Return_True_WhenUser_IsAnAdmin()
        {
            TestHelpersIdentity.PopulateAdminIdentity(TestHelpersCommonFields.GuidOne, TestHelpersCommonFields.Email);
            Assert.IsTrue(_claimsInformationGetters.IsAdmin());
        }

        [TestMethod]
        public void ClaimsInformationGetters_IsApplicant_Should_Return_False_WhenUser_IsNot()
        {
            Assert.IsFalse(_claimsInformationGetters.IsApplicant());
        }

        [TestMethod]
        public void ClaimsInformationGetters_IsApplicant_Should_Return_True_WhenUser_Is()
        {
            TestHelpersIdentity.PopulateApplicantIdentity(TestHelpersCommonFields.GuidOne, TestHelpersCommonFields.Email);
            Assert.IsTrue(_claimsInformationGetters.IsApplicant());
        }

        [TestMethod]
        public void ClaimsInformationGetters_IsReference_Should_Return_False_WhenUser_IsNot()
        {
            Assert.IsFalse(_claimsInformationGetters.IsReference());
        }

        [TestMethod]
        public void ClaimsInformationGetters_IsReference_Should_Return_True_WhenUser_Is()
        {
            TestHelpersIdentity.PopulateReferenceIdentity(TestHelpersCommonFields.GuidOne, TestHelpersCommonFields.Email);
            Assert.IsTrue(_claimsInformationGetters.IsReference());
        }

        [TestMethod]
        public void ClaimsInformationGetters_IsApplicationEvaluator_Should_Return_False_WhenUser_IsNot()
        {
            Assert.IsFalse(_claimsInformationGetters.IsApplicationEvaluator());
        }

        [TestMethod]
        public void ClaimsInformationGetters_IsApplicationEvaluator_Should_Return_True_WhenUser_Is()
        {
            TestHelpersIdentity.PopulateApplicationEvaluatorIdentity(TestHelpersCommonFields.GuidOne, TestHelpersCommonFields.Email);
            Assert.IsTrue(_claimsInformationGetters.IsApplicationEvaluator());
        }

        [TestMethod]
        public void ClaimsInformationGetters_IsApplicationEvaluatorPendingConfirmantion_Should_Return_False_WhenUser_IsNot()
        {
            Assert.IsFalse(_claimsInformationGetters.IsApplicationEvaluatorPendingConfirmation());
        }

        [TestMethod]
        public void ClaimsInformationGetters_IsApplicationEvaluatorPendingConfirmantion_Should_Return_True_WhenUser_Is()
        {
            TestHelpersIdentity.PopulateApplicationEvaluatorPendingConfirmationIdentity(TestHelpersCommonFields.GuidOne, TestHelpersCommonFields.Email);
            Assert.IsTrue(_claimsInformationGetters.IsApplicationEvaluatorPendingConfirmation());
        }
    }
}
