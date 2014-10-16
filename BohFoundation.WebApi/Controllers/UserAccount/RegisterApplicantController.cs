using System;
using System.Web.Http;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.Domain.Enums;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;

namespace BohFoundation.WebApi.Controllers.UserAccount
{
    [AllowAnonymous]
    [RoutePrefix("api/useraccount")]
    public class RegisterApplicantController : ApiController
    {
        private readonly IRegisterUserService _registerApplicant;

        public RegisterApplicantController(IRegisterUserService registerApplicant)
        {
            _registerApplicant = registerApplicant;
        }

        [Route("registerapplicant")]
        public IHttpActionResult Post([FromBody] RegisterInputModel model)
        {
            try
            {
                var result = _registerApplicant.CreateAccount(model, MemberTypesEnum.Applicant);

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
