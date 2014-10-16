using BohFoundation.Domain.Dtos.Applicant.References;

namespace BohFoundation.MiddleTier.ApplicantsOrchestration.Interfaces.Helpers
{
    public interface ICreateEmailBodyForApplicantReferenceRequest
    {
        ApplicantReferenceForEntityFrameworkDto CreateBody(ApplicantReferenceInputDto message);
    }
}
