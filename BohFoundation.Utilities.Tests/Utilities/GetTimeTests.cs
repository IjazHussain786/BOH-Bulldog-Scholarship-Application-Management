using System;
using BohFoundation.TestHelpers;
using BohFoundation.Utilities.Utilities.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.Utilities.Tests.Utilities
{
    [TestClass]
    public class GetTimeTests
    {
        private GetTime _getTime;

        [TestInitialize]
        public void Initialize()
        {
            _getTime = new GetTime();
        }

        [TestMethod]
        public void GetTime_GetUtcNow_Should_Return_A_Time_That_Is_Very_Close_To_Now()
        {
            TestHelpersTimeAsserts.RecentTime(_getTime.GetUtcNow());
        }

        [TestMethod]
        public void GetTime_GetUtcNow_Should_Return_A_Time_That_Is_Very_Close_To_Now_2()
        {
            var timeBefore = _getTime.GetUtcNow();
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(DateTime.UtcNow, timeBefore);
        
        }

        [TestMethod]
        public void GetTime_GetUtcNow_Should_Return_A_Time_That_Is_Very_Close_To_Now_3()
        {
            var timeBefore = DateTime.UtcNow;
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(_getTime.GetUtcNow(), timeBefore);
        }
    }
}
