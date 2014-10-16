using System;

namespace BohFoundation.Domain.Dtos.Applicant.References
{
    public class ApplicantReferenceForEntityFrameworkDto : ApplicantReferenceInputDto
    {
        public Guid GuidLink { get; set; }
    }
}