using System;
using BohFoundation.Domain.Dtos;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;
using BohFoundation.PersonsRepository.Repositories.Interfaces;
using BohFoundation.Utilities.Context.Interfaces;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;

namespace BohFoundation.MembershipProvider.UserManagement.Manage.Implementation
{
    public class ChangeEmailService : UserManagementBase, IChangeEmailService
    {
        private readonly IClaimsInformationGetters _claimsInformationGetters;
        private readonly IChangeEmailRepository _changeEmailRepository;

        public ChangeEmailService(UserAccountService<RelationalUserAccount> userAccountService, IClaimsInformationGetters claimsInformationGetters, IChangeEmailRepository changeEmailRepository)
            : base(userAccountService)
        {
            _claimsInformationGetters = claimsInformationGetters;
            _changeEmailRepository = changeEmailRepository;
        }

        public SuccessOrFailureDto ChangeEmail(ChangeEmailInputModelDto emailAddress)
        {
            var usersGuid = _claimsInformationGetters.GetUsersGuid();
            try
            {
                UserAccountService.ChangeEmailRequest(usersGuid, emailAddress.NewEmail);
            }
            catch (Exception ex)
            {
                return new SuccessOrFailureDto { Success = false, ExceptionMessage = ex.Message };
            }
            return new SuccessOrFailureDto { Success = true };
        }

        public SuccessOrFailureDto VerifyEmailAddress(VerificationKeyDto verificationKeyDto)
        {
            return verificationKeyDto.Cancel ? Cancel(verificationKeyDto.VerificationKey) : Confirm(verificationKeyDto);
        }

        private SuccessOrFailureDto Cancel(string id)
        {
            try
            {
                UserAccountService.CancelVerification(id);
                return new SuccessOrFailureDto { Success = true };
            }
            catch (Exception ex)
            {
                return new SuccessOrFailureDto { Success = false, ExceptionMessage = ex.Message };
            }
        }

        private SuccessOrFailureDto Confirm(VerificationKeyDto verificationKeyDto)
        {
            return ChangeEmailConfirmation(verificationKeyDto);
        }

        private SuccessOrFailureDto ChangeEmailConfirmation(VerificationKeyDto changeEmailFromKeyInput)
        {
            try
            {
                RelationalUserAccount account;
                UserAccountService.VerifyEmailFromKey(changeEmailFromKeyInput.VerificationKey, changeEmailFromKeyInput.Password, out account);
                _changeEmailRepository.ChangeEmailAddress(account.Email, account.ID);
            }
            catch (Exception ex)
            {
                return new SuccessOrFailureDto
                {
                    ExceptionMessage = ex.Message,
                    Success = false
                };
            }
            return new SuccessOrFailureDto
            {
                Success = true
            };
        }
    }
}
