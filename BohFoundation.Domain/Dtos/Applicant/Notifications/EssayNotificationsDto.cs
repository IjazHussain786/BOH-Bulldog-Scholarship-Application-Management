using System;

namespace BohFoundation.Domain.Dtos.Applicant.Notifications
{
    public class EssayNotificationsDto
    {
        public DateTime? RevisionDateTime { get; set; }
        public string EssayPrompt { get; set; }
        public string TitleOfEssay { get; set; }
        public int EssayTopicId { get; set; }
    }
}
