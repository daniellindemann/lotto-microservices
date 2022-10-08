namespace LottoService.Application.Interfaces;

public interface ICacheService<T> where T : class, new()
{
    Task SetAsync(string key, T? value);
    Task<T?> GetAsync(string key);
}
