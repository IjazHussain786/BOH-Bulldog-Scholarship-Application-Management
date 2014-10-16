using System;
using System.Globalization;
using BohFoundation.AzureStorage.TableStorage.Implementations;
using BohFoundation.AzureStorage.TableStorage.Implementations.LettersOfRecommendation;
using BohFoundation.AzureStorage.TableStorage.Implementations.LettersOfRecommendation.Entities;
using BohFoundation.Domain.Dtos.Common.AzureQueuryObjects;
using BohFoundation.Domain.Dtos.Reference.Anonymous;
using BohFoundation.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;

namespace BohFoundation.AzureStorage.Tests.TableStorage.LettersOfRecommendation
{
    [TestClass]
    public class AzureLettersOfRecommendationRepositoryTests
    {
        private static AzureLettersOfRecommendationRepository _azureLettersOfRecommendationRepo;
        private static CloudTable _tableToManipulateDirectly;

        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            TestHelpersCommonFields.InitializeFields();

            Setup();
            FirstUpsert();
            SecondUpsert();
        }

        private static void Setup()
        {
            _azureLettersOfRecommendationRepo = new AzureLettersOfRecommendationRepository(TestHelpersCommonFields.AzureConnectionName);
            PartitionKey = "recommendations_" + DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture);
            LetterOfRecommendationGuid = Guid.NewGuid();
            RowKey = LetterOfRecommendationGuid + "_" + DateTime.UtcNow.Year;
        }

        public static Guid LetterOfRecommendationGuid { get; set; }

        private static string PartitionKey { get; set; }
        private static string RowKey { get; set; }

        private static void FirstUpsert()
        {
            FirstLetter = "Words!";

            _azureLettersOfRecommendationRepo.UpsertLetterOfRecommendation(new LetterOfRecommendationKeyValueForEntityFrameworkAndAzureDto
            {
                RowKey = RowKey,
                LetterOfRecommendationGuid = LetterOfRecommendationGuid,
                LetterOfRecommendation = FirstLetter,
                PartitionKey = PartitionKey
            });

            _tableToManipulateDirectly =
                new TableHelper(TestHelpersCommonFields.AzureConnectionName).GetAzureTable("lettersofrecommendation");

            ResultOfUpsert1 = GetTableValue();
            
            GetResult =
                _azureLettersOfRecommendationRepo.GetLetterOfRecommendation(new AzureTableStorageEntityKeyDto
                {
                    PartitionKey = PartitionKey,
                    RowKey = RowKey
                });
        }

        private static LetterOfRecommendationDto GetResult { get; set; }
        private static LetterOfRecommendationAzureTableEntity ResultOfUpsert1 { get; set; }
        private static string FirstLetter { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void AzureLettersOfRecommendationRepository_UpsertLetterOfRecommendation_Should_Insert_Letter()
        {
            Assert.AreEqual(FirstLetter, ResultOfUpsert1.LetterOfRecommendation);
        }

        [TestCategory("Integration"), TestMethod]
        public void AzureLettersOfRecommendationRepository_GetLetterOfRecommendation_Should_Return_Letter()
        {
            Assert.AreEqual(FirstLetter, GetResult.LetterOfRecommendation);
        }

        private static void SecondUpsert()
        {
            SecondLetter = "Reasons!";

            _azureLettersOfRecommendationRepo.UpsertLetterOfRecommendation(new LetterOfRecommendationKeyValueForEntityFrameworkAndAzureDto
            {
                RowKey = RowKey,
                LetterOfRecommendationGuid = LetterOfRecommendationGuid,
                LetterOfRecommendation = SecondLetter,
                PartitionKey = PartitionKey
            });

            ResultOfUpsert2 = GetTableValue();
        }

        private static LetterOfRecommendationAzureTableEntity ResultOfUpsert2 { get; set; }
        private static string SecondLetter { get; set; }

        [TestCategory("Integration"), TestMethod]
        public void AzureLettersOfRecommendationRepository_UpsertLetterOfRecommendation_Should_Update_Letter()
        {
            Assert.AreEqual(SecondLetter, ResultOfUpsert2.LetterOfRecommendation);
        } 

        private static LetterOfRecommendationAzureTableEntity GetTableValue()
        {
            var tableOperation = TableOperation.Retrieve<LetterOfRecommendationAzureTableEntity>(PartitionKey, RowKey);

            var resultOfOperation = _tableToManipulateDirectly.Execute(tableOperation);

            return (LetterOfRecommendationAzureTableEntity)resultOfOperation.Result;
        }
    }
}