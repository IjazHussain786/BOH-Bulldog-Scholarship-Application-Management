using System;
using System.Web.Http;
using BohFoundation.Domain.Dtos.Email;
using BohFoundation.MembershipProvider.UserManagement.Admin.Interfaces;
using BohFoundation.WebApi.Filters;

namespace BohFoundation.WebApi.Controllers.Admin.InviteConfirmApplicationEvaluator
{
    [RequireAdmin]
    [RoutePrefix("api/admin/inviteconfirmapplicationevaluator")]
    public class InviteApplicationEvaluatorController : ApiController
    {
        private readonly IInviteApplicationEvaluator _inviteApplicationEvaluator;

        public InviteApplicationEvaluatorController(IInviteApplicationEvaluator inviteApplicationEvaluator)
        {
            _inviteApplicationEvaluator = inviteApplicationEvaluator;
        }

        [Route("invite")]
        public IHttpActionResult Post([FromBody] SendEmailContactDto contact)
        {
            try
            {
                _inviteApplicationEvaluator.SendApplicationEvaluatorInvitation(contact);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }
    }
}
