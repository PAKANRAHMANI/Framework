using Framework.Caching.Configurations;

namespace Framework.Caching.Initial;

public interface ICachingTypeBuilder
{
    void UseInMemoryCache(Action<InMemoryCacheConfiguration> config);
    void UseDistributedCache(Action<DistributedCacheConfiguration> config);
}