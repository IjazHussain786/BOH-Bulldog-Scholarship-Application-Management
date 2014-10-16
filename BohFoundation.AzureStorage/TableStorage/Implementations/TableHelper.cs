using System;
using System.Configuration;
using BohFoundation.AzureStorage.TableStorage.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace BohFoundation.AzureStorage.TableStorage.Implementations
{
    public class TableHelper : ITableHelper
    {
        private readonly string _dbConnection;

        public TableHelper(string dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public CloudTable GetAzureTable(string tableName)
        {
            CheckIfNullOrEmpty(tableName);
            
            var table = GetCloudTableClient().GetTableReference(tableName);

            table.CreateIfNotExists();

            return table;
        }

        public void DeleteAzureTable(string tableName)
        {
            CheckIfNullOrEmpty(tableName);

            var table = GetCloudTableClient().GetTableReference(tableName);

            table.DeleteIfExists();
        }

        private void CheckIfNullOrEmpty(string tableName)
        {
            if (String.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("tableName");
            }
        }

        private CloudTableClient GetCloudTableClient()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings[_dbConnection].ConnectionString);

            return storageAccount.CreateCloudTableClient();
        }
    }
}
