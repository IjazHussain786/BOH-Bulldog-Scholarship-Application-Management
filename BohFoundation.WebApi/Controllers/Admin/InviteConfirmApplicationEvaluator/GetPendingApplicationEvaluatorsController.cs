using System;
using System.Collections.Generic;
using System.Web.Http;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.MembershipProvider.UserManagement.Admin.Interfaces;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.Admin.InviteConfirmApplicationEvaluator
{
    [RequireAdmin]
    [RoutePrefix("api/admin/inviteconfirmapplicationevaluator")]
    public class GetPendingApplicationEvaluatorsController : ApiController
    {
        private readonly IConfirmApplicationEvaluator _confirmApplicationEvaluator;

        public GetPendingApplicationEvaluatorsController(IConfirmApplicationEvaluator confirmApplicationEvaluator)
        {
            _confirmApplicationEvaluator = confirmApplicationEvaluator;
        }

        [Route("pendingevaluators")]
        public IHttpActionResult Get()
        {
            List<PersonDto> result;
            try
            {
                result = _confirmApplicationEvaluator.GetPendingApplicationEvaluators();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok(new ServerMessage{Data = result});
        }
    }
}
