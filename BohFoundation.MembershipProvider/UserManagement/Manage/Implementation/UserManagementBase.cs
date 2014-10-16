using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;

namespace BohFoundation.MembershipProvider.UserManagement.Manage.Implementation
{
    public abstract class UserManagementBase
    {
        protected readonly UserAccountService<RelationalUserAccount> UserAccountService;

        protected UserManagementBase(UserAccountService<RelationalUserAccount> userAccountService)
        {
            UserAccountService = userAccountService;
        }
    }
}
