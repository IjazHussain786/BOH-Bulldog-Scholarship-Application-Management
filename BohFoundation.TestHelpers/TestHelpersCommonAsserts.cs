using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.TestHelpers
{
    public static class TestHelpersCommonAsserts
    {
        public static void IsGreaterThanZero(int number)
        {
            Assert.IsTrue(number > 0);
        }
    }
}
