using SendGrid;

namespace BohFoundation.Utilities.Email.Interfaces.Email.Helpers
{
    public interface ISendGridClient
    {
        void SendMessage(SendGridMessage message);
    }
}
