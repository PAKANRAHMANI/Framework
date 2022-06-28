using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Framework.Redis
{
    public class RedisCache : IRedisCache
    {

        private readonly IDatabase _database;
        private readonly RedisCacheConfiguration _redisCacheConfiguration;
        public RedisCache(IRedisHelper redisHelper, RedisCacheConfiguration redisCacheConfiguration)
        {
            _redisCacheConfiguration = redisCacheConfiguration;
            _database = redisHelper.GetDatabase(_redisCacheConfiguration.Connection, _redisCacheConfiguration.DbNumber);
        }

        public void Set(string key, object data, int expirationTimeInMinutes)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            var value = JsonConvert.SerializeObject(data);

            var expiresIn = TimeSpan.FromMinutes(expirationTimeInMinutes);

            _database.StringSet(key, value, expiresIn);
        }

        public async Task SetAsync(string key, object data, int expirationTimeInMinutes)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            var value = JsonConvert.SerializeObject(data);

            var expiresIn = TimeSpan.FromMinutes(expirationTimeInMinutes);

            await _database.StringSetAsync(key, value, expiresIn);
        }

        public T Get<T>(string key)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            string value = _database.StringGet(key);
            if (value == null)
                return default;

            var list = JsonConvert.DeserializeObject<List<T>>(value);
            if (list == null || !list.Any())
                return default;

            return list.First();

        }

        public IEnumerable<T> GetValues<T>(string key)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            string value = _database.StringGet(key);
            if (value == null)
                return default;

            var list = JsonConvert.DeserializeObject<IList<T>>(value);
            if (list == null || !list.Any())
                return default;
            return list;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            string value = await _database.StringGetAsync(key);

            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public bool TryGetValue<T>(string key, out T obj)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            string value = _database.StringGet(key);

            if (value == null)
            {
                obj = default(T);
                return false;
            }

            obj = JsonConvert.DeserializeObject<T>(value);

            return true;
        }

        public async Task<IEnumerable<T>> GetValuesAsync<T>(string key)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            var value = await _database.StringGetAsync(key);

            return value.HasValue ? JsonConvert.DeserializeObject<IEnumerable<T>>(Encoding.UTF8.GetString(value)) : default(IEnumerable<T>);
        }

        public void Remove(string key)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            _database.KeyDelete(key);
        }

        public async Task RemoveAsync(string key)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            await _database.KeyDeleteAsync(key);
        }

    }
}