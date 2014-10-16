using System;
using System.Web.Http;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;

namespace BohFoundation.WebApi.Controllers.UserAccount
{
    [Route("api/useraccount/changepassword")]
    public class ChangePasswordController : ApiController
    {
        private readonly IChangePasswordService _changePasswordService;

        public ChangePasswordController(IChangePasswordService changePasswordService)
        {
            _changePasswordService = changePasswordService;
        }

        public IHttpActionResult Post([FromBody] ChangePasswordInputModelDto model)
        {
            try
            {
                var result = _changePasswordService.ChangePassword(model);

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
