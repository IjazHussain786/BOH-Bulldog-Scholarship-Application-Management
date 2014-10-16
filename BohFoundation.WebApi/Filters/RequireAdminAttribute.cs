using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using BohFoundation.Utilities.Context.Interfaces;
using Ninject;

namespace BohFoundation.WebApi.Filters
{
    //ToDo NOT UNIT TESTED
    public class RequireAdminAttribute : AuthorizationFilterAttribute
    {
        private readonly IClaimsInformationGetters _claimsInformationGetters;

        public RequireAdminAttribute()
        {
            _claimsInformationGetters = NinjectWebCommon.Kernel.Get<IClaimsInformationGetters>();
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!_claimsInformationGetters.IsAdmin())
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}