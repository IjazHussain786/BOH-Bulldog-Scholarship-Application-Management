using System;
using BohFoundation.Domain.Dtos;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;
using BohFoundation.Utilities.Context.Interfaces;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;

namespace BohFoundation.MembershipProvider.UserManagement.Manage.Implementation
{
    public class ChangePasswordService : UserManagementBase, IChangePasswordService
    {
        private readonly IClaimsInformationGetters _claimsInformationGetters;

        public ChangePasswordService(UserAccountService<RelationalUserAccount> userAccountService, IClaimsInformationGetters claimsInformationGetters) : base(userAccountService)
        {
            _claimsInformationGetters = claimsInformationGetters;
        }

        public SuccessOrFailureDto ChangePassword(ChangePasswordInputModelDto changePasswordInputModelDto)
        {
            try
            {
                var userGuid = _claimsInformationGetters.GetUsersGuid();
                UserAccountService.ChangePassword(userGuid, changePasswordInputModelDto.OldPassword,
                    changePasswordInputModelDto.NewPassword);
            }
            catch (Exception ex)
            {
                return new SuccessOrFailureDto { ExceptionMessage = ex.Message, Success = false };
            }
            return new SuccessOrFailureDto { Success = true };
        }
    }
}
