using System;
using BohFoundation.Domain.Dtos.Common.AzureQueuryObjects;

namespace BohFoundation.AzureStorage.TableStorage.Implementations.Essay.Entities
{
    public class EssayAzureTableEntityDto : AzureTableStorageEntityKeyDto
    {
        public string Essay { get; set; }
        public string EssayPrompt { get; set; }
        public DateTime RevisionDateTime { get; set; }
        public int EssayTopicId { get; set; }
    }
}
