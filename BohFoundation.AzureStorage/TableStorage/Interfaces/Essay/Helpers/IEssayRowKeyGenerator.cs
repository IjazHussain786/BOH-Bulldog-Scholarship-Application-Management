using System;

namespace BohFoundation.AzureStorage.TableStorage.Interfaces.Essay.Helpers
{
    public interface IEssayRowKeyGenerator
    {
        string CreateRowKeyForEssay(Guid applicantsGuid, int essayId);
    }
}
