using System;
using System.Globalization;
using AutoMapper;
using BohFoundation.Domain.Dtos.Reference.Anonymous;
using BohFoundation.MiddleTier.ReferencesOrchestration.Interfaces.Helpers.BohFoundation.ReferencesRepository.OrchestrationLayer.Interfaces.Helper;

namespace BohFoundation.MiddleTier.ReferencesOrchestration.Implementations.Helpers
{
    public class GenerateLetterOfRecommendationKey : IGenerateLetterOfRecommendationKey
    {
        public GenerateLetterOfRecommendationKey()
        {
            Mapper.CreateMap<LetterOfRecommendationDto, LetterOfRecommendationKeyValueForEntityFrameworkAndAzureDto>();
        }

        public LetterOfRecommendationKeyValueForEntityFrameworkAndAzureDto GenerateKeyValueForLettersOfRecommendation(
            LetterOfRecommendationDto letterOfRecommendationDto)
        {
            var yearPostFix = "_" + DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture);

            var key = letterOfRecommendationDto.LetterOfRecommendationGuid + yearPostFix;

            var mappedLetterOfRecommendation =
                Mapper.Map<LetterOfRecommendationKeyValueForEntityFrameworkAndAzureDto>(letterOfRecommendationDto);

            mappedLetterOfRecommendation.RowKey = key;
            mappedLetterOfRecommendation.PartitionKey = "Recommendation" + yearPostFix;

            return mappedLetterOfRecommendation;
        }
    }
}
