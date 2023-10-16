using Framework.Caching.Extensions.Abstractions;
using Framework.Caching.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Framework.Caching.Extensions.Redis
{
    public class DistributedCache : IDistributedCache
    {
        private readonly IDatabase _database;
        private readonly DistributedCacheConfiguration _distributedCacheConfiguration;

        public DistributedCache(IRedisDataBaseResolver redisHelper, DistributedCacheConfiguration distributedCacheConfiguration)
        {
            _distributedCacheConfiguration = distributedCacheConfiguration;
            _database = redisHelper.GetDatabase(distributedCacheConfiguration.Connection, distributedCacheConfiguration.DbNumber);
        }

        public void Set<T>(string key, T value, int expirationTimeInSecond) where T : class
        {
            if (_distributedCacheConfiguration.UseFromInstanceNameInKey)
                key = _distributedCacheConfiguration.InstanceName + key;
            var data = JsonConvert.SerializeObject(value);

            var expiresIn = TimeSpan.FromSeconds(expirationTimeInSecond);

            _database.StringSet(key, data, expiresIn);
        }

        public T Get<T>(string key) where T : class
        {
            if (_distributedCacheConfiguration.UseFromInstanceNameInKey)
                key = _distributedCacheConfiguration.InstanceName + key;

            string value = _database.StringGet(key);
            if (value == null)
                return default;

            var obj = JsonConvert.DeserializeObject<T>(value);
            if (obj == null)
                return default;

            return obj;
        }
    }
}
