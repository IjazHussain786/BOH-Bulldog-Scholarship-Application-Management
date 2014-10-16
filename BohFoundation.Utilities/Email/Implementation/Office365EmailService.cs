using System.Net.Mail;
using AutoMapper;
using BohFoundation.Domain.Dtos.Email;
using BohFoundation.Utilities.Email.Interfaces.Email;
using BohFoundation.Utilities.Email.Interfaces.Email.Helpers;

namespace BohFoundation.Utilities.Email.Implementation
{
    public class Office365EmailService : IEmailService
    {
        private readonly ISendEmailFromOffice365 _sendEmail;

        public Office365EmailService(ISendEmailFromOffice365 sendEmail)
        {
            _sendEmail = sendEmail;

            Mapper.CreateMap<SendEmailWithSubjectAndBodyDto, SendEmailDtoWithSubjectBodyAndSender>();
        }

        public void SendEmailToOneUser(SendEmailWithSubjectAndBodyDto sendEmail)
        {
            var sendEmailWithDefaultSender = Mapper.Map<SendEmailDtoWithSubjectBodyAndSender>(sendEmail);
            sendEmailWithDefaultSender.SendersEmail = "Robot@BOHFoundation.org";
            sendEmailWithDefaultSender.SendersFullName = "BOHFoundation Mail Robot";
            SendEmailToOneUser(sendEmailWithDefaultSender);
        }

        private void SendEmailToOneUser(SendEmailDtoWithSubjectBodyAndSender sendEmailDtoWithSender)
        {
            var from = new MailAddress(sendEmailDtoWithSender.SendersEmail, sendEmailDtoWithSender.SendersFullName);
            var to = new MailAddress(sendEmailDtoWithSender.RecipientEmailAddress,
                sendEmailDtoWithSender.RecipientFullName);


            var message = new MailMessage(from, to)
            {
                Body = sendEmailDtoWithSender.Body,
                From = new MailAddress(sendEmailDtoWithSender.SendersEmail, sendEmailDtoWithSender.SendersFullName),
                Subject = sendEmailDtoWithSender.Subject
            };
            
            _sendEmail.SendMessage(message);
        }
    }
}
