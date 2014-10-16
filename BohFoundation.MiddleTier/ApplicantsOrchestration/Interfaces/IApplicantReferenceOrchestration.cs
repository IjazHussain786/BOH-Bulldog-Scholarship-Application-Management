using System.Collections.Generic;
using BohFoundation.Domain.Dtos.Applicant.References;

namespace BohFoundation.MiddleTier.ApplicantsOrchestration.Interfaces
{
    public interface IApplicantReferenceOrchestration
    {
        ICollection<ReferenceDto> GetReferences();
        void AddReference(ApplicantReferenceInputDto reference);
    }
}
