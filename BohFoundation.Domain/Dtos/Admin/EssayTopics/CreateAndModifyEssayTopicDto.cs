namespace BohFoundation.Domain.Dtos.Admin.EssayTopics
{
    public class CreateAndModifyEssayTopicDto
    {
        public int Id { get; set; }
        public string TitleOfEssay { get; set; }
        public string EssayPrompt { get; set; }
    }
}
