using Framework.Caching.Extensions.Abstractions;
using Framework.Caching.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace Framework.Caching.Extensions.DotnetCore;

public class InMemoryCache : IInMemoryCache
{
    private readonly IMemoryCache _memoryCache;
    private readonly InMemoryCacheConfiguration _cacheConfiguration;

    public InMemoryCache(IInMemoryCacheProvider inMemoryCacheProvider, InMemoryCacheConfiguration cacheConfiguration)
    {
        _memoryCache = inMemoryCacheProvider.GetMemoryCache();
        _cacheConfiguration = cacheConfiguration;
    }

    public void Set<T>(string key, T value, int expirationTimeInMinutes) where T : class
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(expirationTimeInMinutes))
                .SetPriority((CacheItemPriority)_cacheConfiguration.CachePriority);

        _memoryCache.Set(key, value, cacheEntryOptions);
    }

    public T Get<T>(string key) where T : class
    {
        return _memoryCache.TryGetValue(key, out T value) ? value : null;
    }
}
