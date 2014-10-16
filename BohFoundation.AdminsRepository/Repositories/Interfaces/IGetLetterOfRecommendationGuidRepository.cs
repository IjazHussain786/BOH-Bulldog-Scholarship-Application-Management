using BohFoundation.Domain.Dtos.Admin.References;

namespace BohFoundation.AdminsRepository.Repositories.Interfaces
{
    public interface IGetLetterOfRecommendationGuidRepository
    {
        GuidSentToReferenceDto GetLetterOfRecommendationGuid(GetLetterOfRecommendationGuidDto dto);
    }
}
