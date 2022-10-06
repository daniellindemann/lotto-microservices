namespace LottoService.Application.Interfaces;

public interface ICacheService<T> where T : class, new()
{
    Task Set(string key, T? value);
    Task<T?> Get(string key);
}