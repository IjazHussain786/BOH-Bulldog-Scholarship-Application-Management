using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using BohFoundation.Utilities.Context.Interfaces;
using Ninject;

namespace BohFoundation.WebApi.Filters
{
    //ToDo NOT UNIT TESTED
    public class RequireApplicantAttribute : AuthorizationFilterAttribute
    {
        private readonly IClaimsInformationGetters _claimsInformationGetters;
        private readonly IDeadlineUtilities _deadlineVerifier;

        public RequireApplicantAttribute()
        {
            _claimsInformationGetters = NinjectWebCommon.Kernel.Get<IClaimsInformationGetters>();
            _deadlineVerifier = NinjectWebCommon.Kernel.Get<IDeadlineUtilities>();
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!_claimsInformationGetters.IsApplicant() || _deadlineVerifier.IsAfterDeadline())
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}