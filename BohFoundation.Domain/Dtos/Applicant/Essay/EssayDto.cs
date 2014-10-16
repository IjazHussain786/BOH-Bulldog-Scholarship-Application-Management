using System;

namespace BohFoundation.Domain.Dtos.Applicant.Essay
{
    public class EssayDto
    {
        public string Essay { get; set; }
        public DateTime RevisionDateTime { get; set; }
        public string EssayPrompt { get; set; }
        public int EssayTopicId { get; set; }
    }
}
