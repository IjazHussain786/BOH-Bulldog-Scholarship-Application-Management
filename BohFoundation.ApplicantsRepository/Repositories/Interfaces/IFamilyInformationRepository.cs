using BohFoundation.Domain.Dtos.Applicant.Family;

namespace BohFoundation.ApplicantsRepository.Repositories.Interfaces
{
    public interface IFamilyInformationRepository
    {
        void UpsertFamilyInformation(FamilyInformationDto familyInformationDto);
        FamilyInformationDto GetFamilyInformation();
    }
}
