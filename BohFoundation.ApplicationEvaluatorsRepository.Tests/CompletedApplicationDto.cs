using BohFoundation.Domain.Dtos.Applicant.Extracurricular;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication;

namespace BohFoundation.ApplicationEvaluatorsRepository.Tests
{
    public class CompletedApplicationDto
    {
        public ApplicantsGeneralInformationDto ApplicantsGeneralInformation { get; set; }
        public CompletedAcademicInformationDto AcademicInformation { get; set; }
        public ExtracurricularActivitiesDto ExtracurricularActivities { get; set; }
        public CollectionsOfEssaysAndLettersOfRecommendationDto CollectionsOfEssaysAndLettersOfRecommendation { get; set; }
    }
}
