using System;
using BohFoundation.Domain.Enums;

namespace BohFoundation.Domain.Dtos.Person
{
    public class PhoneInformationDto
    {
        public long PhoneNumber { get; set; }
        public TimeOfDayEnum BestTimeToContactByPhone { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
