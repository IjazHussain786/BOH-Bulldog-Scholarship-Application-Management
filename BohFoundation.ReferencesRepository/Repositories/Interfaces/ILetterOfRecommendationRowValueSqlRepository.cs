using BohFoundation.Domain.Dtos.Reference;
using BohFoundation.Domain.Dtos.Reference.Anonymous;

namespace BohFoundation.ReferencesRepository.Repositories.Interfaces
{
    public interface ILetterOfRecommendationRowValueSqlRepository
    {
        void UpsertLetterOfRecommendationKeyValues(LetterOfRecommendationKeyValueForEntityFrameworkAndAzureDto model);
    }
}
