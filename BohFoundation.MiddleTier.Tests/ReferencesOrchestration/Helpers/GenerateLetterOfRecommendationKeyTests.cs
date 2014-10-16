using System;
using System.Globalization;
using BohFoundation.Domain.Dtos.Reference.Anonymous;
using BohFoundation.MiddleTier.ReferencesOrchestration.Implementations.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.MiddleTier.Tests.ReferencesOrchestration.Helpers
{
    [TestClass]
    public class GenerateLetterOfRecommendationKeyTests
    {
        private GenerateLetterOfRecommendationKey _generateLetterOfRecommendationKey;
        private Guid _guid;
        private string _letterOfRecommendation;
        private LetterOfRecommendationKeyValueForEntityFrameworkAndAzureDto Result { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            _guid = Guid.NewGuid();
            _letterOfRecommendation = "asdjkfljklnsadfnjkasdfjkln8923789adfsxhn";
            _generateLetterOfRecommendationKey = new GenerateLetterOfRecommendationKey();

            Result = _generateLetterOfRecommendationKey.GenerateKeyValueForLettersOfRecommendation(
                new LetterOfRecommendationDto { LetterOfRecommendationGuid = _guid, LetterOfRecommendation = _letterOfRecommendation });
        }

        [TestMethod]
        public void GenerateLetterOfRecommendationKey_GenerateKeyValueForLettersOfRecommendation_Returns_Properly_Formatted_Key()
        {
            Assert.AreEqual(_guid + "_" + DateTime.UtcNow.Year, Result.RowKey);
        }

        [TestMethod]
        public void GenerateLetterOfRecommendationKey_GenerateKeyValueForLettersOfRecommendation_Returns_Guid()
        {
            Assert.AreEqual(_guid, Result.LetterOfRecommendationGuid);
        }

        [TestMethod]
        public void GenerateLetterOfRecommendationKey_GenerateKeyValueForLettersOfRecommendation_Returns_Letter()
        {
            Assert.AreEqual(_letterOfRecommendation, Result.LetterOfRecommendation);
        }

        [TestMethod]
        public void GenerateLetterOfRecommendationKey_GenerateKeyValueForLettersOfRecommendation_Returns_PartitionKey()
        {
            Assert.AreEqual("Recommendation_" + DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture), Result.PartitionKey);
        }
    }
}