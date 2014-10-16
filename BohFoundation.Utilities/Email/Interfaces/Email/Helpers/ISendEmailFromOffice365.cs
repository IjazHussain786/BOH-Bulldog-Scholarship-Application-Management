using System.Net.Mail;

namespace BohFoundation.Utilities.Email.Interfaces.Email.Helpers
{
    public interface ISendEmailFromOffice365
    {
        void SendMessage(MailMessage message);
    }
}
