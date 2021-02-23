using System;
using RandomNumberService.Application;
using RandomNumberService.Application.Common.Exceptions;
using RandomNumberService.Application.Common.Interfaces;

namespace RandomNumberService.Infrastructure
{
    public class RandomNumberService : IRandomNumberService
    {
        public int Generate(int min, int max)
        {
            if (min == max ||
                min > max)
            {
                throw new ValueRangeException(min, max);
            }

            var rng = new Random();
            var number = rng.Next(min, max);
            return number;
        }
    }
}
