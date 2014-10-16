using System.Collections.Generic;
using BohFoundation.Domain.Dtos.Applicant.References;

namespace BohFoundation.ApplicantsRepository.Repositories.Interfaces
{
    public interface IApplicantsReferencesRepository
    {
        ICollection<ReferenceDto> GetReferences();
        void AddReference(ApplicantReferenceForEntityFrameworkDto referenceInput);
    }
}
