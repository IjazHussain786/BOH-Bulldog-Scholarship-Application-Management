using BohFoundation.Domain.Dtos.Applicant.Essay;

namespace BohFoundation.ApplicantsRepository.Repositories.Interfaces
{
    public interface IApplicantsEssayRepository
    {
        void UpsertEssay(EssayDto essayDto);
        EssayDto GetEssay(int essayTopicId);
    }
}
