using System;
using System.Collections.Generic;
using BohFoundation.Domain.EntityFrameworkModels.Admins;

namespace BohFoundation.Domain.EntityFrameworkModels.Common
{
    public class EssayTopic
    {
        public int Id { get; set; }
        public string TitleOfEssay { get; set; } 
        public string EssayPrompt { get; set; }
        
        public byte[] ConcurrencyTimestamp { get; set; }
        public DateTime RevisionDateTime { get; set; }
        
        public virtual ICollection<GraduatingClass> ForWhatGraduatingYears { get; set; }
        public virtual Admin LastRevisionAuthor { get; set; }
    }
}