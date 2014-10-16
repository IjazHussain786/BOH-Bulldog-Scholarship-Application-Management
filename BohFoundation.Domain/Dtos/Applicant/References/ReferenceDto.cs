using BohFoundation.Domain.Dtos.Person;

namespace BohFoundation.Domain.Dtos.Applicant.References
{
    public class ReferenceDto
    {
        public NameDto ReferenceName { get; set; }
        public string ReferenceEmailAddress { get; set; }
        public bool ReferenceLetterReceived { get; set; }
    }
}
