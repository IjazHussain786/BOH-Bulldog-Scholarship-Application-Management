using System;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Table;

namespace BohFoundation.AzureStorage.TableStorage.Implementations.Essay.Entities
{
    public class EssayAzureTableEntity : TableEntity
    {
        public EssayAzureTableEntity(int graduatingYear, string rowKey)
        {
            PartitionKey = graduatingYear.ToString(CultureInfo.InvariantCulture);
            RowKey = rowKey;
        }

        public EssayAzureTableEntity()
        {
            
        }

        public string Essay { get; set; }
        public string EssayPrompt { get; set; }
        public DateTime RevisionDateTime { get; set; }
        public int EssayTopicId { get; set; }
    }
}
