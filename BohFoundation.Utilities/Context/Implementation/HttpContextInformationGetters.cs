using System.Web;
using BohFoundation.Utilities.Context.Interfaces.Context;

namespace BohFoundation.Utilities.Context.Implementation
{
    //todo not tested.
    public class HttpContextInformationGetters : IHttpContextInformationGetters
    {
        //http://www.gotknowhow.com/articles/how-to-get-base-url-in-aspnet
        public string GetRequestHttpBaseUrl()
        {
            var context = HttpContext.Current;
            var baseUrl = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + context.Request.ApplicationPath.TrimEnd('/');
            return baseUrl;
        }
    }
}
