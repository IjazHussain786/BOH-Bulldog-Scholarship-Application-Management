using System;

namespace BohFoundation.Utilities.Context.Interfaces
{
    public interface IDeadlineUtilities
    {
        bool IsAfterDeadline();
        DateTime GetApplicantsDeadlineInUtc();
    }
}
