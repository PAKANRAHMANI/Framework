using Framework.Caching.Extensions.Abstractions;

namespace Framework.Caching;

public class MultilevelCache(IInMemoryCache memoryCache, IDistributedCache redisCache) : ICache
{
    public T Get<T>(string key, Func<T> query, int expirationTimeInSecond) where T : class
    {
        var memoryData = memoryCache.Get<T>(key);

        if (memoryData is not null)
            return memoryData;

        var redisData = redisCache.Get<T>(key);

        if (redisData is not null)
        {
            memoryCache.Set(key, redisData, expirationTimeInSecond);

            return redisData;
        }

        var data = query?.Invoke();

        if (data is null) return null;

        memoryCache.Set(key, data, expirationTimeInSecond);

        redisCache.Set(key, data, expirationTimeInSecond);

        return data;

    }

    public async Task<T> Get<T>(string key, Func<Task<T>> query, int expirationTimeInSecond) where T : class
    {
        var memoryData = memoryCache.Get<T>(key);

        if (memoryData is not null)
            return memoryData;

        var redisData = redisCache.Get<T>(key);

        if (redisData is not null)
        {
            memoryCache.Set(key, redisData, expirationTimeInSecond);

            return redisData;
        }

        var data =await query?.Invoke()!;

        if (data is null) return null;

        memoryCache.Set(key, data, expirationTimeInSecond);

        redisCache.Set(key, data, expirationTimeInSecond);

        return data;
    }

    public T Get<T>(string key, Func<T> query) where T : class
    {
        var memoryData = memoryCache.Get<T>(key);

        if (memoryData is not null)
            return memoryData;

        var redisData = redisCache.Get<T>(key);

        if (redisData is not null)
        {
            memoryCache.Set(key, redisData);

            return redisData;
        }

        var data = query?.Invoke();

        if (data is null) return null;

        memoryCache.Set(key, data);

        redisCache.Set(key, data);

        return data;
    }

    public async Task<T> Get<T>(string key, Func<Task<T>> query) where T : class
    {
        var memoryData = memoryCache.Get<T>(key);

        if (memoryData is not null)
            return memoryData;

        var redisData = redisCache.Get<T>(key);

        if (redisData is not null)
        {
            memoryCache.Set(key, redisData);

            return redisData;
        }

        var data = await query?.Invoke()!;

        if (data is null) return null;

        memoryCache.Set(key, data);

        redisCache.Set(key, data);

        return data;
    }

    public void Set<T>(string key, T value, int expirationTimeInMinutes) where T : class
    {
        memoryCache.Set(key, value, expirationTimeInMinutes);

        redisCache.Set(key, value, expirationTimeInMinutes);
    }

    public void Set<T>(string key, T value) where T : class
    {
        memoryCache.Set(key, value);

        redisCache.Set(key, value);
    }

    public void Remove(string key)
    {
        memoryCache.Remove(key);

        redisCache.Remove(key);
    }
}
