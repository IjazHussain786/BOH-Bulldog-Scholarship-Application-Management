using System;
using System.Collections.Generic;
using BohFoundation.Domain.Dtos.Person;

namespace BohFoundation.Domain.Dtos.Admin.EssayTopics
{
    public class EssayTopicDto
    {
        public int Id { get; set; }
        public string TitleOfEssay { get; set; }
        public string EssayPrompt { get; set; }

        public DateTime RevisionDateTime { get; set; }

        public ICollection<int> ForWhatGraduatingYears { get; set; }
        public NameDto LastRevisionAuthor { get; set; }
    }
}
