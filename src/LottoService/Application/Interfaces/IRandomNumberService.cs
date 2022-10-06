namespace LottoService.Application.Interfaces;

public interface IRandomNumberService
{
    Task<int> Generate(int min, int max);
}
