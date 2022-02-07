using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace LottoService.Infrastructure.Persistence
{
    public class RedisContext : IRedisContext
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;

        private RedisContext()
        {
        }

        public RedisContext(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public static RedisContext Empty = new RedisContext();

        private bool Connect()
        {
            try
            {
                // TODO: _connectionMultiplexer is not initialized correctly. Check if there's time
                if (_connectionMultiplexer == null)
                    return false;

                if (_database == null)
                {
                    _database = _connectionMultiplexer.GetDatabase();
                }

                return true;
            }
            catch (RedisConnectionException)
            {
                return false;
            }
        }

        public string Get(string key)
        {
            if (Connect())
            {
                var val = _database.StringGet(key);
                return val;
            }

            return null;
        }

        public IEnumerable<TObj> GetAsArray<TObj>(string key)
        {
            if (Connect())
            {
                var val = Get(key);
                if (val == null)
                    return null;

                var deserializedEnumerable = JsonConvert.DeserializeObject<IEnumerable<TObj>>($"[{val}]");
                return deserializedEnumerable;
            }

            return null;
        }

        public void SetAsSeparatedItems<TObj>(string key, IEnumerable<TObj> seperatedItems)
        {
            if (Connect())
            {
                var itemsString = JsonConvert.SerializeObject(seperatedItems);
                var insertString = itemsString.Trim('[', ']');

                Set(key, insertString);
            }
        }

        public void Set(string key, string value)
        {
            if (Connect())
            {
                if (!_database.StringSet(key, value))
                    throw new RedisException($"Unable to update key '{key}'");
            }
        }

        public void Set<TObj>(string key, TObj value)
        {
            if (Connect())
            {
                Set(key, JsonConvert.SerializeObject(value));
            }
        }
    }
}
