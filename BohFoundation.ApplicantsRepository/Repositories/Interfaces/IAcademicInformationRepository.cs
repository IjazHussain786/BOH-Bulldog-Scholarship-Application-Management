using BohFoundation.Domain.Dtos.Applicant.Academic;

namespace BohFoundation.ApplicantsRepository.Repositories.Interfaces
{
    public interface IAcademicInformationRepository
    {
        void UpsertAcademicInformation(AcademicInformationDto academicInformationDto);
        AcademicInformationDto GetAcademicInformation();
    }
}
