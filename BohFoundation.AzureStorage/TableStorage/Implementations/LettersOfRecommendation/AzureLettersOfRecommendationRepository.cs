using AutoMapper;
using BohFoundation.AzureStorage.TableStorage.Implementations.LettersOfRecommendation.Entities;
using BohFoundation.AzureStorage.TableStorage.Interfaces.LettersOfRecommendation;
using BohFoundation.Domain.Dtos.Common.AzureQueuryObjects;
using BohFoundation.Domain.Dtos.Reference.Anonymous;
using Microsoft.WindowsAzure.Storage.Table;

namespace BohFoundation.AzureStorage.TableStorage.Implementations.LettersOfRecommendation
{
    public class AzureLettersOfRecommendationRepository : IAzureLettersOfRecommendationRepository
    {
        private CloudTable LetterOfRecommendationTable { get; set; }

        public AzureLettersOfRecommendationRepository(string dbConnection)
        {
            Initialize(dbConnection);
            Mapper.CreateMap<LetterOfRecommendationAzureTableEntity, LetterOfRecommendationDto>();
        }

        public void UpsertLetterOfRecommendation(LetterOfRecommendationKeyValueForEntityFrameworkAndAzureDto essayEntity)
        {
            var entityToInsert = new LetterOfRecommendationAzureTableEntity(essayEntity.RowKey, essayEntity.PartitionKey)
            {
                LetterOfRecommendation = essayEntity.LetterOfRecommendation
            };

            var operation = TableOperation.InsertOrReplace(entityToInsert);
            
            LetterOfRecommendationTable.Execute(operation);
        }

        public LetterOfRecommendationDto GetLetterOfRecommendation(AzureTableStorageEntityKeyDto dto)
        {
            var operation = TableOperation.Retrieve<LetterOfRecommendationAzureTableEntity>(dto.PartitionKey, dto.RowKey);

            var result = LetterOfRecommendationTable.Execute(operation);

            var letterOfRecommendationTableEntity = (LetterOfRecommendationAzureTableEntity) result.Result;

            return Mapper.Map<LetterOfRecommendationDto>(letterOfRecommendationTableEntity);
        }

        private void Initialize(string dbConnection)
        {
            var tableHelper = new TableHelper(dbConnection);
            LetterOfRecommendationTable = tableHelper.GetAzureTable("lettersofrecommendation");
        }
    }
}
