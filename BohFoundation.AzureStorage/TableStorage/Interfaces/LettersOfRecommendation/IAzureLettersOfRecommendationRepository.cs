using BohFoundation.Domain.Dtos.Common.AzureQueuryObjects;
using BohFoundation.Domain.Dtos.Reference.Anonymous;

namespace BohFoundation.AzureStorage.TableStorage.Interfaces.LettersOfRecommendation
{
    public interface IAzureLettersOfRecommendationRepository
    {
        void UpsertLetterOfRecommendation(LetterOfRecommendationKeyValueForEntityFrameworkAndAzureDto essayEntity);
        LetterOfRecommendationDto GetLetterOfRecommendation(AzureTableStorageEntityKeyDto dto);
    }
}