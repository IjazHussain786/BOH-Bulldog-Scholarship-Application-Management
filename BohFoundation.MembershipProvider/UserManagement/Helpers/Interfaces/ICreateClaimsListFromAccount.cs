using System.Collections.Generic;
using System.Security.Claims;
using BrockAllen.MembershipReboot.Relational;

namespace BohFoundation.MembershipProvider.UserManagement.Helpers.Interfaces
{
    public interface ICreateClaimsListFromAccount
    {
        List<Claim> CreateClaimsList(RelationalUserAccount account);
    }
}
