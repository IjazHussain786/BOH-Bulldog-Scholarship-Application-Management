using System;
using System.Collections.Generic;
using BohFoundation.Domain.EntityFrameworkModels.ApplicationEvaluators;
using BohFoundation.Domain.EntityFrameworkModels.Common;

namespace BohFoundation.Domain.EntityFrameworkModels.Applicants
{
    public class Essay
    {
        public int Id { get; set; }
        public string RowKey { get; set; }
        public string PartitionKey { get; set; }
        public DateTime RevisionDateTime { get; set; }
        public int CharacterLength { get; set; }
        
        public virtual EssayTopic EssayTopic { get; set; }
        public virtual Applicant Applicant { get; set; }

        public virtual ICollection<EssayRating> EssayRatings { get; set; } 
    }
}