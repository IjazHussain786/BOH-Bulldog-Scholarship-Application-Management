using System;

namespace BohFoundation.Domain.Dtos.Admin.References
{
    public class GuidSentToReferenceDto
    {
        public Guid GuidSentToReference { get; set; }
        public bool AlreadyUpdated { get; set; }
        public string ErrorMessage { get; set; }
    }
}
