using BohFoundation.Domain.Dtos;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.Domain.Enums;

namespace BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces
{
    public interface IRegisterUserService
    {
        SuccessOrFailureDto CreateAccount(RegisterInputModel model, MemberTypesEnum memberType);
    }
}
