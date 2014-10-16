using System;
using System.Web.Http;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.Domain.Enums;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;

namespace BohFoundation.WebApi.Controllers.UserAccount
{
    [AllowAnonymous]
    [RoutePrefix("api/useraccount")]
    public class RegisterApplicationEvaluatorController : ApiController
    {
        private readonly IRegisterUserService _registerUser;

        public RegisterApplicationEvaluatorController(IRegisterUserService registerUser)
        {
            _registerUser = registerUser;
        }

        [Route("registerapplicationevaluator")]
        public IHttpActionResult Post([FromBody] RegisterInputModel model)
        {
            try
            {
                var result = _registerUser.CreateAccount(model, MemberTypesEnum.PendingApplicationEvaluator);

                if (!result.Success)
                {
                    return BadRequest(result.ExceptionMessage);
                }

                return Ok();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
