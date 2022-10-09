namespace RandomNumberService.Application.Interfaces;

public interface IRandomNumberService
{
    int Generate(int min, int max);
}
