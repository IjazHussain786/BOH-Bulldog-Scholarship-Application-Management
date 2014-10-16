using System.Collections.Generic;
using BohFoundation.Domain.EntityFrameworkModels.Common;
using BohFoundation.Domain.EntityFrameworkModels.Persons;

namespace BohFoundation.Domain.EntityFrameworkModels.Admins
{
    public class Admin
    {
        public int Id { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<EssayTopic> EssayTopics { get; set; } 
    }
}