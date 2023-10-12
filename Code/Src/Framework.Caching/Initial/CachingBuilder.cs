using Framework.Caching.Configurations;
using Framework.Caching.Providers.Redis;
using Framework.Caching.Strategy;
using Framework.Caching.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Caching.Initial
{
    public class CacheBuilder:
        ICachingStrategyBuilder,
        ICachingTypeBuilder
    {
        private readonly IServiceCollection _serviceCollection;

        private CacheBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }
        public static ICachingStrategyBuilder Setup(IServiceCollection serviceCollection) => new CacheBuilder(serviceCollection);


        public void MultiLayerCache(Action<CacheConfiguration> config)
        {
            _serviceCollection.AddMemoryCache();

            _serviceCollection.AddSingleton<IRedisDataBaseResolver, RedisDataBaseResolver>();

            _serviceCollection.AddSingleton<ICache, MultilayerCaching>();

            _serviceCollection.AddSingleton<InMemoryCache>();

            _serviceCollection.AddSingleton<DistributedCache>();

            var cacheConfig = new CacheConfiguration();

            config.Invoke(cacheConfig);

            _serviceCollection.AddSingleton<DistributedCacheConfiguration>(cacheConfig.DistributedCacheConfiguration);

            _serviceCollection.AddSingleton<InMemoryCacheConfiguration>(cacheConfig.InMemoryCacheConfiguration);

        }

        public ICachingTypeBuilder SingleCache()
        {
            _serviceCollection.AddSingleton<ICache, SingleCachingStrategy>();

            return this;
        }

        public void UseInMemoryCache(Action<InMemoryCacheConfiguration> config)
        {
            var cacheConfig = new InMemoryCacheConfiguration();

            config.Invoke(cacheConfig);

            _serviceCollection.AddSingleton<InMemoryCacheConfiguration>(cacheConfig);

            _serviceCollection.AddSingleton<ICacheControl, InMemoryCache>();

        }

        public void UseDistributedCache(Action<DistributedCacheConfiguration> config)
        {
            var cacheConfig = new DistributedCacheConfiguration();

            config.Invoke(cacheConfig);

            _serviceCollection.AddSingleton<DistributedCacheConfiguration>(cacheConfig);

            _serviceCollection.AddSingleton<ICacheControl, DistributedCache>();

        }
    }
}
