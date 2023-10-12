using Framework.Caching.Configurations;

namespace Framework.Caching.Initial;

public interface ICachingStrategyBuilder
{
    void MultiLayerCache(Action<CacheConfiguration> config);

    ICachingTypeBuilder SingleCache();

}