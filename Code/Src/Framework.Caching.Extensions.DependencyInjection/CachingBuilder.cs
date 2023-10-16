using Framework.Caching.Extensions.Abstractions;
using Framework.Caching.Extensions.Configuration;
using Framework.Caching.Extensions.DotnetCore;
using Framework.Caching.Extensions.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Caching.Extensions.DependencyInjection
{
	public class CacheBuilder :
		ICachingStrategyBuilder,
		ICachingTypeBuilder
	{
		private readonly IServiceCollection _serviceCollection;

		private CacheBuilder(IServiceCollection serviceCollection)
		{
			_serviceCollection = serviceCollection;
		}
		public static ICachingStrategyBuilder Setup(IServiceCollection serviceCollection) => new CacheBuilder(serviceCollection);


		public void MultilevelCache(Action<CacheConfiguration> config)
		{
			_serviceCollection.AddMemoryCache();

			_serviceCollection.AddSingleton<IRedisDataBaseResolver, RedisDataBaseResolver>();

			_serviceCollection.AddSingleton<ICache, MultilevelCache>();

			_serviceCollection.AddSingleton<IInMemoryCache, InMemoryCache>();

			_serviceCollection.AddSingleton<IDistributedCache, DistributedCache>();

			var cacheConfig = new CacheConfiguration();

			config.Invoke(cacheConfig);

			_serviceCollection.AddSingleton<DistributedCacheConfiguration>(cacheConfig.DistributedCacheConfiguration);

			_serviceCollection.AddSingleton<InMemoryCacheConfiguration>(cacheConfig.InMemoryCacheConfiguration);

			_serviceCollection.AddSingleton<IInMemoryCacheProvider, InMemoryCacheProvider>();
		}

		public ICachingTypeBuilder SingleLevelCache()
		{
			_serviceCollection.AddSingleton<ICache, SingleLevelCache>();

			return this;
		}

		public void UseInMemoryCache(Action<InMemoryCacheConfiguration> config)
		{
			var cacheConfig = new InMemoryCacheConfiguration();

			config.Invoke(cacheConfig);

			_serviceCollection.AddMemoryCache();

			_serviceCollection.AddSingleton<InMemoryCacheConfiguration>(cacheConfig);

			_serviceCollection.AddSingleton<ICacheControl, InMemoryCache>();

			_serviceCollection.AddSingleton<IInMemoryCacheProvider, InMemoryCacheProvider>();

		}

		public void UseDistributedCache(Action<DistributedCacheConfiguration> config)
		{
			var cacheConfig = new DistributedCacheConfiguration();

			config.Invoke(cacheConfig);

			_serviceCollection.AddSingleton<IRedisDataBaseResolver, RedisDataBaseResolver>();

			_serviceCollection.AddSingleton<DistributedCacheConfiguration>(cacheConfig);

			_serviceCollection.AddSingleton<ICacheControl, DistributedCache>();

		}
	}
}
