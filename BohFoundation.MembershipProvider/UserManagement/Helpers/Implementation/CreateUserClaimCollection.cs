using BohFoundation.Domain.Claims;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.Domain.Enums;
using BohFoundation.MembershipProvider.UserManagement.Helpers.Interfaces;
using BrockAllen.MembershipReboot;

namespace BohFoundation.MembershipProvider.UserManagement.Helpers.Implementation
{
    public class CreateUserClaimCollection : ICreateUserClaimCollection
    {
        private UserClaimCollection UserClaimsCollection { get; set; }

        public CreateUserClaimCollection()
        {
            UserClaimsCollection = new UserClaimCollection();
        }

        public UserClaimCollection CreateClaimsCollection(RegisterInputModel model, MemberTypesEnum memberType)
        {
            AddFirstName(model.FirstName);
            AddLastName(model.LastName);
            switch (memberType)
            {
                    case MemberTypesEnum.Applicant: 
                        AddApplicantClaims(model);
                        break;
                    case MemberTypesEnum.PendingApplicationEvaluator: 
                        AddPendingApplicationEvaluatorClaims();
                        break;
            }

            return UserClaimsCollection;
        }

        private void AddPendingApplicationEvaluatorClaims()
        {
            AddClaim(ClaimsNames.ApplicationEvaluatorPendingConfirmation, true.ToString());
        }

        private void AddApplicantClaims(RegisterInputModel model)
        {
            AddClaim(ClaimsNames.GraduatingYear, model.GraduatingYear.ToString());
            AddClaim(ClaimsNames.Applicant, true.ToString());
        }

        private void AddLastName(string lastName)
        {
            AddClaim(ClaimsNames.LastName, lastName);
        }

        private void AddFirstName(string firstName)
        {
            AddClaim(ClaimsNames.FirstName, firstName);
        }

        private void AddClaim(string nameOfClaim, string nameOfInput)
        {
            UserClaimsCollection.Add(nameOfClaim, nameOfInput);
        }
    }
}
