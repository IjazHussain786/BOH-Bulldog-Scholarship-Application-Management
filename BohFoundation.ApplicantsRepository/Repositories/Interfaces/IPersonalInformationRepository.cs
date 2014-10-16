using BohFoundation.Domain.Dtos.Applicant.PersonalInformation;

namespace BohFoundation.ApplicantsRepository.Repositories.Interfaces
{
    public interface IPersonalInformationRepository
    {
        void UpsertPersonalInformation(ApplicantPersonalInformationDto applicantPersonalInformationDto);
        ApplicantPersonalInformationDto GetPersonalInformation();
    }
}
