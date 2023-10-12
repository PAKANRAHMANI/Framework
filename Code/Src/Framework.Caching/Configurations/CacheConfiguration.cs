using Framework.Caching.Initial;

namespace Framework.Caching.Configurations;

public class CacheConfiguration
{
    public CacheConfiguration()
    {
        this.DistributedCacheConfiguration = new DistributedCacheConfiguration();
        this.InMemoryCacheConfiguration = new InMemoryCacheConfiguration();
    }
    public DistributedCacheConfiguration DistributedCacheConfiguration { get; set; }
    public InMemoryCacheConfiguration InMemoryCacheConfiguration { get; set; }
}