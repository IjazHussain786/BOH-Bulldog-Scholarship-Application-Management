namespace BohFoundation.Domain.EntityFrameworkModels.Applicants
{
    public class ApplicantMetadata
    {
        public int Id { get; set; }

        public bool TranscriptMatchesDatabaseValues { get; set; }
        public bool TranscriptDoesNotMatchDatabaseValues { get; set; }

        public bool AcceptanceNonSelectionLetterSent { get; set; }
        public bool Finalist { get; set; }

        public bool NotCompletedButFinalizedApplication { get; set; }
        public bool ApplicationFinalized { get; set; }

        public virtual Applicant Applicant { get; set; }
    }
}