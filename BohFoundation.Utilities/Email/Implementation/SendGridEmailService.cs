using System.Net.Mail;
using AutoMapper;
using BohFoundation.Domain.Dtos.Email;
using BohFoundation.Utilities.Email.Interfaces.Email;
using BohFoundation.Utilities.Email.Interfaces.Email.Helpers;
using SendGrid;

namespace BohFoundation.Utilities.Email.Implementation
{
    public class SendGridEmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;
        
        public SendGridEmailService(ISendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient;

            Mapper.CreateMap<SendEmailWithSubjectAndBodyDto, SendEmailDtoWithSubjectBodyAndSender>();
        }

        public void SendEmailToOneUser(SendEmailWithSubjectAndBodyDto sendEmail)
        {
            var sendEmailWithDefaultSender = Mapper.Map<SendEmailDtoWithSubjectBodyAndSender>(sendEmail);
            sendEmailWithDefaultSender.SendersEmail = "Robot@BOHFoundation.org";
            sendEmailWithDefaultSender.SendersFullName = "BOHFoundation Mail Robot";
            SendEmailToOneUser(sendEmailWithDefaultSender);
        }

        public void SendEmailToOneUser(SendEmailDtoWithSubjectBodyAndSender sendEmail)
        {
            var message = new SendGridMessage
            {
                From = new MailAddress(sendEmail.SendersEmail, sendEmail.SendersFullName),
                Subject = sendEmail.Subject,
                Text = sendEmail.Body
            };
            message.AddTo(sendEmail.RecipientEmailAddress);
            _sendGridClient.SendMessage(message);
        }
    }
}
