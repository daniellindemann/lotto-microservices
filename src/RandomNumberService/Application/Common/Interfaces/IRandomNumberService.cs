namespace RandomNumberService.Application.Common.Interfaces
{
    public interface IRandomNumberService
    {
        int Generate(int min, int max);
    }
}
