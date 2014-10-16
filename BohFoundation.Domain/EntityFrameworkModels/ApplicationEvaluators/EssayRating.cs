using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Common;

namespace BohFoundation.Domain.EntityFrameworkModels.ApplicationEvaluators
{
    public class EssayRating
    {
        public int Id { get; set; }
        public virtual Essay Essay { get; set; }
        public virtual GenericRating Rating { get; set; }
        public virtual ApplicantRating ApplicantRating { get; set; }
    }
}