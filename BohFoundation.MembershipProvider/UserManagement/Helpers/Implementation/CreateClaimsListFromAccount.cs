using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BohFoundation.MembershipProvider.UserManagement.Helpers.Interfaces;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;

namespace BohFoundation.MembershipProvider.UserManagement.Helpers.Implementation
{
    public class CreateClaimsListFromAccount : ICreateClaimsListFromAccount
    {
        public List<Claim> CreateClaimsList(RelationalUserAccount account)
        {
            return account.GetAllClaims().ToList();
        }
    }
}
