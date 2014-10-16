using Microsoft.WindowsAzure.Storage.Table;

namespace BohFoundation.AzureStorage.TableStorage.Interfaces
{
    public interface ITableHelper
    {
        CloudTable GetAzureTable(string tableName);
        void DeleteAzureTable(string tableName);
    }
}