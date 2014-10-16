using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BohFoundation.Domain.Claims;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Ninject;

namespace BohFoundation.WebApi.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        private List<Claim> Claims { get; set; } 

        public override Task ValidateClientAuthentication(
            OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();

            return Task.FromResult<object>(null);
        }

        //Todo TEST
        public override Task ValidateTokenRequest(OAuthValidateTokenRequestContext context)
        {
            if (context.TokenRequest.IsResourceOwnerPasswordCredentialsGrantType)
            {
                var password = context.TokenRequest.ResourceOwnerPasswordCredentialsGrant.Password;
                var emailAddress = context.TokenRequest.ResourceOwnerPasswordCredentialsGrant.UserName;
                
                var loginInputDto = new LoginInputDto
                {
                    Password = password,
                    EmailAddress = emailAddress
                };

                var loginServices = NinjectWebCommon.Kernel.Get<ILoginServices>();

                var result = loginServices.LogIn(loginInputDto);

                if (result.Success)
                {
                    Claims = result.Claims;
                    context.Validated();
                }
                else
                {
                    context.SetError("Invalid password or username.");
                }
            }

            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var claimsIdentity = new ClaimsIdentity("MembershipReboot-Password");

            claimsIdentity.AddClaims(Claims);

            var ticket = new AuthenticationTicket(claimsIdentity, new AuthenticationProperties());

            context.Validated(ticket);

            return base.GrantResourceOwnerCredentials(context);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            AddExpiredDate(context);
            AddExplainerToToken(context, "emailAddress", ClaimTypes.Email);
            AddExplainerToToken(context, "guid", ClaimTypes.NameIdentifier);
            AddExplainerToToken(context, "graduatingYear", ClaimsNames.GraduatingYear);
            AddExplainerToToken(context, "firstName", ClaimsNames.FirstName);
            AddExplainerToToken(context, "lastName", ClaimsNames.LastName);

            AddExplainerToTokenIsRole(context, "applicant", ClaimsNames.Applicant);
            AddExplainerToTokenIsRole(context, "admin", ClaimsNames.Admin);
            AddExplainerToTokenIsRole(context, "reference", ClaimsNames.Reference);
            AddExplainerToTokenIsRole(context, "applicationEvaluator", ClaimsNames.ApplicationEvaluator);
            AddExplainerToTokenIsRole(context, "applicationEvaluatorPendingConfirmation", ClaimsNames.ApplicationEvaluatorPendingConfirmation);
            AddExplainerToTokenIsRole(context, "finalizedApplicant", ClaimsNames.ApplicantAfterFinalization);

            return Task.FromResult<object>(null);
        }

        private void AddExpiredDate(OAuthTokenEndpointContext context)
        {
            var dateValue = context.Properties.Dictionary.FirstOrDefault(x => x.Key == ".expires").Value;
            context.AdditionalResponseParameters.Add("expires", dateValue);
        }

        private void AddExplainerToTokenIsRole(OAuthTokenEndpointContext context, string nameOfProperty, string lookupValue)
        {
            var claim = context.Identity.Claims.FirstOrDefault(x => x.Type == lookupValue);
            if (claim != null)
            {
                if (claim.Value == "True")
                {
                    context.AdditionalResponseParameters.Add(nameOfProperty, true);
                }
            }
            else
            {
                context.AdditionalResponseParameters.Add(nameOfProperty, false);
            }
        }

        private void AddExplainerToToken(OAuthTokenEndpointContext context, string nameOfProperty, string lookupValue)
        {
            var claim = context.Identity.Claims.FirstOrDefault(x => x.Type == lookupValue);
            if (claim != null)
            {
                context.AdditionalResponseParameters.Add(nameOfProperty, claim.Value);
            }
        }
    }
}