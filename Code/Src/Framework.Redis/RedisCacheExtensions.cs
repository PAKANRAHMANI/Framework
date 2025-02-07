using System;
using Framework.Config;
using Microsoft.Extensions.DependencyInjection;

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

        services.AddSingleton(cacheConfig);

        services.AddSingleton(redisCacheConfiguration);

		services.AddSingleton<IRedisDataBaseResolver, RedisDataBaseResolver>();

		services.AddSingleton<IRedisCache, RedisCache>();

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

        services.AddSingleton(cacheConfig);

        services.AddSingleton(redisCacheConfiguration);

		services.AddSingleton<IRedisDataBaseResolver, RedisDataBaseResolver>();

		services.AddSingleton<IRedisHashsetCache, RedisHashsetCache>();

		return services;
	}
}
