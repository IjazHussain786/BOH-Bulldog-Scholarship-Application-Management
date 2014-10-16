using System;

namespace BohFoundation.Domain.Dtos.Person
{
    public class ContactInformationDto
    {
        public string EmailAddress { get; set; }
        public PhoneInformationDto PhoneInformation { get; set; }
        public AddressDto Address { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
