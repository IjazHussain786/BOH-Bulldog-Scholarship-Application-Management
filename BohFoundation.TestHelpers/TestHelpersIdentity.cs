using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using BohFoundation.Domain.Claims;

namespace BohFoundation.TestHelpers
{
    public static class TestHelpersIdentity
    {
            public static void PopulateIdentityWithStandardAnon()
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new ClaimsIdentity(new GenericIdentity(String.Empty), new Collection<Claim>()), new string[0]);
            }

            public static void PopulateAdminIdentity(Guid usersGuid, string email)
            {
                PopulateIdentity(usersGuid, email, ClaimsNames.Admin);
            }

            public static void PopulateReferenceIdentity(Guid usersGuid, string email)
            {
                PopulateIdentity(usersGuid, email, ClaimsNames.Reference);
            }

            public static void PopulateApplicantIdentity(Guid usersGuid, string email)
            {
                PopulateIdentity(usersGuid, email, ClaimsNames.Applicant);
            }

            public static void PopulateApplicationEvaluatorIdentity(Guid usersGuid, string email)
            {
                PopulateIdentity(usersGuid, email, ClaimsNames.ApplicationEvaluator);
            }

            public static void PopulateApplicationEvaluatorPendingConfirmationIdentity(Guid usersGuid, string email)
            {
                PopulateIdentity(usersGuid, email, ClaimsNames.ApplicationEvaluatorPendingConfirmation);
            }

            public static void PopulateIdentityWithNoPermissions(Guid usersGuid, string email)
            {
                PopulateIdentity(usersGuid, email, new string[0]);
            }
            
            public static void PopulateIdentity(Guid userGuid, string email, string claimName)
            {
                var array = new[] {claimName};
                PopulateIdentity(userGuid, email, array);
            }

            public static void PopulateIdentity(Guid usersGuid, string email, string[] claimsArray)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usersGuid.ToString()),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimsNames.FirstName, TestHelpersCommonFields.FirstName),
                    new Claim(ClaimsNames.LastName, TestHelpersCommonFields.LastName),
                    new Claim(ClaimsNames.GraduatingYear, TestHelpersCommonFields.GraduatingYear.ToString())
                };

                claims.AddRange(claimsArray.Select(claimName => new Claim(claimName, true.ToString())));

                var claimsIdentity = new ClaimsIdentity(new Collection<Claim>(claims));

                ClaimsPrincipal.Current.AddIdentity(claimsIdentity);

                Thread.CurrentPrincipal = new GenericPrincipal(new ClaimsIdentity(new GenericIdentity(email), claims),
                    new string[0]);
            }
        }
    }

