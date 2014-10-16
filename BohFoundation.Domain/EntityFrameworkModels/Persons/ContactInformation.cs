using System;

namespace BohFoundation.Domain.EntityFrameworkModels.Persons
{
    public class ContactInformation
    {
        private string _emailAddress;

        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value.ToLowerInvariant(); }
        }
        
        public int Id { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual Address Address { get; set; }
        public virtual PhoneInformation PhoneInformation { get; set; }
        public virtual Person Person { get; set; }
    }
}