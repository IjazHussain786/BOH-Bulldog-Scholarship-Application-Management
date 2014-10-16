namespace BohFoundation.Domain.Dtos.Common.AzureQueuryObjects
{
    public class AzureTableStorageEntityKeyDto
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
}
