using AutoMapper;
using BohFoundation.AzureStorage.TableStorage.Implementations.Essay.Entities;
using BohFoundation.AzureStorage.TableStorage.Interfaces.Essay;
using BohFoundation.Domain.Dtos.Applicant.Essay;
using BohFoundation.Domain.Dtos.Common.AzureQueuryObjects;
using Microsoft.WindowsAzure.Storage.Table;

namespace BohFoundation.AzureStorage.TableStorage.Implementations.Essay
{
    public class AzureEssayRepository : IAzureEssayRepository
    {
        private CloudTable EssayTable { get; set; }

        public AzureEssayRepository(string dbConnection)
        {
            Initialize(dbConnection);

            Mapper.CreateMap<EssayAzureTableEntity, EssayDto>();
            Mapper.CreateMap<EssayAzureTableEntityDto, EssayAzureTableEntity>();
        }

        public void UpsertEssay(EssayAzureTableEntityDto essayEntity)
        {
            var tableOperation = TableOperation.InsertOrReplace(Mapper.Map<EssayAzureTableEntity>(essayEntity));
            EssayTable.Execute(tableOperation);
        }

        public EssayDto GetEssay(AzureTableStorageEntityKeyDto essayEntityToRetrieve)
        {
            var tableOperation = TableOperation.Retrieve<EssayAzureTableEntity>(essayEntityToRetrieve.PartitionKey,
                essayEntityToRetrieve.RowKey);

            var result = EssayTable.Execute(tableOperation);

            var essayAzureTableEntity = (EssayAzureTableEntity)result.Result;

            return Mapper.Map<EssayDto>(essayAzureTableEntity);
        }

        private void Initialize(string dbConnection)
        {
            var tableHelper = new TableHelper(dbConnection);
            EssayTable = tableHelper.GetAzureTable("essays");
        }
    }
}
