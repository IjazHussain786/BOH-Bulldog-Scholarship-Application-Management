namespace BohFoundation.Domain.Dtos.Applicant.References
{
    public class ApplicantReferenceInputDto
    {
        private string _referenceEmail;

        public string ReferenceEmail
        {
            get { return _referenceEmail; }
            set { _referenceEmail = value.ToLowerInvariant(); }
        }

        public string Salutation { get; set; }
        public string Signature { get; set; }
        public string MessageParagraph { get; set; }
        public string RelationshipToReference { get; set; }
    }
}
