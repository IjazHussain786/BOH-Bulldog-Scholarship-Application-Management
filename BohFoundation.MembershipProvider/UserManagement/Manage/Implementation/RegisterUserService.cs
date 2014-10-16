using System;
using BohFoundation.Domain.Dtos;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.Enums;
using BohFoundation.MembershipProvider.UserManagement.Helpers.Interfaces;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;
using BohFoundation.PersonsRepository.Repositories.Interfaces;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;

namespace BohFoundation.MembershipProvider.UserManagement.Manage.Implementation
{
    public class RegisterUserService : UserManagementBase, IRegisterUserService
    {
        private readonly ICreateUserClaimCollection _createUserClaimCollection;
        private readonly ICreatePersonRepository _personsRepository;

        private MemberTypesEnum _memberType;

        public RegisterUserService(UserAccountService<RelationalUserAccount> userAccountService, ICreateUserClaimCollection createUserClaimCollection, ICreatePersonRepository personsRepository)
            : base(userAccountService)
        {
            _createUserClaimCollection = createUserClaimCollection;
            _personsRepository = personsRepository;
        }

        public SuccessOrFailureDto CreateAccount(RegisterInputModel model, MemberTypesEnum memberType)
        {
            _memberType = memberType;

            try
            {
                var result = UserAccountService.CreateAccount(null, model.Password, model.EmailAddress);
                try
                {
                    AddClaims(model, result);
                    AddPersonToApplicationsDomain(model, result);
                }
                catch (Exception ex)
                {
                    return new SuccessOrFailureDto {Success = false, ExceptionMessage = ex.Message};
                }
            }
            catch (Exception ex)
            {
                return new SuccessOrFailureDto{Success = false, ExceptionMessage = ex.Message};
            }
            return new SuccessOrFailureDto{Success = true};
        }

        private void AddPersonToApplicationsDomain(RegisterInputModel model, RelationalUserAccount result)
        {
            var name = new Name
            {
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            _personsRepository.CreatePerson(result.ID, name, _memberType);
        }

        private void AddClaims(RegisterInputModel model, RelationalUserAccount result)
        {
            var claimsCollection = _createUserClaimCollection.CreateClaimsCollection(model, _memberType);
            UserAccountService.AddClaims(result.ID, claimsCollection);
        }
    }
}
