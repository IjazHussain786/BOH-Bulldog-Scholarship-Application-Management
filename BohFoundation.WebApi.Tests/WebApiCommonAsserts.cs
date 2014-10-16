using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.WebApi.Tests
{
    public static class WebApiCommonAsserts
    {
        public static void IsHttpActionResult(object result)
        {
            Assert.IsInstanceOfType(result, typeof(IHttpActionResult));
        }

        public static void IsInternalServerError(object result)
        {
            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
        }

        public static void IsOkResult(object result)
        {
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        public static void IsBadResult(object result)
        {
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        public static void IsBadResultWithMessage(object result)
        {
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        public static void BadResultHasTheCorrectErrorMessage(object result, string expectedMessage)
        {
            var messageFromApi = result as BadRequestErrorMessageResult;
            Assert.AreEqual(expectedMessage, messageFromApi.Message);
        }
    }
}
