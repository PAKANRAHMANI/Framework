using Framework.Caching.Extensions.Abstractions;

namespace Framework.Caching;

public class SingleLevelCache(ICacheControl caching) : ICache
{
    public T Get<T>(string key, Func<T> query, int expirationTimeInSecond) where T : class
	{
		var memoryData = caching.Get<T>(key);

		if (memoryData is not null)
			return memoryData;

		var data = query?.Invoke();

		caching.Set(key, data, expirationTimeInSecond);

		return data;
	}

    public async Task<T> Get<T>(string key, Func<Task<T>> query, int expirationTimeInSecond) where T : class
    {
        var memoryData = caching.Get<T>(key);

        if (memoryData is not null)
            return memoryData;

        var data = await query?.Invoke()!;

        caching.Set(key, data, expirationTimeInSecond);

        return data;
    }

    public T Get<T>(string key, Func<T> query) where T : class
    {
        var memoryData = caching.Get<T>(key);

        if (memoryData is not null)
            return memoryData;

        var data = query?.Invoke();

        caching.Set(key, data);

        return data;
    }

    public async Task<T> Get<T>(string key, Func<Task<T>> query) where T : class
    {
        var memoryData = caching.Get<T>(key);

        if (memoryData is not null)
            return memoryData;

        var data = await query?.Invoke()!;

        caching.Set(key, data);

        return data;
    }

    public void Set<T>(string key, T value, int expirationTimeInSecond) where T : class
	{
		caching.Set(key, value, expirationTimeInSecond);
	}

    public void Set<T>(string key, T value) where T : class
    {
        caching.Set(key, value);
    }

    public void Remove(string key)
	{
		caching.Remove(key);
	}
}
