using Framework.Caching.Extensions.Abstractions;
using Framework.Caching.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Framework.Caching.Extensions.Redis;

public class DistributedCache(
    IRedisDataBaseResolver redisHelper,
    DistributedCacheConfiguration distributedCacheConfiguration)
    : IDistributedCache
{
	private readonly IDatabase _database = redisHelper.GetDatabase(distributedCacheConfiguration.Connection, distributedCacheConfiguration.DbNumber);

    public void Set<T>(string key, T value, int expirationTimeInSecond) where T : class
	{
		if (distributedCacheConfiguration.UseFromInstanceNameInKey)
			key = distributedCacheConfiguration.InstanceName + key;

		var data = JsonConvert.SerializeObject(value);

		var expiresIn = TimeSpan.FromSeconds(expirationTimeInSecond);

		_database.StringSet(key, data, expiresIn);
	}

    public void Set<T>(string key, T value) where T : class
    {
        if (distributedCacheConfiguration.UseFromInstanceNameInKey)
            key = distributedCacheConfiguration.InstanceName + key;

        var data = JsonConvert.SerializeObject(value);

        _database.StringSet(key, data);
    }

    public T Get<T>(string key) where T : class
	{
		if (distributedCacheConfiguration.UseFromInstanceNameInKey)
			key = distributedCacheConfiguration.InstanceName + key;

		string value = _database.StringGet(key);

		if (value == null)
			return default;

		var obj = JsonConvert.DeserializeObject<T>(value);

		if (obj == null)
			return default;

		return obj;
	}

	public void Remove(string key)
	{
		_database.KeyDelete(key);
	}
}
