using System;
using Framework.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Framework.Redis;

public static class RedisCacheExtensions
{
	public static IServiceCollection AddRedisCache(this IServiceCollection services, Action<RedisCacheConfiguration> options)
	{
		var redisCacheConfiguration = new RedisCacheConfiguration();

		options.Invoke(redisCacheConfiguration);

        var cacheConfig = new CacheConfiguration
        {
            RedisCacheConfiguration = redisCacheConfiguration
        };

        services.TryAddSingleton(cacheConfig);

        services.TryAddSingleton(redisCacheConfiguration);

		services.TryAddSingleton<IRedisDataBaseResolver, RedisDataBaseResolver>();

		services.TryAddSingleton<IRedisCache, RedisCache>();

		return services;
	}

	public static IServiceCollection AddHashsetRedisCache(this IServiceCollection services, Action<RedisHashsetCacheConfiguration> options)
	{
		var redisCacheConfiguration = new RedisHashsetCacheConfiguration();

		options.Invoke(redisCacheConfiguration);

        var cacheConfig = new CacheConfiguration
        {
            RedisHashsetCacheConfiguration = redisCacheConfiguration
        };

        services.TryAddSingleton(cacheConfig);

        services.TryAddSingleton(redisCacheConfiguration);

		services.TryAddSingleton<IRedisDataBaseResolver, RedisDataBaseResolver>();

		services.TryAddSingleton<IRedisHashsetCache, RedisHashsetCache>();

		return services;
	}
}
