using System.Collections.Generic;
using BohFoundation.Domain.EntityFrameworkModels.Persons;

namespace BohFoundation.Domain.EntityFrameworkModels.References
{
    public class Reference
    {
        public int Id { get; set; }
        public string Occupation { get; set; }
        public bool HasMembershipRebootAccount { get; set; }

        public virtual Person Person { get; set; }
        public virtual ICollection<LetterOfRecommendation> LettersOfRecommendation { get; set; } 
    }
}