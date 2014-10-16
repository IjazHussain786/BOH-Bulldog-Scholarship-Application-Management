using System;
using System.Collections.Generic;
using BohFoundation.Domain.Dtos.UserManagement;

namespace BohFoundation.MembershipProvider.Repositories.Interfaces
{
    public interface IMembershipRebootCustomQueries
    {
        int CountPendingApplicationEvaluators();
        List<PersonDto> GetPendingApplicationEvaluators();
        void RemovePendingApplicationEvaluatorClaim(int key);
        void RemoveApplicantClaim(int key);
    }
}
