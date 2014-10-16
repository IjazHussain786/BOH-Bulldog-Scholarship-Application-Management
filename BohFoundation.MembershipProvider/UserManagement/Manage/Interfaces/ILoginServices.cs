using BohFoundation.Domain.Dtos.UserManagement;

namespace BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces
{
    public interface ILoginServices
    {
        SuccessOrFailureDtoWithClaims LogIn(LoginInputDto loginInput);
    }
}
