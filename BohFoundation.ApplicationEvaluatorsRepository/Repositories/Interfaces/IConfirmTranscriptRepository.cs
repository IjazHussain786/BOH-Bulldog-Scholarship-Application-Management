using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants;

namespace BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces
{
    public interface IConfirmTranscriptRepository
    {
        void ConfirmTranscript(ConfirmTranscriptDto confirmTranscript);
    }
}
