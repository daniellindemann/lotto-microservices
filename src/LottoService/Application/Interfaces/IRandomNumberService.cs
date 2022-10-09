namespace LottoService.Application.Interfaces;

public interface IRandomNumberService
{
    Task<int> GenerateAsync(int min, int max);
}
