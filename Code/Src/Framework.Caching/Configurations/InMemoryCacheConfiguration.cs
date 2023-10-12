using Microsoft.Extensions.Caching.Memory;

namespace Framework.Caching.Configurations;

public class InMemoryCacheConfiguration
{
    public long SizeLimit { get; set; }
    public CacheItemPriority CachePriority { get; set; }
}