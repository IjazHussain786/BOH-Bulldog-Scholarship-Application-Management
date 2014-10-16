using System;

namespace BohFoundation.Domain.EntityFrameworkModels.Persons
{
    public class Name
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual Person Person { get; set; }
    }
}