using System;

namespace BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces
{
    public interface IChangeApplicantToFinalizedApplicantService
    {
        void FlipApplicant(Guid applicantsGuid);
        void FlipApplicant();
    }
}
