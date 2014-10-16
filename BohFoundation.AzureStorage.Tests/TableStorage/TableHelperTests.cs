using System;
using System.Threading.Tasks;
using BohFoundation.AzureStorage.TableStorage.Implementations;
using BohFoundation.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;

namespace BohFoundation.AzureStorage.Tests.TableStorage
{
    [TestClass]
    public class TableHelperTests
    {
        private static TableHelper _tableHelper;
        private const string TableNamePrefix = "table";
        
        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            Setup();
            CreateTable();
            DeleteTable();
        }

        private static void Setup()
        {
            TestHelpersCommonFields.InitializeFields();
            TableName = TableNamePrefix + TestHelpersCommonFields.GuidOneWithNoDashes;

            _tableHelper = new TableHelper(TestHelpersCommonFields.AzureConnectionName);
        }
        
        private static string TableName { get; set; }

        private static void CreateTable()
        {
            ResultOfCreate = _tableHelper.GetAzureTable(TableName);
        }

        private static void DeleteTable()
        {
            _tableHelper.DeleteAzureTable(TableName);
        }

        private static CloudTable ResultOfCreate { get; set; }

        [TestMethod, TestCategory("Integration")]
        public void TableHelper_GetAzureTable_Should_Return_Cloud_Table()
        {
            Assert.IsInstanceOfType(ResultOfCreate, typeof(CloudTable));   
        }

        [TestMethod, TestCategory("Integration")]
        public void TableHelper_GetAzureTable_Should_Return_Cloud_Table_With_Correct_Name()
        {
            Assert.AreEqual(TableName, ResultOfCreate.Name);
        }
        
        [TestMethod, TestCategory("Integration"), ExpectedException(typeof(ArgumentException))]
        public void TableHelpers_GetAzureTable_Null_Throws_ArguementException()
        {
            _tableHelper.GetAzureTable(null);
        }

        [TestMethod, TestCategory("Integration"), ExpectedException(typeof(ArgumentException))]
        public void TableHelpers_GetAzureTable_EmptyString_Throws_ArguementException()
        {
            _tableHelper.GetAzureTable("");
        }
    }
}
