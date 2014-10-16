using System;
using BohFoundation.Domain.Dtos.Person;

namespace BohFoundation.Domain.Dtos.Reference.Anonymous
{
    public class ReferencePersonalInformationDto
    {
        public string Occupation { get; set; }
        public string EmailAddress { get; set; } 
        public NameDto Name { get; set; }
        public PhoneInformationDto PhoneInformationDto { get; set; }
        public Guid GuidSentToReference { get; set; }
    }
}
