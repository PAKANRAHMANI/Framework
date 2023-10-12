﻿using Framework.Caching.Types;
using Newtonsoft.Json.Linq;

namespace Framework.Caching.Strategy;

public class MultilayerCaching : ICache
{
    private readonly InMemoryCache _memoryCache;
    private readonly DistributedCache _redisCache;

    public MultilayerCaching(InMemoryCache memoryCache, DistributedCache redisCache)
    {
        _memoryCache = memoryCache;
        _redisCache = redisCache;
    }

    public T Get<T>(string key, int? expirationTimeInMinutes = null, Func<T> query = null) where T : class
    {
        var memoryData = _memoryCache.Get<T>(key);

        if (memoryData is not null)
            return memoryData;

        var redisData = _redisCache.Get<T>(key);

        if (redisData is not null)
        {
            if (expirationTimeInMinutes != null) _memoryCache.Set(key, redisData, expirationTimeInMinutes.Value);

            return redisData;
        }
        var data = query?.Invoke();

        if (expirationTimeInMinutes == null) return data;

        _memoryCache.Set(key, data, expirationTimeInMinutes.Value);

        _redisCache.Set(key, data, expirationTimeInMinutes.Value);

        return data;
    }

    public void Set<T>(string key, T value, int expirationTimeInMinutes) where T : class
    {
        _memoryCache.Set(key, value, expirationTimeInMinutes);

        _redisCache.Set(key, value, expirationTimeInMinutes);
    }
}