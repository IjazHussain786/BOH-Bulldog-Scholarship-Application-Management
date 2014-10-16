using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.Domain.Enums;
using BrockAllen.MembershipReboot;

namespace BohFoundation.MembershipProvider.UserManagement.Helpers.Interfaces
{
    public interface ICreateUserClaimCollection
    {
        UserClaimCollection CreateClaimsCollection(RegisterInputModel model, MemberTypesEnum memberType);
    }
}
