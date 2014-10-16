using BohFoundation.AzureStorage.TableStorage.Implementations.Essay.Entities;
using BohFoundation.Domain.Dtos.Applicant.Essay;
using BohFoundation.Domain.Dtos.Common.AzureQueuryObjects;

namespace BohFoundation.AzureStorage.TableStorage.Interfaces.Essay
{
    public interface IAzureEssayRepository
    {
        void UpsertEssay(EssayAzureTableEntityDto essayEntity);
        EssayDto GetEssay(AzureTableStorageEntityKeyDto essayEntityToRetrieve);
    }
}
