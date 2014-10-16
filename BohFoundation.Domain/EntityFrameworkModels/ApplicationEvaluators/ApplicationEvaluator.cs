using System.Collections.Generic;
using BohFoundation.Domain.EntityFrameworkModels.Persons;

namespace BohFoundation.Domain.EntityFrameworkModels.ApplicationEvaluators
{
    public class ApplicationEvaluator
    {
        public int Id { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<ApplicantRating> ApplicantRating { get; set; }
    }
}