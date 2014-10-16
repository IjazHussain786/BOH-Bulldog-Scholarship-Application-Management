using System;
using BohFoundation.Domain.Dtos.Applicant.Extracurricular;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication;

namespace BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces
{
    public interface IGetCompletedApplicationRepository
    {
        ApplicantsGeneralInformationDto GetGeneralInformation(Guid applicantsGuid);
        ExtracurricularActivitiesDto GetExtracurricularActivities(Guid applicantsGuid);
        CompletedAcademicInformationDto GetAcademicInformation(Guid applicantsGuid);
        CollectionsOfEssaysAndLettersOfRecommendationDto GetCollectionsOfEssaysAndLettersOfRecommendation(
            Guid applicantsGuid);
    }
}
