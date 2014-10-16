using System;

namespace BohFoundation.Domain.EntityFrameworkModels.Applicants.Family
{
    public class W2
    {
        public int Id { get; set; }
        public string LinkToW2 { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual FamilyInformation FamilyInformation { get; set; }
    }
}