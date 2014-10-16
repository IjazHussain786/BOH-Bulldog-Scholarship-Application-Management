using System;

namespace BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants
{
    public class ConfirmTranscriptDto
    {
        public Guid ApplicantsGuid { get; set; }
        public bool InformationMatchesTranscriptPdf { get; set; }
    }
}