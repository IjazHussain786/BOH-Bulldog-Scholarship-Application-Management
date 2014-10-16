using System;
using BohFoundation.Utilities.Utilities.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.Utilities.Tests.Utilities
{
    [TestClass]
    public class RandomObjectGeneratorTests
    {
        private RandomObjectGenerator _randomObjectGenerator;

        [TestInitialize]
        public void Initialize()
        {
            _randomObjectGenerator = new RandomObjectGenerator();
        }

        [TestMethod]
        public void RandomObjectGenerator_GenerateGuid_Return_NotNew_Guid()
        {
            Assert.AreNotEqual(new Guid(), _randomObjectGenerator.GenerateNewGuid());
        }
    }
}
