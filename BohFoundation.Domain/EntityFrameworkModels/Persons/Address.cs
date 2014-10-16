using System;

namespace BohFoundation.Domain.EntityFrameworkModels.Persons
{
    public class Address
    {
        public int Id { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual ContactInformation ContantInformation { get; set; }
    }
}