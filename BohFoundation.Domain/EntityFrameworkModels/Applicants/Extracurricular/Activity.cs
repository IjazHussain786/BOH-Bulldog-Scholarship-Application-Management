using System;

namespace BohFoundation.Domain.EntityFrameworkModels.Applicants.Extracurricular
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortSummaryOfWhatIsInvolved { get; set; }

        public virtual ExtracurricularActivities ExtracurricularActivities { get; set; }
    }
}