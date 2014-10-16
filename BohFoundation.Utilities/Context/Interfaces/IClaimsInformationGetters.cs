using System;

namespace BohFoundation.Utilities.Context.Interfaces
{
    public interface IClaimsInformationGetters
    {
        Guid GetUsersGuid();
        string GetUsersEmail();
        string GetUsersFirstName();
        string GetUsersLastName();
        string GetUsersFullName();
        int GetApplicantsGraduatingYear();
        bool IsAdmin();
        bool IsApplicant();
        bool IsReference();
        bool IsApplicationEvaluator();
        bool IsApplicationEvaluatorPendingConfirmation();
    }
}
