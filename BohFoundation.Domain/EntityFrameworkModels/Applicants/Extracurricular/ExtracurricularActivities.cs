using System;
using System.Collections.Generic;

namespace BohFoundation.Domain.EntityFrameworkModels.Applicants.Extracurricular
{
    public class ExtracurricularActivities
    {
        public int Id { get; set; }
        public bool PaidWork { get; set; }
        public bool HasNonPaidActivities { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual ICollection<Job> Jobs { get; set; } //Appears if paid work is true.
        public virtual ICollection<Activity> Activities { get; set; } //clubs, sports, church things, etc. 
        
        public virtual Applicant Applicant { get; set; }
    }
}