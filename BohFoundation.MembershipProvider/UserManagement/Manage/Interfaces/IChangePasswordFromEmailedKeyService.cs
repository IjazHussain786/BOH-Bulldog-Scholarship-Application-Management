using BohFoundation.Domain.Dtos;
using BohFoundation.Domain.Dtos.UserManagement;

namespace BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces
{
    public interface IChangePasswordFromEmailedKeyService
    {
        SuccessOrFailureDto ResetPasswordRequest(ResetPasswordThruEmailDto resetPasswordDto);
        SuccessOrFailureDto ChangePasswordFromResetKey(ChangePasswordFromResetKeyDto changePasswordFromResetInputModelDto);
    }
}
