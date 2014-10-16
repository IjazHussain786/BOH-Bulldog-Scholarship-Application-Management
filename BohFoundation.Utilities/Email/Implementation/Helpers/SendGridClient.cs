using System.Configuration;
using System.Net;
using System.Net.Configuration;
using BohFoundation.Utilities.Email.Interfaces.Email.Helpers;
using SendGrid;

namespace BohFoundation.Utilities.Email.Implementation.Helpers
{
    public class SendGridClient : ISendGridClient
    {
        //todo NOT UNIT TESTED
        public void SendMessage(SendGridMessage message)
        {
            var configSettings = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            if (configSettings == null) return;
            var credentials = new NetworkCredential(configSettings.Network.UserName, configSettings.Network.Password);
            var client = new Web(credentials);
            client.Deliver(message);
        }

    }
}
