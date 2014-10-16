using System;
using BohFoundation.Utilities.Utilities.Interfaces;

namespace BohFoundation.Utilities.Utilities.Implementations
{
    public class RandomObjectGenerator : IRandomObjectGenerator
    {
        public Guid GenerateNewGuid()
        {
            return Guid.NewGuid();
        }
    }
}
