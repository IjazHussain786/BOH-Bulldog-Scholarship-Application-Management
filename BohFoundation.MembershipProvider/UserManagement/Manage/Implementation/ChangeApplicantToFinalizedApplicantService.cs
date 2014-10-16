using System;
using BohFoundation.Domain.Claims;
using BohFoundation.MembershipProvider.Repositories.Interfaces;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;
using BohFoundation.Utilities.Context.Interfaces;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;

namespace BohFoundation.MembershipProvider.UserManagement.Manage.Implementation
{
    public class ChangeApplicantToFinalizedApplicantService : UserManagementBase, IChangeApplicantToFinalizedApplicantService
    {
        private readonly IClaimsInformationGetters _claimsInformationGetters;
        private readonly IMembershipRebootCustomQueries _customQueries;

        public ChangeApplicantToFinalizedApplicantService(UserAccountService<RelationalUserAccount> userAccountService, IClaimsInformationGetters claimsInformationGetters, IMembershipRebootCustomQueries customQueries) : base(userAccountService)
        {
            _claimsInformationGetters = claimsInformationGetters;
            _customQueries = customQueries;
        }

        public void FlipApplicant(Guid applicantsGuid)
        {
            RemoveApplicantClaim(applicantsGuid);
            UserAccountService.AddClaim(applicantsGuid, ClaimsNames.ApplicantAfterFinalization, true.ToString());
        }

        public void FlipApplicant()
        {
            FlipApplicant(_claimsInformationGetters.GetUsersGuid());
        }

        private void RemoveApplicantClaim(Guid applicantsGuid)
        {
            var account = UserAccountService.GetByID(applicantsGuid);
            _customQueries.RemoveApplicantClaim(account.Key);
        }
    }
}