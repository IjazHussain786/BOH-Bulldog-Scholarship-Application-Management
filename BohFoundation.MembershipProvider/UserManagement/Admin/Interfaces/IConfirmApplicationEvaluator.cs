using System.Collections.Generic;
using BohFoundation.Domain.Dtos.Admin;
using BohFoundation.Domain.Dtos.UserManagement;

namespace BohFoundation.MembershipProvider.UserManagement.Admin.Interfaces
{
    public interface IConfirmApplicationEvaluator
    {
        List<PersonDto> GetPendingApplicationEvaluators();
        int CountOfPendingApplicationEvaluators();
        void ConfirmOrRejectApplicationEvaluator(ConfirmApplicationEvaluatorDto confirmApplicationEvaluatorDto);
    }
}
