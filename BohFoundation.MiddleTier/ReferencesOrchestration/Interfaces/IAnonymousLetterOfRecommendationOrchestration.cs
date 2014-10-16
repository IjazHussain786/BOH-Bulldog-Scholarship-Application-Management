using BohFoundation.Domain.Dtos.Reference.Anonymous;

namespace BohFoundation.MiddleTier.ReferencesOrchestration.Interfaces
{
    public interface IAnonymousLetterOfRecommendationOrchestration
    {
        void AddLetterOfRecommendation(LetterOfRecommendationDto dto);
    }
}