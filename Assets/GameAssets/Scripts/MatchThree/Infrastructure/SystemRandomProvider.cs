using System;
using MatchThree.Domain.Contracts;

namespace MatchThree.Infrastructure
{
    public class SystemRandomProvider : IRandomProvider
    {
        private readonly Random _random;

        public SystemRandomProvider()
        {
            _random = new Random();
        }

        public int Range(int minInclusive, int maxExclusive)
        {
            int result = _random.Next(minInclusive, maxExclusive);

            return result;
        }
    }
}
