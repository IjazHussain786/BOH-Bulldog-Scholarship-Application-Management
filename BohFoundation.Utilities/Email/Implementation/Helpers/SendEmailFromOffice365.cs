using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using BohFoundation.Utilities.Email.Interfaces.Email.Helpers;

namespace BohFoundation.Utilities.Email.Implementation.Helpers
{
    //ToDo Not Tested
    public class SendEmailFromOffice365 : ISendEmailFromOffice365
    {
        public void SendMessage(MailMessage message)
        {
            var configSettings = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            if (configSettings == null) return;
            var credentials = new NetworkCredential(configSettings.Network.UserName, configSettings.Network.Password);

            var client = new SmtpClient(configSettings.Network.Host, configSettings.Network.Port) {EnableSsl = true, Credentials = credentials};

            client.Send(message);
        }
    }
}
