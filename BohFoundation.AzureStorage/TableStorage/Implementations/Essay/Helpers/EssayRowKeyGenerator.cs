using System;
using BohFoundation.AzureStorage.TableStorage.Interfaces.Essay.Helpers;

namespace BohFoundation.AzureStorage.TableStorage.Implementations.Essay.Helpers
{
    public class EssayRowKeyGenerator : IEssayRowKeyGenerator
    {
        public string CreateRowKeyForEssay(Guid applicantsGuid, int essayTopicId)
        {
            return "ESSAYTOPICID_" + essayTopicId + "_USERSGUID_" + applicantsGuid;
        }
    }
}
