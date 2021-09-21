using System.Collections.Generic;

namespace LottoService.Infrastructure.Persistence
{
    public interface IRedisContext
    {
        string Get(string key);
        void Set(string key, string value);
        IEnumerable<TObj> GetAsArray<TObj>(string key);
        void SetAsSeparatedItems<TObj>(string key, IEnumerable<TObj> seperatedItems);
        void Set<TObj>(string key, TObj value);
    }
}
