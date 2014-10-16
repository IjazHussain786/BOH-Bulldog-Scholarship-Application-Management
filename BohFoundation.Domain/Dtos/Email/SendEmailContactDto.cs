namespace BohFoundation.Domain.Dtos.Email
{
    public class SendEmailContactDto
    {
        public string RecipientFirstName { get; set; }
        public string RecipientLastName { get; set; }
        public string RecipientEmailAddress { get; set; }
        public string RecipientFullName { get { return RecipientFirstName + " " + RecipientLastName; } }
    }
}
