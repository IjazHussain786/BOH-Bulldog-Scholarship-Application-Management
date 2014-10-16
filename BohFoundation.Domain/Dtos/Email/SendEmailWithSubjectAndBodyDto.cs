namespace BohFoundation.Domain.Dtos.Email
{
    public class SendEmailWithSubjectAndBodyDto : SendEmailContactDto
    {
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}
