using Framework.Caching.Configurations;
using Framework.Caching.Providers.Redis;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Framework.Caching.Types
{
    public class DistributedCache : ICacheControl
    {
        private readonly IDatabase _database;
        private readonly DistributedCacheConfiguration _distributedCacheConfiguration;

        public DistributedCache(IRedisDataBaseResolver redisHelper, DistributedCacheConfiguration distributedCacheConfiguration)
        {
            _distributedCacheConfiguration = distributedCacheConfiguration;
            _database = redisHelper.GetDatabase(distributedCacheConfiguration.Connection, distributedCacheConfiguration.DbNumber);
        }

        public void Set<T>(string key, T value, int expirationTimeInMinutes) where T : class
        {
            if (_distributedCacheConfiguration.UseFromInstanceNameInKey)
                key = _distributedCacheConfiguration.InstanceName + key;
            var data = JsonConvert.SerializeObject(value);

            var expiresIn = TimeSpan.FromMinutes(expirationTimeInMinutes);

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