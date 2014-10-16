using System;
using System.Linq;
using System.Security.Claims;
using BohFoundation.Domain.Claims;
using BohFoundation.Utilities.Context.Interfaces;
using BohFoundation.Utilities.Context.Interfaces.Context;

namespace BohFoundation.Utilities.Context.Implementation
{
    public class ClaimsInformationGetters : IClaimsInformationGetters
    {
        public Guid GetUsersGuid()
        {
            Guid userGuid;
            
            var claim = GetClaim(ClaimTypes.NameIdentifier);
            if (claim == null) return new Guid();
            
            var userGuidString = claim.Value;
            Guid.TryParse(userGuidString, out userGuid);
            
            return userGuid;
        }

        public string GetUsersEmail()
        {
            return GetClaim(ClaimTypes.Email).Value;
        }

        public string GetUsersFirstName()
        {
            return GetClaim(ClaimsNames.FirstName).Value;
        }

        public string GetUsersLastName()
        {
            return GetClaim(ClaimsNames.LastName).Value;
        }

        public string GetUsersFullName()
        {
            return GetUsersFirstName() + " " + GetUsersLastName();
        }

        public int GetApplicantsGraduatingYear()
        {
            return Convert.ToInt32(GetClaim(ClaimsNames.GraduatingYear).Value);
        }

        public bool IsAdmin()
        {
            return IsClaimOnPrinciple(ClaimsNames.Admin);
        }

        public bool IsApplicant()
        {
            return IsClaimOnPrinciple(ClaimsNames.Applicant);
        }

        public bool IsReference()
        {
            return IsClaimOnPrinciple(ClaimsNames.Reference);
        }

        public bool IsApplicationEvaluator()
        {
            return IsClaimOnPrinciple(ClaimsNames.ApplicationEvaluator);
        }

        public bool IsApplicationEvaluatorPendingConfirmation()
        {
            return IsClaimOnPrinciple(ClaimsNames.ApplicationEvaluatorPendingConfirmation);
        }

        public bool IsClaimOnPrinciple(string claimType)
        {
            var claimOrNull = GetClaim(claimType);
            return claimOrNull != null;
        }

        public Claim GetClaim(string claimName)
        {
            return ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == claimName);
        }
    }
}
