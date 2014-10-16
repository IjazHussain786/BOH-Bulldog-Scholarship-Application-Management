namespace BohFoundation.Domain.Dtos.Reference.Anonymous
{
    public class LetterOfRecommendationKeyValueForEntityFrameworkAndAzureDto : LetterOfRecommendationDto
    {
        public string RowKey { get; set; }
        public string PartitionKey { get; set; }
    }
}
