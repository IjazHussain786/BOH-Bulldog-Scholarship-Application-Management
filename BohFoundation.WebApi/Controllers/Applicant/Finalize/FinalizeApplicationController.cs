using System;
using System.Web.Http;
using BohFoundation.MiddleTier.ApplicantsOrchestration.Interfaces;
using BohFoundation.WebApi.Filters;

namespace BohFoundation.WebApi.Controllers.Applicant.Finalize
{
    [Route("api/applicant/finalize"), RequireApplicant]
    public class FinalizeApplicationController : ApiController
    {
        private readonly IFinalizeApplicationOrchestration _finalizeApplication;

        public FinalizeApplicationController(IFinalizeApplicationOrchestration finalizeApplication)
        {
            _finalizeApplication = finalizeApplication;
        }

        public IHttpActionResult Post()
        {
            try
            {
                _finalizeApplication.FinalizeApplication();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }
    }
}
