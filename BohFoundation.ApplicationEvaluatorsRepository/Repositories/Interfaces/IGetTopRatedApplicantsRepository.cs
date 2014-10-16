using BohFoundation.Domain.Dtos.ApplicationEvaluator.RatingSummary;

namespace BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces
{
    public interface IGetTopRatedApplicantsRepository
    {
        Top5ApplicantsDto GetTop5Applicants(int graduatingClass);
    }
}
