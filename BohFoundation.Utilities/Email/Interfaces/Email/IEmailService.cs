using BohFoundation.Domain.Dtos.Email;

namespace BohFoundation.Utilities.Email.Interfaces.Email
{
    public interface IEmailService
    {
        void SendEmailToOneUser(SendEmailWithSubjectAndBodyDto sendEmail);

        //void SendEmailToOneUser(SendEmailDtoWithSubjectBodyAndSender sendEmailDtoWithSender);
    }
}
