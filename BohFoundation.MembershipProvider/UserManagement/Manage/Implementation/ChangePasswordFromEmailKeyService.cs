using System;
using BohFoundation.Domain.Dtos;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;

namespace BohFoundation.MembershipProvider.UserManagement.Manage.Implementation
{
    public class ChangePasswordFromEmailKeyService : UserManagementBase, IChangePasswordFromEmailedKeyService
    {
        public ChangePasswordFromEmailKeyService(UserAccountService<RelationalUserAccount> userAccountService) : base(userAccountService)
        {
        }

        public SuccessOrFailureDto ResetPasswordRequest(ResetPasswordThruEmailDto dto)
        {
            try
            {
                UserAccountService.ResetPassword(dto.EmailAddress);
            }
            catch (Exception ex)
            {
                return new SuccessOrFailureDto { Success = false, ExceptionMessage = ex.Message };
            }
            return new SuccessOrFailureDto { Success = true };
        }

        public SuccessOrFailureDto ChangePasswordFromResetKey(ChangePasswordFromResetKeyDto changePasswordFromResetInputModelDto)
        {
            bool success;
            try
            {
                success = UserAccountService.ChangePasswordFromResetKey(changePasswordFromResetInputModelDto.Key,
                    changePasswordFromResetInputModelDto.NewPassword);
            }
            catch (Exception ex)
            {
                return new SuccessOrFailureDto { Success = false, ExceptionMessage = ex.Message };
            }
            return new SuccessOrFailureDto { Success = success };
        }
    }
}
