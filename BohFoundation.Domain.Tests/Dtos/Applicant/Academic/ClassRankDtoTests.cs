using BohFoundation.Domain.Dtos.Applicant.Academic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.Domain.Tests.Dtos.Academic
{
    [TestClass]
    public class ClassRankDtoTests
    {
        [TestMethod]
        public void ClassRank_PercentileCalculation_CalculationRoundedToHundreths()
        {
            var classRank = new ClassRankDto {ClassNumericalRank = 73, GraduatingClassSize = 154};
            Assert.AreEqual(.53, classRank.ClassPercentile);
        }
    }
}
