using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants;

namespace BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces
{
    public interface IGetListOfFinalizedApplicantsRepository
    {
        AllFinalizedApplicantsForAGraduatingYearDto GetAllFinalizedApplicantsByGraduatingYear(int year);
    }
}
