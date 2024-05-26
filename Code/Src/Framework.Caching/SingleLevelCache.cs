using Framework.Caching.Extensions.Abstractions;

namespace Framework.Caching;

public class SingleLevelCache : ICache
{
	private readonly ICacheControl _caching;

	public SingleLevelCache(ICacheControl caching)
	{
		_caching = caching;
	}

	public T Get<T>(string key, Func<T> query, int expirationTimeInSecond) where T : class
	{
		var memoryData = _caching.Get<T>(key);

		if (memoryData is not null)
			return memoryData;

		var data = query?.Invoke();

		_caching.Set(key, data, expirationTimeInSecond);

		return data;
	}

	public void Set<T>(string key, T value, int expirationTimeInSecond) where T : class
	{
		_caching.Set(key, value, expirationTimeInSecond);
	}

	public void Remove(string key)
	{
		_caching.Remove(key);
	}
}
