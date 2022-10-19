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
        public RedisCache(IRedisDataBaseResolver redisHelper, RedisCacheConfiguration redisCacheConfiguration)
        {
            _redisCacheConfiguration = redisCacheConfiguration;
            _database = redisHelper.GetDatabase(_redisCacheConfiguration.Connection, _redisCacheConfiguration.DbNumber);
        }


        public void HashSet(string key, object data)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            _database.HashSet(key, data.ToHashEntries());
        }
        public void HashSet(string key,string field, object data)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            var value = JsonConvert.SerializeObject(data);

            _database.HashSet(key,field, value);
        }
        public async Task HashSetAsync(string key, object data)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            await _database.HashSetAsync(key, data.ToHashEntries());
        }
        public async Task HashSetAsync(string key, string field, object data)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            var value = JsonConvert.SerializeObject(data);

            await _database.HashSetAsync(key,field, value);
        }

        public T HashGetAll<T>(string key)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            if (!_database.KeyExists(key))
                return default;

            var stockHashEntries = _database.HashGetAll(key);

            return stockHashEntries.ConvertFromRedis<T>();
        }

        public async Task<T> HashGetAllAsync<T>(string key)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            if (!_database.KeyExists(key))
                return default;

            var stockHashEntries = await _database.HashGetAllAsync(key);

            return stockHashEntries.ConvertFromRedis<T>();
        }

        public T HashGet<T>(string key, string fieldName)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            if (!_database.KeyExists(key))
                return default;

            var value = _database.HashGet(key, fieldName);
            if (!value.HasValue)
                return default(T);

            if ((typeof(T).IsClass || typeof(T).IsArray) && !(typeof(T)).IsString())
                return JsonConvert.DeserializeObject<T>(value);

            return (T)Convert.ChangeType(value, typeof(T));
        }

        public async Task<T> HashGetAsync<T>(string key, string fieldName)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            if (!await _database.KeyExistsAsync(key))
                return default;

            var value = await _database.HashGetAsync(key, fieldName);

            if (!value.HasValue)
                return default(T);

            if ((typeof(T).IsClass || typeof(T).IsArray) && !(typeof(T)).IsString())
                return JsonConvert.DeserializeObject<T>(value);

            return (T)Convert.ChangeType(value, typeof(T));
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

            var obj = JsonConvert.DeserializeObject<T>(value);
            if (obj == null)
                return default;

            return obj;

        }
        public IEnumerable<T> GetValues<T>(string key)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            string value = _database.StringGet(key);
            if (value == null)
                return default;

            var list = JsonConvert.DeserializeObject<List<T>>(value);
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

            return value.HasValue ? JsonConvert.DeserializeObject<List<T>>(Encoding.UTF8.GetString(value)) : default(List<T>);
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

        public bool LockRelease(string key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            return _database.LockRelease(key, value, flags);
        }

        public bool LockTake(string key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            return _database.LockTake(key, value, expiry, flags);
        }

        public bool KeyExists(string key, CommandFlags flags = CommandFlags.None)
        {
            if (_redisCacheConfiguration.UseFromInstanceNameInKey)
                key = _redisCacheConfiguration.InstanceName + key;

            return _database.KeyExists(key, flags);
        }
    }
}