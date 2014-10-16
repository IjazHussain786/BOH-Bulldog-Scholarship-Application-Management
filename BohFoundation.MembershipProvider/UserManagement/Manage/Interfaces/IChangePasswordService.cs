using BohFoundation.Domain.Dtos;
using BohFoundation.Domain.Dtos.UserManagement;

namespace BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces
{
    public interface IChangePasswordService
    {
        SuccessOrFailureDto ChangePassword(ChangePasswordInputModelDto changePasswordInputModelDto);
    }
}
