using BohFoundation.Domain.Dtos.Reference.Anonymous;

namespace BohFoundation.MiddleTier.ReferencesOrchestration.Interfaces.Helpers
{
    namespace BohFoundation.ReferencesRepository.OrchestrationLayer.Interfaces.Helper
    {
        public interface IGenerateLetterOfRecommendationKey
        {
            LetterOfRecommendationKeyValueForEntityFrameworkAndAzureDto GenerateKeyValueForLettersOfRecommendation(
                LetterOfRecommendationDto letterOfRecommendationDto);
        }
    }
}
