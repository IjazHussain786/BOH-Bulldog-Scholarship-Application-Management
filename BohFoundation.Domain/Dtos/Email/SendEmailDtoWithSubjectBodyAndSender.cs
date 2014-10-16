namespace BohFoundation.Domain.Dtos.Email
{
    public class SendEmailDtoWithSubjectBodyAndSender : SendEmailWithSubjectAndBodyDto
    {
        public string SendersFullName { get; set; }
        public string SendersEmail { get; set; }
    }
}