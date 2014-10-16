using System;
using System.Collections.Generic;
using System.Linq;
using BohFoundation.Domain.Claims;
using BohFoundation.Domain.Dtos.Admin;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.Enums;
using BohFoundation.MembershipProvider.Repositories.Interfaces;
using BohFoundation.MembershipProvider.UserManagement.Admin.Interfaces;
using BohFoundation.MembershipProvider.UserManagement.Helpers.Interfaces;
using BohFoundation.MembershipProvider.UserManagement.Manage.Implementation;
using BohFoundation.PersonsRepository.Repositories.Interfaces;
using BohFoundation.Utilities.Context.Interfaces;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;

namespace BohFoundation.MembershipProvider.UserManagement.Admin.Implementation
{
    public class ConfirmApplicationEvaluator : UserManagementBase, IConfirmApplicationEvaluator
    {
        private readonly IClaimsInformationGetters _claimsInformationGetters;
        private readonly IMembershipRebootCustomQueries _membershipRebootCustomQueries;
        private readonly ICreateClaimsListFromAccount _createClaimsListFromAccount;
        private readonly ICreatePersonRepository _personsRepository;

        public ConfirmApplicationEvaluator(IClaimsInformationGetters claimsInformationGetters, IMembershipRebootCustomQueries membershipRebootCustomQueries, UserAccountService<RelationalUserAccount> userAccountService,  ICreateClaimsListFromAccount createClaimsListFromAccount, ICreatePersonRepository personsRepository) : base(userAccountService)
        {
            _claimsInformationGetters = claimsInformationGetters;
            _membershipRebootCustomQueries = membershipRebootCustomQueries;
            _createClaimsListFromAccount = createClaimsListFromAccount;
            _personsRepository = personsRepository;
        }

        public List<PersonDto> GetPendingApplicationEvaluators()
        {
            if(!IsAdmin()) throw new UnauthorizedAccessException();
            return _membershipRebootCustomQueries.GetPendingApplicationEvaluators();
        }

        public int CountOfPendingApplicationEvaluators()
        {
            if (!IsAdmin()) throw new UnauthorizedAccessException();
            return _membershipRebootCustomQueries.CountPendingApplicationEvaluators();
        }

        public void ConfirmOrRejectApplicationEvaluator(ConfirmApplicationEvaluatorDto confirmApplicationEvaluatorDto)
        {
            if (!IsAdmin()) throw new UnauthorizedAccessException();
            var account = UserAccountService.GetByEmail(confirmApplicationEvaluatorDto.EmailAddress);
            if (confirmApplicationEvaluatorDto.Confirm)
            {
                _membershipRebootCustomQueries.RemovePendingApplicationEvaluatorClaim(account.Key);
                UserAccountService.AddClaim(account.ID, ClaimsNames.ApplicationEvaluator, true.ToString());
                AddUserToDomainDb(account, confirmApplicationEvaluatorDto);
            }
            else
            {
                UserAccountService.DeleteAccount(account.ID);
            }
        }

        private void AddUserToDomainDb(RelationalUserAccount account, ConfirmApplicationEvaluatorDto dto)
        {
            if (dto.CreateAdmin)
            {
                UserAccountService.AddClaim(account.ID, ClaimsNames.Admin, true.ToString());
                AddClassifiedUserToDomain(MemberTypesEnum.Admin, account);
            }
            else
            {
                AddClassifiedUserToDomain(MemberTypesEnum.ApplicationEvaluator, account);
            }
        }

        private void AddClassifiedUserToDomain(MemberTypesEnum type, RelationalUserAccount account)
        {
            _personsRepository.CreatePerson(account.ID, CreateName(account), type);
        }

        private Name CreateName(RelationalUserAccount account)
        {
            var claimsList = _createClaimsListFromAccount.CreateClaimsList(account);
            return new Name
            {
                FirstName = claimsList.First(claim => claim.Type == ClaimsNames.FirstName).Value,
                LastName = claimsList.First(claim => claim.Type == ClaimsNames.LastName).Value
            };
        }

        private bool IsAdmin()
        {
            return _claimsInformationGetters.IsAdmin();
        }
    }
}
