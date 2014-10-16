using System;
using BohFoundation.Domain.Enums;

namespace BohFoundation.Domain.EntityFrameworkModels.Persons
{
    public class PhoneInformation
    {
        public int Id { get; set; }
        public long PhoneNumber { get; set; }
        public TimeOfDayEnum BestTimeToContactByPhone { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual ContactInformation ContactInformation { get; set; }
    }
}
