using BohFoundation.Domain.Dtos;
using BohFoundation.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.MembershipProvider.Tests.UnitTests.CommonStaticItemsForTests
{
    public static class MembershipProviderCommonAsserts
    {
        public static void AssertAFailureExceptionMessage(SuccessOrFailureDto result)
        {
            Assert.AreEqual(TestHelpersCommonFields.ExceptionMessage, result.ExceptionMessage);
        }

        public static void AssertATrueSuccess(SuccessOrFailureDto result)
        {
            Assert.IsInstanceOfType(result, typeof (SuccessOrFailureDto));
            Assert.AreEqual(true, result.Success);
        }

        public static void AssertAFalseSuccess(SuccessOrFailureDto result)
        {
            Assert.IsInstanceOfType(result, typeof (SuccessOrFailureDto));
            Assert.AreEqual(false, result.Success);
        }

        public static void DidMethodCreateRightSuccess(bool success, SuccessOrFailureDto result)
        {
            if (success)
            {
                AssertATrueSuccess(result);
            }
            else
            {
                AssertAFalseSuccess(result);
                AssertAFailureExceptionMessage(result);
            }
        }

    }
}
