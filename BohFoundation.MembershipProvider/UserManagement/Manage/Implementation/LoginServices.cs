using System;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.MembershipProvider.UserManagement.Helpers.Interfaces;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;

namespace BohFoundation.MembershipProvider.UserManagement.Manage.Implementation
{
    public class LoginServices : UserManagementBase, ILoginServices
    {
        private readonly ICreateClaimsListFromAccount _createClaims;

        public LoginServices(UserAccountService<RelationalUserAccount> userAccountService, ICreateClaimsListFromAccount createClaims) : base(userAccountService)
        {
            _createClaims = createClaims;
        }

        public SuccessOrFailureDtoWithClaims LogIn(LoginInputDto loginInput)
        {
            try
            {
                RelationalUserAccount account;
                if (UserAccountService.AuthenticateWithUsernameOrEmail(loginInput.EmailAddress, loginInput.Password, out account))
                {
                    var claims = _createClaims.CreateClaimsList(account);
                    return new SuccessOrFailureDtoWithClaims { Success = true, Claims = claims};
                }
            }
            catch (Exception ex)
            {
                return new SuccessOrFailureDtoWithClaims { ExceptionMessage = ex.Message, Success = false };
            }
            return new SuccessOrFailureDtoWithClaims { Success = false };
        }
    }
}
