using System.Collections.Generic;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;

namespace BohFoundation.Domain.EntityFrameworkModels.Common
{
    public class GraduatingClass
    {
        public int Id { get; set; }
        public int GraduatingYear { get; set; }

        public virtual ICollection<EssayTopic> EssayTopics { get; set; }
        public virtual ICollection<ApplicantPersonalInformation> PeopleInClass { get; set; } 
    }
}
