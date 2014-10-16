using BohFoundation.AzureStorage.TableStorage.Interfaces.LettersOfRecommendation;
using BohFoundation.Domain.Dtos.Reference.Anonymous;
using BohFoundation.MiddleTier.ReferencesOrchestration.Interfaces;
using BohFoundation.MiddleTier.ReferencesOrchestration.Interfaces.Helpers.BohFoundation.ReferencesRepository.OrchestrationLayer.Interfaces.Helper;
using BohFoundation.ReferencesRepository.Repositories.Interfaces;

namespace BohFoundation.MiddleTier.ReferencesOrchestration.Implementations
{
    public class AnonymousLetterOfRecommendationOrchestration : IAnonymousLetterOfRecommendationOrchestration
    {
        private readonly ILetterOfRecommendationRowValueSqlRepository _sqlRepository;
        private readonly IAzureLettersOfRecommendationRepository _azureRepository;
        private readonly IGenerateLetterOfRecommendationKey _generateLetterOfRecommendationKey;

        public AnonymousLetterOfRecommendationOrchestration(ILetterOfRecommendationRowValueSqlRepository sqlRepository,
            IAzureLettersOfRecommendationRepository azureRepository,
            IGenerateLetterOfRecommendationKey generateLetterOfRecommendationKey)
        {
            _sqlRepository = sqlRepository;
            _azureRepository = azureRepository;
            _generateLetterOfRecommendationKey = generateLetterOfRecommendationKey;
        }

        public void AddLetterOfRecommendation(LetterOfRecommendationDto dto)
        {
            var transformedDto = _generateLetterOfRecommendationKey.GenerateKeyValueForLettersOfRecommendation(dto);
            _azureRepository.UpsertLetterOfRecommendation(transformedDto);
            _sqlRepository.UpsertLetterOfRecommendationKeyValues(transformedDto);
        }
    }
}
