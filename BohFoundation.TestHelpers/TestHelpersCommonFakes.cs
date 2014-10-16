using BohFoundation.Utilities.Context.Implementation;
using BohFoundation.Utilities.Context.Interfaces;
using BohFoundation.Utilities.Context.Interfaces.Context;
using FakeItEasy;

namespace BohFoundation.TestHelpers
{
    public static class TestHelpersCommonFakes
    {
        public static void InitializeFakes()
        {
            ClaimsInformationGetters = A.Fake<IClaimsInformationGetters>();
            DeadlineUtilities = A.Fake<IDeadlineUtilities>();
        }

        public static IDeadlineUtilities DeadlineUtilities { get; set; }
        public static IClaimsInformationGetters ClaimsInformationGetters { get; set; }
    }
}
