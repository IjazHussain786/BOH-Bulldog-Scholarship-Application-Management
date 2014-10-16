using BohFoundation.Domain.Dtos.Applicant.Extracurricular;

namespace BohFoundation.ApplicantsRepository.Repositories.Interfaces
{
    public interface IExtracurricularActivitiesRepository
    {
        void UpsertExtracurricularActivities (ExtracurricularActivitiesDto extracurricularActivities);
        ExtracurricularActivitiesDto GetExtracurricularActivities();
    }
}
