using BohFoundation.Domain.Dtos;
using BohFoundation.Domain.Dtos.UserManagement;

namespace BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces
{
    public interface IChangeEmailService
    {
        SuccessOrFailureDto ChangeEmail(ChangeEmailInputModelDto emailAddress);
        SuccessOrFailureDto VerifyEmailAddress(VerificationKeyDto verificationKeyDto);
    }
}
