using System;
using BohFoundation.AzureStorage.TableStorage.Implementations.Essay.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.AzureStorage.Tests.TableStorage.Essays.Helpers
{
    [TestClass]
    public class EssayRowKeyGeneratorTests
    {
        private EssayRowKeyGenerator _essayRowKeyGenerator;

        [TestInitialize]
        public void Initialize()
        {
            Id = 2;
            Guid = Guid.NewGuid();
            _essayRowKeyGenerator = new EssayRowKeyGenerator();
        }

        private Guid Guid { get; set; }
        private int Id { get; set; }

        [TestMethod]
        public void EssayRowKeyGenerator_Should_Return_Proper_RowKey()
        {
            var expected = "ESSAYTOPICID_" + Id + "_USERSGUID_" + Guid;

            var result = _essayRowKeyGenerator.CreateRowKeyForEssay(Guid, Id);
            Assert.AreEqual(expected,result);
        }
    }
}
