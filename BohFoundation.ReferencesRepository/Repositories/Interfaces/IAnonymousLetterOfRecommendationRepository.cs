using BohFoundation.Domain.Dtos.Reference;
using BohFoundation.Domain.Dtos.Reference.Anonymous;

namespace BohFoundation.ReferencesRepository.Repositories.Interfaces
{
    public interface IAnonymousLetterOfRecommendationRepository
    {
        InformationForReferenceFormDto GetInformationForReferenceForm(GuidForLetterOfRecommendationDto model);
        void UpsertReferencesPersonalInformation(ReferencePersonalInformationDto model);
    }
}
