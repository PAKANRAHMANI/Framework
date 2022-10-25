using System;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Redis
{
    public static class RedisCacheExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, Action<RedisCacheConfiguration> options)
        {
            var redisCacheConfiguration = new RedisCacheConfiguration();

            options.Invoke(redisCacheConfiguration);

            services.AddSingleton(redisCacheConfiguration);

            services.AddSingleton<IRedisDataBaseResolver, RedisDataBaseResolver>();

            services.AddSingleton<IRedisCache, RedisCache>();

            return services;
        }
        public static IServiceCollection AddHashsetRedisCache(this IServiceCollection services, Action<RedisHashsetCacheConfiguration> options)
        {
            var redisCacheConfiguration = new RedisHashsetCacheConfiguration();

            options.Invoke(redisCacheConfiguration);

            services.AddSingleton(redisCacheConfiguration);

            services.AddSingleton<IRedisDataBaseResolver, RedisDataBaseResolver>();

            services.AddSingleton<IRedisHashsetCache, RedisHashsetCache>();

            return services;
        }
    }
}
