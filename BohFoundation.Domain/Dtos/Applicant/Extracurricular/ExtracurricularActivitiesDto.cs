using System;
using System.Collections.Generic;

namespace BohFoundation.Domain.Dtos.Applicant.Extracurricular
{
    public class ExtracurricularActivitiesDto
    {
        public bool PaidWork { get; set; }
        public bool HasNonPaidActivities { get; set; }
        public DateTime LastUpdated { get; set; }

        public ICollection<JobDto> Jobs { get; set; }
        public ICollection<ActivityDto> Activities { get; set; } 
    }
}
