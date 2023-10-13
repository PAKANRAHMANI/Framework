using Framework.Caching.Extensions.Configuration;

namespace Framework.Caching.Extensions.DependencyInjection;

public interface ICachingTypeBuilder
{
    void UseInMemoryCache(Action<InMemoryCacheConfiguration> config);
    void UseDistributedCache(Action<DistributedCacheConfiguration> config);
}