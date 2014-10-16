using System.Collections.Generic;
using BohFoundation.Domain.Dtos.Applicant.Academic;

namespace BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication
{
    public class CompletedAcademicInformationDto
    {
        public AcademicInformationDto BasicAcademicInformation { get; set; }
        public ICollection<LowGradeDto> LowGrades { get; set; }
        public TranscriptBlobReferenceDto TranscriptBlobReference { get; set; }
        public bool TranscriptMatchesDatabaseValues { get; set; }
        public bool TranscriptDoesNotMatchDatabaseValues { get; set; }
    }
}
