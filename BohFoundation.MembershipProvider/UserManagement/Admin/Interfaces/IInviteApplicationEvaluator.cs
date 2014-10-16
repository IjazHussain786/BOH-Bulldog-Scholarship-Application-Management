using BohFoundation.Domain.Dtos.Email;

namespace BohFoundation.MembershipProvider.UserManagement.Admin.Interfaces
{
    public interface IInviteApplicationEvaluator
    {
        void SendApplicationEvaluatorInvitation(SendEmailContactDto sendEmailToDto);
    }
}
