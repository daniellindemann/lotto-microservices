using System;

namespace RandomNumberService.Application.Common.Exceptions
{
    public class ValueRangeException : Exception
    {
        public ValueRangeException(int min, int max) : base(
            $"Value range invalid. Min value ({min}) must be less than max value ({max}).")
        {
        }
    }
}
