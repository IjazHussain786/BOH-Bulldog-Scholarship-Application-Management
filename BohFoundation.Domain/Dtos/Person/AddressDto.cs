using System;

namespace BohFoundation.Domain.Dtos.Person
{
    public class AddressDto
    {
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
