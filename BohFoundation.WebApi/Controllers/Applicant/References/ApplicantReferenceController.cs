using System;
using System.Collections.Generic;
using System.Web.Http;
using BohFoundation.Domain.Dtos.Applicant.References;
using BohFoundation.MiddleTier.ApplicantsOrchestration.Interfaces;
using BohFoundation.WebApi.Filters;
using BohFoundation.WebApi.Models;

namespace BohFoundation.WebApi.Controllers.Applicant.References
{
    [Route("api/applicant/references"), RequireApplicant]
    public class ApplicantReferenceController : ApiController
    {
        private readonly IApplicantReferenceOrchestration _applicantReference;

        public ApplicantReferenceController(IApplicantReferenceOrchestration applicantReference)
        {
            _applicantReference = applicantReference;
        }
        
        public IHttpActionResult Get()
        {
            ICollection<ReferenceDto> referenceDtos;
            try
            {
                referenceDtos = _applicantReference.GetReferences();
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(new ServerMessage(referenceDtos));
        }

        public IHttpActionResult Post(ApplicantReferenceInputDto applicantReferenceDto)
        {
            try
            {
                _applicantReference.AddReference(applicantReferenceDto);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();
        }
    }
}
