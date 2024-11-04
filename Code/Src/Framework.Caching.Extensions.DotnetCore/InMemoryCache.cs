using Framework.Caching.Extensions.Abstractions;
using Framework.Caching.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace Framework.Caching.Extensions.DotnetCore;

public class InMemoryCache(IMemoryCache memoryCache, InMemoryCacheConfiguration cacheConfiguration)
    : IInMemoryCache, ICacheControl
{
    public void Set<T>(string key, T value, int expirationTimeInSecond) where T : class
	{
		var cacheEntryOptions = new MemoryCacheEntryOptions()
				.SetAbsoluteExpiration(TimeSpan.FromSeconds(expirationTimeInSecond))
				.SetPriority((CacheItemPriority)cacheConfiguration.CachePriority);

		memoryCache.Set(key, value, cacheEntryOptions);
	}
    public void Set<T>(string key, T value) where T : class
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetPriority((CacheItemPriority)cacheConfiguration.CachePriority);

        memoryCache.Set(key, value, cacheEntryOptions);
    }
    public T Get<T>(string key) where T : class
	{
		return memoryCache.TryGetValue(key, out T value) ? value : null;
	}

	public void Remove(string key)
	{
		memoryCache.Remove(key);
	}
}
