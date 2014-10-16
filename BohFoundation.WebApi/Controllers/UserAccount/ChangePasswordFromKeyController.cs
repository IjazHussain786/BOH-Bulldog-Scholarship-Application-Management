using System;
using System.Web.Http;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;

namespace BohFoundation.WebApi.Controllers.UserAccount
{
    [AllowAnonymous]
    [RoutePrefix("api/useraccount/changepassword")]
    public class ChangePasswordFromKeyController : ApiController
    {
        private readonly IChangePasswordFromEmailedKeyService _changePasswordFromEmailedKeyService;

        public ChangePasswordFromKeyController(IChangePasswordFromEmailedKeyService changePasswordFromEmailedKeyService)
        {
            _changePasswordFromEmailedKeyService = changePasswordFromEmailedKeyService;
        }

        [Route("request")]
        public IHttpActionResult Post([FromBody] ResetPasswordThruEmailDto model)
        {
            try
            {
                var result = _changePasswordFromEmailedKeyService.ResetPasswordRequest(model);

                if (result.Success)
                {
                    return Ok();
                }
                return BadRequest("That email wasn't found or there was an error on the server. Try again.");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("withkey")]
        [HttpPost]
        public IHttpActionResult ChangePasswordFromResetKey([FromBody] ChangePasswordFromResetKeyDto model)
        {
            try
            {
                var result = _changePasswordFromEmailedKeyService.ChangePasswordFromResetKey(model);

                if (result.Success)
                {
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
