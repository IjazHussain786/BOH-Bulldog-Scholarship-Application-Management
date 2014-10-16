using System;
using System.Web.Http;
using BohFoundation.Domain.Dtos.Admin;
using BohFoundation.MembershipProvider.UserManagement.Admin.Interfaces;
using BohFoundation.WebApi.Filters;

namespace BohFoundation.WebApi.Controllers.Admin
{
    [RequireAdmin]
    [RoutePrefix("api/admin")]
    public class AdminNotificationController : ApiController
    {
        private readonly IConfirmApplicationEvaluator _confirmApplicationEvaluator;

        public AdminNotificationController(IConfirmApplicationEvaluator confirmApplicationEvaluator)
        {
            _confirmApplicationEvaluator = confirmApplicationEvaluator;
        }

        [Route("notifications")]
        public IHttpActionResult Get()
        {
            var counts = new AdminNotificationCountsDto();
            try
            {
                counts.PendingApplicationEvaluators =
                    _confirmApplicationEvaluator.CountOfPendingApplicationEvaluators();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok(counts);
        }
    }
}
