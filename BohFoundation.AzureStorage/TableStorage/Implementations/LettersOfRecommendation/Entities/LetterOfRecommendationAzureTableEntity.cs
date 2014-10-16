using Microsoft.WindowsAzure.Storage.Table;

namespace BohFoundation.AzureStorage.TableStorage.Implementations.LettersOfRecommendation.Entities
{
    public class LetterOfRecommendationAzureTableEntity : TableEntity
    {
        public LetterOfRecommendationAzureTableEntity(string rowKey, string partitionKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public LetterOfRecommendationAzureTableEntity()
        {
        }

        public string LetterOfRecommendation { get; set; }
    }
}