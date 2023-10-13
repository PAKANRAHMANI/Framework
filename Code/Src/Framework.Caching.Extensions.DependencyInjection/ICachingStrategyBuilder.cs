using Framework.Caching.Extensions.Configuration;

namespace Framework.Caching.Extensions.DependencyInjection;

public interface ICachingStrategyBuilder
{
    void MultilevelCache(Action<CacheConfiguration> config);

    ICachingTypeBuilder SingleLevelCache();

}