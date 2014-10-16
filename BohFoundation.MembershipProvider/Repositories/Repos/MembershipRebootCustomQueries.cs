using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using BohFoundation.Domain.Claims;
using BohFoundation.Domain.Dtos.UserManagement;
using BohFoundation.MembershipProvider.Repositories.Contexts;
using BohFoundation.MembershipProvider.Repositories.Interfaces;
using BrockAllen.MembershipReboot.Relational;

namespace BohFoundation.MembershipProvider.Repositories.Repos
{
    public class MembershipRebootCustomQueries : IMembershipRebootCustomQueries
    {
        private readonly string _dbConnection;

        public MembershipRebootCustomQueries(string nameOfConnection)
        {
            _dbConnection = nameOfConnection;
        }

        public int CountPendingApplicationEvaluators()
        {
            int count;
            using (var context = new MembershipRebootContext(_dbConnection))
            {
                count =
                    context.Claims.Count(
                        x => x.Type == ClaimsNames.ApplicationEvaluatorPendingConfirmation && x.Value == "True");
            }
            return count;
        }

        public List<PersonDto> GetPendingApplicationEvaluators()
        {
            var listToReturn = new List<PersonDto>();
            using (var context = new MembershipRebootContext(_dbConnection))
            {
                var listOfKeyValues =
                    context.Claims.Where(
                        x => x.Type == ClaimsNames.ApplicationEvaluatorPendingConfirmation && x.Value == "True").ToList();

                listToReturn.AddRange(listOfKeyValues.Select(claim => context.Users.First(x => x.Key == claim.ParentKey)).Select(relationalUserAccount => new PersonDto
                {
                    EmailAddress    = relationalUserAccount.Email, 
                    FirstName       = GetClaim(ClaimsNames.FirstName, relationalUserAccount), 
                    LastName        = GetClaim(ClaimsNames.LastName, relationalUserAccount)
                }));
            }
            return listToReturn;
        }

        public void RemovePendingApplicationEvaluatorClaim(int key)
        {
            RemoveClaim(key, ClaimsNames.ApplicationEvaluatorPendingConfirmation);
        }

        public void RemoveApplicantClaim(int key)
        {
            RemoveClaim(key, ClaimsNames.Applicant);
        }

        private void RemoveClaim(int key, string claimType)
        {
            using (var context = new MembershipRebootContext(_dbConnection))
            {
                var claim =
                    context.Claims.First(
                        x =>
                            x.ParentKey == key &&
                            x.Type == claimType &&
                            x.Value == "True");
                context.Claims.Remove(claim);
                context.SaveChanges();
            }
        }

        private string GetClaim(string claimsName, RelationalUserAccount account)
        {
            return account.Claims.First(x => x.Type == claimsName).Value;
        }
    }
}
