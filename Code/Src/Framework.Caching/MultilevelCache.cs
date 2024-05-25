using Framework.Caching.Extensions.Abstractions;

namespace Framework.Caching;

public class MultilevelCache : ICache
{
	private readonly IInMemoryCache _memoryCache;
	private readonly IDistributedCache _redisCache;

	public MultilevelCache(IInMemoryCache memoryCache, IDistributedCache redisCache)
	{
		_memoryCache = memoryCache;
		_redisCache = redisCache;
	}

	public T Get<T>(string key, Func<T> query, int expirationTimeInSecond) where T : class
	{
		var memoryData = _memoryCache.Get<T>(key);

		if (memoryData is not null)
			return memoryData;

		var redisData = _redisCache.Get<T>(key);

		if (redisData is not null)
		{
			_memoryCache.Set(key, redisData, expirationTimeInSecond);

			return redisData;
		}

		var data = query?.Invoke();

		_memoryCache.Set(key, data, expirationTimeInSecond);

		_redisCache.Set(key, data, expirationTimeInSecond);

		return data;
	}

	public void Set<T>(string key, T value, int expirationTimeInMinutes) where T : class
	{
		_memoryCache.Set(key, value, expirationTimeInMinutes);

		_redisCache.Set(key, value, expirationTimeInMinutes);
	}

	public void Remove(string key)
	{
		_memoryCache.Remove(key);

		_redisCache.Remove(key);
	}
}
