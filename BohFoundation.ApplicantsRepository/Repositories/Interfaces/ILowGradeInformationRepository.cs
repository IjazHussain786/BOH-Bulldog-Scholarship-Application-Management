using System.Collections.Generic;
using BohFoundation.Domain.Dtos.Applicant.Academic;

namespace BohFoundation.ApplicantsRepository.Repositories.Interfaces
{
    public interface ILowGradeInformationRepository
    {
        void UpsertLowGradeInformation(ICollection<LowGradeDto> lowGrades);
        LowGradesWithGpaDto GetLowGradeInformation();
    }
}
