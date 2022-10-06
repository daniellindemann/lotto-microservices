namespace RandomNumberService.Application.Exceptions;

public class ValueRangeException : Exception
{
    public ValueRangeException(int min, int max)
        : base($"Value range invalid. Min value ({min}) must be less than max value ({max}).")
    {
    }
}
