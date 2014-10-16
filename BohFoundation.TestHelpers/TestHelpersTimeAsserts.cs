using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.TestHelpers
{
    public static class TestHelpersTimeAsserts
    {
        public static void RecentTime(DateTime timeToTest)
        {
            var now = DateTime.UtcNow;
            RecentTime(timeToTest, now);
        }

        private static void RecentTime(DateTime timeToTest, DateTime slightlyLaterTime)
        {
            Assert.AreEqual(slightlyLaterTime.Year, timeToTest.Year);
            Assert.AreEqual(slightlyLaterTime.Hour, timeToTest.Hour);
            Assert.AreEqual(slightlyLaterTime.Month, timeToTest.Month);
            Assert.AreEqual(slightlyLaterTime.Day, timeToTest.Day);
            IsGreaterThanOrEqual(slightlyLaterTime, timeToTest);
        }

        public static void IsGreaterThanOrEqual(DateTime moreRecentTime, DateTime laterTime)
        {
            Assert.IsTrue(moreRecentTime >= laterTime);
        }
    }
}