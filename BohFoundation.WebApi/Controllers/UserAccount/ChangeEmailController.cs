using System;
using System.Web.Http;
using BohFoundation.Domain.Dtos;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;

namespace BohFoundation.WebApi.Controllers.UserAccount
{
    [RoutePrefix("api/useraccount/changeemail")]
    public class ChangeEmailController : ApiController
    {
        private readonly IChangeEmailService _changeEmailServices;

        public ChangeEmailController(IChangeEmailService changeEmailServices)
        {
            _changeEmailServices = changeEmailServices;
        }

        [Route("request")]
        public IHttpActionResult Post([FromBody] ChangeEmailInputModelDto model)
        {
            try
            {
                var result = _changeEmailServices.ChangeEmail(model);

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

        [Route("confirm")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult ConfirmEmail([FromBody] VerificationKeyDto verificationKeyModel)
        {
            try
            {
                var success = SendVerificationKeyToBackEnd(verificationKeyModel);
                if (success.Success)
                {
                    return Ok();
                }
                return BadRequest(success.ExceptionMessage);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("cancel/{verificationKey}")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult CancelConfirmation([FromUri] string verificationKey)
        {
            try
            {
                var dto = CreateVerificationKeyDto(true, verificationKey);
                var success = SendVerificationKeyToBackEnd(dto);
                if (success.Success)
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


        private SuccessOrFailureDto SendVerificationKeyToBackEnd(VerificationKeyDto verificationKeyDto)
        {
            return _changeEmailServices.VerifyEmailAddress(verificationKeyDto);
        }

        private VerificationKeyDto CreateVerificationKeyDto(bool cancel, string verificationKey)
        {
            return new VerificationKeyDto { Cancel = cancel, VerificationKey = verificationKey };
        }
    }
}
