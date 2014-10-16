using System;
using BohFoundation.Utilities.Utilities.Interfaces;

namespace BohFoundation.Utilities.Utilities.Implementations
{
    public class GetTime : IGetTime
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
