using System;
using System.Web.Http;
using BohFoundation.Domain.Dtos.Admin;
using BohFoundation.MembershipProvider.UserManagement.Admin.Interfaces;
using BohFoundation.WebApi.Filters;

namespace BohFoundation.WebApi.Controllers.Admin.InviteConfirmApplicationEvaluator
{
    [RequireAdmin]
    [RoutePrefix("api/admin/inviteconfirmapplicationevaluator")]
    public class ConfirmApplicationEvaluatorController : ApiController
    {
        private readonly IConfirmApplicationEvaluator _confirmApplicationEvaluator;

        public ConfirmApplicationEvaluatorController(IConfirmApplicationEvaluator confirmApplicationEvaluator)
        {
            _confirmApplicationEvaluator = confirmApplicationEvaluator;
        }

        [Route("confirm")]
        public IHttpActionResult Post([FromBody] ConfirmApplicationEvaluatorDto model)
        {
            try
            {
                _confirmApplicationEvaluator.ConfirmOrRejectApplicationEvaluator(model); 
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }
    }
}
